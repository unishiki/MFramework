#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace Game.Editor.ParticlePrefabCollector
{
    public class ParticlePrefabPreviewControlWindow : EditorWindow
    {
        private Vector2 _leftScroll;
        private GUIStyle _panelStyle;

        private GUIStyle _bottomBarStyle;

        // Cached reflection info for Selection Outline
        private static PropertyInfo _showSelectionOutlineProp;
        private static bool _selectionOutlineChecked;


        private void OnGUI()
        {
            // Middle main area: single column with options and actions
            using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
            {
                // Panel background with padding and small margin
                EnsureStyles();
                GUILayout.Space(4);
                using (new EditorGUILayout.VerticalScope(_panelStyle, GUILayout.ExpandHeight(true)))
                {
                    _leftScroll = EditorGUILayout.BeginScrollView(_leftScroll, GUILayout.ExpandHeight(true));
                    // Spacing controls ABOVE toggles
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("排列间隔 (1-100)", GUILayout.Width(120));
                        int curSpacing = ParticlePrefabPreviewSceneHelper.GetPreviewSpacingInt();
                        // Single direct numeric input
                        int inputSpacing = EditorGUILayout.IntField(curSpacing, GUILayout.Width(60));
                        inputSpacing = Mathf.Clamp(inputSpacing, 1, 100);
                        if (inputSpacing != curSpacing)
                        {
                            ParticlePrefabPreviewSceneHelper.PersistSpacingToConfigAndRelayout(inputSpacing);
                        }
                    }
#if UNITY_2019_1_OR_NEWER
                    EditorGUILayout.Space(6);
#else
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
#endif
                    // Layout plane toggle (persisted)
                    bool newVertical = EditorGUILayout.ToggleLeft("垂直排列（XY平面）",
                        ParticlePrefabPreviewSceneHelper.VerticalLayout);
                    if (newVertical != ParticlePrefabPreviewSceneHelper.VerticalLayout)
                    {
                        ParticlePrefabPreviewSceneHelper.PersistVerticalLayoutToConfigAndRelayout(newVertical);
                    }

#if UNITY_2019_1_OR_NEWER
                    EditorGUILayout.Space(6);
#else
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
#endif
                    // Loop all particles toggle (persisted)
                    bool newLoopAll =
                        EditorGUILayout.ToggleLeft("全部循环播放", ParticlePrefabPreviewSceneHelper.LoopAll);
                    if (newLoopAll != ParticlePrefabPreviewSceneHelper.LoopAll)
                    {
                        ParticlePrefabPreviewSceneHelper.LoopAll = newLoopAll;
                        ParticlePrefabPreviewSceneHelper.PersistLoopAllToConfig();
                        ParticlePrefabPreviewSceneHelper.ApplyLoopingToSpawned(newLoopAll, true);
                        SceneView.RepaintAll();
                    }

#if UNITY_2019_1_OR_NEWER
                    EditorGUILayout.Space(6);
#else
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
#endif
                    // Selection Outline toggle (reflection-based, not persisted)
                    bool outlineAvailable = EnsureSelectionOutlineProperty();
                    if (outlineAvailable)
                    {
                        bool currentOutline = GetSelectionOutline();
                        bool newOutline = EditorGUILayout.ToggleLeft("Selection Outline", currentOutline);
                        if (newOutline != currentOutline)
                        {
                            SetSelectionOutline(newOutline);
                            SceneView.RepaintAll();
                        }
                    }
                    else
                    {
                        using (new EditorGUI.DisabledScope(true))
                        {
                            EditorGUILayout.ToggleLeft("Selection Outline (当前版本不支持)", true);
                        }
                    }

                    EditorGUILayout.HelpBox(
                        "该开关不会保存到配置，且某些版本的Unity可能不支持。如需恢复，请在Scene视图的Gizmos菜单中勾选/取消 'Selection Outline'。",
                        MessageType.Info);
#if UNITY_2019_1_OR_NEWER
                    EditorGUILayout.Space(4);
#else
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
#endif
                    var newBoundary =
                        EditorGUILayout.ToggleLeft("显示间隔边框", ParticlePrefabPreviewSceneHelper.ShowBoundaries);
                    var newLabel =
                        EditorGUILayout.ToggleLeft("显示名称标签", ParticlePrefabPreviewSceneHelper.ShowLabels);
                    if (newBoundary != ParticlePrefabPreviewSceneHelper.ShowBoundaries ||
                        newLabel != ParticlePrefabPreviewSceneHelper.ShowLabels)
                    {
                        ParticlePrefabPreviewSceneHelper.ShowBoundaries = newBoundary;
                        ParticlePrefabPreviewSceneHelper.ShowLabels = newLabel;
                        ParticlePrefabPreviewSceneHelper.PersistTogglesToConfig();
                        SceneView.RepaintAll();
                    }

#if UNITY_2019_1_OR_NEWER
                    EditorGUILayout.Space(10);
#else
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
#endif
                    // Play button below all toggles
                    if (GUILayout.Button("一键播放", GUILayout.Height(28)))
                    {
                        ParticlePrefabPreviewSceneHelper.PlayAllParticles();
                    }

                    EditorGUILayout.EndScrollView();
                }

                // Push bottom toolbar to window bottom
                GUILayout.FlexibleSpace();

                // Bottom bar: page nav + End Preview on the right
                using (new EditorGUILayout.HorizontalScope(_bottomBarStyle, GUILayout.Height(42)))
                {
                    // Query page info first
#if UNITY_2019_1_OR_NEWER
                    ParticlePrefabCollectorWindow.GetPageInfo(out var cur, out var max, out var startIdx,
                        out var endIdx, out var total);
#else
                    var _v = ParticlePrefabCollectorWindow.GetPageInfo_Old();
                    int cur = _v[0];
                    int max = _v[1];
                    int startIdx = _v[2];
                    int endIdx = _v[3];
                    int total = _v[4];
#endif

                    // Show paging buttons only when more than one page (max > 0)
                    if (max > 0)
                    {
                        if (GUILayout.Button("上一页", GUILayout.Width(70)))
                        {
                            ParticlePrefabCollectorWindow.PrevPageFromControl();
                        }

                        if (GUILayout.Button("下一页", GUILayout.Width(70)))
                        {
                            ParticlePrefabCollectorWindow.NextPageFromControl();
                        }
                    }
#if UNITY_2019_1_OR_NEWER
                    GUILayout.Label($"第 {cur + 1}/{max + 1} 页 显示 {startIdx}-{endIdx}/{total}", GUILayout.Width(220));
#else
                    GUILayout.Label(string.Format("第 {0}/{1} 页 显示 {2}-{3}/{4}", cur + 1, max + 1, startIdx, endIdx, total), GUILayout.Width(220));
#endif
                    GUILayout.FlexibleSpace();
                    var prevColor = GUI.backgroundColor;
                    GUI.backgroundColor = new Color(0.86f, 0.27f, 0.27f);
                    if (GUILayout.Button("结束预览", GUILayout.Height(28), GUILayout.Width(120)))
                    {
                        EndPreviewAndClose();
                    }

                    GUI.backgroundColor = prevColor;
                }
            }
        }

        private void OnDestroy()
        {
            // Closing this window should also end preview
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            // Ensure preview end logic is invoked
            ParticlePrefabCollectorWindow.EndPreviewFromControl();
        }

        private void EndPreviewAndClose()
        {
            ParticlePrefabCollectorWindow.EndPreviewFromControl();
            Close();
        }

        private void EnsureStyles()
        {
#if UNITY_2019_1_OR_NEWER
            _panelStyle ??= new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(12, 12, 10, 10),
                margin = new RectOffset(6, 6, 0, 6)
            };

            _bottomBarStyle ??= new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 6, 6), // more top/bottom padding to center content
                margin = new RectOffset(0, 0, 0, 0)
            };
#else
            if (_panelStyle == null)
            {
                _panelStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(12, 12, 10, 10),
                    margin = new RectOffset(6, 6, 0, 6)
                };
            }
            if (_bottomBarStyle == null)
            {
                _bottomBarStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    padding = new RectOffset(10, 10, 6, 6), // more top/bottom padding to center content
                    margin = new RectOffset(0, 0, 0, 0)
                };
            }
#endif
        }

        // Reflection helpers for Selection Outline (cached)
        private static bool EnsureSelectionOutlineProperty()
        {
            if (_selectionOutlineChecked) 
                return _showSelectionOutlineProp != null;
            
            _selectionOutlineChecked = true;
            var annotationUtilityType = Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
            
            if (annotationUtilityType == null) 
                return false;
            
            _showSelectionOutlineProp = annotationUtilityType.GetProperty("showSelectionOutline",
                BindingFlags.Static | BindingFlags.NonPublic);
            return _showSelectionOutlineProp != null;
        }

        private static bool GetSelectionOutline()
        {
            if (!EnsureSelectionOutlineProperty())
            {
                return true; // fallback
            }

            try
            {
#if UNITY_2019_1_OR_NEWER
                return (bool)_showSelectionOutlineProp.GetValue(null);
#else
                return (bool)_showSelectionOutlineProp.GetValue(null, null);
#endif
            }
            catch
            {
                return true;
            }
        }

        private static void SetSelectionOutline(bool enable)
        {
            if (!EnsureSelectionOutlineProperty())
            {
                Debug.LogError("无法访问 SceneView.showSelectionOutline 属性，可能当前Unity版本已不支持，请手动在Gizmos中修改。");
                return;
            }

            try
            {
#if UNITY_2019_1_OR_NEWER
                _showSelectionOutlineProp.SetValue(null, enable);
#else
                _showSelectionOutlineProp.SetValue(null, enable, null);
#endif
            }
            catch (Exception e)
            {
#if UNITY_2019_1_OR_NEWER
                Debug.LogError($"设置 Selection Outline 失败: {e.Message}");
#else
                Debug.LogError(string.Format("设置 Selection Outline 失败: {0}", e.Message));
#endif
            }
        }
    }
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
public class SFX_RampCreate : EditorWindow
{
    Color[] colorall;
    bool isAlpha;
    public string tex2dName = "SFX_Ramp";
    public Texture tex;
    public Gradient gradient = new Gradient();
    public ParticleSystem particle;
    public TrailRenderer trail;
    public LineRenderer line;
    public string path = "还未选择保存路径";
    public string texname = "SFX_Ramp";
    public int serial = 1;
    public Vector2 resolution = new Vector2(256, 8);
    public float[] gaodus;
    private bool error = false;

    [MenuItem(EditorSettings.MENU_ITEM_RampCreate, true)]
    static bool RampCreateWindowValid()
    {
#if !UNITY_2018_3_OR_NEWER
        return false;
#endif
        return true;
    }
    [MenuItem(EditorSettings.MENU_ITEM_RampCreate, false, EditorSettings.MENU_SORT_RampCreate)]
    static void RampCreateWindow()
    {
        SFX_RampCreate window = EditorWindow.GetWindow<SFX_RampCreate>();
        window.minSize = new Vector2(350, 600);
        window.maxSize = new Vector2(550, 600);
        window.titleContent = new GUIContent("渐变图生成工具");
        window.Show();
    }
    void SetPath1()
    {
        path = EditorUtility.OpenFolderPanel("", "", "");
    }
    void GetSelectionPath()
    {
        if (Selection.objects.Length > 0)
        {
            path = System.IO.Directory.GetParent(AssetDatabase.GetAssetPath(Selection.objects[0])).ToString();
        }
        else
        {
            path = "当前选择为空!";
        }
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("生成一张新的渐变贴图");
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.Space();
#if UNITY_2018_3_OR_NEWER
        gradient = EditorGUILayout.GradientField("设置输出Gradient", gradient);
#endif
        EditorGUILayout.Space();
        isAlpha = EditorGUILayout.ToggleLeft("<< 包含Alpha通道", isAlpha);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("设置输出分辨率");
        resolution = EditorGUILayout.Vector2Field(GUIContent.none, resolution);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("设为   256*8"))
            resolution = new Vector2(256, 8);
        if (GUILayout.Button("设为   512*8"))
            resolution = new Vector2(512, 8);
        EditorGUILayout.Space();
        
        GUIStyle style = new GUIStyle("textfield");
        tex2dName = EditorGUILayout.TextField("输出文件命名：", tex2dName, style);
        serial = int.Parse(EditorGUILayout.TextField("输出文件序号：", serial.ToString(), style));
        EditorGUILayout.Space(20);
        
        EditorGUILayout.LabelField(">>当前保存路径为：");
        GUI.color = error ? new Color(1, .3f, .3f) : Color.white;
        EditorGUILayout.LabelField(path);
        GUI.color = Color.white;
        EditorGUILayout.Space();

        bool nowpath = GUILayout.Button("--获取当前选择路径--");
        if (nowpath)
        {
            GetSelectionPath();
            error = false;
        }

        bool ispath = GUILayout.Button("--手动选择保存路径--");
        if (ispath)
        {
            SetPath1();
            error = false;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("-------------------------------------------------------------------------------------------------------");
        bool shengcheng = GUILayout.Button("> 生成渐变图 <");
        if (shengcheng)
        {
            if (!Directory.Exists(path))
            {
                error = true;
                path = "还未选择保存路径或路径错误！";
            }
            else
            {
                error = false;
                OutRampTex();
            }
        }
        EditorGUILayout.LabelField("-------------------------------------------------------------------------------------------------------");
        if (GUILayout.Button("打开导出文件夹"))
        {
            Application.OpenURL("file://" + path);
        }
    }
    void OutRampTex()
    {
        colorall = new Color[(int)(resolution.x * resolution.y)];
        if (isAlpha == false)
        {
            gaodus = new float[(int)resolution.y];
            gaodus[0] = 0;
            float gao = 0;
            for (int g = 0; g < resolution.y; g++)
            {
                if (g == 0)
                {
                }
                else
                {
                    gao += resolution.x;
                    gaodus[g] = gao;
                }
            }
            for (int a = 0; a < resolution.y; a++)
            {
                for (int c = 0; c < resolution.x; c++)
                {
                    float temp = c / resolution.x;
                    colorall[(int)gaodus[a] + c] = gradient.Evaluate(temp);
                }
            }
        }
        else
        {
            gaodus = new float[(int)resolution.y];
            gaodus[0] = 0;
            float gao = 0;
            for (int g = 0; g < resolution.y; g++)
            {
                if (g == 0)
                {
                }
                else
                {
                    gao += resolution.x;
                    gaodus[g] = gao;
                }
            }
            for (int a = 0; a < resolution.y; a++)
            {
                for (int c = 0; c < resolution.x; c++)
                {
                    float temp = c / resolution.x;
                    colorall[(int)gaodus[a] + c] = gradient.Evaluate(temp);
                    colorall[(int)gaodus[a] + c].a = gradient.Evaluate(temp).a;
                }
            }
        }
        Save(colorall);
        Debug.Log(string.Format("<color=#51FF95>{0}</color>", "Ramp图已生成。  " + "名称：" + tex2dName + "_" + serial + "  保存路径：" + path));
    }
    void Save(Color[] colors)
    {
        TextureFormat _texFormat;
        if (isAlpha)
        {
            _texFormat = TextureFormat.ARGB32;
        }
        else
        {
            _texFormat = TextureFormat.RGB24;
        }
        Texture2D tex = new Texture2D((int)resolution.x, (int)resolution.y, _texFormat, false);
        tex.SetPixels(colors);
        tex.Apply();
        byte[] bytes;
        bytes = tex.EncodeToPNG();
        string sname = tex2dName + "_" + serial;
        serial += 1;
        File.WriteAllBytes(path + "/" + sname + ".png", bytes);
        AssetDatabase.Refresh();
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class AssetBundleLabel
{
    #region 自动标记
    //思路
    //1.找到Res资源文件夹
    //2.遍历Res资源文件夹，找到每个子目录
    //3.遍历每个子目录 直到找到到文件 不是文件就是文件夹就递归

    //优化
    //1.若文件名包含"A"或"P"字符，替换为全小写文件名
    //2.对不使用名为"3A_GameOver_OnlyTimeline"的lua脚本的场景，在设置标签时自动刷新lua生成的Text
    //3.可对单个场景资源设置标签
    //4.可多选场景或文件夹来设置标签
    //5.增加快捷键为Ctrl+L
    //6.取消误操作时对工程下所有场景设置标签的操作

    [MenuItem("MFramework/Set Labels %_l", false, 1)]
    public static void SetBundleLabels()
    {
        //清除所有没有用的标记
        // AssetDatabase.RemoveUnusedAssetBundleNames();
        //1.找到资源文件夹
        var dir = "Assets";
        foreach (var obj in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets))
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path))
                continue;
            if (System.IO.Directory.Exists(path))
            {
                dir = path;
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                ProcessDirectories(directoryInfo);
            }
            else if (System.IO.File.Exists(path) && (new FileInfo(path)).Extension.Equals(".unity"))
            {
                //dir = System.IO.Path.GetDirectoryName(path);
                dir = path;
                SetLabel(new FileInfo(dir));
            }
        }
        //DirectoryInfo directoryInfo = new DirectoryInfo(dir);
        //ProcessDirectories(directoryInfo);
        if (dir.Equals("Assets"))
            EditorUtility.DisplayDialog("错误", "文件选择错误", "好的");
        else
            EditorUtility.DisplayDialog("设置标签成功", "设置标签 成功", "好的");
    }

    private static void ProcessDirectories(DirectoryInfo directory)
    {
        // 打印当前目录路径
        //Debug.Log("processing folder: " + directory.FullName);

        // 遍历当前目录中的文件
        foreach (FileInfo file in directory.GetFiles())
        {
            //Debug.Log("processing file: " + file.FullName);
            if (file.Extension.Equals(".unity"))
            {
                SetLabel(file);
            }
        }

        // 遍历子目录
        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
        {
            ProcessDirectories(subDirectory); // 递归遍历子目录
        }
    }

    private static void SetLabel(FileInfo file)
    {
        // 替换场景资源名称为全小写
        //if (file.Name.Contains("A") || file.Name.Contains("P"))
        //{
        //    AssetDatabase.RenameAsset(file.FullName.Remove(0, file.FullName.IndexOf("Assets")), file.Name.ToLower());
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh();
        //}

        // lua生成txt
        //Scene scene = EditorSceneManager.OpenScene(file.FullName.Remove(0, file.FullName.IndexOf("Assets")), OpenSceneMode.Additive);
        //GameObject[] objects = scene.GetRootGameObjects();
        //for (int i = 0; i < objects.Length; i++)
        //{
        //    if (objects[i].GetComponentInChildren<MM_LuaBehaviour>() != null)
        //    {
        //        MM_LuaBehaviour mm = objects[i].GetComponentInChildren<MM_LuaBehaviour>();
        //        if (mm.LuaTextAsset != null && mm.LuaTextAsset.name.Contains("3A_GameOver_OnlyTimeline"))
        //        {
        //            EditorSceneManager.SaveOpenScenes();
        //            EditorSceneManager.CloseScene(scene, true);
        //            break;
        //        }
        //        mm.LuaTextAsset = null;
        //        I_MM_LuaBehaviour.CopyLuaToTxtWhenSetLabel(mm);
        //        EditorSceneManager.MarkSceneDirty(scene);
        //        EditorSceneManager.SaveOpenScenes();
        //        EditorSceneManager.CloseScene(scene, true);
        //        break;
        //    }
        //}

        string bundName = Path.GetFileNameWithoutExtension(file.Name);
        int idx = file.FullName.IndexOf("Assets");
        string assetPath = file.FullName.Substring(idx);

        // assetImporter.assetBundleVariant = "u3d";
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
        assetImporter.assetBundleName = bundName;
        //Debug.LogErrorFormat("added bundle name: {0} for file {1}.", bundName, assetPath);
    }
    #endregion
}
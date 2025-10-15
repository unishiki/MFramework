using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class AssetBundleLabel
{
    #region �Զ����
    //˼·
    //1.�ҵ�Res��Դ�ļ���
    //2.����Res��Դ�ļ��У��ҵ�ÿ����Ŀ¼
    //3.����ÿ����Ŀ¼ ֱ���ҵ����ļ� �����ļ������ļ��о͵ݹ�

    //�Ż�
    //1.���ļ�������"A"��"P"�ַ����滻ΪȫСд�ļ���
    //2.�Բ�ʹ����Ϊ"3A_GameOver_OnlyTimeline"��lua�ű��ĳ����������ñ�ǩʱ�Զ�ˢ��lua���ɵ�Text
    //3.�ɶԵ���������Դ���ñ�ǩ
    //4.�ɶ�ѡ�������ļ��������ñ�ǩ
    //5.���ӿ�ݼ�ΪCtrl+L
    //6.ȡ�������ʱ�Թ��������г������ñ�ǩ�Ĳ���

    [MenuItem("MFramework/Set Labels %_l", false, 1)]
    public static void SetBundleLabels()
    {
        //�������û���õı��
        // AssetDatabase.RemoveUnusedAssetBundleNames();
        //1.�ҵ���Դ�ļ���
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
            EditorUtility.DisplayDialog("����", "�ļ�ѡ�����", "�õ�");
        else
            EditorUtility.DisplayDialog("���ñ�ǩ�ɹ�", "���ñ�ǩ �ɹ�", "�õ�");
    }

    private static void ProcessDirectories(DirectoryInfo directory)
    {
        // ��ӡ��ǰĿ¼·��
        //Debug.Log("processing folder: " + directory.FullName);

        // ������ǰĿ¼�е��ļ�
        foreach (FileInfo file in directory.GetFiles())
        {
            //Debug.Log("processing file: " + file.FullName);
            if (file.Extension.Equals(".unity"))
            {
                SetLabel(file);
            }
        }

        // ������Ŀ¼
        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
        {
            ProcessDirectories(subDirectory); // �ݹ������Ŀ¼
        }
    }

    private static void SetLabel(FileInfo file)
    {
        // �滻������Դ����ΪȫСд
        //if (file.Name.Contains("A") || file.Name.Contains("P"))
        //{
        //    AssetDatabase.RenameAsset(file.FullName.Remove(0, file.FullName.IndexOf("Assets")), file.Name.ToLower());
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh();
        //}

        // lua����txt
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
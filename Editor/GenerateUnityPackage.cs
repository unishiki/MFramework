using UnityEditor;
using UnityEngine;
using System.IO;

namespace MFramework
{
    /// <summary>
    /// ���Ƶ���UnityPackageѡ��
    /// </summary>
    public class GenerateUnityPackage : MonoBehaviour
    { 
        [MenuItem("MFramework/����ѡ��/������ǰʱ��", false, 1)]
        private static void MenuClicked_CopyNowTimeToClipBoard()
        {
            GUIUtility.systemCopyBuffer = "MFramework_" + System.DateTime.Now.ToString("yyyyMMdd_HH");
        }
        [MenuItem("MFramework/����ѡ��/����UnityPackage %e", false, 2)]
        private static void MenuClicked_Export()
        {
            string assetPathName = "Assets/MFramework";
            string fileName = "MFramework_" + System.DateTime.Now.ToString("yyyyMMdd_HH") + ".unitypackage";
            AssetDatabase.ExportPackage(assetPathName, fileName, ExportPackageOptions.Recurse);
            string fileOriPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), fileName);
            string fileTarPath = Path.Combine(Application.dataPath, "MFPackageExport", fileName);
            FileProcessor.CopyFile(fileOriPath, fileTarPath, true, true);
            string openFolderPath = FileProcessor.GetFileParentPath(fileTarPath);
            if (FileProcessor.FileExists(openFolderPath))
            {
                FileProcessor.OpenInFolder(openFolderPath);
                FileProcessor.DeleteFile(fileOriPath);
            }
        }
    }
}
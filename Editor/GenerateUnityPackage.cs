using UnityEditor;
using UnityEngine;
using System.IO;

namespace MFramework
{
    /// <summary>
    /// 控制导出UnityPackage选项
    /// </summary>
    public class GenerateUnityPackage : MonoBehaviour
    { 
        [MenuItem("MFramework/导出选项/拷贝当前时间", false, 1)]
        private static void MenuClicked_CopyNowTimeToClipBoard()
        {
            GUIUtility.systemCopyBuffer = "MFramework_" + System.DateTime.Now.ToString("yyyyMMdd_HH");
        }
        [MenuItem("MFramework/导出选项/导出UnityPackage %e", false, 2)]
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
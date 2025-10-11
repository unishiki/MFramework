#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;
using System.Text;

namespace MFramework
{
	/// <summary>
	/// 文件处理器
	/// </summary>
	public static class FileProcessor
	{
		/// <summary>
		/// 拷贝文件，默认不覆盖，不自动创建目标路径
		/// </summary>
		/// <param name="oriPath"></param>
		/// <param name="tarPath"></param>
		/// <param name="overwrite"></param>
		/// <param name="autoCreatePath"></param>
		public static void CopyFile(string oriFile, string tarFile, bool overwrite = false, bool autoCreatePath = false)
		{
			var oriFilePath = Directory.GetParent(oriFile).ToString();
			var tarFilePath = Directory.GetParent(tarFile).ToString();

			if (File.Exists(oriFile))
			{
				if (Directory.Exists(tarFilePath))
				{
					try
					{
						File.Copy(oriFile, tarFile, overwrite);
#if UNITY_EDITOR
						AssetDatabase.Refresh();
#endif
						Debug.Log("拷贝成功");
						return;
					}
					catch
					{
						Debug.LogErrorFormat("拷贝失败  {0} to  {1}", oriFile, tarFile);
						return;
					}
				}
				else if (!Directory.Exists(tarFilePath) && autoCreatePath)
				{
					try
					{
						Directory.CreateDirectory(tarFilePath);
#if UNITY_EDITOR
						AssetDatabase.Refresh();
#endif
						CopyFile(oriFile, tarFile);
						return;
					}
					catch
					{
						Debug.LogError("创建失败 拷贝失败");
						return;
					}
				}
                else
                {
					Debug.LogError("目标目录不存在");
					return;
				}
			}
			else
			{
				Debug.LogErrorFormat("源目标不存在  {0}", oriFile);
				return;
			}
		}
		/// <summary>
		/// 打开目标文件夹
		/// </summary>
		/// <param name="path"></param>
		public static void OpenInFolder(string path)
		{
			Application.OpenURL("file:///" + path);
		}
		/// <summary>
		/// 判断文件是否存在
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool FileExists(string filePath)
        {
			if (Directory.Exists(filePath))
            {
				return true;
            }
			else
            {
				if (File.Exists(filePath))
                {
					return true;
                }
				return false;
            }
        }
		/// <summary>
		/// 获取文件的父目录
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetFileParentPath(string filePath)
        {
			return Directory.GetParent(filePath).ToString();
        }
		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="filePath"></param>
		public static void DeleteFile(string filePath)
        {
			if (FileExists(filePath))
            {
				File.Delete(filePath);
			}
        }
		/// <summary>
		/// 将StringBuilder写入文件
		/// </summary>
		/// <param name="sb"></param>
		/// <param name="path"></param>
		/// <param name="append"></param>
		public static void SaveTextToFile(StringBuilder sb, string path, bool append = false)
		{
			FileInfo fileinfo = new FileInfo(path);
			if (!FileExists(GetFileParentPath(path)) || fileinfo.Extension != ".txt")
            {
				return;
			}
			StreamWriter sw = new StreamWriter(path, append, new UTF8Encoding(false));
			sw.WriteLine(sb.ToString());
			sw.Close();
		}
		/// <summary>
		/// 递归获取路径下所有文件
		/// </summary>
		/// <param name="dirPath"></param>
		/// <returns></returns>
		public static FileInfo[] GetAllFilesInFolder(string dirPath)
        {
			DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
			dirPath = dirInfo.FullName.Replace("\\", "/");
			FileInfo[] files = dirInfo.GetFiles("*", SearchOption.AllDirectories);
			return files;
		}
		/// <summary>
		/// 获取文件在编辑器下的路径
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string GetAssetPathByFullName(FileInfo file)
        {
			if (!file.FullName.Contains("Assets"))
            {
				return "file not exists in Editor";

			}
			return file.FullName.Remove(0, file.FullName.IndexOf("Assets"));
		}
	}
}


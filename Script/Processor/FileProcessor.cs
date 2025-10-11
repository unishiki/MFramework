#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;
using System.Text;

namespace MFramework
{
	/// <summary>
	/// �ļ�������
	/// </summary>
	public static class FileProcessor
	{
		/// <summary>
		/// �����ļ���Ĭ�ϲ����ǣ����Զ�����Ŀ��·��
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
						Debug.Log("�����ɹ�");
						return;
					}
					catch
					{
						Debug.LogErrorFormat("����ʧ��  {0} to  {1}", oriFile, tarFile);
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
						Debug.LogError("����ʧ�� ����ʧ��");
						return;
					}
				}
                else
                {
					Debug.LogError("Ŀ��Ŀ¼������");
					return;
				}
			}
			else
			{
				Debug.LogErrorFormat("ԴĿ�겻����  {0}", oriFile);
				return;
			}
		}
		/// <summary>
		/// ��Ŀ���ļ���
		/// </summary>
		/// <param name="path"></param>
		public static void OpenInFolder(string path)
		{
			Application.OpenURL("file:///" + path);
		}
		/// <summary>
		/// �ж��ļ��Ƿ����
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
		/// ��ȡ�ļ��ĸ�Ŀ¼
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetFileParentPath(string filePath)
        {
			return Directory.GetParent(filePath).ToString();
        }
		/// <summary>
		/// ɾ���ļ�
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
		/// ��StringBuilderд���ļ�
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
		/// �ݹ��ȡ·���������ļ�
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
		/// ��ȡ�ļ��ڱ༭���µ�·��
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


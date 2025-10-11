#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SFX_TexLevels : EditorWindow
{
	[MenuItem("Assets/Shiki_Tools/Texture/调整色阶", true)]
	private static bool LevelsValid()
	{
		return Selection.objects.Length == 1 && Selection.objects[0] as Texture2D;
	}

	[MenuItem("Assets/Shiki_Tools/Texture/调整色阶", false, 16)]
	private static void Levels()
	{
		SFX_TexLevels window = EditorWindow.GetWindow<SFX_TexLevels>();
		window.titleContent = new GUIContent("调整色阶");
		window.position = new Rect(900f, 400f, 650f, 800f);
		window.minSize = new Vector2(750f, 800f);
		window.Show();
		bool flag = PlayerSettings.colorSpace == ColorSpace.Linear;
		if (flag)
		{
			Shader.SetGlobalFloat("_ColorSpaceValue", 0.45454544f);
		}
		else
		{
			Shader.SetGlobalFloat("_ColorSpaceValue", 1f);
		}
	}

	public static bool iscaidan = true;

	private string aspath = "";

	private string path = "";

	private Texture2D sourceTex0;

	private float shadow = 0f;

	private float highLight = 1f;

	private float midtone = 1f;

	private Shader shader;

	private Material mat;
	private int ser = 1;
	bool readable = false;
	TextureImporter tex;

	private void OnEnable()
	{
		sourceTex0 = Selection.objects[0] as Texture2D;
		aspath = AssetDatabase.GetAssetPath(Selection.objects[0]);
		path = GetFilePath(aspath);
		shader = Shader.Find("Shiki_Tools/SFX_TexLevels");
		mat = new Material(shader);
		ser = 1;

		tex = ModelImporter.GetAtPath(AssetDatabase.GetAssetPath(sourceTex0)) as TextureImporter;
	}
	public static string GetFilePath(string texpath)
	{
		string str = texpath.Replace("Assets", "");
		return Application.dataPath + str;
	}

	private void OnGUI()
	{
		bool flag = sourceTex0;
		if (flag)
		{
			GUILayout.Space(5f);
			GUILayout.Label("调整色阶：根据 暗区、中间调、亮区 3个滑条调整色阶  (注意：暗区值不能大于亮区值，亮区值不能小于暗区值)", new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.Label("暗区                                                                中间调                                                             亮区", new GUILayoutOption[0]);
			GUILayout.Space(1f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			shadow = GUILayout.HorizontalSlider(shadow, 0f, 1f, new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			shadow = Mathf.Clamp(shadow, 0f, highLight);
			GUILayout.Space(11f);
			midtone = GUILayout.HorizontalSlider(midtone, 0f, 9.99f, new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			GUILayout.Space(11f);
			highLight = GUILayout.HorizontalSlider(highLight, 0f, 1f, new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			});
			highLight = Mathf.Clamp(highLight, shadow, 1f);
			GUILayout.EndHorizontal();
			GUILayout.Space(15f);
			GUILayout.Label(string.Concat(new string[]
			{
				"暗区当前值：",
				shadow.ToString(),
				",最小值0 最大值",
				highLight.ToString(),
				"    |||      中间调当前值",
				midtone.ToString(),
				",最小值0  最大值10    |||       亮区当前值",
				highLight.ToString(),
				" 最小值",
				shadow.ToString(),
				" 最大值1"
			}), new GUILayoutOption[0]);
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			bool flag2 = GUILayout.Button("覆盖保存！", new GUILayoutOption[]
			{
				GUILayout.Width(100f),
				GUILayout.Height(40f)
			});
			if (flag2)
			{
				Save();
			}
			GUILayout.Space(10f);
			bool flag5 = GUILayout.Button("原址另存！", new GUILayoutOption[]
			{
				GUILayout.Width(100f),
				GUILayout.Height(40f)
			});
			if (flag5)
			{
				Save2();
			}


			GUILayout.Space(10f);
			GUILayout.Space(50f);
			bool flag3 = GUILayout.Button("重置到默认值！", new GUILayoutOption[]
			{
				GUILayout.Width(100f),
				GUILayout.Height(40f)
			});
			if (flag3)
			{
				shadow = 0f;
				midtone = 1f;
				highLight = 1f;
			}
			GUILayout.EndHorizontal();
			float width = base.position.width;
			float height = base.position.height;
			float num = Mathf.Max(width - 20f, 0f);
			float num2 = Mathf.Max(height - 150f, 0f);
			int width2 = sourceTex0.width;
			int height2 = sourceTex0.height;
			float num3 = (float)width2 / (float)height2;
			float num4 = num / num2;
			bool flag4 = num3 > num4;
			float num5;
			float num6;
			if (flag4)
			{
				num5 = num;
				num6 = num5 / num3;
			}
			else
			{
				num6 = num2;
				num5 = num6 * num3;
			}
			Rect rect = new Rect(20f, 150f, num5, num6);
			mat.SetTexture("_MainTex", sourceTex0);
			mat.SetFloat("_shadow", shadow);
			mat.SetFloat("_Midtone", midtone);
			mat.SetFloat("_highLight", highLight);
			EditorGUI.DrawPreviewTexture(rect, sourceTex0, mat);
		}
	}
	private void Save2()
    {
		path = path.Insert(path.Length - 4, "_NewLevel_" + ser);
		ser++;
		Save();
	}
	private void Save()
	{
		if (!tex.isReadable)
		{
			readable = true;
			tex.isReadable = true;
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(sourceTex0));
		}

		Color[] array = new Color[0];
		array = sourceTex0.GetPixels();
		for (int i = 0; i < array.Length; i++)
		{
			float a = array[i].a;
			array[i].r = Mathf.Clamp01(Mathf.Pow(Mathf.Max(array[i].r - shadow, 0f) / (highLight - shadow), 1f / midtone));
			array[i].g = Mathf.Clamp01(Mathf.Pow(Mathf.Max(array[i].g - shadow, 0f) / (highLight - shadow), 1f / midtone));
			array[i].b = Mathf.Clamp01(Mathf.Pow(Mathf.Max(array[i].b - shadow, 0f) / (highLight - shadow), 1f / midtone));
			array[i].a = a;
		}
		TextureFormat textureFormat = TextureFormat.ARGB32;
		Texture2D texture2D = new Texture2D(sourceTex0.width, sourceTex0.height, textureFormat, false);
		texture2D.SetPixels(array);
		texture2D.Apply();

		
		string[] array2 = path.Split(new char[]
		{
			'.'
		});
		bool flag = array2[array2.Length - 1] == "png";
		byte[] bytes;
		if (flag)
		{
			bytes = ImageConversion.EncodeToPNG(texture2D);
		}
		else
		{
			bool flag2 = array2[array2.Length - 1] == "jpg";
			if (flag2)
			{
				bytes = ImageConversion.EncodeToJPG(texture2D);
			}
			else
			{
				bool flag3 = array2[array2.Length - 1] == "tga";
				if (flag3)
				{
#if UNITY_2018_3_OR_NEWER
					bytes = ImageConversion.EncodeToTGA(texture2D);
#else
					return;
#endif
				}
				else
				{
					bytes = ImageConversion.EncodeToPNG(texture2D);
				}
			}
		}

		if (readable)
		{
			tex.isReadable = false;
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(sourceTex0));
		}

		File.WriteAllBytes(path, bytes);
		path = AssetDatabase.GetAssetPath(sourceTex0);
		AssetDatabase.Refresh();
		string str = ColorUtility.ToHtmlStringRGB(new Color(0f, 1f, 1f, 1f));
	}
}
#endif
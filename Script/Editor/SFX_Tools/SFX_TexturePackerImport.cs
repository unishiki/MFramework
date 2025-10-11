#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SFX_TexturePackerImport : MonoBehaviour
{
	public static class TexturePackerImport
	{

		[MenuItem("Assets/Shiki_Tools/Texture/图集拆分", false, 201)]
		static void ProcessToSprite()
		{
			TextAsset txt = (TextAsset)Selection.activeObject;

			string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(txt));
			TexturePacker.MetaData meta = TexturePacker.GetMetaData(txt.text);
			string texturePath = rootPath + "/" + meta.image;

			List<SpriteMetaData> sprites = TexturePacker.ProcessToSprites(txt.text);

			string path = rootPath + "/" + meta.image;
			TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
			texImp.spritesheet = sprites.ToArray();
			texImp.textureType = TextureImporterType.Sprite;
			texImp.spriteImportMode = SpriteImportMode.Multiple;

			//TextureImporterSettings texSettings = new TextureImporterSettings();
			//texImp.ReadTextureSettings(texSettings);
			//texSettings.spriteAlignment = (int)SpriteAlignment.Custom;
			//texImp.SetTextureSettings(texSettings);
			//texImp.spritePivot =;

			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

		}
	}

	[MenuItem("Assets/Shiki_Tools/Texture/图集拆分", true)]
	static bool ValidateProcessTexturePacker()
	{
		Object o = Selection.activeObject;

		if (o == null)
			return false;

		if (o.GetType() == typeof(TextAsset))
		{
			return (((TextAsset)o).text.hashtableFromJson()).IsTexturePackerTable();
		}

		return false;

	}
}


public static class TexturePackerExtensions
{
	public static Rect TPHashtableToRect(this Hashtable table)
	{
		return new Rect(float.Parse(table["x"].ToString()), float.Parse(table["y"].ToString()), float.Parse(table["w"].ToString()), float.Parse(table["h"].ToString()));
	}

	public static Vector2 TPHashtableToVector2(this Hashtable table)
	{
		if (table.ContainsKey("x") && table.ContainsKey("y"))
		{
			return new Vector2(float.Parse(table["x"].ToString()), float.Parse(table["y"].ToString()));
		}
		else
		{
			return new Vector2(float.Parse(table["w"].ToString()), float.Parse(table["h"].ToString()));
		}
	}

	public static Vector2 TPVector3toVector2(this Vector3 vec)
	{
		return new Vector2(vec.x, vec.y);
	}

	public static bool IsTexturePackerTable(this Hashtable table)
	{
		if (table == null) return false;

		if (table.ContainsKey("meta"))
		{
			Hashtable metaTable = (Hashtable)table["meta"];
			if (metaTable.ContainsKey("app"))
			{
				return true;
			}
		}

		return false;
	}
}

public class TexturePacker
{
	public class PackedFrame
	{
		public string name;
		public Rect frame;
		public Rect spriteSourceSize;
		public Vector2 sourceSize;
		public bool rotated;
		public bool trimmed;
		Vector2 atlasSize;

		public PackedFrame(string name, Vector2 atlasSize, Hashtable table)
		{
			this.name = name;
			this.atlasSize = atlasSize;

			frame = ((Hashtable)table["frame"]).TPHashtableToRect();
			spriteSourceSize = ((Hashtable)table["spriteSourceSize"]).TPHashtableToRect();
			sourceSize = ((Hashtable)table["sourceSize"]).TPHashtableToVector2();
			rotated = (bool)table["rotated"];
			trimmed = (bool)table["trimmed"];
		}

		public Mesh BuildBasicMesh(float scale, Color32 defaultColor)
		{
			return BuildBasicMesh(scale, defaultColor, Quaternion.identity);
		}

		public Mesh BuildBasicMesh(float scale, Color32 defaultColor, Quaternion rotation)
		{
			Mesh m = new Mesh();
			Vector3[] verts = new Vector3[4];
			Vector2[] uvs = new Vector2[4];
			Color32[] colors = new Color32[4];


			if (!rotated)
			{
				verts[0] = new Vector3(frame.x, frame.y, 0);
				verts[1] = new Vector3(frame.x, frame.y + frame.height, 0);
				verts[2] = new Vector3(frame.x + frame.width, frame.y + frame.height, 0);
				verts[3] = new Vector3(frame.x + frame.width, frame.y, 0);
			}
			else
			{
				verts[0] = new Vector3(frame.x, frame.y, 0);
				verts[1] = new Vector3(frame.x, frame.y + frame.width, 0);
				verts[2] = new Vector3(frame.x + frame.height, frame.y + frame.width, 0);
				verts[3] = new Vector3(frame.x + frame.height, frame.y, 0);
			}




			uvs[0] = verts[0].TPVector3toVector2();
			uvs[1] = verts[1].TPVector3toVector2();
			uvs[2] = verts[2].TPVector3toVector2();
			uvs[3] = verts[3].TPVector3toVector2();

			for (int i = 0; i < uvs.Length; i++)
			{
				uvs[i].x /= atlasSize.x;
				uvs[i].y /= atlasSize.y;
				uvs[i].y = 1.0f - uvs[i].y;
			}

			if (rotated)
			{
				verts[3] = new Vector3(frame.x, frame.y, 0);
				verts[0] = new Vector3(frame.x, frame.y + frame.height, 0);
				verts[1] = new Vector3(frame.x + frame.width, frame.y + frame.height, 0);
				verts[2] = new Vector3(frame.x + frame.width, frame.y, 0);
			}


			//v-flip
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i].y = atlasSize.y - verts[i].y;
			}

			//original origin
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i].x -= frame.x - spriteSourceSize.x + (sourceSize.x / 2.0f);
				verts[i].y -= (atlasSize.y - frame.y) - (sourceSize.y - spriteSourceSize.y) + (sourceSize.y / 2.0f);
			}

			//scaler
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i] *= scale;
			}

			//rotator
			if (rotation != Quaternion.identity)
			{
				for (int i = 0; i < verts.Length; i++)
				{
					verts[i] = rotation * verts[i];
				}
			}

			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = defaultColor;
			}


			m.vertices = verts;
			m.uv = uvs;
			m.colors32 = colors;
			m.triangles = new int[6] { 0, 3, 1, 1, 3, 2 };

			m.RecalculateNormals();
			m.RecalculateBounds();
			m.name = name;

			return m;
		}

		public SpriteMetaData BuildBasicSprite(float scale, Color32 defaultColor)
		{
			SpriteMetaData smd = new SpriteMetaData();
			Rect rect;

			if (!rotated)
			{
				rect = this.frame;
			}
			else
			{
				rect = new Rect(frame.x, frame.y, frame.height, frame.width);
			}


			/* Look if frame is outside from texture */

			if ((frame.x + rect.width) > atlasSize.x || (frame.y + rect.height) > atlasSize.y ||
				(frame.x < 0 || frame.y < 0))
			{
				Debug.Log(this.name + " is outside from texture! Sprite is ignored!");
				smd.name = "IGNORE_SPRITE";
				return smd;

			}
			//calculate Height 
			/* Example: Texture: 1000 Width x 500 height 
		 	 * Sprite.Recht(0,0,100,100) --> Sprite is on the bottom left
			 */

			rect.y = atlasSize.y - frame.y - rect.height;
			smd.rect = rect;
			smd.alignment = 0;
			smd.name = name;
			smd.pivot = frame.center;
			return smd;
		}
	}

	public class MetaData
	{
		public string image;
		public string format;
		public Vector2 size;
		public float scale;
		public string smartUpdate;

		public MetaData(Hashtable table)
		{
			image = (string)table["image"];
			format = (string)table["format"];
			size = ((Hashtable)table["size"]).TPHashtableToVector2();
			scale = float.Parse(table["scale"].ToString());
			smartUpdate = (string)table["smartUpdate"];
		}
	}

	public static List<SpriteMetaData> ProcessToSprites(string text)
	{
		Hashtable table = text.hashtableFromJson();
		MetaData meta = new MetaData((Hashtable)table["meta"]);
		List<PackedFrame> frames = new List<PackedFrame>();
		Hashtable frameTable = (Hashtable)table["frames"];

		foreach (DictionaryEntry entry in frameTable)
		{
			frames.Add(new PackedFrame((string)entry.Key, meta.size, (Hashtable)entry.Value));
		}

		List<SpriteMetaData> sprites = new List<SpriteMetaData>();
		for (int i = 0; i < frames.Count; i++)
		{
			SpriteMetaData smd = frames[i].BuildBasicSprite(0.01f, new Color32(128, 128, 128, 128));
			Debug.Log(smd.pivot);
			if (!smd.name.Equals("IGNORE_SPRITE"))
				sprites.Add(smd);
		}

		return sprites;

	}

	public static Mesh[] ProcessToMeshes(string text)
	{
		return ProcessToMeshes(text, Quaternion.identity);
	}

	public static Mesh[] ProcessToMeshes(string text, Quaternion rotation)
	{
		Hashtable table = text.hashtableFromJson();

		MetaData meta = new MetaData((Hashtable)table["meta"]);

		List<PackedFrame> frames = new List<PackedFrame>();
		Hashtable frameTable = (Hashtable)table["frames"];

		foreach (DictionaryEntry entry in frameTable)
		{
			frames.Add(new PackedFrame((string)entry.Key, meta.size, (Hashtable)entry.Value));
		}

		List<Mesh> meshes = new List<Mesh>();
		for (int i = 0; i < frames.Count; i++)
		{
			meshes.Add(frames[i].BuildBasicMesh(0.01f, new Color32(128, 128, 128, 128), rotation));
		}

		return meshes.ToArray();
	}

	public static MetaData GetMetaData(string text)
	{
		Hashtable table = text.hashtableFromJson();
		MetaData meta = new MetaData((Hashtable)table["meta"]);

		return meta;
	}
}
#endif
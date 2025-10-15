#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SFX_Tex2DTools : MonoBehaviour
{
    [MenuItem(EditorSettings.MENU_ITEM_Grayscale1, true)]
    private static bool Grayscale1Valid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_Grayscale1, false, EditorSettings.MENU_SORT_Grayscale1)]
    static void Grayscale1()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            
            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadbable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (tex.isReadable == false)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadbable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                Vector3 rgb = new Vector3(colorall[i].r, colorall[i].g, colorall[i].b);
                float max1 = Mathf.Max(rgb.x, rgb.y, rgb.z);
                colorall[i].r = max1;
                colorall[i].g = max1;
                colorall[i].b = max1;
            }
            string name1 = origTex.name + "_Grayscale_Max";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadbable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }

    [MenuItem(EditorSettings.MENU_ITEM_Grayscale2, true)]
    private static bool Grayscale2Valid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_Grayscale2, false, EditorSettings.MENU_SORT_Grayscale2)]
    static void Grayscale2()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }

            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (tex.isReadable == false)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                Vector3 rgb = new Vector3(colorall[i].r, colorall[i].g, colorall[i].b);
                float max1 = rgb.x * 0.33333f + rgb.y * 0.33333f + rgb.z * 0.33333f;
                colorall[i].r = max1;
                colorall[i].g = max1;
                colorall[i].b = max1;
            }
            string name1 = origTex.name + "_Grayscale_RGB333";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }

    [MenuItem(EditorSettings.MENU_ITEM_Grayscale3, true)]
    private static bool Grayscale3Valid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_Grayscale3, false, EditorSettings.MENU_SORT_Grayscale3)]
    static void Grayscale3()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }

            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (tex.isReadable == false)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                float rr = colorall[i].r;
                float gg = colorall[i].g;
                float bb = colorall[i].b;
                float Gray = 0;
                Gray = rr * 0.299f + gg * 0.587f + bb * 0.114f;
                colorall[i].r = Gray;
                colorall[i].g = Gray;
                colorall[i].b = Gray;
            }
            string name1 = origTex.name + "_Grayscale_Normal";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }

    [MenuItem(EditorSettings.MENU_ITEM_DeBlack1, true)]
    private static bool DeBlack1Valid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_DeBlack1, false, EditorSettings.MENU_SORT_DeBlack1)]
    static void DeBlack1()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }

            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                Vector3 rgb = new Vector3(colorall[i].r, colorall[i].g, colorall[i].b);
                float max2 = Mathf.Max(rgb.x, rgb.y, rgb.z);
                colorall[i].r = 1;
                colorall[i].g = 1;
                colorall[i].b = 1;
                colorall[i].a = max2;
            }
            string name1 = origTex.name + "_DeBlack_w";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }

    [MenuItem(EditorSettings.MENU_ITEM_DeBlack2, true)]
    private static bool DeBlack2Valid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_DeBlack2, false, EditorSettings.MENU_SORT_DeBlack2)]
    static void DeBlack2()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }

            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                if (colorall[i].r + colorall[i].g + colorall[i].b == 0)
                {
                    colorall[i].a = 0;
                }
            }
            string name1 = origTex.name + "_DeBlack";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_DeBlack3, true)]
    private static bool DeBlack3Valid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_DeBlack3, false, EditorSettings.MENU_SORT_DeBlack3)]
    static void DeBlack3()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }

            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                if (colorall[i].r + colorall[i].g + colorall[i].b == 3)
                {
                    colorall[i].a = 0;
                }
            }
            string name1 = origTex.name + "_DeBlack";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColor, true)]
    private static bool InvertColorValid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColor, false, EditorSettings.MENU_SORT_InvertColor)]
    static void InvertColor()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                colorall[i].r = 1 - colorall[i].r;
                colorall[i].g = 1 - colorall[i].g;
                colorall[i].b = 1 - colorall[i].b;
            }
            string name1 = origTex.name + "_InvertColor";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColorR, true)]
    private static bool InvertColorRValid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColorR, false, EditorSettings.MENU_SORT_InvertColorR)]
    static void InvertColorR()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                colorall[i].r = 1 - colorall[i].r;
            }
            string name1 = origTex.name + "_InvertColorR";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColorG, true)]
    private static bool InvertColorGValid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColorG, false, EditorSettings.MENU_SORT_InvertColorG)]
    static void InvertColorG()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                colorall[i].g = 1 - colorall[i].g;
            }
            string name1 = origTex.name + "_InvertColorG";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColorB, true)]
    private static bool InvertColorBValid()
    {
        if (Selection.objects.Length < 1)
            return false;
        else
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                if (Selection.objects[i] as Texture2D) return true;
            }
            return false;
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_InvertColorB, false, EditorSettings.MENU_SORT_InvertColorB)]
    static void InvertColorB()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            bool unreadable = false;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = ModelImporter.GetAtPath(path1) as TextureImporter;
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] colorall = origTex.GetPixels();
            for (int i = 0; i < colorall.Length; i++)
            {
                colorall[i].b = 1 - colorall[i].b;
            }
            string name1 = origTex.name + "_InvertColorB";
            Vector2 size = new Vector2(origTex.width, origTex.height);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
            Save(colorall, size, name1, path1, tex);
        }
    }



    static void Save(Color[] colors, Vector2 size, string name1, string path1, TextureImporter ttt)
    {
        TextureFormat _texFormat;
        _texFormat = TextureFormat.ARGB32;
        Texture2D tex = new Texture2D((int)size.x, (int)size.y, _texFormat, false);
        tex.SetPixels(colors);
        tex.Apply();
        byte[] bytes;
        bytes = tex.EncodeToPNG();
        string sname = name1;
        string dataPath = GetAbsDataPath();
        string savePath = GetAbsPath(path1);
        File.WriteAllBytes((dataPath + savePath) + "/" + sname + ".png", bytes);
        AssetDatabase.Refresh();
        TextureImporter tex1 = ModelImporter.GetAtPath(savePath + "/" + sname + ".png") as TextureImporter;
        tex1.isReadable = true;
        tex1.textureType = ttt.textureType;
        tex1.alphaSource = TextureImporterAlphaSource.FromInput;
        tex1.alphaIsTransparency = true;
        tex1.isReadable = false;
        AssetDatabase.ImportAsset(savePath + "/" + sname + ".png");
        AssetDatabase.Refresh();
        Debug.Log(string.Format("<color=#51FF95>{0}</color>", "图片已完成转换。本次输出新文件：" + sname + ".png"));
    }

    static string GetAbsDataPath()
    {
        string patht = Application.dataPath;
        string[] split = patht.Split('/');
        string newpath = "";
        for (int i = 0; i < split.Length; i++)
        {
            if (i != split.Length - 1)
            {
                if (i != 0)
                {
                    newpath = newpath + "/" + split[i];
                }
                if (i == 0)
                {
                    newpath = newpath + split[i];
                }
            }
        }
        newpath = newpath + "/";
        return newpath;
    }
    static string GetAbsPath(string patht)
    {
        string[] split = patht.Split('/');
        string newpath = "";
        for (int i = 0; i < split.Length; i++)
        {
            if (i != split.Length - 1)
            {
                if (i != 0)
                {
                    newpath = newpath + "/" + split[i];
                }
                if (i == 0)
                {
                    newpath = "" + split[i];
                }
            }
        }
        return newpath;
    }
}
#endif
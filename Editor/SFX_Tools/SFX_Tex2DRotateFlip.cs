#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SFX_Tex2DRotateFlip : Editor
{
    static int IsFormat(string path)
    {
        string[] split = path.Split('.');
        int size = split.Length -1;
        int format = 0;
        if (split[size] == "jpg" || split[size] == "JPG")
        {
            format = 0;
        }
        else if (split[size] == "png" || split[size] == "PNG")
        {
            format = 1;
        }
        else if (split[size] == "TGA" || split[size] == "tga")
        {
            format = 2;
        }
        else
        {
            format = 1; // 如果没有合适格式则输出Png
        }
        return format;
    }
    [MenuItem(EditorSettings.MENU_ITEM_TexRotatePlus90, true)]
    private static bool Shun90Valid()
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
    [MenuItem(EditorSettings.MENU_ITEM_TexRotatePlus90, false, EditorSettings.MENU_SORT_TexRotatePlus90)]
    static void Shun90()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }

            Texture2D origTex = Selection.objects[k] as Texture2D;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = TextureImporter.GetAtPath(path1) as TextureImporter;
            bool unreadable = false;
            int format;
            format = IsFormat(path1);
            if (!tex.isReadable)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] endcolor = new Color[(int)(origTex.height * origTex.width)];
            Texture2D rotateTex = new Texture2D(origTex.height, origTex.width, TextureFormat.RGBAFloat, false);
            int times = 0;
            for (int i = 0; i < origTex.width; i++)
            {
                for (int j = 0; j < origTex.height; j++)
                {
                    endcolor[times] = origTex.GetPixel((origTex.width - i) - 1, j);
                    times++;
                }
            }
            rotateTex.SetPixels(endcolor);
            rotateTex.Apply();
            Save(endcolor, new Vector2(rotateTex.width, rotateTex.height), origTex.name, path1, tex, format);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_TexRotateMinus90, true)]
    private static bool Ni90Valid()
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
    [MenuItem(EditorSettings.MENU_ITEM_TexRotateMinus90, false, EditorSettings.MENU_SORT_TexRotateMinus90)]
    static void Ni90()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = TextureImporter.GetAtPath(path1) as TextureImporter;
            bool unreadable = false;
            int geshi;
            geshi = IsFormat(path1);
            if (tex.isReadable == false)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] endcolor = new Color[(int)(origTex.height * origTex.width)];
            Texture2D rotateTex = new Texture2D(origTex.height, origTex.width, TextureFormat.RGBAFloat, false);
            int times = 0;
            for (int i = 0; i < origTex.width; i++)
            {
                for (int j = 0; j < origTex.height; j++)
                {
                    endcolor[times] = origTex.GetPixel(i, (origTex.height - j) - 1);
                    times++;
                }
            }
            rotateTex.SetPixels(endcolor);
            rotateTex.Apply();
            Save(endcolor, new Vector2(rotateTex.width, rotateTex.height), origTex.name, path1, tex, geshi);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
        }
    }

    [MenuItem(EditorSettings.MENU_ITEM_TexRotate180, true)]
    private static bool Rot180Valid()
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
    [MenuItem(EditorSettings.MENU_ITEM_TexRotate180, false, EditorSettings.MENU_SORT_TexRotate180)]
    static void Rot180()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = TextureImporter.GetAtPath(path1) as TextureImporter;
            bool unreadable = false;
            int format;
            format = IsFormat(path1);
            if (tex.isReadable == false)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] endcolor = new Color[(int)(origTex.height * origTex.width)];
            Color[] lowcolor = new Color[(int)(origTex.height * origTex.width)];
            lowcolor = origTex.GetPixels();
            int maxTimes = origTex.height * origTex.width;
            maxTimes -= 1;
            Texture2D rotateTex = new Texture2D(origTex.width, origTex.height, TextureFormat.RGBAFloat, false);
            int times = 0;
            for (int i = 0; i < origTex.width; i++)
            {
                for (int j = 0; j < origTex.height; j++)
                {
                    endcolor[times] = lowcolor[maxTimes];
                    times += 1;
                    maxTimes -= 1;
                }
            }
            rotateTex.SetPixels(endcolor);
            rotateTex.Apply();
            Save(endcolor, new Vector2(rotateTex.width, rotateTex.height), origTex.name, path1, tex, format);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_FlipHorizontal, true)]
    private static bool FlipHorizontalValid()
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
    [MenuItem(EditorSettings.MENU_ITEM_FlipHorizontal, false, EditorSettings.MENU_SORT_FlipHorizontal)]
    static void FlipHorizontal()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = TextureImporter.GetAtPath(path1) as TextureImporter;
            bool unreadable = false;
            int format;
            format = IsFormat(path1);
            if (tex.isReadable == false)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] endcolor = new Color[(int)(origTex.width * origTex.height)];
            Texture2D rotateTex = new Texture2D(origTex.width, origTex.height, TextureFormat.RGBAFloat, false);
            int times = 0;
            for (int j = 0; j < origTex.height; j++)
            {
                for (int i = 0; i < origTex.width; i++)
                {
                    endcolor[times] = origTex.GetPixel((origTex.width - i) - 1, j);
                    times += 1;
                }
            }
            rotateTex.SetPixels(endcolor);
            rotateTex.Apply();
            Save(endcolor, new Vector2(rotateTex.width, rotateTex.height), origTex.name, path1, tex, format);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
        }
    }
    [MenuItem(EditorSettings.MENU_ITEM_FlipVertical, true)]
    private static bool FlipVerticalValid()
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
    [MenuItem(EditorSettings.MENU_ITEM_FlipVertical, false, EditorSettings.MENU_SORT_FlipVertical)]
    static void FlipVertical()
    {
        for (int k = 0; k < Selection.objects.Length; k++)
        {
            if (!(Selection.objects[k] as Texture2D))
            {
                continue;
            }
            Texture2D origTex = Selection.objects[k] as Texture2D;
            string path1 = AssetDatabase.GetAssetPath(origTex);
            TextureImporter tex = TextureImporter.GetAtPath(path1) as TextureImporter;
            bool unreadable = false;
            int format;
            format = IsFormat(path1);
            if (tex.isReadable == false)
            {
                tex.isReadable = true;
                AssetDatabase.ImportAsset(path1);
                unreadable = true;
            }
            Color[] endcolor = new Color[(int)(origTex.width * origTex.height)];
            Texture2D rotateTex = new Texture2D(origTex.width, origTex.height, TextureFormat.RGBAFloat, false);
            int times = 0;
            for (int j = 0; j < origTex.height; j++)
            {
                for (int i = 0; i < origTex.width; i++)
                {
                    endcolor[times] = origTex.GetPixel(i, origTex.height - j - 1);
                    times++;
                }
            }
            rotateTex.SetPixels(endcolor);
            rotateTex.Apply();
            Save(endcolor, new Vector2(rotateTex.width, rotateTex.height), origTex.name, path1, tex, format);
            if (unreadable)
            {
                tex.isReadable = false;
                AssetDatabase.ImportAsset(path1);
            }
            AssetDatabase.ImportAsset(path1);
        }
    }
    static void Save(Color[] colors, Vector2 size, string name1, string path1, TextureImporter ttt ,int format)
    {
        TextureFormat texformat;
        if (format == 0)
        {
            texformat = TextureFormat.RGB24;
        }
        else if (format == 1)
        {
            texformat = TextureFormat.RGBAFloat;
        }
        else if (format == 2)
        {
            texformat = TextureFormat.RGBAFloat;
        }
        else
        {
            texformat = TextureFormat.RGBAFloat;
        }
        Texture2D tex = new Texture2D((int)size.x, (int)size.y, texformat, false);
        tex.SetPixels(colors);
        tex.Apply();
        byte[] bytes;
        string suffix;
        if (format == 0) 
        {
            bytes = tex.EncodeToJPG(100);
            suffix = ".jpg";
        }
        else if (format == 1) 
        {
            bytes = tex.EncodeToPNG();
            suffix = ".png";
        }
        else if (format == 2)
        {
            bytes = tex.EncodeToTGA();
            suffix = ".tga";
        }
        else 
        {
            bytes = tex.EncodeToPNG();
            suffix = ".png";
        }
        string dataPathTemp = GetAbsDataPath();
        string savePath = GetAbsPath(path1);
        File.WriteAllBytes((dataPathTemp + savePath) + "/" + name1 + suffix, bytes);
        AssetDatabase.Refresh();
        Debug.Log(string.Format("<color=#4CFFB3>{0}</color>", "保存成功！" + "  本次输出新文件：" + name1 + suffix));
        AssetDatabase.Refresh();
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
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public enum ColorChannel
{
    R,G,B,A
}
public class SFX_TexMerge : EditorWindow
{
    Color[] colorall;
    Color[] colorr;
    Color[] colorg;
    Color[] colorb;
    Color[] colora;
    Texture2D texAll;
    Texture2D rr;
    Texture2D gg;
    Texture2D bb;
    Texture2D aa;
    public string tex2dName = "SFX_Merge";
    public Texture2D tex_in_1;
    public Texture2D tex_in_2;
    public Texture2D tex_in_3;
    public Texture2D tex_in_4;
    public ColorChannel tex_channel_1;
    public ColorChannel tex_channel_2;
    public ColorChannel tex_channel_3;
    public ColorChannel tex_channel_4;
    public bool isAlpha = false;
    public bool isBlackWhite = false;


    public string path = "还未选择保存路径";
    public string texname = "SFX_Merge";
    public int serial = 1;
    public Vector2 resolution = new Vector2(256, 256);
    public float[] gaodus;
    private bool error = false;

    [MenuItem(EditorSettings.MENU_ITEM_TexMergeCreate, true)]
    static bool TexMergeCreateWindowValid()
    {
        return true;
    }
    [MenuItem(EditorSettings.MENU_ITEM_TexMergeCreate, false, EditorSettings.MENU_SORT_TexMergeCreate)]
    static void TexMergeCreateWindow()
    {
        SFX_TexMerge window = EditorWindow.GetWindow<SFX_TexMerge>();
        window.minSize = new Vector2(550, 1250);
        window.maxSize = new Vector2(550, 1250);
        window.titleContent = new GUIContent("贴图合并工具");
        window.Show();
    }
    void SetPath1()
    {
        path = EditorUtility.OpenFolderPanel("", "", "");
    }
    void GetSelectionPath()
    {
        if (Selection.objects.Length > 0)
        {
            try
            {
                path = System.IO.Directory.GetParent(AssetDatabase.GetAssetPath(Selection.objects[0])).ToString();
                error = false;
            }
            catch
            {
                path = "当前选择错误!";
                error = true;
            }
        }
        else
        {
            path = "当前选择为空!";
            error = true;
        }
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("放入对应通道的图像");
        GUILayout.Label("（每张图将成为最终图像的一个通道）");
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.Space();
        tex_in_1 = (Texture2D)EditorGUILayout.ObjectField("Tex R:", tex_in_1, typeof(Texture2D), false);
        tex_channel_1 = (ColorChannel)EditorGUILayout.EnumPopup("此图选取通道", tex_channel_1);
        EditorGUILayout.Space();
        tex_in_2 = (Texture2D)EditorGUILayout.ObjectField("Tex G:", tex_in_2, typeof(Texture2D), false);
        tex_channel_2 = (ColorChannel)EditorGUILayout.EnumPopup("此图选取通道", tex_channel_2);
        EditorGUILayout.Space();
        tex_in_3 = (Texture2D)EditorGUILayout.ObjectField("Tex B:", tex_in_3, typeof(Texture2D), false);
        tex_channel_3 = (ColorChannel)EditorGUILayout.EnumPopup("此图选取通道", tex_channel_3);
        EditorGUILayout.Space();
        
        isAlpha = EditorGUILayout.ToggleLeft("<< 包含Alpha通道", isAlpha);
        if (isAlpha)
        {
            tex_in_4 = (Texture2D)EditorGUILayout.ObjectField("Tex A:", tex_in_4, typeof(Texture2D), false);
            tex_channel_4 = (ColorChannel)EditorGUILayout.EnumPopup("此图选取通道", tex_channel_4);
            EditorGUILayout.Space();
        }
        isBlackWhite = EditorGUILayout.ToggleLeft("<< 输出为黑白图", isBlackWhite);
        EditorGUILayout.Space(30);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("设置输出分辨率");
        resolution = EditorGUILayout.Vector2Field(GUIContent.none, resolution);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("设为   256*256"))
            resolution = new Vector2(256, 256);
        if (GUILayout.Button("设为   512*512"))
            resolution = new Vector2(512, 512);
        if (GUILayout.Button("设为   第一张图分辨率"))
        {
            if (tex_in_1 != null)
                resolution = new Vector2(tex_in_1.width, tex_in_1.height);
            else if (tex_in_2 != null)
                resolution = new Vector2(tex_in_2.width, tex_in_2.height);
            else if (tex_in_3 != null)
                resolution = new Vector2(tex_in_3.width, tex_in_3.height);
            else if (tex_in_4 != null)
                resolution = new Vector2(tex_in_4.width, tex_in_4.height);
            else
                resolution = new Vector2(256, 256);
        }
        EditorGUILayout.Space(30);
        
        GUIStyle style = new GUIStyle("textfield");
        tex2dName = EditorGUILayout.TextField("输出文件命名：", tex2dName, style);
        serial = int.Parse(EditorGUILayout.TextField("输出文件序号：", serial.ToString(), style));
        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField(">>当前保存路径为：");
        GUI.color = error ? new Color(1, .3f, .3f) : Color.white;
        EditorGUILayout.LabelField(path);
        GUI.color = Color.white;
        EditorGUILayout.Space();

        bool nowpath = GUILayout.Button("--获取当前选择路径--");
        if (nowpath)
        {
            error = false;
            GetSelectionPath();
        }

        bool ispath = GUILayout.Button("--手动选择保存路径--");
        if (ispath)
        {
            SetPath1();
            error = false;
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("-------------------------------------------------------------------------------------------------------");
        bool shengcheng = GUILayout.Button("> 生成合并图 <");
        if (shengcheng)
        {
            if (!Directory.Exists(path))
            {
                error = true;
                path = "还未选择保存路径或路径错误！";
            }
            else if (tex_in_1 == null && tex_in_2 == null && tex_in_3 == null && ((tex_in_4 == null && isAlpha) || !isAlpha))
            {
                error = true;
                path = "还未选择贴图资源!";
            }
            else
            {
                error = false;
                OutMergeTex();
                Save(colorall);
                Debug.Log(string.Format("<color=#51FF95>{0}</color>", "Merge图已生成。  " + "名称：" + tex2dName + "_" + serial + "  保存路径：" + path));
            }
        }
        EditorGUILayout.LabelField("-------------------------------------------------------------------------------------------------------");
        if (GUILayout.Button("打开导出文件夹"))
        {
            Application.OpenURL("file://" + path);
        }

        EditorGUILayout.LabelField("-------------------------------------------------------------------------------------------------------");
        bool refresh = GUILayout.Button("刷新预览图", GUILayout.Width(100));
        if (refresh)
        {
            if (tex_in_1 != null || tex_in_2 != null || tex_in_3 != null || (tex_in_4 != null && isAlpha))
            {
                OutMergeTex();
            }
            else
            {
                error = true;
                path = "还未生成合并图!";
                return;
            }
        }

        if (texAll != null)
        {
            //GUI.DrawTexture(new Rect(130, 900, 300, 300), new Texture2D(300, 300));
            GUI.DrawTexture(new Rect(125, 895, 1, 310), new Texture2D(1, 1));
            GUI.DrawTexture(new Rect(435, 895, 1, 310), new Texture2D(1, 1));
            GUI.DrawTexture(new Rect(125, 895, 310, 1), new Texture2D(1, 1));
            GUI.DrawTexture(new Rect(125, 1205, 310, 1), new Texture2D(1, 1));
            GUI.DrawTexture(new Rect(130, 900, 300, 300), texAll);
        }
    }
    void OutMergeTex()
    {
        colorr = new Color[0];
        colorg = new Color[0];
        colorb = new Color[0];
        colora = new Color[0];
        colorall = new Color[0];
        SetSize();
        texAll = new Texture2D((int)resolution.x, (int)resolution.y);
        if (tex_in_1 != null)
        {
            colorr = rr.GetPixels();
            colorall = rr.GetPixels();
        }
        if (tex_in_2 != null)
        {
            colorg = gg.GetPixels();
        }
        if (tex_in_3 != null)
        {
            colorb = bb.GetPixels();
        }
        if (isAlpha && tex_in_4 != null)
        {
            colora = aa.GetPixels();
        }

        for (int i = 0; i < resolution.x * resolution.y; i++)
        {
            if (tex_in_1 != null)
            {
                if (tex_channel_1 == ColorChannel.R)
                {
                    colorall[i].r = colorr[i].r;
                }
                if (tex_channel_1 == ColorChannel.G)
                {
                    colorall[i].r = colorr[i].g;
                }
                if (tex_channel_1 == ColorChannel.B)
                {
                    colorall[i].r = colorr[i].b;
                }
                if (tex_channel_1 == ColorChannel.A)
                {
                    colorall[i].r = colorr[i].a;
                }
            }
            else
            {
                colorall[i].r = 0.0f;
            }
            if (tex_in_2 != null)
            {
                if (tex_channel_2 == ColorChannel.R)
                {
                    colorall[i].g = colorg[i].r;
                }
                if (tex_channel_2 == ColorChannel.G)
                {
                    colorall[i].g = colorg[i].g;
                }
                if (tex_channel_2 == ColorChannel.B)
                {
                    colorall[i].g = colorg[i].b;
                }
                if (tex_channel_2 == ColorChannel.A)
                {
                    colorall[i].g = colorg[i].a;
                }
            }
            else
            {
                colorall[i].g = 0.0f;
            }
            if (tex_in_3 != null)
            {
                if (tex_channel_3 == ColorChannel.R)
                {
                    colorall[i].b = colorb[i].r;
                }
                if (tex_channel_3 == ColorChannel.G)
                {
                    colorall[i].b = colorb[i].g;
                }
                if (tex_channel_3 == ColorChannel.B)
                {
                    colorall[i].b = colorb[i].b;
                }
                if (tex_channel_3 == ColorChannel.A)
                {
                    colorall[i].b = colorb[i].a;
                }
            }
            else
            {
                colorall[i].b = 0.0f;
            }
            if (isAlpha && tex_in_4 != null)
            {
                if (tex_channel_4 == ColorChannel.R)
                {
                    colorall[i].a = colora[i].r;
                }
                if (tex_channel_4 == ColorChannel.G)
                {
                    colorall[i].a = colora[i].g;
                }
                if (tex_channel_4 == ColorChannel.B)
                {
                    colorall[i].a = colora[i].b;
                }
                if (tex_channel_4 == ColorChannel.A)
                {
                    colorall[i].a = colora[i].a;
                }
            }
            else
            {
                colorall[i].a = 1.0f;
            }
        }

        if (isBlackWhite)
        {
            Color[] temp = colorall;
            List<float> tt = new List<float>();
            for (int j = 0; j < temp.Length; j++)
            {
                tt.Add(temp[j].r * 0.33334f + temp[j].g * 0.33334f + temp[j].b * 0.33334f);
                colorall[j].r = tt[j];
                colorall[j].g = tt[j];
                colorall[j].b = tt[j];
            }
        }

        if (tex_in_1 != null || tex_in_2 != null || tex_in_3 != null || (tex_in_4 != null && isAlpha))
        {
            texAll.SetPixels(colorall);
            texAll.Apply();
        }
    }
    void SettexImport(Texture2D aaa)
    {
        string path = AssetDatabase.GetAssetPath(aaa);
        TextureImporter tex = AssetImporter.GetAtPath(path) as TextureImporter;
        TextureImporterPlatformSettings set = new TextureImporterPlatformSettings();
        tex.isReadable = true;
        AssetDatabase.ImportAsset(path);
        set.format = TextureImporterFormat.RGBA32;
        tex.SetPlatformTextureSettings(set);
        AssetDatabase.ImportAsset(path);
    }
    Texture2D ScaleTexture(Texture2D source, float targetWidth, float targetHeight)
    {
        Texture2D result = new Texture2D((int)targetWidth, (int)targetHeight, source.format, false);

        float incX = (1.0f / targetWidth);
        float incY = (1.0f / targetHeight);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }
    void SetSize()
    {
        if (tex_in_1 != null)
        {
            SettexImport(tex_in_1);
            rr = tex_in_1;
            string path = AssetDatabase.GetAssetPath(rr);
            rr = ScaleTexture(tex_in_1, resolution.x, resolution.y);
            rr.Apply();
            TextureImporter tex = AssetImporter.GetAtPath(path) as TextureImporter;
            TextureImporterPlatformSettings temp = new TextureImporterPlatformSettings();
            temp.format = TextureImporterFormat.Automatic;
            tex.SetPlatformTextureSettings(temp);
            tex.isReadable = false;
            AssetDatabase.ImportAsset(path);
        }
        if (tex_in_2 != null)
        {
            SettexImport(tex_in_2);
            gg = tex_in_2;
            string path = AssetDatabase.GetAssetPath(gg);
            gg = ScaleTexture(tex_in_2, resolution.x, resolution.y);
            gg.Apply();
            TextureImporter tex = AssetImporter.GetAtPath(path) as TextureImporter;
            TextureImporterPlatformSettings temp = new TextureImporterPlatformSettings();
            temp.format = TextureImporterFormat.Automatic;
            tex.SetPlatformTextureSettings(temp);
            tex.isReadable = false;
            AssetDatabase.ImportAsset(path);
        }
        if (tex_in_3 != null)
        {
            SettexImport(tex_in_3);
            bb = tex_in_3;
            string path = AssetDatabase.GetAssetPath(bb);
            bb = ScaleTexture(tex_in_3, resolution.x, resolution.y);
            bb.Apply();
            TextureImporter tex = AssetImporter.GetAtPath(path) as TextureImporter;
            TextureImporterPlatformSettings temp = new TextureImporterPlatformSettings();
            temp.format = TextureImporterFormat.Automatic;
            tex.SetPlatformTextureSettings(temp);
            tex.isReadable = false;
            AssetDatabase.ImportAsset(path);
        }
        if (isAlpha && tex_in_4 != null)
        {
            SettexImport(tex_in_4);
            aa = tex_in_4;
            string path = AssetDatabase.GetAssetPath(aa);
            aa = ScaleTexture(tex_in_4, resolution.x, resolution.y);
            aa.Apply();
            TextureImporter tex = AssetImporter.GetAtPath(path) as TextureImporter;
            TextureImporterPlatformSettings temp = new TextureImporterPlatformSettings();
            temp.format = TextureImporterFormat.Automatic;
            tex.SetPlatformTextureSettings(temp);
            tex.isReadable = false;
            AssetDatabase.ImportAsset(path);
        }
    }
    void Save(Color[] colors)
    {
        TextureFormat _texFormat;
        if (isAlpha)
        {
            _texFormat = TextureFormat.ARGB32;
        }
        else
        {
            _texFormat = TextureFormat.RGB24;
        }
        Texture2D tex = new Texture2D((int)resolution.x, (int)resolution.y, _texFormat, false);
        tex.SetPixels(colors);
        tex.Apply();
        byte[] bytes;
        bytes = tex.EncodeToPNG();
        string sname = tex2dName + "_" + serial;
        serial += 1;
        File.WriteAllBytes(path + "/" + sname + ".png", bytes);
        AssetDatabase.Refresh();
    }
}
#endif
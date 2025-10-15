#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class SFX_TexturePackerCreate : EditorWindow
{
    /// 处理图片
    List<string> texturePaths = new List<string>(10);

    public int maxAtlasSize = 4096;
    public int space = 2;
    public string atlas_name = "pack";
    public int serial = 1;
    public string path_in = "请选择输入文件夹";
    public string path_out = "请选择输出文件夹";
    private bool error = false;

    [MenuItem(EditorSettings.MENU_ITEM_AtlasCreate, false, EditorSettings.MENU_SORT_AtlasCreate)]
    static void AtlasCreateWindow()
    {
        SFX_TexturePackerCreate window = EditorWindow.GetWindow<SFX_TexturePackerCreate>();
        window.minSize = new Vector2(450, 500);
        window.maxSize = new Vector2(750, 500);
        window.titleContent = new GUIContent("图集打包工具");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("按文件夹内的图片生成图集");
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(">>设置输入文件夹路径：");
        GUI.color = error ? new Color(1, .3f, .3f) : Color.white;
        EditorGUILayout.LabelField(path_in);
        GUI.color = Color.white;
        EditorGUILayout.Space();

        bool nowpath = GUILayout.Button("--获取当前选择路径--");
        if (nowpath)
        {
            error = false;
            GetSelectionPath();
        }

        bool ispath = GUILayout.Button("--手动选择输入路径--");
        if (ispath)
        {
            error = false;
            SetPathIn();
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField(">>设置输出参数：");
        GUIStyle style = new GUIStyle("textfield");
        maxAtlasSize = int.Parse(EditorGUILayout.TextField("最大尺寸：", maxAtlasSize.ToString(), style));
        space = int.Parse(EditorGUILayout.TextField("图片间距：", space.ToString(), style));
        atlas_name = EditorGUILayout.TextField("图集名称：", atlas_name, style);
        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField(">>设置输出文件夹路径：");
        EditorGUILayout.LabelField(path_out);
        EditorGUILayout.Space(20);

        bool nowpath2 = GUILayout.Button("--获取当前选择路径父级--");
        if (nowpath2)
        {
            GetSelectionOutPath(true);
        }
        bool nowpath3 = GUILayout.Button("--获取当前选择路径--");
        if (nowpath3)
        {
            GetSelectionOutPath(false);
        }

        bool ispath2 = GUILayout.Button("--手动选择保存路径--");
        if (ispath2)
        {
            SetPathOut();
        }
        EditorGUILayout.Space(20);
        bool shengcheng = GUILayout.Button("> 生成图集 <");
        if (shengcheng)
        {
            if (!Directory.Exists(path_out))
            {
                path_out = "请选择输出文件夹";
            }
            else
            {
                OutAtlas();
            }
        }
        EditorGUILayout.LabelField("----------------------------------------------------------------------------------------------------------------------------------------");
        if (GUILayout.Button("打开导出文件夹"))
        {
            Application.OpenURL("file://" + path_out);
        }
    }

    void SetPathIn()
    {
        path_in = EditorUtility.OpenFolderPanel("", "", "");
    }
    void GetSelectionPath()
    {
        if (Selection.objects.Length > 0)
        {
            var s = AssetDatabase.GetAssetPath(Selection.objects[0]).Remove(0, "Assets/".Length);
            path_in = Path.Combine(Application.dataPath, s);
        }
        else
        {
            path_in = "请选择输入文件夹";
        }
    }
    void SetPathOut()
    {
        path_out = EditorUtility.OpenFolderPanel("", "", "");
    }
    void GetSelectionOutPath(bool pa)
    {
        if (Selection.objects.Length > 0)
        {
            var s = AssetDatabase.GetAssetPath(Selection.objects[0]).Remove(0, "Assets/".Length);
            path_out = pa ? Directory.GetParent(AssetDatabase.GetAssetPath(Selection.objects[0])).ToString() : Path.Combine(Application.dataPath, s);
        }
        else
        {
            path_out = "请选择输出文件夹";
        }
    }

    void OutAtlas()
    {
        var allFile = GetAllFilesInFolder(path_in);
        for (int i = 0; i < allFile.Length; i++)
        {
            var ext = allFile[i].Extension.ToLower();
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
            {
                texturePaths.Add(allFile[i].FullName);
            }
        }

        if (texturePaths.Count < 1)
        {
            path_in = "输入图片为空，请重新选择";
            return;
        }

        var textures = ReadTextures(texturePaths.ToArray());
        Texture2D tex = CreateAtlas(textures);
        WriteCaptureDataToFile(tex, path_out, atlas_name);
        AssetDatabase.Refresh();
        texturePaths.Clear();
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
    public void WriteCaptureDataToFile(Texture2D texture, string dataPath, string filename)
    {
        if (!Directory.Exists(dataPath))
        {
            try
            {
                Directory.CreateDirectory(dataPath);
            }
            catch (Exception e)
            {
                Debug.LogError("CreateDirectory e:" + e);
            }
        }
        string path_full = System.IO.Path.Combine(dataPath, filename) + ".png";//dataPath + filename + ".jpg";

        // 存入文件
        //StartCoroutine();
        SaveTexture2DtoFile(texture, path_full);
    }
    /// <summary>
    /// 保存图片到指定目录
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private void SaveTexture2DtoFile(Texture2D texture, string path, Action<object> callback = null)
    {
        //等待渲染线程结束  
        //yield return new WaitForEndOfFrame();

        byte[] textureData = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, textureData);

        callback?.Invoke(null);
        //Debug.Log("图片文件写入完毕：" + path);
    }

    public class TextureData
    {
        public string name;
        public Texture2D texture;
        public int width;
        public int height;
        public int top;
        public int right;
        public int bottom;
        public int left;
    }
    /// <summary>
    /// 读取图片资源路径,返回TextureData数组
    /// </summary>
    private TextureData[] ReadTextures(string[] assetPaths)
    {
        TextureData[] textureDatas = new TextureData[assetPaths.Length];
        for (int i = 0; i < assetPaths.Length; i++)
        {
            TextureData data = new TextureData();
            Texture2D texture = LoadImgByPath(assetPaths[i]);

            texture = ClampSingle(texture);
            data.texture = texture;
            data.width = texture.width;
            data.height = texture.height;
            textureDatas[i] = data;
        }
        return textureDatas;
    }
    private Texture2D LoadImgByPath(string path)
    {
        //创建文件读取流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        //int width = 256;
        //int height = 256;
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);
        return texture;
    }
    private static Texture2D ClampSingle(Texture2D sourceTexture)
    {
        int sourceWidth = sourceTexture.width;
        int sourceHeight = sourceTexture.height;
        Color32[] sourcePixels = sourceTexture.GetPixels32();
        int targetWidth = sourceWidth;
        int targetHeight = sourceHeight;
        Texture2D targetTexture = new Texture2D(targetWidth, targetHeight);
        targetTexture.SetPixels32(sourcePixels);
        targetTexture.Apply();
        return targetTexture;
    }
    /// <summary>
    /// 返回填充的图集,本质上为一个合并好的texture2D
    /// </summary>
    /// <param name="textureDatas"></param>
    /// <returns></returns>
    private Texture2D CreateAtlas(TextureData[] textureDatas)
    {
        Texture2D atlas = new Texture2D(maxAtlasSize, maxAtlasSize);//创建图集按照最大尺寸
        Rect[] rect;
        rect = atlas.PackTextures(GetPackTextures(textureDatas), space, maxAtlasSize, false); // 传入纹理数组,纹理间距,最大尺寸,不关闭可读

        float atlasW = atlas.width;
        float atlasH = atlas.height;

        StringBuilder sb = new StringBuilder();
        sb.Append("{\"frames\": {").Append(Environment.NewLine);

        for (int i = 0; i < rect.Length; i++)
        {
            rect[i].width = rect[i].width * atlasW;
            rect[i].x *= atlasW;
            rect[i].height = rect[i].height * atlasH;
            rect[i].y *= atlasH;
            //rect[i].x = atlasW - rect[i].x;
            rect[i].y = atlasH - rect[i].y - rect[i].height;
            //Debug.LogError(rect[i]);
            sb.Append(Environment.NewLine).Append("\"").Append(texturePaths[i].Substring(texturePaths[i].LastIndexOf('\\') + 1, texturePaths[i].Length - texturePaths[i].LastIndexOf('\\') - 1)).Append("\":").Append(Environment.NewLine).Append("{").Append(Environment.NewLine)
            .Append("\"frame\":{\"x\":").Append(rect[i].x.ToString()).Append(",\"y\":").Append(rect[i].y.ToString()).Append(",\"w\":").Append(rect[i].width.ToString()).Append(",\"h\":").Append(rect[i].height.ToString()).Append("},").Append(Environment.NewLine)
            .Append("\"rotated\":false,").Append(Environment.NewLine)
            .Append("\"trimmed\":true,").Append(Environment.NewLine)
            .Append("\"spriteSourceSize\":{\"x\":").Append(rect[i].x.ToString()).Append(",\"y\":").Append(rect[i].y.ToString()).Append(",\"w\":").Append(rect[i].width.ToString()).Append(",\"h\":").Append(rect[i].height.ToString()).Append("},").Append(Environment.NewLine)
            .Append("\"sourceSize\":{\"w\":").Append(textureDatas[i].width.ToString()).Append(",\"h\":").Append(textureDatas[i].height.ToString()).Append("}").Append(Environment.NewLine)
            .Append("},");
        }
        sb.Append("},").Append(Environment.NewLine)
        .Append("\"meta\":{").Append(Environment.NewLine)
        .Append("\"app\":\"awa\",").Append(Environment.NewLine)
        .Append("\"version\":\"awa\",").Append(Environment.NewLine)
        .Append("\"image\":\"").Append(atlas_name).Append(".png\",").Append(Environment.NewLine)
        .Append("\"format\":\"RGBA8888\",").Append(Environment.NewLine)
        .Append("\"size\":{\"w\":").Append(atlasW.ToString()).Append(",\"h\":").Append(atlasH.ToString()).Append("},").Append(Environment.NewLine)
        .Append("\"scale\":\"1\",").Append(Environment.NewLine)
        .Append("\"smartupdate\":\"awa\"").Append(Environment.NewLine)
        .Append("}").Append(Environment.NewLine).Append("}");
        SaveTextToFile(sb, path_out);
        return atlas;
    }

    /// <summary>
    /// 获取打包的texture2D贴图数据数组
    /// </summary>
    /// <param name="textureDatas"></param>
    /// <returns></returns>
    private Texture2D[] GetPackTextures(TextureData[] textureDatas)
    {
        Texture2D[] result = new Texture2D[textureDatas.Length];//创建贴图数组
        for (int i = 0; i < textureDatas.Length; i++)
        {//遍历贴图数据数组
            result[i] = textureDatas[i].texture;//把纹理数据保存下来
        }
        return result;
    }
    private void SaveTextToFile(StringBuilder sb, string path)
    {
        path = System.IO.Path.Combine(path, atlas_name) + ".txt";
        StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
        sw.WriteLine(sb.ToString());
        sw.Close();
    }
}
#endif
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
    /// ����ͼƬ
    List<string> texturePaths = new List<string>(10);

    public int maxAtlasSize = 4096;
    public int space = 2;
    public string atlas_name = "pack";
    public int serial = 1;
    public string path_in = "��ѡ�������ļ���";
    public string path_out = "��ѡ������ļ���";
    private bool error = false;

    [MenuItem(EditorSettings.MENU_ITEM_AtlasCreate, false, EditorSettings.MENU_SORT_AtlasCreate)]
    static void AtlasCreateWindow()
    {
        SFX_TexturePackerCreate window = EditorWindow.GetWindow<SFX_TexturePackerCreate>();
        window.minSize = new Vector2(450, 500);
        window.maxSize = new Vector2(750, 500);
        window.titleContent = new GUIContent("ͼ���������");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("���ļ����ڵ�ͼƬ����ͼ��");
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(">>���������ļ���·����");
        GUI.color = error ? new Color(1, .3f, .3f) : Color.white;
        EditorGUILayout.LabelField(path_in);
        GUI.color = Color.white;
        EditorGUILayout.Space();

        bool nowpath = GUILayout.Button("--��ȡ��ǰѡ��·��--");
        if (nowpath)
        {
            error = false;
            GetSelectionPath();
        }

        bool ispath = GUILayout.Button("--�ֶ�ѡ������·��--");
        if (ispath)
        {
            error = false;
            SetPathIn();
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField(">>�������������");
        GUIStyle style = new GUIStyle("textfield");
        maxAtlasSize = int.Parse(EditorGUILayout.TextField("���ߴ磺", maxAtlasSize.ToString(), style));
        space = int.Parse(EditorGUILayout.TextField("ͼƬ��ࣺ", space.ToString(), style));
        atlas_name = EditorGUILayout.TextField("ͼ�����ƣ�", atlas_name, style);
        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField(">>��������ļ���·����");
        EditorGUILayout.LabelField(path_out);
        EditorGUILayout.Space(20);

        bool nowpath2 = GUILayout.Button("--��ȡ��ǰѡ��·������--");
        if (nowpath2)
        {
            GetSelectionOutPath(true);
        }
        bool nowpath3 = GUILayout.Button("--��ȡ��ǰѡ��·��--");
        if (nowpath3)
        {
            GetSelectionOutPath(false);
        }

        bool ispath2 = GUILayout.Button("--�ֶ�ѡ�񱣴�·��--");
        if (ispath2)
        {
            SetPathOut();
        }
        EditorGUILayout.Space(20);
        bool shengcheng = GUILayout.Button("> ����ͼ�� <");
        if (shengcheng)
        {
            if (!Directory.Exists(path_out))
            {
                path_out = "��ѡ������ļ���";
            }
            else
            {
                OutAtlas();
            }
        }
        EditorGUILayout.LabelField("----------------------------------------------------------------------------------------------------------------------------------------");
        if (GUILayout.Button("�򿪵����ļ���"))
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
            path_in = "��ѡ�������ļ���";
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
            path_out = "��ѡ������ļ���";
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
            path_in = "����ͼƬΪ�գ�������ѡ��";
            return;
        }

        var textures = ReadTextures(texturePaths.ToArray());
        Texture2D tex = CreateAtlas(textures);
        WriteCaptureDataToFile(tex, path_out, atlas_name);
        AssetDatabase.Refresh();
        texturePaths.Clear();
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

        // �����ļ�
        //StartCoroutine();
        SaveTexture2DtoFile(texture, path_full);
    }
    /// <summary>
    /// ����ͼƬ��ָ��Ŀ¼
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private void SaveTexture2DtoFile(Texture2D texture, string path, Action<object> callback = null)
    {
        //�ȴ���Ⱦ�߳̽���  
        //yield return new WaitForEndOfFrame();

        byte[] textureData = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, textureData);

        callback?.Invoke(null);
        //Debug.Log("ͼƬ�ļ�д����ϣ�" + path);
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
    /// ��ȡͼƬ��Դ·��,����TextureData����
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
        //�����ļ���ȡ��
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //�����ļ����Ȼ�����
        byte[] bytes = new byte[fileStream.Length];
        //��ȡ�ļ�
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //�ͷ��ļ���ȡ��
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //����Texture
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
    /// ��������ͼ��,������Ϊһ���ϲ��õ�texture2D
    /// </summary>
    /// <param name="textureDatas"></param>
    /// <returns></returns>
    private Texture2D CreateAtlas(TextureData[] textureDatas)
    {
        Texture2D atlas = new Texture2D(maxAtlasSize, maxAtlasSize);//����ͼ���������ߴ�
        Rect[] rect;
        rect = atlas.PackTextures(GetPackTextures(textureDatas), space, maxAtlasSize, false); // ������������,������,���ߴ�,���رտɶ�

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
    /// ��ȡ�����texture2D��ͼ��������
    /// </summary>
    /// <param name="textureDatas"></param>
    /// <returns></returns>
    private Texture2D[] GetPackTextures(TextureData[] textureDatas)
    {
        Texture2D[] result = new Texture2D[textureDatas.Length];//������ͼ����
        for (int i = 0; i < textureDatas.Length; i++)
        {//������ͼ��������
            result[i] = textureDatas[i].texture;//���������ݱ�������
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
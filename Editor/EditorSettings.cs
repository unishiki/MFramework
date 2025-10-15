public static class EditorSettings
{
    // Root path
    private const string ROOT_MENU = "Assets/";
    private const string ROOT_NAME = "Shiki_Tools/";

    // Type path
    private const string PARTICLE_TOOLS_ROOT = "Particle/";
    private const string TEXTURE_TOOLS_ROOT = "Texture/";

    // Menu path + Sort
    public const string MENU_ITEM_ParticlePrefabCollector = ROOT_MENU + ROOT_NAME + PARTICLE_TOOLS_ROOT + "粒子预览工具";
    public const int MENU_SORT_ParticlePrefabCollector = 2;

    public const string MENU_ITEM_TexRotatePlus90 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "旋转/顺时针90°";
    public const int MENU_SORT_TexRotatePlus90 = 1;
    public const string MENU_ITEM_TexRotateMinus90 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "旋转/逆时针90°";
    public const int MENU_SORT_TexRotateMinus90 = 2;
    public const string MENU_ITEM_TexRotate180 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "旋转/180°";
    public const int MENU_SORT_TexRotate180 = 3;
    public const string MENU_ITEM_FlipHorizontal = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "翻转/水平翻转";
    public const int MENU_SORT_FlipHorizontal = 4;
    public const string MENU_ITEM_FlipVertical = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "翻转/垂直翻转";
    public const int MENU_SORT_FlipVertical = 5;
    public const string MENU_ITEM_Grayscale1 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "灰度图/RGB求Max";
    public const int MENU_SORT_Grayscale1 = 6;
    public const string MENU_ITEM_Grayscale2 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "灰度图/RGB*0.33333";
    public const int MENU_SORT_Grayscale2 = 7;
    public const string MENU_ITEM_Grayscale3 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "灰度图/灰度公式";
    public const int MENU_SORT_Grayscale3 = 8;
    public const string MENU_ITEM_DeBlack1 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "一键透明/黑色部分转透明 有色部分转白色";
    public const int MENU_SORT_DeBlack1 = 9;
    public const string MENU_ITEM_DeBlack2 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "一键透明/黑色部分转透明";
    public const int MENU_SORT_DeBlack2 = 10;
    public const string MENU_ITEM_DeBlack3 = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "一键透明/白色部分转透明";
    public const int MENU_SORT_DeBlack3 = 11;
    public const string MENU_ITEM_InvertColor = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "反相/RGB";
    public const int MENU_SORT_InvertColor = 12;
    public const string MENU_ITEM_InvertColorR = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "反相/R";
    public const int MENU_SORT_InvertColorR = 13;
    public const string MENU_ITEM_InvertColorG = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "反相/G";
    public const int MENU_SORT_InvertColorG = 14;
    public const string MENU_ITEM_InvertColorB = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "反相/B";
    public const int MENU_SORT_InvertColorB = 15;
    public const string MENU_ITEM_Levels = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "调整色阶工具";
    public const int MENU_SORT_Levels = 100;
    public const string MENU_ITEM_RampCreate = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "渐变图生成工具";
    public const int MENU_SORT_RampCreate = 101;
    public const string MENU_ITEM_TexMergeCreate = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "贴图合并工具";
    public const int MENU_SORT_TexMergeCreate = 102;
    public const string MENU_ITEM_AtlasCreate = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "图集打包工具";
    public const int MENU_SORT_AtlasCreate = 200;
    public const string MENU_ITEM_AtlasProcessToSprite = ROOT_MENU + ROOT_NAME + TEXTURE_TOOLS_ROOT + "图集拆分";
    public const int MENU_SORT_AtlasProcessToSprite = 201;


    // Used By ParticlePrefabCollector
    public const string ScanResultAssetPath =
            "Assets/MFramework/Editor/Particle_Tools/ParticlePrefabCollector/ParticlePrefabScanResult.asset";
    public const string ConfigAssetPath =
            "Assets/MFramework/Editor/Particle_Tools/ParticlePrefabCollector/ParticlePrefabPreviewConfig.asset";


}

using UnityEditor;

public class CreateAssetBundles 
{
    // Start is called before the first frame update
    [MenuItem("Assets/Build AssetBundles WIN")]
    static void BuildAllAssetBundlesWIN()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Win", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
    [MenuItem("Assets/Build AssetBundles Android")]
    static void BuildAllAssetBundlesAndoid()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
    [MenuItem("Assets/Build AssetBundles IOS")]
    static void BuildAllAssetBundlesIOS()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/IOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
    [MenuItem("Assets/Build ALL AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildAllAssetBundlesWIN();
        BuildAllAssetBundlesAndoid();
        BuildAllAssetBundlesIOS();
    }
}

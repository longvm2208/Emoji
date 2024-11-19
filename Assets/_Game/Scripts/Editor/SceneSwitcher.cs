using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSwitcher
{
    private const string ScenesFolderPath = "Assets/_Game/Scenes/";

    [MenuItem("Scenes/Load #q", priority = 1)]
    public static void OpenLoad()
    {
        OpenScene("Load");
    }

    [MenuItem("Scenes/Load #q", true)]
    public static bool OpenLoadValidate()
    {
        return OpenSceneValidate("Load");
    }

    [MenuItem("Scenes/Home #w", priority = 2)]
    public static void OpenHome()
    {
        OpenScene("Home");
    }

    [MenuItem("Scenes/Home #w", true)]
    public static bool OpenHomeValidate()
    {
        return OpenSceneValidate("Home");
    }

    [MenuItem("Scenes/Level1 #1", priority = 101)]
    public static void OpenLevel1()
    {
        OpenScene("Level1");
    }

    [MenuItem("Scenes/Level1 #1", true)]
    public static bool OpenLevel1Validate()
    {
        return OpenSceneValidate("Level1");
    }

    [MenuItem("Scenes/Level2 #2", priority = 102)]
    public static void OpenLevel2()
    {
        OpenScene("Level2");
    }

    [MenuItem("Scenes/Level2 #2", true)]
    public static bool OpenLevel2Validate()
    {
        return OpenSceneValidate("Level2");
    }

    [MenuItem("Scenes/Level3 #3", priority = 103)]
    public static void OpenLevel3()
    {
        OpenScene("Level3");
    }

    [MenuItem("Scenes/Level3 #3", true)]
    public static bool OpenLevel3Validate()
    {
        return OpenSceneValidate("Level3");
    }

    [MenuItem("Scenes/Level4 #4", priority = 104)]
    public static void OpenLevel4()
    {
        OpenScene("Level4");
    }

    [MenuItem("Scenes/Level4 #4", true)]
    public static bool OpenLevel4Validate()
    {
        return OpenSceneValidate("Level4");
    }

    [MenuItem("Scenes/Level5 #5", priority = 105)]
    public static void OpenLevel5()
    {
        OpenScene("Level5");
    }

    [MenuItem("Scenes/Level5 #5", true)]
    public static bool OpenLevel5Validate()
    {
        return OpenSceneValidate("Level5");
    }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(ScenesFolderPath + sceneName + ".unity");
        }
    }

    private static bool OpenSceneValidate(string sceneName)
    {
        return System.IO.File.Exists(ScenesFolderPath + sceneName + ".unity");
    }
}
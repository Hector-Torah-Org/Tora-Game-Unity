using UnityEditor;

public static class BuildScript
{
    public static void BuildWebGL()
    {
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            locationPathName = "Build/WebGL",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
    }
}
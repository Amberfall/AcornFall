using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

public class BuildScripts : MonoBehaviour
{
    [MenuItem("Build/Build LinuxServer")]
    public static void BuildLinuxServer()
    {
        BuildPlayerOptions buildPlayerOptions = new()
        {
            scenes = new[] { "Assets/Scenes/TitleScene.unity" },
            locationPathName = "Builds/BuildLinuxServer/RootGameServer.x86_64",
            target = BuildTarget.StandaloneLinux64,
            subtarget = (int)StandaloneBuildSubtarget.Server,
            options = BuildOptions.Development
        };
        
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            Debug.Log($"Build output path: {summary.outputPath}");

            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + "scp -r " + Path.GetDirectoryName(summary.outputPath) + " self.trevor@ggj.skipsabeatmusic.com:~");
            ProcessInfo.CreateNoWindow = false;
            ProcessInfo.UseShellExecute = true;

            Process = Process.Start(ProcessInfo);
            Process.WaitForExit();
            var exitCode = Process.ExitCode;
            Process.Close();
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    //[MenuItem("Build/Build WebGL")]
    //public static void BuildWebGL()
    //{
    //    BuildPlayerOptions buildPlayerOptions = new()
    //    {
    //        scenes = new[] { "Assets/Scenes/TestScene.unity" },
    //        locationPathName = "BuildWebGL",
    //        target = BuildTarget.LinuxHeadlessSimulation,
    //        options = BuildOptions.Development
    //    };

    //    BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    //    BuildSummary summary = report.summary;

    //    if (summary.result == BuildResult.Succeeded)
    //    {
    //        Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
    //    }

    //    if (summary.result == BuildResult.Failed)
    //    {
    //        Debug.Log("Build failed");
    //    }
    //}

    //[MenuItem("Build/TestCopyToCloud")]
    //public static void CopyToCloud()
    //{
    //    //string path = EditorUtility.OpenFolderPanel("Load png Textures", "", "");

    //    ProcessStartInfo ProcessInfo;
    //    Process Process;

    //    ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + "scp -r \"C:\\Users\\selft\\Documents\\GitHub\\LinuxServerTest\\Builds\\BuildLinuxServer\" self.trevor@ggj.skipsabeatmusic.com:~");
    //    ProcessInfo.CreateNoWindow = false;
    //    ProcessInfo.UseShellExecute = true;

    //    Process = Process.Start(ProcessInfo);
    //    Process.WaitForExit();
    //    var exitCode = Process.ExitCode;
    //    Process.Close();
    //}
}

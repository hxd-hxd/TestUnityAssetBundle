using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 构建 AB 包
/// </summary>
public class BuildAB
{
    [MenuItem("Test/打开持久化目录")]
    static void Open()
    {
        //EditorUtility.RevealInFinder(Application.persistentDataPath);
        ExplorePath(Application.persistentDataPath);
    }

    [MenuItem("Test/BuileAB")]
    static void Build()
    {
        // 构建的目标平台
        BuildTarget target = BuildTarget.StandaloneWindows;
        // 要构建成 ab 包的资源
        string resPath = "Assets/Res";
        // ab 包输出路径
        string outputPath = "Assets/AssetBundle/" + target;
        RecreateDirectory(outputPath);

        // 指定资源
        AssetBundleBuild b1 = new AssetBundleBuild();
        b1.assetBundleName = "ab";
        b1.assetNames = new string[] { resPath };
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>()
        {
            b1
        };

        // 构建 ab 包
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
            outputPath, 
            builds.ToArray(), 
            BuildAssetBundleOptions.None, 
            target);

        // 拷贝到使用路径
        string useABPath = Application.streamingAssetsPath + "/AB";
        RecreateDirectory(useABPath);
        
        //File.Copy(outputPath, useABPath);
        FullDirRplacer(outputPath, useABPath);

        AssetDatabase.Refresh();
    }

    // 重建一个目录
    static void RecreateDirectory(string dir)
    {
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        else
        {
            Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
        }
    }

    // 将一个目录内容完整替换到另外一个目录
    static void FullDirRplacer(string sourceDir, string targetDir)
    {
        Debug.Log($"正在替换目录：{sourceDir} -> {targetDir}");

        string[] dirs = Directory.GetDirectories(sourceDir);
        string[] files = Directory.GetFiles(sourceDir);

        // 复制所有文件
        foreach (string file in files)
        {
            if(Path.GetExtension(file) == ".meta") continue;
            File.Copy(file, targetDir + "/" + Path.GetFileName(file));
        }

        // 创建子目录，递归复制子目录
        foreach (string dir in dirs)
        {
            string dirPath = targetDir + "/" + Path.GetFileName(dir);
            Debug.Log($"子目录：{dirPath}");
            RecreateDirectory(dirPath);
            FullDirRplacer(dir, dirPath);
        }
    }

    /// <summary>
    /// 浏览文件夹
    /// </summary>
    /// <param name="path"></param>
    public static void ExplorePath(string path)
    {
        //System.Diagnostics.Process.Start("explorer.exe", path);
        System.Diagnostics.Process.Start(path);
    } 

    [MenuItem("Test/DirectoryGetFiles")]
    static void Test_DirectoryGetFiles()
    {
        foreach (var item in Directory.GetFiles("Assets/AssetBundle/" + BuildTarget.StandaloneWindows))
        {
            Debug.Log(item);
            Debug.Log(Path.GetFileName(item));
            Debug.Log(Path.GetExtension(item));
        }
    }
    [MenuItem("Test/替换目录")]
    static void Test_FullDirRplacer()
    {
        FullDirRplacer(
            Application.dataPath + "/a",
            Application.dataPath + "/b"
            );

        AssetDatabase.Refresh();
    }

}

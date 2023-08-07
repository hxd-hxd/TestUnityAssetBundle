using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ���� AB ��
/// </summary>
public class BuildAB
{
    [MenuItem("Test/�򿪳־û�Ŀ¼")]
    static void Open()
    {
        //EditorUtility.RevealInFinder(Application.persistentDataPath);
        ExplorePath(Application.persistentDataPath);
    }

    [MenuItem("Test/BuileAB")]
    static void Build()
    {
        // ������Ŀ��ƽ̨
        BuildTarget target = BuildTarget.StandaloneWindows;
        // Ҫ������ ab ������Դ
        string resPath = "Assets/Res";
        // ab �����·��
        string outputPath = "Assets/AssetBundle/" + target;
        RecreateDirectory(outputPath);

        // ָ����Դ
        AssetBundleBuild b1 = new AssetBundleBuild();
        b1.assetBundleName = "ab";
        b1.assetNames = new string[] { resPath };
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>()
        {
            b1
        };

        // ���� ab ��
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
            outputPath, 
            builds.ToArray(), 
            BuildAssetBundleOptions.None, 
            target);

        // ������ʹ��·��
        string useABPath = Application.streamingAssetsPath + "/AB";
        RecreateDirectory(useABPath);
        
        //File.Copy(outputPath, useABPath);
        FullDirRplacer(outputPath, useABPath);

        AssetDatabase.Refresh();
    }

    // �ؽ�һ��Ŀ¼
    static void RecreateDirectory(string dir)
    {
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        else
        {
            Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
        }
    }

    // ��һ��Ŀ¼���������滻������һ��Ŀ¼
    static void FullDirRplacer(string sourceDir, string targetDir)
    {
        Debug.Log($"�����滻Ŀ¼��{sourceDir} -> {targetDir}");

        string[] dirs = Directory.GetDirectories(sourceDir);
        string[] files = Directory.GetFiles(sourceDir);

        // ���������ļ�
        foreach (string file in files)
        {
            if(Path.GetExtension(file) == ".meta") continue;
            File.Copy(file, targetDir + "/" + Path.GetFileName(file));
        }

        // ������Ŀ¼���ݹ鸴����Ŀ¼
        foreach (string dir in dirs)
        {
            string dirPath = targetDir + "/" + Path.GetFileName(dir);
            Debug.Log($"��Ŀ¼��{dirPath}");
            RecreateDirectory(dirPath);
            FullDirRplacer(dir, dirPath);
        }
    }

    /// <summary>
    /// ����ļ���
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
    [MenuItem("Test/�滻Ŀ¼")]
    static void Test_FullDirRplacer()
    {
        FullDirRplacer(
            Application.dataPath + "/a",
            Application.dataPath + "/b"
            );

        AssetDatabase.Refresh();
    }

}

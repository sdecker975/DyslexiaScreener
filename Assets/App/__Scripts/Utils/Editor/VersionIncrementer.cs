using UnityEditor;
using UnityEngine;

public static class VersionIncrementer {

    private enum VersionType {

        Major,
        Minor,
        SubMinor

    }

    [MenuItem("Tools/IncrementBuildNumber")]
    private static void IncrementBuildNumber() {
        // ios
        var oldNumberString = PlayerSettings.iOS.buildNumber;

        if (!int.TryParse(oldNumberString, out var buildNumber)) {
            Debug.LogWarning($"Can't parse build number {oldNumberString}");
            return;
        }

        buildNumber++;
        PlayerSettings.iOS.buildNumber = buildNumber.ToString();
        Debug.Log($"iOS BuildNumber set to {buildNumber}");

        // android
        var bundleVersionCode = PlayerSettings.Android.bundleVersionCode;
        bundleVersionCode++;
        PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
        Debug.Log($"Android BundleVersionCode set to {bundleVersionCode}");

        BuildInfo.Instance.BuildId = buildNumber.ToString();
    }

    [MenuItem("Tools/ResetBuildNumber")]
    private static void ResetBuildNumber() {
        PlayerSettings.iOS.buildNumber = "1";
        PlayerSettings.Android.bundleVersionCode = 1;
        
        BuildInfo.Instance.BuildId = PlayerSettings.iOS.buildNumber;
    }

    [MenuItem("Tools/New Version/SubMinor (0.0.x)")]
    private static void IncrementSubMinor() {
        IncrementVersion(VersionType.SubMinor);
        IncrementBuildNumber();
    }

    [MenuItem("Tools/New Version/Minor (0.x.0)")]
    private static void IncrementMinor() {
        IncrementVersion(VersionType.Minor);
        IncrementBuildNumber();
    }

    [MenuItem("Tools/New Version/Major (x.0.0)")]
    private static void IncrementMajor() {
        IncrementVersion(VersionType.Major);
        IncrementBuildNumber();
    }

    private static void IncrementVersion(VersionType versionType) {
        var versionText = PlayerSettings.bundleVersion.Trim();
        Debug.Log($"currentVersion= {versionText}");

        var lines = versionText.Split('.');

        if (lines.Length < 2) {
            Debug.LogWarning($"Invalid versionText= {versionText}");
            return;
        }

        var major = 1;
        var minor = 0;
        var subMinor = 0;

        // does not have subminor
        if (lines.Length == 2) {
            major = int.Parse(lines[0].Trim());
            minor = int.Parse(lines[1].Trim());
        }

        // already has subminor
        if (lines.Length == 3) {
            major = int.Parse(lines[0].Trim());
            minor = int.Parse(lines[1].Trim());
            subMinor = int.Parse(lines[2].Trim());
        }

        if (versionType == VersionType.Major) {
            major++;
            minor = 0;
            subMinor = 0;
        } else if (versionType == VersionType.Minor) {
            minor++;
            subMinor = 0;
        } else {
            subMinor++;
        }

        var newVersion = $"{major}.{minor}.{subMinor}".Trim();
        Debug.Log($"oldVersion= {versionText} newVersion= {newVersion}");
        PlayerSettings.bundleVersion = newVersion;
    }

}
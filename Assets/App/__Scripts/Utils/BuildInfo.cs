using UnityEngine;

// TODO: add method to create or ignore creation of asset in Resources folder e.g. only 1 should exist
[CreateAssetMenu(fileName = "BuildInfo", menuName = "Zumoko/BuildInfo", order = 0)]
public class BuildInfo : ScriptableObject {

    private static BuildInfo instance;

    public static BuildInfo Instance {
        get {
            if (!instance) {
                instance = Resources.Load<BuildInfo>($"{nameof(BuildInfo)}");
            }

            return instance;
        }
    }
    
    [SerializeField] private string buildId = "1";

    public string BuildId {
        get => buildId;
        set {
            buildId = value;
            Save();
        }
    }

    private void Save() {
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        #endif
    }

}
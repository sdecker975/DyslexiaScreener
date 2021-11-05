using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class VersionDisplayer : MonoBehaviour {

    [Header(HeaderNames.SetInInspector)]
    [SerializeField] private string versionPrefix = "v.";
    [SerializeField] private bool showBuildId;
    
    [Header(HeaderNames.DynamicDebug)]
    [SerializeField] private Text versionText;
    
    [SerializeField] private BuildInfo buildInfo;

    private void Awake() {
        versionText = GetComponent<Text>();
        buildInfo = BuildInfo.Instance;

        UpdateVersionText();
    }

    private void UpdateVersionText() {
        var text = $"{versionPrefix}{Application.version}";
        
        if (showBuildId) {
            text = $"{versionPrefix}{Application.version} ({buildInfo.BuildId})";
        }

        versionText.text = text;
    }

}
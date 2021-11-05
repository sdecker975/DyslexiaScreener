using UnityEngine;
using UnityEngine.Serialization;

public class AddChildOffline : MonoBehaviour {

    [SerializeField] private StudentInfo studentInfo;
    [FormerlySerializedAs("AddChildOfflineButton")] 
    public GameObject addChildOfflineButton;
    public GameObject offlineStudentCanvas;
    
    private float timerToCheckInternet = 2f;

    private void Start() {
        offlineStudentCanvas.SetActive(false);

        addChildOfflineButton.SetActive(!InternetAvailable.internetAvailableStatic);
    }

    private void Update() {
        timerToCheckInternet -= Time.deltaTime;

        if (!(timerToCheckInternet <= 0)) {
            return;
        }

        timerToCheckInternet = 2f;

        addChildOfflineButton.SetActive(!InternetAvailable.internetAvailableStatic);
    }

    public void OpenOfflineInputPanel() {
        if (!offlineStudentCanvas.activeInHierarchy) {
            studentInfo.CreateAndSelectNewChild();
            offlineStudentCanvas.SetActive(true);
        }
    }

    public void CloseOfflineInputPanel() {
        if (offlineStudentCanvas.activeInHierarchy) {
            offlineStudentCanvas.SetActive(false);
        }
    }

}
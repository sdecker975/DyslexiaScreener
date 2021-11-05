using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeTestScript : MonoBehaviour
{
    public AudioMixer volumeMixer;
    public AudioSource testSource;
    public Slider volumeSlider;
    public Text testButtonText;
    public GameObject volumeTestPopup;
    bool buttonPressed = false;
    private void Start()
    {
        volumeTestPopup.SetActive(false);
        volumeSlider.value = 0f;
        volumeMixer.SetFloat("Volume", 0f);
    }

    public void SetLevel(float sliderValue)
    {
        // setting volume in mixer
        volumeMixer.SetFloat("Volume", sliderValue);
    }
    public void TestAudioButton()
    {
        
        if (!buttonPressed)
        {
            
            testSource.Play();
            testButtonText.text = "Stop";
            buttonPressed = !buttonPressed;

        }else if (buttonPressed)
        {
            if (testSource.isPlaying)
                testSource.Stop();

            testButtonText.text = "Test sound";
            buttonPressed = !buttonPressed;
        }
        
    }
    public void OpenVolumePopup()
    {
        if(!volumeTestPopup.activeInHierarchy)
        volumeTestPopup.SetActive(true);
    }

    public void CloseVolumePopup()
    {
        
        if (volumeTestPopup.activeInHierarchy)
        {
            if(testSource.isPlaying)
            testSource.Stop();

            buttonPressed = false;
            testButtonText.text = "Test sound";
            volumeTestPopup.SetActive(false);
        }
        
    }

}

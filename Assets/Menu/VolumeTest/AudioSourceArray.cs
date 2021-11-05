using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioSourceArray : MonoBehaviour {

    public AudioMixerGroup masterMixerGroup;

    private void Awake() {
        // finding all audio sources in scene 
        var audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        foreach (var item in audioSources) {
            item.outputAudioMixerGroup = masterMixerGroup;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuitScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CheckQuit.canQuit = true;
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			System.Diagnostics.Process.GetCurrentProcess().Kill();
		#endif
	}
}

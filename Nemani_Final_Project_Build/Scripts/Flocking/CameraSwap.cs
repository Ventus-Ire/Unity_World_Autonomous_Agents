using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour {

	public Camera[] cameras;
	private int currentCameraIndex;

	private string text;
	// Use this for initialization
	void Start () {
		currentCameraIndex = 0;
		for (int i = 1; i < cameras.Length; i++) {
			cameras [i].gameObject.SetActive (false);
		}

		if (cameras.Length > 0) {
			cameras [0].gameObject.SetActive (true);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) {
			currentCameraIndex++;
			cameras [currentCameraIndex - 1].gameObject.SetActive (false);
		}
		if (currentCameraIndex < cameras.Length) {
			
			//Debug.Log (currentCameraIndex);
			cameras[currentCameraIndex].gameObject.SetActive(true);
		}
		else {
			cameras[currentCameraIndex - 1].gameObject.SetActive(false);
			currentCameraIndex = 0;
			cameras[currentCameraIndex].gameObject.SetActive(true);
		}
		if (currentCameraIndex == 0) {
			text = "1st Camera View";
		}
		if (currentCameraIndex == 1) {
			text = "Fish Camera View";
		}
		if (currentCameraIndex == 2) {
			text = "Follow Down View";
		}
		if (currentCameraIndex == 3) {
			text = "1st Person";
		}
		if (currentCameraIndex == 4) {
			text = "Overview";
		}
		if (currentCameraIndex == 5) {
			text = "Sideview";
		}
			
	}
	void OnGUI()
	{
		GUI.color = Color.white;
		GUI.skin.box.fontSize = 15;
		GUI.skin.box.wordWrap = true;
		GUI.Box (new Rect (20, 20, 350, 60), "You are using the Camera: " + text + " Press C to change cameras. Press D for Debug lines.");
	}


}

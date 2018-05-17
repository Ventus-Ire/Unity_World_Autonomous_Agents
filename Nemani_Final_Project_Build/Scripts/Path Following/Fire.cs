using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
	public GameObject spirit;
	public GameObject[] fire;

	// Use this for initialization
	void Start () {
		fire = GameObject.FindGameObjectsWithTag ("Fire");
		spirit = GameObject.FindGameObjectWithTag ("Human");
	}
	
	// Update is called once per frame
	void Update () {
		//*******************
		//Reason why the green spirits are path following
		//*******************
		//Turn off the particle system and turn it back on
		foreach (GameObject g in fire) {
			Vector3 destination = spirit.transform.position;
			//Debug.Log (destination);
			Vector3 offset = destination - g.transform.position;
			if (offset.magnitude < 2) {
				g.gameObject.SetActive (false);

			} 
			if (offset.magnitude >= 100)  {
				g.gameObject.SetActive (true);
			}
		}

	}
}

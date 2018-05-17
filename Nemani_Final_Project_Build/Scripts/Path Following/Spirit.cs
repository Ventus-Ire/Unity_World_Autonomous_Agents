using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour {
	public Path path;
	public float reach = 1f;
	public float speed = 5f;
	public float rotation = 10f;
	private int nodeID = 0;
	public GameObject slowField;
	public bool draw = false;

	// Use this for initialization
	void Start () {
		slowField = GameObject.FindGameObjectWithTag ("Slow");
	}
	
	// Update is called once per frame
	void Update () {
		//*******************
		//Path Following
		//*******************
		Vector3 destination = path.GetNode (nodeID);
		Vector3 offset = destination - transform.position;
		if (offset.sqrMagnitude > reach) {
			offset = offset.normalized;
			transform.Translate (offset * speed * Time.deltaTime, Space.World);
			Quaternion look = Quaternion.LookRotation (offset);
			transform.rotation = Quaternion.Slerp (transform.rotation, look, rotation * Time.deltaTime);
		} else {
			ChangeDestNode ();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			if (draw == false) {
				draw = true;
			}
			else if (draw == true) {
				draw = false;
			}
		}
		OnRenderObject ();
		
	}
	//*******************
	//RESISTANCE HERE
	//*******************
	void OnTriggerEnter(Collider other){
		if (other.tag == "Slow") {
			speed = speed / 2;
		}
		
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "Slow") {
			speed = speed * 2;
		}
	}
	//Updates the node we already arrived at the previous one
	void ChangeDestNode() {
		nodeID++;
		if (nodeID >= path.nodes.Length) {
			nodeID = 0;
		}
	}
	//For Debugging
	void OnRenderObject() {
		if (draw == true) {
			Debug.DrawLine (transform.position, path.GetNode (nodeID));
		}
	}

}

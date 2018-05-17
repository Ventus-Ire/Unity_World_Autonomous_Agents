using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
	public GameObject target;
	public float distance = 5.0f;
	public float height = 1.5f;
	public float heightDamping = 2.0f;
	public float positionDamping = 2.0f;
	public float rotationDamping = 2.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!target)
			return;
		target = GameObject.Find("CameraSpirit");
		float wantedHeight = target.transform.position.y + height;
		float currentHeight = transform.position.y;

		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime * 3);

		Vector3 wantedPosition = target.transform.position - target.transform.forward * distance;
		transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * positionDamping * 3);

		transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);

		transform.forward = Vector3.Lerp (transform.forward, target.transform.forward, Time.deltaTime * rotationDamping * 3);
	}
}

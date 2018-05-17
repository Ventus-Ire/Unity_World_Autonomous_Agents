using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAgent : Vehicle {

	public float radiusWander = 3f;
	float boundsWeight = 100f;
	Vector3 center = new Vector3 (3, -24.9f, 76f);

	public bool draw;
	private Vector3 ultimateForce;

	// Use this for initialization
	protected override void Start () {

		base.Start ();
	}

	//Calculates the force that is meant to be applied
	public override void CalcSteeringForces() {


		ultimateForce = Vector3.zero;
		//Debug.Log (FollowField (transform.position));
		ultimateForce += FollowField (transform.position);
		//ultimateForce = Vector3.ClampMagnitude (ultimateForce, 10);
		//Debug.Log (ultimateForce);
		if (OutOfBounds ()) 
		{
			ultimateForce += Seek(center) * boundsWeight;
		}
		ApplyForce(ultimateForce);


		//For DEBUGGING
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
	//Draw the debug lines
	void OnRenderObject() {
		if (draw == true) {
			Debug.DrawLine (transform.position, ultimateForce.normalized + transform.position, Color.red);
		}
	
	}

	//Check to make sure that most of the fish don't swim through walls.
	private bool OutOfBounds()
	{
		Vector3 currPos = gameObject.transform.position;

		if (currPos.x <= -34f || currPos.x >= 26f || currPos.z <= 38 || currPos.z >= 126) {
			return true;
		} else {
			return false;
		}
	}
		



}

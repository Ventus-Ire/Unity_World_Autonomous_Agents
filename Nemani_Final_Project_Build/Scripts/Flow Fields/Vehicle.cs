using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Vehicle script for Autonomous Agents (AA)
// Applied to an autonomous agent
// Currently, Seek method is written

public abstract class Vehicle : MonoBehaviour 
{
	public abstract void CalcSteeringForces ();
	// Vectors a vehicle needs (for force-based movement)
	public Vector3 vehiclePosition;
	public Vector3 direction;
	public Vector3 velocity;
	public Vector3 acceleration;

	// Floats a vehicle needs for AA
	public float mass;			// Using now
	public float maxSpeed;		// Using now
	public float maxForce;		// Using later

	public float wanderAngle = 3f;
	private Vector3 wanderTarget = Vector3.zero;
	private float wanderJitter = 200f;

	//Obstacle Avoidance
	public float safeRadius = 5f;

	// Use this for initialization
	protected virtual void Start () 
	{
		
		// Grab the transform's position 
		vehiclePosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Grab the transform's position - FOR TESTING
		vehiclePosition = transform.position;

		CalcSteeringForces ();

		UpdatePosition ();
		SetTransform ();

	}

	//Updtate Position
	public void UpdatePosition() {
		// Framerate independent movement
		velocity += acceleration * Time.deltaTime;
		vehiclePosition += velocity * Time.deltaTime;

		// "Draw" the object at its position
		transform.position = vehiclePosition;

		// Derive direction from velocity
		direction = velocity.normalized;

		// Start fresh with acceleration
		acceleration = Vector3.zero;
	}

	public void SetTransform() {
		gameObject.transform.rotation = Quaternion.Euler (0.0f, gameObject.transform.rotation.y, 0.0f);
		gameObject.transform.forward = direction;
	}

	// Seek
	// Returns: Seek force (Vector3)
	// Params: Seeking target's position
	// Moves a vehicle at max speed toward location
	public Vector3 Seek(Vector3 targetPosition)
	{
		// Find desired velocity
		Vector3 desiredVelocity = targetPosition - vehiclePosition;

		// Scale to max speed
		desiredVelocity.Normalize();
		desiredVelocity = desiredVelocity * maxSpeed;

		// Calculate steering force
		Vector3 steeringForce = desiredVelocity - velocity;

		// Return steering force
		return steeringForce;
	}

	//FOLLOW FLOW FIELDS
	public Vector3 FollowField(Vector3 position) {
		GameObject target = GameObject.Find ("Plane");
		FlowField script = target.GetComponent<FlowField>();

		position += script.GetFlowDirection (position);
		Vector3 desired = position - vehiclePosition;
		desired.Normalize();
		desired = desired * maxSpeed;
		Vector3 steeringForce = desired - velocity;
		return steeringForce;

	}

	public Vector3 Flee(Vector3 targetPosition)
	{
		// Find desired velocity
		Vector3 desiredVelocity = vehiclePosition - targetPosition;

		// Scale to max speed
		desiredVelocity.Normalize();
		desiredVelocity = desiredVelocity * maxSpeed;

		// Calculate steering force
		Vector3 steeringForce = desiredVelocity - velocity;

		// Return steering force
		return steeringForce;
	}



	// void ApplyForce()
	// Applies an incoming force to the accel vector
	// Forces will accumulate
	public void ApplyForce(Vector3 force)
	{
		// A = F/M
		acceleration += force / (mass * 2);
	}


	//Attempting Obstacle Avoidance
	public Vector3 AvoidObstacle(GameObject obst, float safeDistance)
	{
		Vector3 avoidVector = obst.transform.position - gameObject.transform.position;
		float magnitudeAvoidVector = avoidVector.sqrMagnitude;
		float radiusObj = obst.GetComponent<BoxCollider> ().size.x;
		radiusObj *= .8f;


		if (magnitudeAvoidVector - (radiusObj + safeRadius) > safeDistance) {
			return Vector3.zero;
		} else if (Vector3.Dot (avoidVector, gameObject.transform.forward) < 0) {
			return Vector3.zero;
		} else if ((radiusObj + safeRadius) < Vector3.Dot (avoidVector, gameObject.transform.right)) {
			return Vector3.zero;
		} else {
			float dotProduct = Vector3.Dot (avoidVector, gameObject.transform.right);

			Vector3 desiredVelocity = Vector3.zero;
			Vector3 steeringForce = Vector3.zero;
			if (dotProduct < 0) {

				desiredVelocity = gameObject.transform.right;
				desiredVelocity.Normalize ();
				desiredVelocity *= maxSpeed;
				steeringForce = (desiredVelocity - velocity) * (safeDistance / magnitudeAvoidVector);

			}
			if (dotProduct > 0) {

				desiredVelocity = gameObject.transform.right * -1;
				desiredVelocity.Normalize ();
				desiredVelocity *= maxSpeed;
				steeringForce = (desiredVelocity - velocity) * (safeDistance / magnitudeAvoidVector);

			}
			//Debug.Log (steeringForce);
			return steeringForce;//

		}

	}

	//Returns the closest game object with a tag of "name"
	public GameObject FindClosest(string name) {
		GameObject[] humans;
		humans = GameObject.FindGameObjectsWithTag (name);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject human in humans) {
			Vector3 diff = human.transform.position - position;
			float curDist = diff.sqrMagnitude;
			if (curDist < distance) {
				closest = human;
				distance = curDist;
			}
		}
		return closest;
	}
	/*
	public Vector3 CalcWander(float wanderRadius, float wanderDistance, GameObject obj)
	{
		Vector3 circleCenterM = Vector3.zero;
		Vector3 pointOnCircle = Vector3.zero;
		Vector3 heading = obj.transform.position; //check
		heading.Normalize();
		//Debug.Log(heading);
		wanderTarget += new Vector3 (Random.Range (-wanderJitter, wanderJitter), 0f,
			Random.Range (-wanderJitter, wanderJitter));
		wanderTarget = Vector3.Normalize (wanderTarget);
		wanderTarget *= wanderRadius / 2;
		//Debug.Log (wanderTarget);
		circleCenterM = new Vector3 ((heading.x * wanderDistance) + obj.transform.position.x, 0f,
			                        (heading.z * wanderDistance) + obj.transform.position.z);
		pointOnCircle = new Vector3 (circleCenterM.x + wanderTarget.x, 0f,
			                        circleCenterM.z + wanderTarget.z);
		Debug.Log (pointOnCircle - obj.transform.position);
		return pointOnCircle - obj.transform.position;
	}
	*/

	public Vector3 CalcWander() {
		Vector3 circleCenter;
		circleCenter = velocity;
		circleCenter.Normalize ();
		circleCenter *= 3f;

		Vector3 displacement;
		displacement = new Vector3 (0,0, -1);
		displacement *= 2f;
		displacement += setAngle (displacement, wanderAngle);
		wanderAngle += Random.Range (1f, 4f);

		Vector3 wanderForce;
		wanderForce = circleCenter - displacement;
		return wanderForce;
	}

	private Vector3 setAngle(Vector3 vector, float number) {
		float length = vector.magnitude;
		float x = Mathf.Cos (number) * length;
		float z = Mathf.Sin (number) * length;
		vector = new Vector3 (x, 0, z);
		return vector;
	}

	public Vector3 Separation(GameObject[] allCars, GameObject me)
	{
		int j = 0;
		Vector3 separationForce = Vector3.zero;
		Vector3 averageDirection = Vector3.zero;
		Vector3 distance = Vector3.zero;
		for (int i = 0; i < allCars.Length; i++)
		{
			distance = transform.position - allCars[i].transform.position;
			if (distance.magnitude < 100f && allCars[i] != me)
			{
				j++;
				separationForce += transform.position - allCars[i].transform.position;
				separationForce.Normalize();
				separationForce *= (1 / .7f);
				averageDirection += separationForce;
			}
		}
		if (j == 0)
		{
			return Vector3.zero;
		}
		else
		{
			//averageDirection = averageDirection / j;
			return averageDirection;
		}
	}

}

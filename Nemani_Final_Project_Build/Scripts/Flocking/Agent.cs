using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	public Vector3 position;
	public Vector3 velocity;
	public Vector3 accel;
	public AgentConfig conf;
	public bool draw;

	public World world;

	//public GameObject debugCube;
	// Use this for initialization
	void Start () {
		world = FindObjectOfType<World> ();
		conf = FindObjectOfType<AgentConfig> ();

		position = transform.position;
		velocity = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));

		//debugCube = Instantiate(debugCube);
	}

	// Update is called once per frame
	void Update () {
		accel = combine ();
		accel = Vector3.ClampMagnitude (accel, conf.maxA);

		velocity = velocity + accel * Time.deltaTime;
		velocity = Vector3.ClampMagnitude (velocity, conf.maxV);

		position += velocity * Time.deltaTime;

		wrapAround (ref position, -world.bound, world.bound);

		transform.position = new Vector3(position.x, 250, position.z);

		if (velocity.magnitude > 0) {
			transform.LookAt (new Vector3(position.x, 250, position.z) 
				+ new Vector3(velocity.x, 250, velocity.z));
			//debugCube.transform.LookAt (new Vector3(position.x, 250, position.z) 
				//+ new Vector3(velocity.x, 250, velocity.z));
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

	void OnRenderObject() {
		if (draw == true) {
			Debug.DrawLine (transform.position, new Vector3(position.x, position.y, position.z +2), Color.red);
			Debug.DrawLine (transform.position, transform.position + combine().normalized, Color.green);
		}
	}
	//*******************
	//FLOCKING HERE
	//*******************
	//Cohesion
	Vector3 cohesion() {
		Vector3 r = new Vector3 ();

		var neighs = world.getNeighbors (this, conf.Rc);
		if (neighs.Count == 0) {
			return r;
		}
		foreach (var agent in neighs) {
			r += agent.position;
		}

		r /= neighs.Count;
		//debugCube.transform.position = new Vector3(r.x, 60, r.z);
		r = r - this.position;

		r = Vector3.Normalize (r);

		return r;
	}
	//Seperation between each neighbor
	Vector3 separtion() {
		Vector3 r = new Vector3 ();
		var neighs = world.getNeighbors (this, conf.Rs);
		if (neighs.Count == 0) {
			return r;
		}

		foreach (var agent in neighs) {
			Vector3 towardsMe = this.position - agent.position;
			if (towardsMe.magnitude > 0) {
				r += towardsMe.normalized / towardsMe.magnitude;
			}
		}
		return r.normalized;
	}
	//Alignment for following a direction
	Vector3 alignment() {
		Vector3 r = new Vector3 ();
		var neighs = world.getNeighbors (this, conf.Ra);
		if (neighs.Count == 0) {
			return r;
		}

		foreach (var agent in neighs) {
			r += agent.velocity;
		}

		return r.normalized;
	}
	//Combines all of 3 for a flocking method
	virtual protected Vector3 combine() {
		Vector3 r = conf.Kc * cohesion () + conf.Ks * separtion () + conf.Ka * alignment () + conf.Kw * wander ();
		return r;
	}
	//Keep the birds in line but not make it obvious
	void wrapAround (ref Vector3 v, float min, float max) {
		v.x = wrapAroundFloat (v.x, min, max);
		v.y = wrapAroundFloat (v.y, min, max);
		v.z = wrapAroundFloat (v.z, min, max);
	}
	float wrapAroundFloat(float value, float min, float max) {
		if (value > max)
			value = min;
		else if (value < min)
			value = max;
		return value;
	}

	//For wandering to continuously move through the world
	public Vector3 wanderTarget;

	Vector3 wander() {
		float jitter = conf.WanderJitter * Time.deltaTime;
		wanderTarget += new Vector3 (RandomBinomial () * jitter, 0, RandomBinomial () * jitter);

		wanderTarget = wanderTarget.normalized;

		wanderTarget *= conf.WanderRadius;

		Vector3 local = wanderTarget + new Vector3 (0, 0, conf.WanderDistance);

		Vector3 worldTarget = transform.TransformPoint (local);
		worldTarget -= this.position;
		return worldTarget.normalized;
	}

	float RandomBinomial() {
		return Random.Range (0f, 1f) - Random.Range (0f, 1f);
	}


}

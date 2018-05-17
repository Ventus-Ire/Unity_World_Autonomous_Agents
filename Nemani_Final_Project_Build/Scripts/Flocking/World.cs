using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public Transform agentPrefab;
	public Transform fishPrefab;
	public int nAgents;

	public List<Agent> agents;

	public float bound;
	public float spawnR;

	// Use this for initialization
	void Start () {
		agents = new List<Agent> ();
		spawn (agentPrefab, nAgents);

		agents.AddRange (FindObjectsOfType<Agent> ());
	}
	
	// Update is called once per frame
	void Update () {
		

	}
	//Spawn the birds or the agents
	void spawn(Transform prefab, int n) {
		for (int i = 0; i < n; i++) {
			var obj = Instantiate (prefab, new Vector3 (Random.Range (-spawnR, spawnR), 250, Random.Range (-spawnR, spawnR)), Quaternion.identity);
		}
	}

	//Get the neighbors for each of the agents
	public List<Agent> getNeighbors(Agent agent, float radius) {
		List<Agent> r = new List<Agent> ();
		foreach(var otherAgent in agents) {
			if (otherAgent == agent)
				continue;
			if (Vector3.Distance (agent.position, otherAgent.position) <= radius) {
				r.Add (otherAgent);
			}
		}
		return r;
	}


}

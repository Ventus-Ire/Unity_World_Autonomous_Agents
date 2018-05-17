using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
	//Keep track of the nodes in the game
	public Transform[] nodes;
	public Vector3 GetNode(int id){
		return nodes [id].position;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

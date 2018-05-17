using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField : MonoBehaviour {
	Vector3[,] field;
	int rows = 50;
	int cols = 50;
	int scale = 20;
	float turnAngle;
	Vector3 dirVec;

	// Use this for initialization
	void Start () {
		//Fill the array with a number of vector3s
		field = new Vector3[rows, cols];
		float xOff = 0;
		for (int i = 0; i < cols; i++) {
			float yOff = 0;
			for (int j = 0; j < rows; j++) {
				//Perlin Noise attempt for flow fields, didn't work correctly
				/*
				float theta = Calc (xOff, yOff);
				field [i,j] = new Vector3 (Mathf.Cos (theta) + gameObject.transform.position.x, gameObject.transform.position.y, Mathf.Sin (theta) + gameObject.transform.position.z);
				*/
				//A Random set of numbers for filling th array
				turnAngle = Random.Range (0f, 359f);
				dirVec = Quaternion.Euler (0, turnAngle, 0) * Vector3.right;
				//Debug.Log (Vector3.right);
				field [i, j] = dirVec;
				yOff += 0.2f;
			}
			xOff += 0.2f;
		}


	}


	void Update() {
	
	}
	//Get the vector from teh 2d array, based on the location
	public Vector3 GetFlowDirection(Vector3 location) {
		int column = (int)(location.x/cols);
		int row = (int)(location.z/rows);
		if (column < 0 || column > 49 || row < 0 || row > 49) {
			//Debug.Log(row);
			//Debug.Log(column);
			//Debug.Log(field [0,0]);
			//Debug.Log(field [0,1]);
			//Debug.Log(field [1,0]);
			//Debug.Log(field [2,0]);
			//Debug.Log(field [11,11]);
			return field [25, 25];
		}
		//Debug.Log(row);
		//Debug.Log(column);
		//Debug.Log(field [0,0]);
		return field [column, row];
	}
		
	//Helper Function for perlin noise
	float Calc(float x, float y) {
		float xc = (float)x / 50 * scale;
		float yc = (float)y / 50 * scale;
		return Mathf.PerlinNoise (xc, yc);
	}

	 
		
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public Shape[] allShapes;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public Shape SpawnShape()
	{
		Shape shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
		return shape;
	}

	Shape GetRandomShape()
	{
		int i = Random.Range(0, allShapes.Length);
		return allShapes[i];
	}
}

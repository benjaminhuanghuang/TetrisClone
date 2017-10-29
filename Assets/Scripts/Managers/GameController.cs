using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private Board gameBoard;
	private Spawner spawner;
	private Shape activeShape;

	// Use this for initialization
	void Start () {
		// gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
		// spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

		gameBoard = GameObject.FindObjectOfType<Board>();
		spawner = GameObject.FindObjectOfType<Spawner>();
		
		if(activeShape == null){
			activeShape = spawner.SpawnShape();
		}
		spawner.transform.position = Vectorf.Round(spawner.transform.position);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

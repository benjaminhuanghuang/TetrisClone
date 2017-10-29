using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private Board gameBoard;
	private Spawner spawner;
	private Shape activeShape;

	private float dropInterval = 1f;
	private float timeToDrop   = 0.5f;


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
		if(!gameBoard || !spawner)
			return;
		if(Time.time >= timeToDrop)
		{
			timeToDrop = Time.time + dropInterval;
			if(activeShape)
			{
				activeShape.MoveDown();
				if(!gameBoard.IsValidPosition(activeShape))
				{
					activeShape.MoveUp();
					if(spawner)
					{
						activeShape = spawner.SpawnShape();
					}
				}
			}
		}
	}
}

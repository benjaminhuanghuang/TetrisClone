using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private Board gameBoard;
	private Spawner spawner;
	private Shape activeShape;

	private float dropInterval = 0.5f;
	private float timeToDrop   = 0.5f;


	float timeToNextKeyLeftRight;
	[Range(0.02f, 1f)]
	public float keyRepeatRateLeftRight = 0.15f;

	float timeToNextKeyDown;
	[Range(0.01f, 1f)]
	public float keyRepeatRateDown = 0.01f;

	float timeToNextKeyRotate;
	[Range(0.02f, 1f)]
	public float keyRepeatRateRotate = 0.25f;

	// Use this for initialization
	void Start () {
		// gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
		// spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
		timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
		timeToNextKeyRotate= Time.time + keyRepeatRateRotate;
		timeToNextKeyDown = Time.time + keyRepeatRateDown;

		gameBoard = GameObject.FindObjectOfType<Board>();
		spawner = GameObject.FindObjectOfType<Spawner>();
		
		if(spawner)
		{
			spawner.transform.position = Vectorf.Round(spawner.transform.position);
			if(!activeShape)
			{
				activeShape = spawner.SpawnShape();
			}
		}

	}
	void PlayerInput()
	{
		if(Input.GetButton("MoveRight") && (Time.time > timeToNextKeyLeftRight) || Input.GetButton("MoveRight"))
		{
			activeShape.MoveRight();
			timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

			if(! gameBoard.IsValidPosition(activeShape))
				activeShape.MoveLeft();
		}
		else if(Input.GetButton("MoveLeft") && (Time.time > timeToNextKeyLeftRight) || Input.GetButton("MoveLeft"))
		{
			activeShape.MoveLeft();
			timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

			if(! gameBoard.IsValidPosition(activeShape))
				activeShape.MoveRight();
		}
		else if(Input.GetButton("Rotate") && (Time.time > timeToNextKeyRotate))
		{
			activeShape.RotateRight();
			timeToNextKeyRotate = Time.time + keyRepeatRateRotate ;

			if(! gameBoard.IsValidPosition(activeShape))
				activeShape.RotateLeft();
		}
		else if(Input.GetButton("MoveDown") && (Time.time > timeToNextKeyDown) || (Time.time > timeToDrop))
		{
			timeToDrop = Time.time + dropInterval;
			timeToNextKeyDown = Time.time + keyRepeatRateDown;

			activeShape.MoveDown();
			if(!gameBoard.IsValidPosition(activeShape))
			{
				LandShape();
			}
		}
	}
	void LandShape()
	{
		timeToNextKeyLeftRight = Time.time;
		timeToNextKeyRotate= Time.time;
		timeToNextKeyDown = Time.time;
		
		activeShape.MoveUp();
		gameBoard.StoreShapeInGrid(activeShape);
		activeShape = spawner.SpawnShape();
	}
	// Update is called once per frame
	void Update () {
		if(!gameBoard || !spawner || !activeShape)
			return;
		PlayerInput();
	}
}

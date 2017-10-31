using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private Board gameBoard;
	private Spawner spawner;
	private Shape activeShape;

	public float dropInterval = 0.1f;
	private float timeToDrop   = 0.5f;


	float timeToNextKeyLeftRight;
	[Range(0.02f, 1f)]
	public float keyRepeatRateLeftRight = 0.25f;

	float timeToNextKeyDown;
	[Range(0.01f, 1f)]
	public float keyRepeatRateDown = 0.02f;

	float timeToNextKeyRotate;
	[Range(0.02f, 1f)]
	public float keyRepeatRateRotate = 0.25f;

	bool gameOver = false;

	public GameObject gameOverPanel;
	public SoundManager soundManager;

	public IconToggle rotIconToggle;
	bool clockwise = true;

	public bool isPaused = false;
	public GameObject pausePanel;

	// Use this for initialization
	void Start () {
		// gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
		// spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
		timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
		timeToNextKeyRotate= Time.time + keyRepeatRateRotate;
		timeToNextKeyDown = Time.time + keyRepeatRateDown;

		gameBoard = GameObject.FindObjectOfType<Board>();
		spawner = GameObject.FindObjectOfType<Spawner>();
		soundManager = GameObject.FindObjectOfType<SoundManager>();
		
		if(spawner)
		{
			spawner.transform.position = Vectorf.Round(spawner.transform.position);
			if(!activeShape)
			{
				activeShape = spawner.SpawnShape();
			}
		}

		if(gameOverPanel)
		{
			gameOverPanel.SetActive(false);
		}

		if(pausePanel)
		{
			pausePanel.SetActive(false);
		}
	}
	void PlayerInput()
	{
		if(Input.GetButton("MoveRight") && (Time.time > timeToNextKeyLeftRight))
		{
			activeShape.MoveRight();
			timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

			if(! gameBoard.IsValidPosition(activeShape))
			{
				activeShape.MoveLeft();
				PlaySound(soundManager.errorSound, 0.5f);
			}
			else
			{
				PlaySound(soundManager.moveSound, 0.5f);
			}
		}
		else if(Input.GetButton("MoveLeft") && (Time.time > timeToNextKeyLeftRight))
		{
			activeShape.MoveLeft();
			timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

			if(! gameBoard.IsValidPosition(activeShape))
			{	
				activeShape.MoveRight();
				PlaySound(soundManager.errorSound, 0.5f);
			}
			else{
				PlaySound(soundManager.moveSound, 0.5f);
			}
		}
		else if(Input.GetButton("Rotate") && (Time.time > timeToNextKeyRotate))
		{
			// activeShape.RotateRight();
			activeShape.RotateClockwise(clockwise);
			timeToNextKeyRotate = Time.time + keyRepeatRateRotate ;

			if(! gameBoard.IsValidPosition(activeShape))
			{
				// activeShape.RotateLeft();
				activeShape.RotateClockwise(!clockwise); 
				PlaySound(soundManager.errorSound, 0.5f);
			}
			else
			{
				PlaySound(soundManager.moveSound, 0.5f);	
			}
		}
		else if(Input.GetButton("MoveDown") && (Time.time > timeToNextKeyDown) || (Time.time > timeToDrop))
		{
			timeToDrop = Time.time + dropInterval;
			timeToNextKeyDown = Time.time + keyRepeatRateDown;

			activeShape.MoveDown();
			if(!gameBoard.IsValidPosition(activeShape))
			{
				if(gameBoard.IsOverLimit(activeShape))
				{
					GameOver();
				}
				else {
					LandShape();
				}
			}
		}
	}
	void LandShape()
	{
		activeShape.MoveUp();
		gameBoard.StoreShapeInGrid(activeShape);
		PlaySound(soundManager.dropSound, 0.75f);

		activeShape = spawner.SpawnShape();

		timeToNextKeyLeftRight = Time.time;
		timeToNextKeyRotate= Time.time;
		timeToNextKeyDown = Time.time;

		gameBoard.ClearAllRows();
		PlaySound(soundManager.dropSound, 0.5f);
		if(gameBoard.completeRows > 0)
		{
			if(gameBoard.completeRows > 1)
			{
				PlaySound(soundManager.GetRandomClip(soundManager.vocalClips), 0.5f);	
				gameBoard.completeRows = 0;
			}
			PlaySound(soundManager.clearRowSound, 0.5f);	
		}
	}
	// Update is called once per frame
	void Update () {
		if(!gameBoard || !spawner || !activeShape || gameOver || !soundManager)
			return;
		PlayerInput();
	}
	
	void GameOver()
	{
		activeShape.MoveUp();
		
		if(gameOverPanel)
		{
			gameOverPanel.SetActive(true);
		}
		PlaySound(soundManager.gameOverSound, 2f);

		gameOver = true;

	}

	public void Restart(){
		Time.timeScale = 1f;
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);    
	}

	private void PlaySound(AudioClip clip, float volMultiplier)
	{
		if(clip && soundManager.fxEnabled)
		{
			AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(soundManager.fxVolume * volMultiplier, 0.05f, 1f));
		}
	}

	public void ToggleRoteDirection()
	{
		clockwise = !clockwise;
		if(rotIconToggle)
		{
			rotIconToggle.ToggleIcon(clockwise);
		}
	}

	public void TogglePause()
	{
		if(gameOver)
		{
			return;
		}

		isPaused = !isPaused;
		if(pausePanel)
		{
			pausePanel.SetActive(isPaused);
			if(soundManager)
			{
				soundManager.musicSource.volume = (isPaused) ? soundManager.musicVolume * 0.25f: soundManager.musicVolume;
			}
			Time.timeScale = isPaused ? 0 : 1;
		}
	}
}

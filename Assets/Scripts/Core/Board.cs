using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public Transform emptySprite;
	public int height = 30;
	public int width = 10;
	public int header = 10;

	private Transform[,] grid;

	// Called when the script instance is being loaded.
	void Awake()
	{
		grid = new Transform[width, header];
	}
	// Use this for initialization
	void Start () {
		DrawEmptyCells();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	bool IsWithinBoard(int x, int y)
	{
		return (x > 0 && x < width && y >=0);
	}

	// Called by game controller
	public bool IsValidPosition(Shape shape)
	{
		foreach(Transform child in shape.transform) // Check child
		{
			Vector2 pos = Vectorf.Round(child.position);
			if(!IsWithinBoard((int)pos.x, (int)pos.y))
			{
				return false;
			}
		}

		return true;
	}

	void DrawEmptyCells()
	{
		for(int y=0; y < height - header; y++)
		{
			for (int x =0 ; x< width; x++){
				Transform clone;
				clone = Instantiate(emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
				clone.name = "Board Space ( x= "+ x.ToString() + " , y = " + y.ToString() + " )";
				clone.transform.parent = transform;
			}
		}
	}
}

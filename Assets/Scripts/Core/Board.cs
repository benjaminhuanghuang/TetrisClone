using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public Transform emptySprite;
	public int height = 30;
	public int width = 10;
	public int header = 10;

	private Transform[,] grid;

	public int completeRows = 0;

	// Called when the script instance is being loaded.
	void Awake()
	{
		grid = new Transform[width, height];
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
		return (x >= 0 && x < width && y >=0);
	}

	bool IsOccupied(int x, int y, Shape shape)
	{
		// pos is is occupied by other shape
		return (grid[x, y] != null && grid[x, y].parent != shape.transform);
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
			if(IsOccupied((int)pos.x, (int)pos.y, shape))
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

	// Update board status
	public void StoreShapeInGrid(Shape shape)
	{
		if(shape == null)	return;

		foreach(Transform child in shape.transform)
		{
			Vector2 pos = Vectorf.Round(child.position);
			grid[(int)pos.x, (int)pos.y] = child;
		}
	}

	bool IsComplete(int y)
	{
		for(int x =0 ; x < width; ++x)
		{
			if(grid[x, y] == null)
				return false;
		}
		return true;
	}

	void CleanRow(int y)
	{
		for(int x =0 ; x < width; ++x)
		{
			if(grid[x, y] != null)
				Destroy(grid[x, y].gameObject);

			grid[x, y] = null;
		}
	}

	void ShiftOneRowDown(int y)
	{
		for(int x =0 ; x < width; ++x)
		{
			if(grid[x, y] != null)
			{
				grid[x, y - 1] = grid[x, y];
				grid[x, y] = null;
				grid[x, y - 1].position += new Vector3(0, -1, 0);
			}
		}
	}

	void ShiftRowsDown(int startY)
	{
		for(int y = startY; y < height; ++y)
		{
			ShiftOneRowDown(y);
		}
	}

	public void ClearAllRows()
	{
		for(int y =0; y < height; ++y)
		{
			if(IsComplete(y))
			{
				completeRows++;
				CleanRow(y);
				ShiftRowsDown(y + 1);
				y --;
			}
		}
	}
	public bool IsOverLimit(Shape shape)
	{
		foreach( Transform child in shape.transform)
		{
			if(child.transform.position.y >= height - header - 1)
			{
				return true;
			}
		}
		return false;
	}
}

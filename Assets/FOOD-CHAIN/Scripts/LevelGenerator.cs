using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	enum gridSpace {
		empty, 
		Background, 
		Wall, // plekken waar je op kan klimmen 
		Floor,// blocks waar je op loopt
		Spawn
	
	};
	gridSpace[,] grid;

	private int roomHeight;
	private int roomWidth;

    float worldUnitsInOneGridCell = 1;

	// maken struct aan om walker values vast te houden kan ook een class zijn maar is beetje onnodig 
	private struct walker{
		public Vector2 dir;
		public Vector2 pos;
	}
	private List<walker> walkers;

	[Space]
	[Header("MapValues")]
	public Vector2 MapSize = new Vector2(50, 50);
	public float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
	public float chanceWalkerDestoy = 0.05f;
	public int maxWalkers = 10;
	public float percentToFill = 0.1f;

	[Space]
	[Header("RoomBlocks")]
	public GameObject WallObj;
	public GameObject BackgroundObj;
	public GameObject FloorObj;
	public GameObject SpawnObj;

	[Space]
	[Header("ObjectParents")]
	public Transform WallParent;
	public Transform BackgroundParent;

	void Start () {

		Setup();
		CreateBackgroundAndLayout();
		CreateWalls(); // legt alle blocks 
		RemoveSingleWalls();
		CreateFloors(); // maakt top facing empty blocks een floor ziet er mooier uit
		SetSpawn();
	
		SpawnLevel();
	}
	void Setup(){

	
		roomHeight = Mathf.RoundToInt(MapSize.x / worldUnitsInOneGridCell);
		roomWidth = Mathf.RoundToInt(MapSize.y / worldUnitsInOneGridCell);
	
		grid = new gridSpace[roomWidth,roomHeight];
	
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
			
				grid[x,y] = gridSpace.empty;
			}
		}
	
		walkers = new List<walker>();

		walker newWalker = new walker();
		newWalker.dir = RandomDirection();

		Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth/ 2.0f),Mathf.RoundToInt(roomHeight/ 2.0f));
		newWalker.pos = spawnPos;

		walkers.Add(newWalker);
	}
	void CreateBackgroundAndLayout(){
		int iterations = 0;
		do{
			
			foreach (walker myWalker in walkers){
				grid[(int)myWalker.pos.x,(int)myWalker.pos.y] = gridSpace.Background;
			}
		
			int numberChecks = walkers.Count; 
			for (int i = 0; i < numberChecks; i++){
				
				if (Random.value < chanceWalkerDestoy && walkers.Count > 1){
					walkers.RemoveAt(i);
					break; 
				}
			}
	
			for (int i = 0; i < walkers.Count; i++){
				if (Random.value < chanceWalkerChangeDir){
					walker thisWalker = walkers[i];
					thisWalker.dir = RandomDirection();
					walkers[i] = thisWalker;
				}
			}
		
			numberChecks = walkers.Count; 
			for (int i = 0; i < numberChecks; i++){
			
				if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers){
				
					walker newWalker = new walker();
					newWalker.dir = RandomDirection();
					newWalker.pos = walkers[i].pos;
					walkers.Add(newWalker);
				}
			}
		
			for (int i = 0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
				thisWalker.pos += thisWalker.dir;
				walkers[i] = thisWalker;				
			}
		
			for (int i =0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
			
				thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth-2);
				thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight-2);
				walkers[i] = thisWalker;
			}
	
			if ((float)NumberOfFloors() / (float)grid.Length > percentToFill){
				break;
			}
			iterations++;
		}while(iterations < 100000);
	}
	void CreateWalls(){
	
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
			
				if (grid[x,y] == gridSpace.Background){
			
					if (grid[x,y+1] == gridSpace.empty){
						grid[x,y+1] = gridSpace.Wall;
					}
					if (grid[x,y-1] == gridSpace.empty){
						grid[x,y-1] = gridSpace.Wall;
					}
					if (grid[x+1,y] == gridSpace.empty){
						grid[x+1,y] = gridSpace.Wall;
					}
					if (grid[x-1,y] == gridSpace.empty){
						grid[x-1,y] = gridSpace.Wall;
					}
				}
			}
		}
	}
	void CreateFloors(){
	
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
			
				if (grid[x,y] == gridSpace.Wall)
				{		
					if (grid[x,y+1] == gridSpace.Background){
						grid[x,y] = gridSpace.Floor;
					}				
				}
			}
		}
	}
	void RemoveSingleWalls(){
	
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
		
				if (grid[x,y] == gridSpace.Wall){
				
					bool allFloors = true;
				
					for (int checkX = -1; checkX <= 1 ; checkX++){
						for (int checkY = -1; checkY <= 1; checkY++){
							if (x + checkX < 0 || x + checkX > roomWidth - 1 || 
								y + checkY < 0 || y + checkY > roomHeight - 1){
							
								continue;
							}
							if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0)){
						
								continue;
							}
							if (grid[x + checkX,y+checkY] != gridSpace.Background){
								allFloors = false;
							}
						}
					}
					if (allFloors){
						grid[x,y] = gridSpace.Background;
					}
				}
			}
		}
	}

	private void SetSpawn()
    {
		for (int x = 0; x < roomWidth - 1; x++)
		{
			for (int y = 0; y < roomHeight - 1; y++)
			{

				if (grid[x, y] == gridSpace.Background)
				{
					if (grid[x, y - 1] == gridSpace.Floor)
					{
						grid[x, y] = gridSpace.Spawn;
						return;

					}
				}
			}
		}
	}

	void SpawnLevel(){
		for (int x = 0; x < roomWidth; x++){
			for (int y = 0; y < roomHeight; y++){
				switch(grid[x,y]){
					case gridSpace.empty:
						break;
					case gridSpace.Background:
						Spawn(x,y,BackgroundObj, BackgroundParent);
						break;
					case gridSpace.Wall:
						Spawn(x,y,WallObj, WallParent);
						break;
					case gridSpace.Floor:
						Spawn(x, y, FloorObj, WallParent);
						break;
					case gridSpace.Spawn:
						Spawn(x, y, SpawnObj, BackgroundParent);
						break;
				}
			}
		}
	}

	Vector2 RandomDirection(){
	
		int choice = Mathf.FloorToInt(Random.value * 3.99f);
	
		switch (choice){
			case 0:
				return Vector2.down;
			case 1:
				return Vector2.left;
			case 2:
				return Vector2.up;
			default:
				return Vector2.right;
		}
	}
	int NumberOfFloors(){
		int count = 0;
		foreach (gridSpace space in grid){
			if (space == gridSpace.Background){
				count++;
			}
		}
		return count;
	}
	void Spawn(float x, float y, GameObject toSpawn,Transform parent){

		Vector2 offset = MapSize / 2.0f;
		Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell- offset;

		Instantiate(toSpawn, spawnPos, Quaternion.identity, parent);
		
  
	
		
	}
}

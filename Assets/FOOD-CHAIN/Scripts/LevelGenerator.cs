using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour {

	enum gridSpace {
		empty, 
		Background, 
		Wall, // plekken waar je op kan klimmen 
		Floor,// blocks waar je op loopt
		Spawn,
		Exit,
		OuterWalls,
		HolderTile
	
	};
	gridSpace[,] grid, OuterWallGrid;

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
	public int ChanceForOnBlockDecor = 3;
	public int ChanceForFoliage = 3;
	[Space]
	[Header("RoomBlocks")]


	public GameObject SpawnObj;
	public GameObject ExitObj;

	public GameObject[] FloorBlocks;
	public GameObject[] WallBlocks;
	public GameObject[] BackgroundBlocks;
	public GameObject[] Foliage;
	public GameObject[] OnBlockDecor;

	[Space]
	[Header("ObjectParents")]
	public Transform WallParent;
	public Transform BackgroundParent;

	public GameObject TestEnemie;
	public GameObject GeneratedMap;

	
	public TileBase MainAutoTile;
	public TileBase BackgroundAutoTile;
	

	public GameObject MapBlocker;
	public Tilemap DestructibleWallLayer;
	public Tilemap BackgroundLayer;

	[HideInInspector]
	public List<Transform> SpawnPoints = new List<Transform>();

	public DestructibleTerrain destructibleTerrain;
	public static LevelGenerator levelGenerator;

	public GameObject spawnPoint;

	public GameObject[] UsableItems;

    private void Awake()
    {
		levelGenerator = this;
	}

    public void SpawnMap()
	{
		Setup();
		CreateBackgroundAndLayout();
		CreateWalls(); // legt alle blocks 
		RemoveSingleWalls();
		CreateFloors(); // maakt top facing empty blocks een floor ziet er mooier uit
		SetExit();
		SetSpawn();

		SpawnWalls();
		CreateMapBoundry();

		SpawnLevel();

		SpawnProps();

	
	}

	public void RemoveMap()
	{
		SpawnPoints.Clear();
		DestructibleWallLayer.ClearAllTiles();
		BackgroundLayer.ClearAllTiles();
		Destroy(GeneratedMap);
	}


	void Setup(){

		

		GeneratedMap = new GameObject();
		GeneratedMap.name = "Generated map";

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

		Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth/ 2f),Mathf.RoundToInt(roomHeight/ 2f));
	
		newWalker.pos = spawnPos;

		walkers.Add(newWalker);


	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
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
				//else
				//if (grid[x, y] == gridSpace.Wall)
				//{

				//	if (grid[x, y + 1] == gridSpace.empty)
				//	{
				//		grid[x, y + 1] = gridSpace.Wall;
				//	}
				//	if (grid[x, y - 1] == gridSpace.empty)
				//	{
				//		grid[x, y - 1] = gridSpace.Wall;
				//	}
				//	if (grid[x + 1, y] == gridSpace.empty)
				//	{
				//		grid[x + 1, y] = gridSpace.Wall;
				//	}
				//	if (grid[x - 1, y] == gridSpace.empty)
				//	{
				//		grid[x - 1, y] = gridSpace.Wall;
				//	}
				//}
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

	private void SpawnWalls()
    {
        for (int i = 0; i < 5; i++)
        {
			for (int x = 0; x < roomWidth - 1; x++)
			{
				for (int y = 0; y < roomHeight - 1; y++)
				{

					if (grid[x, y] == gridSpace.Wall)
					{
						// check of dit in de grid bestaat
						if (CheckSafePlace(x, y + 1))
						{
							if (grid[x, y + 1] == gridSpace.empty)
							{
								grid[x, y + 1] = gridSpace.OuterWalls;
							}
						}
						if (CheckSafePlace(x, y - 1))
						{

							if (grid[x, y - 1] == gridSpace.empty)
							{
								grid[x, y - 1] = gridSpace.OuterWalls;
							}
						}
						if (CheckSafePlace(x + 1, y))
						{
							if (grid[x + 1, y] == gridSpace.empty)
							{
								grid[x + 1, y] = gridSpace.OuterWalls;
							}
						}
						if (CheckSafePlace(x - 1, y))
						{
							if (grid[x - 1, y] == gridSpace.empty)
							{
								grid[x - 1, y] = gridSpace.OuterWalls;
							}
						}



					}


				}
			}

			for (int x = 0; x < roomWidth - 1; x++)
			{
				for (int y = 0; y < roomHeight - 1; y++)
				{




					if (grid[x, y] == gridSpace.OuterWalls)
					{
						// check of dit in de grid bestaat
						if (CheckSafePlace(x, y + 1))
						{
							if (grid[x, y + 1] == gridSpace.empty)
							{
								grid[x, y + 1] = gridSpace.Wall;
							}
						}
						if (CheckSafePlace(x, y - 1))
						{

							if (grid[x, y - 1] == gridSpace.empty)
							{
								grid[x, y - 1] = gridSpace.Wall;
							}
						}
						if (CheckSafePlace(x + 1, y))
						{
							if (grid[x + 1, y] == gridSpace.empty)
							{
								grid[x + 1, y] = gridSpace.Wall;
							}
						}
						if (CheckSafePlace(x - 1, y))
						{
							if (grid[x - 1, y] == gridSpace.empty)
							{
								grid[x - 1, y] = gridSpace.Wall;
							}
						}


					}
				}





			}
		}
      
	}

	// spawn dit nadat de map is gespawnd
	private void SpawnProps()
    {
  
		for (int x = 0; x < roomWidth - 1; x++)
		{
			for (int y = 0; y < roomHeight - 1; y++)
			{

				if (grid[x, y] == gridSpace.Background)
				{
					if (grid[x, y - 1] == gridSpace.Floor && grid[x, y+1] == gridSpace.Background)
					{
						int rand = Random.Range(0, 8);
						if (rand == 1)
						{
							Spawn(x, y, TestEnemie, GeneratedMap.transform);
						}		
						

					
					}


					

				}
          				
			}
		}

		StartCoroutine(GameManager.gameManager.WaveManager());
	}



	private void SetExit()
    {
		for (int x = roomWidth; x-- > 0; )
		{
			for (int y = roomHeight; y-- > 0;)
			{

				if (grid[x, y] == gridSpace.Background)
				{
					if (grid[x, y - 1] == gridSpace.Floor)
					{
						grid[x, y] = gridSpace.Exit;
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
					//	SpawnRandomObject(x, y, WallBlocks, GeneratedMap.transform, true);
						break;
					case gridSpace.Background:
					//	SpawnRandomObject(x, y, BackgroundBlocks, GeneratedMap.transform,true);
						SpawnTile(x, y, true);
					
						break;
					case gridSpace.Wall:

						SpawnTile(x, y,false);
						SpawnRandomObject(x, y, OnBlockDecor, GeneratedMap.transform,false);
						break;
					case gridSpace.Floor:

						SpawnTile(x, y,false);
						SpawnRandomObject(x, y, OnBlockDecor, GeneratedMap.transform,false);
						SpawnRandomObject(x, y+1, Foliage, GeneratedMap.transform, false);

						GameObject go = Instantiate(spawnPoint);
						int posX = x;
						posX -= 25;
						int posY = y;
						posY -= 24;
						go.transform.position = new Vector2(posX, posY);
						go.transform.SetParent(GeneratedMap.transform);
						SpawnPoints.Add(go.transform);

						break;
					case gridSpace.Spawn:
						Spawn(x, y, SpawnObj, GeneratedMap.transform);
						break;
					case gridSpace.Exit:
						Spawn(x, y, ExitObj,  GeneratedMap.transform);
						break;
					case gridSpace.OuterWalls:
						SpawnTile(x, y,false);
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

	private void SpawnTile(int x,int y,bool isBackground)
    {
        if (isBackground)
        {
			BackgroundLayer.SetTile(new Vector3Int(x, y, 0), BackgroundAutoTile);
		}
        else
        {
			DestructibleWallLayer.SetTile(new Vector3Int(x, y, 0), MainAutoTile);
		}
			
		// als we een tile gaan destroyenn checken we gwn of de positie binnen de van deze tile binnen de 
		// room with en room height is als dit nie zo is dan mogen we hem niet destr
	  

    }

	private void SpawnRandomObject(int x, int y, GameObject[] Objects, Transform Parent,bool isBackground)
	{
        if (isBackground)
        {
			int index = Random.Range(0, Objects.Length);
			GameObject go = Objects[index];

			GameObject Decor = Spawn(x, y, go, Parent);
		}
        else
        {
			int rand = Random.Range(0, 5);
			if (rand == 1)
			{
				
				int index = Random.Range(0, Objects.Length);
				GameObject go = Objects[index];

				GameObject Decor = Spawn(x, y, go, Parent);
				
			}
		}
	
	
		


	}

	GameObject Spawn(float x, float y, GameObject toSpawn,Transform parent){

		Vector2 offset = MapSize / 2.0f;
		Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell- offset;

	  return	Instantiate(toSpawn, spawnPos, Quaternion.identity, parent);
		
  
	
		
	}

	public bool CheckSafePlace(int x, int y)
	{

		bool cellExists = Mathf.Min(grid.GetLength(0) - 1, Mathf.Abs(x)) == x && Mathf.Min(grid.GetLength(1) - 1, Mathf.Abs(y)) == y;

		return cellExists;

	}

	private void CreateMapBoundry()
    {
		int WallThickness = 7;
		//OuterWallGrid = new gridSpace[roomWidth*2, roomHeight*2];
		for (int x = 0; x < roomWidth; x++)
		{
			for (int y = 0; y < roomHeight; y++)
			{

				if (grid[x, y] == gridSpace.Wall || grid[x, y] == gridSpace.OuterWalls || grid[x, y] == gridSpace.Floor)
				{
                    // check of dit in de grid bestaat


                    // spawn een layer van walls om de laatste bestaande walls 
                    // als je een block gaat destroyen check je of die binnen de map radius valt 
                    // als dit zo is destroy hem en spawn een nieuwe background erop
                    for (int i = 0; i < WallThickness; i++)
                    {
						if (CheckSafePlace(x + i, y))
						{
							if (grid[x + i, y] == gridSpace.empty)
							{
								SpawnTile(x + i, y,false);
								PlaceMapBlocker(i, WallThickness, x + i, y);
							}
                        }
                        else
                        {
							SpawnTile(x + i, y, false);
							PlaceMapBlocker(i, WallThickness, x + i, y);
						}

						if (CheckSafePlace(x - i, y))
						{
							if (grid[x - i, y] == gridSpace.empty)
							{
								SpawnTile(x - i, y, false);
								PlaceMapBlocker(i, WallThickness, x - i, y);
							}
                        }
                        else
						{
							SpawnTile(x - i, y, false);
							PlaceMapBlocker(i, WallThickness, x-i, y);
						}

						if (CheckSafePlace(x, y + i))
						{
							if (grid[x, y + i] == gridSpace.empty)
							{
								SpawnTile(x, y + i, false);
								PlaceMapBlocker(i, WallThickness, x, y + i);
							}
                        }
                        else
                        {
							SpawnTile(x, y + i, false);
							PlaceMapBlocker(i, WallThickness, x, y + i);
						}

						if (CheckSafePlace(x, y - i))
						{
							if (grid[x, y - i] == gridSpace.empty)
							{
								SpawnTile(x, y - i, false);
								PlaceMapBlocker(i, WallThickness, x, y - i);
							}
                        }
                        else
                        {
							SpawnTile(x, y - i, false);
							PlaceMapBlocker(i, WallThickness, x, y - i);
						}

						if (CheckSafePlace(x+i, y - i))
						{
							if (grid[x+i, y - i] == gridSpace.empty)
							{
								SpawnTile(x+i, y - i, false);
								PlaceMapBlocker(i, WallThickness, x + i, y - i);
							}
						}
						else
						{
							SpawnTile(x+i, y - i, false);
							PlaceMapBlocker(i, WallThickness, x + i, y - i);
						}

						if (CheckSafePlace(x-i, y + i))
						{
							if (grid[x-i, y + i] == gridSpace.empty)
							{
								SpawnTile(x-i, y + i, false);
								PlaceMapBlocker(i, WallThickness, x - i, y + i);
							}
						}
						else
						{
							SpawnTile(x-i, y + i, false);
							PlaceMapBlocker(i, WallThickness, x - i, y + i);
						}
						

					}

                 






				}


			}
		}

		
		

	}
	private void PlaceMapBlocker(int i, int WallThickness, int x, int y)
	{
		int holder = WallThickness;
		holder -= 1;
		if (i == holder)
		{
			GameObject go = Spawn(x, y, MapBlocker, GeneratedMap.transform);
		}
	}

	
}



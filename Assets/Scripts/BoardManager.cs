using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public class Count
    {
        public int Minimum;
        public int Maximum;

        public Count(int Min, int Max)
        {
            Minimum = Min;
            Maximum = Max;
        }
    }

    public int Columns = 8;
    public int Rows = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject Exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();

        for(int x=1;x<Columns-1;x++)
        {
            for(int y=1;y<Rows;y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for(int x=-1;x<Columns+1;x++)
        {
            for(int y=-1;y<Rows+1;y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == Columns || y == -1 || y == Rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject Instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                Instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int Minimum, int Maximum)
    {
        int objectCount = Random.Range(Minimum, Maximum + 1);
        for(int i=0;i<objectCount;i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }

    }

    public void SetupScene(int Level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.Minimum, wallCount.Maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.Minimum, foodCount.Maximum);
        int enemyCount = (int)Mathf.Log(Level, 2);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Debug.Log(enemyCount);
        Instantiate(Exit, new Vector3(Columns - 1, Rows - 1, 0f), Quaternion.identity);
    }
	void Start () {


		
	}
	

	void Update () {
		
	}
}

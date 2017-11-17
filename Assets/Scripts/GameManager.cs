using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    public BoardManager boardManager;

    int Level = 3;


	void Awake () {

        if(Instance==null)
        {
            Instance = this;
        }
        else if(Instance!=null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardManager = GetComponent<BoardManager>();
        InitGame();
		
	}
	
    void InitGame()
    {
        boardManager.SetupScene(Level);
        
    }

	void Update () {
		
	}
}

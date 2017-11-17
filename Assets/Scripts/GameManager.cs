using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = .1f;

    public static GameManager Instance = null;
    public BoardManager boardManager;

    public int playerFoodPoints = 100;

    [HideInInspector]
    public bool playersTurn = true;

    Text levelText;
    GameObject levelImage;

    int Level = 0;
    List<Enemy> Enemies;
    bool enemiesMoving;
    bool doingSetup;

	void Awake () {

        if(Instance==null)
        {
            Instance = this;
        }
        else if(Instance!=this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Enemies = new List<Enemy>();

        boardManager = GetComponent<BoardManager>();
        //InitGame();
		
	}
	
    private void OnLevelFinishedLoading(Scene _Scene, LoadSceneMode Mode)
    {
        Level++;
        InitGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("levelImage");
        levelText = GameObject.Find("levelText").GetComponent<Text>();
        levelText.text = "Day " + Level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);


        Enemies.Clear();
        boardManager.SetupScene(Level);
        
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

	void Update () {

        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
	}

    public void GameOver()
    {
        levelText.text="After " + Level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    public void AddEnemyToList(Enemy Script)
    {
        Enemies.Add(Script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(Enemies.Count==0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i=0; i<Enemies.Count;i++)
        {
            Enemies[i].MoveEnemy();
            yield return new WaitForSeconds(Enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}

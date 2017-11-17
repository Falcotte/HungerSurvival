using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int wallDamage = 1;

    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    Animator _Animator;
    int Food;

	protected override void Start () {

        _Animator = GetComponent<Animator>();
        Food = GameManager.Instance.playerFoodPoints;

        base.Start();
		
	}

    void Update()
    {
        if (!GameManager.Instance.playersTurn) return;

        int Horizontal = 0;
        int Vertical = 0;

        Horizontal = (int)Input.GetAxisRaw("Horizontal");
        Vertical = (int)Input.GetAxisRaw("Vertical");

        if (Horizontal != 0)
            Vertical = 0;

        if(Horizontal!=0 || Vertical!=0)
        {
            AttemptMove<Wall>(Horizontal, Vertical);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        _Animator.SetTrigger("playerChop");
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag=="Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if(Other.tag=="Food")
        {
            Food += pointsPerFood;
            Other.gameObject.SetActive(false);
        }
        else if(Other.tag=="Soda")
        {
            Food += pointsPerSoda;
            Other.gameObject.SetActive(false);
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int Loss)
    {
        _Animator.SetTrigger("playerHit");
        Food -= Loss;
        CheckIfGameOver();
    }
    private void OnDisable()
    {
        GameManager.Instance.playerFoodPoints = pointsPerFood;
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        Food--;
        base.AttemptMove<T>(xDir, yDir);
        CheckIfGameOver();
        GameManager.Instance.playersTurn = false;
    }

	private void CheckIfGameOver()
    {
        if(Food<=0)
        {
            GameManager.Instance.GameOver();
        }
    }
}

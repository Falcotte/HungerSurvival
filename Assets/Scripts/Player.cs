using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

    public int wallDamage = 1;

    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    Animator _Animator;
    int Food;

	protected override void Start () {

        _Animator = GetComponent<Animator>();
        Food = GameManager.Instance.playerFoodPoints;

        foodText.text = "Food: " + Food;

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
            foodText.text = "+" + pointsPerFood + " Food: " + Food;
            SoundManager.Instance.RandomizeFx(eatSound1, eatSound2);
            Other.gameObject.SetActive(false);
        }
        else if(Other.tag=="Soda")
        {
            Food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + Food;
            SoundManager.Instance.RandomizeFx(drinkSound1, drinkSound2);
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
        foodText.text = "-" + Loss + " Food: " + Food;
        CheckIfGameOver();
    }
    private void OnDisable()
    {
        GameManager.Instance.playerFoodPoints = Food;
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        Food--;
        foodText.text = "Food: " + Food;
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D Hit;
        if(Move(xDir,yDir, out Hit))
        {
            SoundManager.Instance.RandomizeFx(moveSound1, moveSound2);
        }
        CheckIfGameOver();
        GameManager.Instance.playersTurn = false;
    }

	private void CheckIfGameOver()
    {
        if(Food<=0)
        {
            SoundManager.Instance.PlaySingle(gameOverSound);
            SoundManager.Instance.musicSource.Stop();
            GameManager.Instance.GameOver();
        }
    }
}

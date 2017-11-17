using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite;
    public int Hp = 4;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    SpriteRenderer spriteRenderer;

	void Awake () {

        spriteRenderer = GetComponent<SpriteRenderer>();
		
	}

    public void DamageWall(int Loss)
    {
        SoundManager.Instance.RandomizeFx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        Hp -= Loss;

        if (Hp <= 0)
            gameObject.SetActive(false);
    }

}

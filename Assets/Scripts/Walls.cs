using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    public Sprite dmgSprite;
    public int Hp = 4;

    SpriteRenderer spriteRenderer;

	void Awake () {

        spriteRenderer = GetComponent<SpriteRenderer>();
		
	}

    public void DamageWall(int Loss)
    {
        spriteRenderer.sprite = dmgSprite;
        Hp -= Loss;

        if (Hp <= 0)
            gameObject.SetActive(false);
    }

}

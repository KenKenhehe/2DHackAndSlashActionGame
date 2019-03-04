using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGeneralHandler : MonoBehaviour {
    public Color OriginColor;
    public Color DamagedColor;
    public int healthPoints;
    public Text healthPointText;

    SceneEventHandler sceneEventHandler;
    Animator animator;
    SpriteRenderer renderer;
    GameStateSwitch gameSwitch;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        sceneEventHandler = FindObjectOfType<SceneEventHandler>();
        gameSwitch = FindObjectOfType<GameStateSwitch>();
        healthPointText.text = healthPoints.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeEnemyDamage()
    {
        healthPoints -= 10;
        healthPointText.text = healthPoints.ToString();
        if(healthPoints <= 0)
        {
            PlayerDead();
        }
        StartCoroutine(DamageEffect(0));
    }

    IEnumerator DamageEffect(int damage)
    {
        renderer.color = DamagedColor;

        yield return new WaitForSeconds(.2f);

        renderer.color = OriginColor;
    }

    void DeactivateControl()
    {
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void PlayerDead()
    {
        animator.SetTrigger("Dead");
        DeactivateControl();
        sceneEventHandler.gameOver = true;
        gameSwitch.ShowGameOverState(false);
        renderer.sortingOrder = 2;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }
}

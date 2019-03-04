using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public PlayerMovement target;
    public float speed;
    Rigidbody2D rb2d;
    public EnemyShooterController enemy;

    Vector2 moveDirection;

	// Use this for initialization
	void Start () {
        target = FindObjectOfType<PlayerMovement>();
        rb2d = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2(
            (target.transform.position.x - transform.position.x > 0 ? 1 : -1) * speed * Time.deltaTime, 
            0);
        rb2d.velocity = moveDirection;
        Destroy(gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    private void FixedUpdate()
    {
        if (target != null)
        {
            DetactMissHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerAttack>() != null)
        {
            Destroy(gameObject);
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerGeneralHandler>().TakeEnemyDamage();
        }
    }

    void DetactMissHit()
    {
        if(Vector2.Distance(transform.position, target.transform.position) <= 1f)
        {
            if(target.GetComponent<PlayerMovement>() != null && target.GetComponent<PlayerMovement>().dodging == true)
            {
                Time.timeScale = .01f;
            }
        }
    }
}

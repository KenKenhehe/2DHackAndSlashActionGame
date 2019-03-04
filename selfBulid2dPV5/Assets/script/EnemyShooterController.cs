using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : MonoBehaviour {
    
    PlayerAttack playerToFocus;
    public GameObject bulletPref;
    public GameObject bloodFX;
    public float speed;
    public float maxX;
    public float minX;
    public Color damagedColor;
    public Color originColor;
    public bool facingRight;

    //set to random on instantiate.
    float respondRange;
    int health;
    float fireRate;
    //

    public bool takingDamage = false;
    float fireTime = 0;
    Animator animator;
    ShakeController shakeController;
    SpriteRenderer renderer;
    Rigidbody2D rb2d;
    WaitForSeconds hitDuration = new WaitForSeconds(0.05f);
    SceneEventHandler sceneEventHandler;
	// Use this for initialization
	void Start () {
        sceneEventHandler = FindObjectOfType<SceneEventHandler>();
        playerToFocus = FindObjectOfType<PlayerAttack>();
        fireRate = Random.Range(1.5f, 3);
        health = Random.Range(10, 20);
        respondRange = Random.Range(3.5f, 8);
        renderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        shakeController = FindObjectOfType<ShakeController>();
        facingRight = false;
        animator = GetComponent<Animator>();
        originColor = renderer.color;
        //StartCoroutine(ShootAtPlayer());
	}
	
	// Update is called once per frame
	void Update () {
        if (playerToFocus != null && sceneEventHandler.gameOver == false)
        {
            MoveTowardsPlayer();
            FacePlayer();
            ShootAtPlayer();
            //OnlyMoveBetween(minX, maxX);
        }
	}

    void OnlyMoveBetween(float minX, float maxX)
    {
        if (transform.position.x >= maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
    }


    void ShootAtPlayer()
    {
        fireTime += Time.deltaTime;
        if (fireTime >= fireRate && takingDamage == false)
        {
            Instantiate(bulletPref, transform.position, Quaternion.identity);
            fireTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void FacePlayer()
    {
        if(playerToFocus.transform.position.x > transform.position.x)
        {
            facingRight = true;
            renderer.flipX = true;
        }
        else
        {
            facingRight = false;
            renderer.flipX = false;
        }
    }

    public void TakeDamage(int damage)
    {
        GameObject bloodfX = Instantiate<GameObject>(bloodFX, transform);
        bloodfX.transform.Rotate(0, facingRight ? 0 : 180, 0);
        health -= damage;
        animator.SetTrigger("Damaged");
        shakeController.CamShake();
        if (health <= 1)
        {
            shakeController.CamShake();
            Destroy(gameObject);
        }
        StartCoroutine(DamagedEffect());
        StartCoroutine(DamagedState());
    }

    public IEnumerator DamagedEffect()
    {
        renderer.color = damagedColor;

        yield return hitDuration;

        renderer.color = originColor;
    }

    IEnumerator DamagedState()
    {
        takingDamage = true;

        yield return new WaitForSeconds(.5f);

        takingDamage = false;
    }

    void MoveTowardsPlayer()
    {
        if(playerToFocus.transform.position.x - respondRange > transform.position.x)
        {
            rb2d.velocity = Vector2.right * speed;

        }
        else if(playerToFocus.transform.position.x + respondRange < transform.position.x)
        {
            rb2d.velocity = Vector2.left * speed;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
        }

        if(rb2d.velocity != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
        }
    }
}

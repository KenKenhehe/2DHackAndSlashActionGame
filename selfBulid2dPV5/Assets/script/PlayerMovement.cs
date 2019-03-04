using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public GameObject dodgeFXRight;
    public GameObject dodgeFXLeft;

    public float speed;
    public float dodgeSpeed;
    public float walkSpeed;
    public float jumpHeight;
    public float horizontalMovement;
    public bool canTurn = true;
    
    public bool dodging = false;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public bool facingRight = true;
    public bool canJump = true;
    Rigidbody2D rb2d;
    public float screenMinX = -10;
    public float screenMaxX = 13;

    PlayerAttack playerAttack;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAttack = GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void Update () {     
        MovementInput();
        OnlyMoveBetween(screenMinX, screenMaxX);
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

    void MovementInput()
    {
        if (canTurn == true)
        {
            horizontalMovement = Input.GetAxis("Horizontal");
        }
        rb2d.velocity = new Vector2(horizontalMovement * speed * Time.deltaTime, rb2d.velocity.y);

        //walk animation
        if (horizontalMovement != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else if (horizontalMovement == 0)
        {
            animator.SetBool("isWalking", false);
        }
        //facing which way
        if(horizontalMovement < 0)
        {            
            spriteRenderer.flipX = true;
            facingRight = false;
        }
        else if(horizontalMovement > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }

        if (Input.GetKeyDown(KeyCode.L) && playerAttack.attacking == false)
        {
            Dodge();
        }
    }

    void Dodge()
    {
        if (facingRight == true)
        {
            Instantiate(dodgeFXRight, new Vector3(
                transform.position.x + 1f,
                transform.position.y,
                transform.position.z), Quaternion.identity);
        }
        else if(facingRight == false)
        {
            Instantiate(dodgeFXLeft, new Vector3(
                transform.position.x - 1f,
                transform.position.y,
                transform.position.z), Quaternion.identity);
        }
        animator.SetTrigger("Dodging");
        StartCoroutine(DoudgeEffect());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            canJump = true;
        } 
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            canJump = false;
        }
    }

    IEnumerator DoudgeEffect()
    {
        dodging = true;
        if (horizontalMovement == 0)
        {
            rb2d.velocity = new Vector2(
                facingRight == true ? (dodgeSpeed) * Time.deltaTime : -(dodgeSpeed) * Time.deltaTime, 
                rb2d.velocity.y
                );
        }
        else {
            rb2d.velocity = new Vector2((dodgeSpeed + speed) * horizontalMovement * Time.deltaTime, rb2d.velocity.y);
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(.3f);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        speed = walkSpeed;
        dodging = false;
    }
}

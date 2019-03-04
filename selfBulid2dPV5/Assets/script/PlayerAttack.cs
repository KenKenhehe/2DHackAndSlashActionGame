using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    public float friction;
    public bool attacking;

    public float attack1Range;
    public float attack2Range;
    public float attack3Range;

    public float attackForce;
    public float attackRangeOffset = 0;
    public float startAttackingTime;
    public LayerMask enemyLayerMask;

    public int attack1Damage;
    public int attack2Damage;
    public int attack3Damage;

    float startComboTime;
    float resetTimer = 1f;

    float timesBetweenAttack;
    float attackLastTime = .24f;
    WaitForSeconds attackAfterSecond = new WaitForSeconds(0.05f);

    Animator animator;
    Rigidbody2D rb2d;
    PlayerMovement playerMovement;
    BoxCollider2D bx2d;
    EnemyShooterController enemyBeingHit;
    ShakeController shakeController;
    SpriteRenderer renderer;
    public string[] attackTriggerNames = new string[] { "Attack", "Attack2", "Attack3" };

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        bx2d = GetComponent<BoxCollider2D>();
        shakeController = FindObjectOfType<ShakeController>();
        renderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        ProcessInupt();
	}

    void ProcessInupt()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            attacking = true;
            PlayAttackAnimationOnAttackNum();
        }
        if (attacking == true)
        {
            animator.SetBool("isWalking", false);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("KnightWalk"))
        {
            foreach(string attackTriggerName in attackTriggerNames)
            {
                if(attackTriggerName != "Attack")
                {
                    animator.ResetTrigger(attackTriggerName);
                    attackLastTime = .24f;
                }
            }
        }

        if (attacking)
        {
            playerMovement.speed = 30;
        }
        else if (attacking == false)
        {
            playerMovement.speed = playerMovement.walkSpeed;
        }
    }

    //the moment sprite became of of the 4 attack1 sprite, apply damage to enemy(if in range)
    void AttackAtRightTime(int damage, float range, float shockForce)
    {
        Vector3 attackRangeOrigin = playerMovement.facingRight ?
        transform.position + new Vector3(attackRangeOffset, 0, 0) : transform.position - new Vector3(attackRangeOffset, 0, 0);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackRangeOrigin, range, enemyLayerMask);
        foreach (Collider2D obj in hitObjects)
        {
            obj.GetComponent<EnemyShooterController>().TakeDamage(damage);
            obj.transform.position = new Vector3(
                (playerMovement.facingRight == true ? obj.transform.position.x + shockForce : obj.transform.position.x - shockForce),
                obj.transform.position.y,
                obj.transform.position.z);
        }
    }

    //only plays the animation, damage is handled elsewhere...
    void PlayAttackAnimationOnAttackNum()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("KnightWalk"))
        {
            animator.SetTrigger(attackTriggerNames[0]);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("KinghtAttack1"))
        {
            animator.SetTrigger(attackTriggerNames[1]);
            attackLastTime = .48f;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            animator.SetTrigger(attackTriggerNames[2]);
            attackLastTime = .9f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + new Vector3(attackRangeOffset, 0, 0), attack3Range);
    }

    void Attack1()
    {
        AttackAtRightTime(attack1Damage, attack1Range, .3f);
    }

    void Attack2()
    {
        AttackAtRightTime(attack2Damage, attack2Range, .4f);
    }

    void Attack3()
    {
        AttackAtRightTime(attack3Damage, attack3Range, 1f);
    }

    void DisableAttack()
    {
        attacking = false;
    }

    void EnableFriction()
    {
        if (playerMovement.dodging == false)
        {
            playerMovement.speed = 10;
        }
    }

    void DisableFriction()
    {
        playerMovement.speed = playerMovement.walkSpeed;
    }
}

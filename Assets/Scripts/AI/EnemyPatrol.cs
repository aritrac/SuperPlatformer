using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides a simple patrolling behaviour between two positions
/// </summary>
public class EnemyPatrol : MonoBehaviour {

    public Transform leftBound, rightBound;                 // left/right boundaries between which enemy moves
    public float speed;                                     // speed at which enemy moves
    public float maxDelay, minDelay;                        // for random delay before enemy turns back

    bool canTurn;                                           // to check when the enemy can turn
    float originalSpeed;                                    // will help in turning the enemy
    Rigidbody2D rb;
    SpriteRenderer sr;                                      // helps in flipping player direction
    Animator anim;                                          // to show the animations

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        SetStartingDirection();
        canTurn = true;
	}
	
	void Update () {
        Move();
        FlipOnEdges();
	}

    private void Move()
    {
        Vector2 temp = rb.velocity;
        temp.x = speed;
        rb.velocity = temp;
    }

    void SetStartingDirection()
    {
        if (speed > 0)
            sr.flipX = true;
        else if (speed < 0)
            sr.flipX = false;
    }

    void FlipOnEdges()
    {
        if(sr.flipX && transform.position.x >= rightBound.position.x)
        {
            if (canTurn)
            {
                canTurn = false;
                originalSpeed = speed;
                speed = 0;
                StartCoroutine("TurnLeft", originalSpeed);
            }
        }
        else if (!sr.flipX && transform.position.x <= leftBound.position.x)
        {
            if (canTurn)
            {
                canTurn = false;
                originalSpeed = speed;
                speed = 0;
                StartCoroutine("TurnRight", originalSpeed);
            }
        }
    }

    IEnumerator TurnLeft(float originalSpeed)
    {
        anim.SetBool("isIdle", true);       //transitioning from walking to idle animation during the delay
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        anim.SetBool("isIdle", false);       //transitioning from idle to walking animation post the delay
        sr.flipX = false;
        speed = -originalSpeed;
        canTurn = true;
    }

    IEnumerator TurnRight(float originalSpeed)
    {
        anim.SetBool("isIdle", true);       //transitioning from walking to idle animation during the delay
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        anim.SetBool("isIdle", false);       //transitioning from idle to walking animation post the delay
        sr.flipX = true;
        speed = -originalSpeed;
        canTurn = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(leftBound.position, rightBound.position);
    }
}

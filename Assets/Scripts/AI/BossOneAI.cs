using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The AI Engine of Level One Boss
/// </summary>
public class BossOneAI : MonoBehaviour {

    public float jumpSpeed;                         //the jumping speed of the level boss in y axis
    public int startJumpingAt;                      //the health level at which the level boss starts jumping
    public int jumpDelay;                           //delay in seconds before another jump
    public int health;                              //the health of the level boss
    public Slider bossHealth;                       //health meter of the level boss
    public GameObject bossBullet;                   //the bullet which level boss fires
    public float delayBeforeFiring;                 //delay in seconds before firing bullet

    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector3 bulletSpawnPos;                    //this is where the bullet will be fired from
    bool canFire, isJumping;                        //to check when boss can fire and jump

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        canFire = false;

        bulletSpawnPos = gameObject.transform.Find("BulletSpawnPos").transform.position;   //Using Find instead of FindChild as it is deprecated

        Invoke("Reload", Random.Range(1f, delayBeforeFiring));
	}
	
	void Update () {
        if (canFire)
        {
            FireBullets();
            canFire = false;

            if(health < startJumpingAt && !isJumping)
            {
                InvokeRepeating("Jump", 0, jumpDelay);
                isJumping = true;
            }
        }
	}

    void Reload()
    {
        canFire = true;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpSpeed));
    }

    void FireBullets()
    {
        Instantiate(bossBullet, bulletSpawnPos, Quaternion.identity);

        Invoke("Reload", delayBeforeFiring);
    }

    void RestoreColor()
    {
        sr.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if (health == 0)
            {
                //show enemy destroyed SFX animation
                GameCtrl.instance.BulletHitEnemy(gameObject.transform); //time to kill and remove the boss from scene

                //play the boss explosion audio
                AudioCtrl.instance.EnemyExplosion(gameObject.transform.position);
            }
            if(health > 0)
            {
                health--;                                               //reducing the boss health
                bossHealth.value = (float)health;

                //Change the boss color momentarily to show bullet hit effect
                sr.color = Color.red;

                //Restore the original color of the boss
                Invoke("RestoreColor", 0.1f);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the player's bullet movement and collisions with the enemies
/// </summary>
public class PlayerBulletCtrl : MonoBehaviour {

    //1. declare a variable rb of type Rigidbody2D and a public Vector2 velocity
    //2. get the ref to the Rigidbody2D in the Start method
    //3. assign the velocity in the Update method
    //4. test

    public Vector2 velocity;
    Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();	
	}

    void Update()
    {
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameCtrl.instance.BulletHitEnemy(collision.gameObject.transform);
            Destroy(gameObject);
        }
        else if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Untagged") && !collision.gameObject.CompareTag("BigCoin"))    //destroys the bullet if it hit anything apart from Enemy
        {
            Destroy(gameObject);
        }
    }
}

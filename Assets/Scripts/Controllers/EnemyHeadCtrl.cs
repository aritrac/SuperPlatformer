using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the enemy behaviour when the player jumps on the enemy's head
/// </summary>
public class EnemyHeadCtrl : MonoBehaviour {

    public GameObject enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFeet"))
        {
            GameCtrl.instance.PlayerStompsEnemy(enemy);

            //play the enemy stomped effect
            AudioCtrl.instance.EnemyHit(enemy.transform.position);

            //Show the enemy stomped SFX
            SFXCtrl.instance.ShowEnemyPoof(enemy.transform.position);
        }
    }
}

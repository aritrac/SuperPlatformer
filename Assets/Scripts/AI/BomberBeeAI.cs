using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// The AI Engine of the Bomber Bee
/// </summary>
public class BomberBeeAI : MonoBehaviour {

    public float destroyBeeDelay;                               //how long to wait before bee is destroyed
    public float beeSpeed;                                      //speed at which bee moves

    public void ActivateBee(Vector3 playerPos)
    {
        transform.DOMove(playerPos, beeSpeed, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            //show explosion
            SFXCtrl.instance.EnemyExplosion(collision.gameObject.transform.position);

            //play explosion sound for bomber bee explosion
            AudioCtrl.instance.EnemyExplosion(collision.gameObject.transform.position);

            //destroy bee
            Destroy(gameObject, destroyBeeDelay);
        }
    }
}

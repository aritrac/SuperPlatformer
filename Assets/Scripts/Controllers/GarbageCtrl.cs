using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys any game object that comes in contact with it
/// For the player, the level is restarted
/// </summary>
public class GarbageCtrl : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameCtrl.instance.PlayerDied(collision.gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}

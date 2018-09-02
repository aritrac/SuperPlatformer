using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows the level complete menu
/// </summary>
public class LevelDone : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameCtrl.instance.LevelComplete();
        }
    }
}

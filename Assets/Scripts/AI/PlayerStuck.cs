using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks when the player is stuck
/// </summary>
public class PlayerStuck : MonoBehaviour {

    public GameObject player;               //to get access to the PlayerCtrl script

    PlayerCtrl playerCtrl;                  //to reference the PlayerCtrl script

	void Start () {
        playerCtrl = player.GetComponent<PlayerCtrl>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCtrl.isStuck = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerCtrl.isStuck = false;
    }
}

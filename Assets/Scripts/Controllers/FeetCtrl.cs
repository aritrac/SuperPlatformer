using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helps in showing the dust particle effects when the player lands
/// </summary>
public class FeetCtrl : MonoBehaviour {

    public GameObject player;
    public Transform dustParticlePos;

    PlayerCtrl playerCtrl;

    private void Start()
    {
        playerCtrl = gameObject.transform.parent.gameObject.GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.gameObject.GetComponent<PlayerCtrl>().SFXOn)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                SFXCtrl.instance.ShowPlayerLanding(dustParticlePos.position);
                playerCtrl.isJumping = false;
            }

            if (collision.gameObject.CompareTag("Platform"))
            {
                SFXCtrl.instance.ShowPlayerLanding(dustParticlePos.position);
                playerCtrl.isJumping = false;
                player.transform.parent = collision.gameObject.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            player.transform.parent = null;
        }
    }
}

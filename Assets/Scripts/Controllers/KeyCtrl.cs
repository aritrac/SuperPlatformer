using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates the HUD when a key is collected by the player
/// </summary>
public class KeyCtrl : MonoBehaviour {

    public int keyNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Update the key count in game data
            GameCtrl.instance.UpdateKeyCount(keyNumber);

            //play the key found effect
            AudioCtrl.instance.KeyFound(collision.gameObject.transform.position);

            //show the key sparkle SFX
            SFXCtrl.instance.ShowKeySparkle(keyNumber);

            //remove the key from scene
            Destroy(gameObject);
        }
    }
}

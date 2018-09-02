using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// checks for collisions with a breakable gameobject and then informs the SFXCtrl to do the needful
/// </summary>

public class HeadCtrl : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Breakable"))
        {
            //inform the SFXCtrl to do the needful
            SFXCtrl.instance.HandleBoxBreaking(collision.gameObject.transform.parent.transform.position);

            //set the velocity of the cat to zero
            gameObject.transform.parent.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }
}

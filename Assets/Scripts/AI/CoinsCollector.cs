using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script "receives" the coins which fly towards it when player picks them
/// </summary>
public class CoinsCollector : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }
}

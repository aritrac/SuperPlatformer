using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the platform's dropping behaviour
/// </summary>
public class DroppingPlatform : MonoBehaviour {

    public float droppingDelay;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerFeet"))
        {
            Invoke("StartDropping", droppingDelay);
        }
    }

    void StartDropping()
    {
        rb.isKinematic = false;
    }
}

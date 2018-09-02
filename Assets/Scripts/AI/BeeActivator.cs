using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates the Bomber Bee when the player comes near it
/// </summary>
public class BeeActivator : MonoBehaviour {

    public GameObject bee;                          //public reference to the bomber bee

    BomberBeeAI bbai;                               //the AI engine of the bomber bee

	void Start () {
        bbai = bee.GetComponent<BomberBeeAI>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bbai.ActivateBee(collision.gameObject.transform.position);
        }
    }
}

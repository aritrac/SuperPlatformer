using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Releases the Dog from the cage
/// </summary>
public class LeverCtrl : MonoBehaviour {

    public GameObject dog;                                  //the gameobject Dog
    public Vector2 jumpSpeed;                               //speed at which the dog jumps out of the cage
    public GameObject[] stairs;                             //stairs through which the dog jumps
    public Sprite levelPulled;                              //the image of the pulled lever

    Rigidbody2D rb;                                         //reference to the rb of the Dog
    SpriteRenderer sr;                                      //reference to the Sprite Renderer of the Lever

	void Start () {
        rb = dog.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.AddForce(jumpSpeed);

            foreach(GameObject stair in stairs)
            {
                stair.GetComponent<BoxCollider2D>().enabled = false;
            }
            SFXCtrl.instance.ShowPlayerLanding(gameObject.transform.position);
            sr.sprite = levelPulled;

            Invoke("ShowLevelCompleteMenu", 3f);
        }
    }

    void ShowLevelCompleteMenu()
    {
        GameCtrl.instance.LevelComplete();
    }
}

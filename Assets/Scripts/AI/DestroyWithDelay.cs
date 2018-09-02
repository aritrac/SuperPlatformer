using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// destroys the game object after a specified delay 
/// </summary>
public class DestroyWithDelay : MonoBehaviour {

    public float delay;         //set this to 1.5

	void Start () {
        Destroy(gameObject, delay);
	}
	
	
	void Update () {
		
	}
}

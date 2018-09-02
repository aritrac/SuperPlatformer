using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Ads in the game
/// </summary>
public class AdsCtrl : MonoBehaviour {

    public static AdsCtrl instance = null;

	void Start () {
		if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

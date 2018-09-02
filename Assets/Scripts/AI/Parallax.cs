using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides the Parallax Effect
/// </summary>
public class Parallax : MonoBehaviour {

    public float speed;                     // speed at which the BG moves. Set this to 0.001
    public GameObject player;               // to get access to the PlayerCtrl script

    float offSetX;                          //this is the secret to horizontal parallax
    Material mat;                           //the material attached to the renderer of the Quad
    PlayerCtrl playerCtrl;                  //this script has the access to the isStuck variable

	void Start () {
        mat = GetComponent<Renderer>().material;
        playerCtrl = player.GetComponent<PlayerCtrl>();
	}
	
	void Update () {
        if (!playerCtrl.isStuck)
        {
            //shows the parallax
            //handles the keyboard and joystick
            offSetX += Input.GetAxisRaw("MyHorizontal") * speed;    //This is for keyboard where Input.GetAxisRaw will return -1 0 and 1

            //handles the mobile parallax
            if (playerCtrl.leftPressed)                             //This case is for mobile UI left press
                offSetX += -speed;
            else if (playerCtrl.rightPressed)                       //This case is for mobile UI right press
                offSetX += speed;

            mat.SetTextureOffset("_MainTex", new Vector2(offSetX, 0));
        }
	}
}

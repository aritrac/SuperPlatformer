using System;
using UnityEngine;

/// <summary>
/// groups the particle effects used in the game
/// </summary>

[Serializable]
public class SFX {

    public GameObject sfx_coin_pickup;          //shown when the player picks coins
    public GameObject sfx_bullet_pickup;        //shown when the player picks the bullet power up
    public GameObject sfx_playerLands;          //shown when the player lands after jumping
    public GameObject sfx_fragment_1;           //box fragment shown when the crate breaks
    public GameObject sfx_fragment_2;           //box fragment shown when the crate breaks 
    public GameObject sfx_splash;               //the splash effect
    public GameObject sfx_enemy_explosion;      //shown when the player bullet hits the enemy
}

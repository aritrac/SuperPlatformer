using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;                                       //allows you to use the [serializable] attribute

/// <summary>
/// Contains audio clips related to the player
/// </summary>
[Serializable]
public class PlayerAudio{

    [Header("Part 1")]
    public AudioClip playerJump;                    //when player jumps
    public AudioClip coinPickup;                    //when player collects a coin
    public AudioClip fireBullets;                   //when player fire bullets
    public AudioClip enemyExplosion;                //when player kills an enemy
    public AudioClip breakCrates;                   //when player breaks a crate

    [Header("Part 2")]
    public AudioClip waterSplash;                   //when player falls in water
    public AudioClip powerUp;                       //when player collects bullet powerup
    public AudioClip keyFound;                      //when player collects level keys
    public AudioClip enemyHit;                      //when player jumps on enemy head
    public AudioClip playerDied;                    //when player dies
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the audio in the game
/// </summary>
public class AudioCtrl : MonoBehaviour {

    public static AudioCtrl instance;                       //for calling public methods in this script
    public PlayerAudio playerAudio;                         //for accessing player audio effects
    public AudioFX audioFX;                                 //for accessing non player audio
    public Transform player;                                //we need this to play sound at player position
    public GameObject BGMusic;                              //to toggle BG Music
    public GameObject btnSound, btnMusic;                   //Sound & Music toggle buttons in Pause Menu
    public Sprite imgSoundOn, imgSoundOff;                  //sprites for sound on/off states
    public Sprite imgMusicOn, imgMusicOff;                  //sprites for music on/off states

    [Tooltip("soundOn is used to toggle sound on/off from the inspector")]
    public bool soundOn;

    [Tooltip("bgMusicOn is used to toggle background music on/off from the inspector")]
    public bool bgMusicOn;

	void Start () {
        if (instance == null)
            instance = this;

        if (DataCtrl.instance.data.playMusic)
        {
            BGMusic.SetActive(true);
            btnMusic.GetComponent<Image>().sprite = imgMusicOn;
        }
        else
        {
            BGMusic.SetActive(false);
            btnMusic.GetComponent<Image>().sprite = imgMusicOff;
        }

        if (DataCtrl.instance.data.playSound)
        {
            soundOn = true;
            btnSound.GetComponent<Image>().sprite = imgSoundOn;
        }
        else
        {
            soundOn = false;
            btnSound.GetComponent<Image>().sprite = imgSoundOff;
        }
    }
	
	public void PlayerJump(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.playerJump, playerPos);
        }
    }

    public void CoinPickup(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.coinPickup, playerPos);
        }
    }

    public void FireBullets(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.fireBullets, playerPos);
        }
    }

    public void EnemyExplosion(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.enemyExplosion, playerPos);
        }
    }

    public void BreakableCrates(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.breakCrates, playerPos);
        }
    }

    public void WaterSplash(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.waterSplash, playerPos);
        }
    }

    public void PowerUp(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.powerUp, playerPos);
        }
    }

    public void KeyFound(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.keyFound, playerPos);
        }
    }

    public void EnemyHit(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.enemyHit, playerPos);
        }
    }

    public void PlayerDied(Vector3 playerPos)
    {
        if (soundOn)
        {
            AudioSource.PlayClipAtPoint(playerAudio.playerDied, playerPos);
        }
    }

    /// <summary>
    /// Toggles sound.
    /// </summary>
    public void ToggleSound()
    {
        if (DataCtrl.instance.data.playSound)
        {
            //turn off all sounds
            soundOn = false;

            //set the sound off image to the sound button
            btnSound.GetComponent<Image>().sprite = imgSoundOff;

            //save the change to the database
            DataCtrl.instance.data.playSound = false;
        }
        else
        {
            //turn on all sounds
            soundOn = true;

            //set the sound on image to the sound button
            btnSound.GetComponent<Image>().sprite = imgSoundOn;

            //save the change to the database
            DataCtrl.instance.data.playSound = true;
        }
    }

    /// <summary>
    /// Toggles music.
    /// </summary>
    public void ToggleMusic()
    {
        if (DataCtrl.instance.data.playMusic)
        {
            //stop the BG Music
            BGMusic.SetActive(false);

            //set the music off image to the music button
            btnMusic.GetComponent<Image>().sprite = imgMusicOff;

            //save the change to the database
            DataCtrl.instance.data.playMusic = false;
        }
        else
        {
            //play the BG Music
            BGMusic.SetActive(true);

            //set the music on image to the music button
            btnMusic.GetComponent<Image>().sprite = imgMusicOn;

            //save the change to the database
            DataCtrl.instance.data.playMusic = true;
        }
    }
}

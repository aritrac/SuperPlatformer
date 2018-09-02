using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;   //RSFB helps Serialization
using DG.Tweening;                                      //This is DoTween which we use for animation

/// <summary>
/// Manages the important gameplay features like keeping the score, restarting levels,
/// saving/loading data, updating the HUD etc
/// </summary>

public class GameCtrl : MonoBehaviour {

    public static GameCtrl instance;
    public float restartDelay;

    [HideInInspector]
    public GameData data;                               //to work with game data in inspector
    public UI ui;                                       //for neatly arranging UI elements
    public GameObject bigCoin;                          //reward the cat gets on killing the enemy
    public GameObject player;                           //the cat game character
    public GameObject lever;                            //the level which releases the dog
    public GameObject enemySpawner;                     //spawns the enemies during boss battle
    public GameObject signPlatform;                     //the one that leads to the boss battle
    
    public int coinValue;                               //value of one small coin
    public int bigCoinValue;                            //value of one big coin
    public int enemyValue;                              //value of one enemy
    public float maxTime;                               //max time allowed to complete the level

    public enum Item
    {
        Coin,
        BigCoin,
        Enemy
    }

    string dataFilePath;                                //path to store the game data file
    BinaryFormatter bf;                                 //helps in saving/loading to binary files   
    float timeLeft;                                     //time left before the timer goes off
    bool timerOn;                                       //checks if timer should be on or off
    bool isPaused;                                      //to pause/unpause the game

    void Awake() {
        if (instance == null)
            instance = this;

        bf = new BinaryFormatter();

        dataFilePath = Application.persistentDataPath + "/game.dat";

        Debug.Log(dataFilePath);
    }

    void Start () {
        DataCtrl.instance.RefreshData();
        data = DataCtrl.instance.data;
        RefreshUI();

        //LevelComplete();

        timeLeft = maxTime;
        timerOn = true;
        isPaused = false;

        HandleFirstBoot();

        UpdateHearts();

        ui.bossHealth.gameObject.SetActive(false);
	}
	
	
	void Update () {
        if (isPaused)
        {
            //set Time.timeScale = 0
            Time.timeScale = 0;
        }
        else
        {
            //set Time.timeScale = 1
            Time.timeScale = 1;
        }
        if (timeLeft > 0 && timerOn)
            UpdateTimer();
	}

    public void RefreshUI()
    {
        ui.txtCoinCount.text = " x " + data.coinCount;
        ui.txtScore.text = "Score: " + data.score;
    }

    private void OnEnable()
    {
 //     Debug.Log("Data Loaded");
        RefreshUI();
    }

    private void OnDisable()
    {
        //     Debug.Log("Data Saved");
        DataCtrl.instance.SaveData(data);

        Time.timeScale = 1;
    }

    /// <summary>
    /// Saves the stars awarded for a level
    /// </summary>
    /// <param name="levelNumber">The level number</param>
    /// <param name="numOfStars">The star count</param>
    public void SetStarsAwarded(int levelNumber, int numOfStars)
    {
        data.levelData[levelNumber].starsAwarded = numOfStars;

        //print star count in console for testing
        Debug.Log("Number of Stars Awarded = " + data.levelData[levelNumber].starsAwarded);
    }

    /// <summary>
    /// Marks the next level from the current level number specified as unlocked
    /// </summary>
    /// <param name="levelNumber">The next level for this level number will be unlocked</param>
    public void UnlockLevel(int levelNumber)
    {
        data.levelData[levelNumber].isUnlocked = true;
    }

    /// <summary>
    /// Gets the current score for level complete menu
    /// </summary>
    /// <returns>The score.</returns>
    public int GetScore()
    {
        return data.score;
    }

    /// <summary>
    /// restarts the level when the player dies
    /// </summary>
    public void PlayerDied(GameObject player)
    {
        player.SetActive(false);
        CheckLives();
        //Invoke("RestartLevel", restartDelay);
    }

    public void PlayerDiedAnimation(GameObject player)
    {
        //throw the player back in the air
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-150f,400f));

        //rotate the player a bit
        player.transform.Rotate(0,0,45f);

        //disable the PlayerCtrl script
        player.GetComponent<PlayerCtrl>().enabled = false;

        //disable the colliders attached to the player so that the player can fall through the ground
        foreach(Collider2D coll2D in player.transform.GetComponents<Collider2D>())
        {
            coll2D.enabled = false;
        }

        //disable the child gameobjects attached to the player cat
        foreach(Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }

        //disable the camera attached to the player cat
        Camera.main.GetComponent<CameraCtrl>().enabled = false;

        //set the velocity of the cat to zero
        rb.velocity = Vector2.zero;

        //restart level
        StartCoroutine("PauseBeforeReload", player);    //Using this instead of Invoke() as invoke only takes method name and delay, but not method param which is needed in this case
    }

    public void PlayerStompsEnemy(GameObject enemy)
    {
        //change the enemy's tag so that the enemy cannot kill the cat
        enemy.tag = "Untagged";

        //destroy the enemy
        Destroy(enemy);

        //update the score
        UpdateScore(Item.Enemy);
    }

    IEnumerator PauseBeforeReload(GameObject player)
    {
        yield return new WaitForSeconds(1.5f);  //causes a specified delay
        PlayerDied(player);
    }

    /// <summary>
    /// restarts the level when the player falls in water
    /// </summary>
    public void PlayerDrowned(GameObject player)
    {
        //throw the player back in the air
        //Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        //rb.AddForce(new Vector2(-150f, 400f));

        //rotate the player a bit
        player.transform.Rotate(0, 0, 45f);

        //disable the PlayerCtrl script
        player.GetComponent<PlayerCtrl>().enabled = false;

        //disable the colliders attached to the player so that the player can fall through the ground
        foreach (Collider2D coll2D in player.transform.GetComponents<Collider2D>())
        {
            coll2D.enabled = false;
        }

        //disable the child gameobjects attached to the player cat
        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }

        //disable the camera attached to the player cat
        Camera.main.GetComponent<CameraCtrl>().enabled = false;

        //set the velocity of the cat to zero
        //rb.velocity = Vector2.zero;

        //restart level
        StartCoroutine("PauseBeforeReload", player);    //Using this instead of Invoke() as invoke only takes method name and delay, but not method param which is needed in this case
    }

    public void UpdateCoinCount()
    {
        data.coinCount += 1;
        ui.txtCoinCount.text = " x " + data.coinCount;
        UpdateScore(Item.Coin);
    }

    public void UpdateScore(Item item)
    {
        int itemValue = 0;

        switch (item)
        {
            case Item.BigCoin:
                itemValue = bigCoinValue;
                break;
            case Item.Coin:
                itemValue = coinValue;
                break;
            case Item.Enemy:
                itemValue = enemyValue;
                break;
            default:
                break;
        }

        data.score += itemValue;
        ui.txtScore.text = "Score: " + data.score;
    }

    /// <summary>
    /// Called when the player bullet hits the enemy
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void BulletHitEnemy(Transform enemy)
    {
        //show the enemy explosion SFX
        Vector3 pos = enemy.position;
        pos.z = 20f;                                //making the explosion come forward so that it is not covered by parallax layers or other stuff
        SFXCtrl.instance.EnemyExplosion(pos);

        //show the big coin
        Instantiate(bigCoin, pos, Quaternion.identity);

        //destroy the enemy
        Destroy(enemy.gameObject);

        //update the score

        //play the enemy explosion sound
        AudioCtrl.instance.EnemyExplosion(pos);

    }

    public void UpdateKeyCount(int keyNumber)
    {
        data.keyFound[keyNumber] = true;

        if (keyNumber == 0)
            ui.key0.sprite = ui.key0Full;
        else if (keyNumber == 1)
            ui.key1.sprite = ui.key1Full;
        else if (keyNumber == 2)
            ui.key2.sprite = ui.key2Full;

        if (data.keyFound[0] && data.keyFound[1])   //If the player has the blue and green key, then activate the SignPlatform
            ShowSignPlatform();
    }

    void ShowSignPlatform()
    {
        signPlatform.SetActive(true);                                           //show the sign platform

        SFXCtrl.instance.ShowPlayerLanding(signPlatform.transform.position);    //show the dust effect while it appears

        timerOn = false;

        ui.bossHealth.gameObject.SetActive(true);
    }

    public void LevelComplete()
    {
        if (timerOn)
            timerOn = false;

        ui.panelMobileUI.SetActive(false);
        ui.levelCompleteMenu.SetActive(true);
    }

    void RestartLevel()
    {
        Debug.Log("SCENE LOADED -> " + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateTimer()
    {
        timeLeft -= Time.deltaTime;
        ui.txtTimer.text = "Timer: " + (int)timeLeft;

        if (timeLeft <= 0)
        {
            ui.txtTimer.text = "Timer: 0";

            //inform the GameCtrl to do the needful
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            PlayerDied(player);
        }
    }

    void HandleFirstBoot()
    {
        if (data.isFirstBoot)
        {
            // set lives to 3
            data.lives = 3;
            //set number of coins to 0
            data.coinCount = 0;
            //set keys collected to 0
            for (int keyNumber = 0; keyNumber <= 2; keyNumber++)
            {
                data.keyFound[keyNumber] = false;
            }
            //set score to 0
            data.score = 0;
            //set isFirstBoot to false
            data.isFirstBoot = false;
        }
    }

    void UpdateHearts()
    {
        if(data.lives == 3)
        {
            ui.heart1.sprite = ui.fullHeart;
            ui.heart2.sprite = ui.fullHeart;
            ui.heart3.sprite = ui.fullHeart;
        }
        if(data.lives == 2)
        {
            ui.heart1.sprite = ui.emptyHeart;
        }

        if(data.lives == 1)
        {
            ui.heart1.sprite = ui.emptyHeart;
            ui.heart2.sprite = ui.emptyHeart;
        }
    }

    void CheckLives()
    {
        int updatedLives = data.lives;
        updatedLives -= 1;
        data.lives = updatedLives;

        if(data.lives == 0)
        {
            Debug.Log("Life Becomes 0");
            data.lives = 3;
            DataCtrl.instance.SaveData(data);
            Invoke("GameOver", restartDelay);
        }
        else
        {
            Debug.Log("Life Decreased");
            DataCtrl.instance.SaveData(data);
            Invoke("RestartLevel", restartDelay);
        }
    }

    public void StopCameraFollow()
    {
        Camera.main.GetComponent<CameraCtrl>().enabled = false;             //stops the camera from following the player
        player.GetComponent<PlayerCtrl>().isStuck = true;                   //stops parallax from happening
        player.transform.Find("Left_Check").gameObject.SetActive(false);    
        player.transform.Find("Right_Check").gameObject.SetActive(false);
    }

    public void ShowLever()
    {
        lever.SetActive(true);
        DeactivateEnemySpawner();
        SFXCtrl.instance.ShowPlayerLanding(lever.gameObject.transform.position);
        AudioCtrl.instance.EnemyExplosion(lever.gameObject.transform.position);
    }

    public void ActivateEnemySpawner()
    {
        enemySpawner.SetActive(true);
    }

    public void DeactivateEnemySpawner()
    {
        enemySpawner.SetActive(false);
    }

    void GameOver()
    {
        // todo
        //ui.panelGameOver.SetActive(true);

        //set timer off if active
        if (timerOn)
            timerOn = false;

        if (ui.panelMobileUI.activeInHierarchy)
            ui.panelMobileUI.SetActive(false);

        //show Game Over menu with sliding animation
        ui.panelGameOver.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0,0.7f,false);
    }

    /// <summary>
    /// Shows the pause panel
    /// </summary>
    public void ShowPausePanel()
    {
        if (ui.panelMobileUI.activeInHierarchy)
            ui.panelMobileUI.SetActive(false);
        //show the pause menu
        ui.panelPause.SetActive(true);

        //animate the pause panel
        ui.panelPause.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0,0.7f, false);

        Invoke("SetPause", 1.1f);
    }

    void SetPause()
    {
        // set the bool
        isPaused = true;
    }

    /// <summary>
    /// Hides the pause panel
    /// </summary>
    public void HidePausePanel()
    {
        isPaused = false;

        if (!ui.panelMobileUI.activeInHierarchy)
            ui.panelMobileUI.SetActive(true);
        //hide the pause menu
        //ui.panelPause.SetActive(false);

        //animate the pause panel
        ui.panelPause.gameObject.GetComponent<RectTransform>().DOAnchorPosY(600f, 0.7f, false);


    }
}

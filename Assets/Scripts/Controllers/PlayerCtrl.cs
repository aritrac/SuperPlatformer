using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages:
/// 1. the player movement and flipping
/// 2. the player animations
/// </summary>
public class PlayerCtrl : MonoBehaviour {

    //First set of instructions for moving the player left and right

    //1. make 2 methods called MoveHorizontal & StopMoving
    //2. get the input using GetAxisRaw and call these methods in Update()
    //3. we need RigidBody2D of the player to move it so get a reference to it
    //4. write code to move/stop the player
    //5. Test
    //6. Increase player speed

    //Second set of instructions for making the player jump

    //1. make 1 private method called jump
    //2. declare a public float variable jumpSpeed
    //3. write code inside Jump method
    //4. call the Jump method from Update when player presses jump button
    //5. test the code written

    //Third set of instructions for making the player flip along the x axis based on the left or right arrow
    //1. declare a private variable of type sprite renderer and name it sr
    //2. get a reference to the SpriteRenderer component and store it in sr
    //3. write code inside MoveHorizontal to flip the player
    //4. test the code written

    //Fourth set of instructions for changing animations based on the current event such as being idle, running, jumping, hurt and falling
    //1. declare a private variable of type Animator and name it anim
    //2. get a reference to the Animator component and store it in anim
    //3. write code for running animation. Test. Stopping animation. Test
    //4. write code for jumping animation. show jump animation not showing.
    //  4.1 declare a boolean isJumping to show jump animation and use it Jump(), MoveHorizontal() & StopMoving()
    //  4.2 show the OnCollisionEnter2D approach, then show the problem with this. then show groundCheck
    //  4.3 declare isGrounded, make the groundCheck game object, allow jump only if isGrounded = true
    //5. write code for falling animation
    //  5.1 make a method called HandleJumpAndFall
    //  5.2 if player is jumping then change animations based on y velocity
    //  State Values:
    //  Idle        = 0
    //  Running     = 1
    //  Jumping     = 2
    //  Falling     = 3
    //  Hurt        = -1
    //6. we will write code for hurt animation in Section 8 when we create enemies

    //Fifth set of instructions to stop multi jump of the player
    //1. declare public bool isGrounded, Transform feet, float feetRadius, LayerMask whatIsGround
    //2. show Physics2D.OverlapCircle() method to check if player is grounded
    //3. then show preferred way for this cat by using Physics2D.OverLapBox

    [Tooltip("this is a positive integer which speeds up the player movement")]

    public int speedBoost;      //set this to 5
    public float jumpSpeed;     //set this to 600
    public bool isGrounded;
    public Transform feet;
    public float feetRadius;
    public LayerMask whatIsGround;
    public float boxWidth;
    public float boxHeight;
    public float delayForDoubleJump;
    public Transform leftBulletSpawnPosition, rightBulletSpawnPosition;
    public GameObject leftBullet, rightBullet;
    public bool SFXOn;
    public bool canFire;
    public bool isJumping;
    public bool isStuck;
    public bool leftPressed, rightPressed;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    bool canDoubleJump;

	void Start () {
        rb =   GetComponent<Rigidbody2D>();
        sr =   GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
	}
	
	void Update () {

        //isGrounded = Physics2D.OverlapCircle(feet.position, feetRadius, whatIsGround);

        isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x, feet.position.y), new Vector2(boxWidth, boxHeight), 360.0f, whatIsGround);

        float playerSpeed = Input.GetAxisRaw("MyHorizontal"); //value will be 1, -1 or 0
        playerSpeed *= speedBoost; //playerSpeed = playerSpeed * speedBoost

        if (playerSpeed != 0) //If player is not stationary, then call the MoveHorizontal method
            MoveHorizontal(playerSpeed);
        else
            StopMoving();   //If player is stationary, then call the StopMoving method

        if (Input.GetButtonDown("MyJump"))
            Jump();

        if (Input.GetButtonDown("Fire1"))
        {
            FireBullets();
        }

        ShowFalling();

        if (leftPressed)
            MoveHorizontal(-speedBoost);

        if (rightPressed)
            MoveHorizontal(speedBoost);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(feet.position, feetRadius);

        Gizmos.DrawWireCube(feet.position,new Vector3(boxWidth, boxHeight, 0));
    }

    void MoveHorizontal(float playerSpeed)
    {
        rb.velocity = new Vector2(playerSpeed,rb.velocity.y);

        if (playerSpeed < 0)        //player is moving in the left direction
            sr.flipX = true;        //hence flipping of sprite to left direction is being done here
        else if (playerSpeed > 0)   //player is moving in the right direction
            sr.flipX = false;       //hence flipping of sprite to right direction is being done here
        if(!isJumping)
            anim.SetInteger("State",1);
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);

        if(!isJumping)
            anim.SetInteger("State", 0);
    }

    void ShowFalling()
    {
        if(rb.velocity.y < 0)
        {
            anim.SetInteger("State",3);
        }
    }

    void Jump()
    {
        if (isGrounded) { 
            isJumping = true;
            rb.AddForce(new Vector2(0,jumpSpeed)); //simply make the player jump in the y axis or upwards
            anim.SetInteger("State", 2);

            //play the jump sound
            AudioCtrl.instance.PlayerJump(gameObject.transform.position);
            Invoke("EnableDoubleJump", delayForDoubleJump);
        }

        if(canDoubleJump && !isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpSpeed)); //simply make the player jump in the y axis or upwards
            anim.SetInteger("State", 2);

            //play the jump sound
            AudioCtrl.instance.PlayerJump(gameObject.transform.position);

            canDoubleJump = false;
        }
    }

    void EnableDoubleJump()
    {
        canDoubleJump = true;

    }

    void FireBullets()
    {
        if (canFire)
        {
            //makes the player fire bullets in the left direction
            if (sr.flipX)
            {
                Instantiate(leftBullet, leftBulletSpawnPosition.position, Quaternion.identity);
            }

            //makes the player fire bullets in the right direction
            if (!sr.flipX)
            {
                Instantiate(rightBullet, rightBulletSpawnPosition.position, Quaternion.identity);
            }

            AudioCtrl.instance.FireBullets(gameObject.transform.position);
        }
    }

    public void MobileMoveLeft()
    {
        leftPressed = true;
    }

    public void MobileMoveRight()
    {
        rightPressed = true;
    }

    public void MobileStop()
    {
        leftPressed = false;
        rightPressed = false;

        StopMoving();
    }

    public void MobileFireBullets()
    {
        FireBullets();
    }

    public void MobileJump()
    {
        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameCtrl.instance.PlayerDiedAnimation(gameObject);

            AudioCtrl.instance.PlayerDied(gameObject.transform.position);
        }
        if (collision.gameObject.CompareTag("BigCoin"))
        {
            GameCtrl.instance.UpdateCoinCount();
            SFXCtrl.instance.ShowBulletSparkle(collision.gameObject.transform.position);
            Destroy(collision.gameObject);
            GameCtrl.instance.UpdateScore(GameCtrl.Item.BigCoin);
            AudioCtrl.instance.CoinPickup(gameObject.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coin":
                if (SFXOn)
                {
                    SFXCtrl.instance.ShowCoinSparkle(collision.gameObject.transform.position);
                }
                AudioCtrl.instance.CoinPickup(gameObject.transform.position);
                GameCtrl.instance.UpdateCoinCount();
                break;
            case "Water":
                if (SFXOn)
                {
                    //show the splash effect
                    SFXCtrl.instance.ShowSplash(gameObject.transform.position);

                    //play the splash effect
                    AudioCtrl.instance.WaterSplash(gameObject.transform.position);
                    //inform the GameCtrl
                    //GameCtrl.instance.PlayerDrowned(collision.gameObject);
                    GameCtrl.instance.PlayerDrowned(gameObject);
                }
                break;
            case "Powerup_Bullet":
                canFire = true;
                Vector3 powerupPos = collision.gameObject.transform.position;

                //play the powerup effect
                AudioCtrl.instance.PowerUp(gameObject.transform.position);

                Destroy(collision.gameObject);
                if (SFXOn)
                    //show bullet sparkle animation
                    SFXCtrl.instance.ShowBulletSparkle(powerupPos);
                break;
            case "Enemy":
                GameCtrl.instance.PlayerDiedAnimation(gameObject);
                AudioCtrl.instance.PlayerDied(gameObject.transform.position);
                break;

            case "BossKey":
                GameCtrl.instance.ShowLever();
                break;
            default:
                break;
        }
    }
}

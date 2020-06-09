﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using System.Runtime.Versioning;

public class PlayerController : MonoBehaviour
{
    //player movement animations
    public Animator movementAnimator;

    public float playerMovement; //takes speed of movement
    public GameObject playerObject;
    public bool isPlayerOne; //holds whether this is player 1 or 2 -a
    //public GameObject spawnPoint; //everything related to spawnPoint variable commented out for now

    private GameObject spawnPosition; //holds object that contains spawn location -a
    private bool gravFlipped;
    private float distToGround;
    private int playerJumps;
    private int gravSwitches;
    private float objectHeight;
    private Vector2 screenBounds;
    private int health;
    public RuntimeAnimatorController[] characters;
    private bool hittable = true;
    private bool inAnimation = false;

    private bool pauseMovement = false; //flag to stop player movement

    // -a
    void Awake()
    {
        /*
        if (isPlayerOne)
            spawnPosition = GameObject.Find("P1Spawn");
        else 
            spawnPosition = GameObject.Find("P2Spawn");

        this.transform.Translate(spawnPosition.transform.position);
        */
    }



    // Start is called before the first frame update
    void Start()
    {

        //player movemen animations
        movementAnimator = GetComponent<Animator>();
        if (isPlayerOne)
            movementAnimator.runtimeAnimatorController = characters[GameManager.player1Char];
        else
        {
            if(GameManager.player1Char == 0)
                movementAnimator.runtimeAnimatorController = characters[1];
            else
                movementAnimator.runtimeAnimatorController = characters[0];
        }
        //spawnPoint = GameObject.FindGameObjectWithTag("Spawn");

        gravFlipped = false;
        playerJumps = 1;
        gravSwitches = 2;
        distToGround = gameObject.GetComponent<Collider2D>().bounds.extents.y;
        GameManager.playerHitDelegate += spawnPlayer;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,
            Screen.height, Camera.main.transform.position.z));
    }

    // when player is moving, play animation. 
    private void Update()
    {
        float horizontalMove;
        if (!GameManager.isPaused)
        {
            if (isPlayerOne && hittable)
            {
                movementAnimator.SetFloat("pause", 1.0f);
                horizontalMove = Input.GetAxisRaw("P1Horizontal");
                if (horizontalMove == 0)
                {
                    movementAnimator.SetBool("isRunning", false);
                    movementAnimator.SetBool("isBacking", false);
                }
                else if (horizontalMove == -1)
                {
                    movementAnimator.SetBool("isBacking", true);
                }
                else
                {
                    movementAnimator.SetBool("isRunning", true);
                }

                if (Input.GetButtonDown("P1Jump") && !inAnimation)                      //added inAnimation clause for argument so that is wont jump
                {
                    inAnimation = true;                                                 //added to make jump animation wait 1 second intervals
                    StartCoroutine(waitAnimation(1));                                   //
                    movementAnimator.SetTrigger("isJumping");
                }
                else if (Input.GetButtonDown("P1Fire1") && !inAnimation)
                {
                    //inAnimation = true;                                               //commented out to allow spamming
                    //StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isKicking");
                }
                else if (Input.GetButtonDown("P1Fire2") && !inAnimation)
                {
                    //inAnimation = true;                                               //commented out to allow spamming   
                    //StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isPunching");
                }
            }
            else if (hittable)
            {
                movementAnimator.SetFloat("pause", 1.0f);
                horizontalMove = Input.GetAxisRaw("P2Horizontal");
                if (horizontalMove == 0)
                {
                    movementAnimator.SetBool("isRunning", false);
                    movementAnimator.SetBool("isBacking", false);
                }
                else if (horizontalMove == -1)
                {
                    movementAnimator.SetBool("isRunning", true); //-a
                }
                else
                {
                    movementAnimator.SetBool("isBacking", true); //-a
                }

                if (Input.GetButtonDown("P2Jump") && !inAnimation)                      //added inAnimation clause for argument so that is wont jump
                {
                    inAnimation = true;                                                 //added to make jump animation wait 1 second intervals
                    StartCoroutine(waitAnimation(1));                                   //
                    movementAnimator.SetTrigger("isJumping");
                }
                else if (Input.GetButtonDown("P2Fire1") && !inAnimation)
                {
                    //inAnimation = true;                                               //commented out to allow spamming
                    //StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isKicking");
                }
                else if (Input.GetButtonDown("P2Fire2") && !inAnimation)
                {
                    //inAnimation = true;                                               //commented out to allow spamming
                    //StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isPunching");
                }
            }
            else
                horizontalMove = 0f;

            movementAnimator.SetFloat("speed", horizontalMove);
        }
        else
            movementAnimator.SetFloat("pause", 0.0f);

    }

    /*
    IEnumerator PauseMovement(float sec)
    {
        pauseMovement = true;
        yield return new WaitForSeconds(sec);
        pauseMovement = false;
    }
    */

    void FixedUpdate()
    {

        //Player Camera Interaction
        if (isGrounded())
        {
            playerJumps = 1;
            //gravSwitches = 1;
        }

        //Gravity Switch
        if (Input.GetKeyDown(KeyCode.V) && gravSwitches > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = -gameObject.GetComponent<Rigidbody2D>().gravityScale;
            gravFlipped = !gravFlipped;
            if (gravFlipped) gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1), ForceMode2D.Impulse);
            else gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
            gravSwitches--;
        }

        //Reset Player
        if (Input.GetKeyDown(KeyCode.R))
        {

        }

        //Player Movement
        if (isPlayerOne && hittable && !GameManager.isPaused)
        {
            if (Input.GetKey(KeyCode.D) && !pauseMovement)
            {
                transform.Translate(Vector3.right * playerMovement * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A) && !pauseMovement)
            {
                transform.Translate(Vector3.left * playerMovement * Time.deltaTime);
            }
        }
        else if(hittable && !GameManager.isPaused) {
            if (Input.GetKey(KeyCode.RightArrow) && !pauseMovement)
            {
                transform.Translate(Vector3.right * playerMovement * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !pauseMovement)
            {
                transform.Translate(Vector3.left * playerMovement * Time.deltaTime);
            }
        }


        //Player Jumps
        if (Input.GetKeyDown(KeyCode.Space) && !gravFlipped && playerJumps > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
            playerJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && gravFlipped && playerJumps > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -4), ForceMode2D.Impulse);
            playerJumps--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag == "Floor" || collision.collider.tag == "Object")
        {
            playerJumps = 1;
            gravSwitches = 1;
        }
        /*
        else if (collision.collider.tag == "Enemy")
        {
            //Reset Gravity
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

            //Trigger event
            GameManager.OnPlayerHit();
            GameManager.playerHitDelegate -= spawnPlayer;
            Destroy(this.gameObject);
        }
        */
        else if (collision.collider.tag == "Obstacle")
        {
            //Reset Gravity
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

            //Particle Effects and Other things to add

            //Trigger event
            GameManager.OnPlayerHit();
            GameManager.playerHitDelegate -= spawnPlayer;
            Destroy(gameObject);
        }
        else if (collision.collider.tag == "Exit")
        {
            GameManager.CurrentLevel++;
            GameManager.loadLevel();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController other = collision.gameObject.GetComponent("PlayerController") as PlayerController;
        if (collision.collider.tag == "Player" && (movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("punch") || movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("kick")
            || movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("solKick") || movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("solPunch"))
            && movementAnimator.GetCurrentAnimatorStateInfo(0).length > .1 && other.hittable)
        {
            other.hittable = false;
            //Reset Gravity
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

            //Trigger event
            
            other.isHit();
   
        }
    }

    void spawnPlayer() //spawnpoint related stuff commented out
    {
        //spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        if (playerObject != null)
        {
           //GameObject go = Instantiate(playerObject, spawnPoint.transform.position, Quaternion.identity);
           //go.name = gameObject.name;
        }     
    }

    bool isGrounded()
    {
        if ((-1 * (transform.position.y - objectHeight)) >= screenBounds.y) return true;
        else if ((-1 * (transform.position.y + objectHeight)) >= screenBounds.y) return true;
        else return false;
    }

    public void isHit()
    {
        decHealth myHealth;
        movementAnimator.SetBool("isFalling", true);

        if(isPlayerOne)
            myHealth = GameObject.Find("P1HealthBar").GetComponent("decHealth") as decHealth;
        else
            myHealth = GameObject.Find("P2HealthBar").GetComponent("decHealth") as decHealth;
            
        myHealth.changeLife();
        StartCoroutine(hitWait());
    }

    IEnumerator hitWait()
    {
        yield return new WaitForSeconds(1);
        decHealth myHealth;
        if (isPlayerOne)
        {
            myHealth = GameObject.Find("P1HealthBar").GetComponent("decHealth") as decHealth;
            if (myHealth.startingLife == 0)
                GameManager.loadWinner(false);
        }
        else {
            myHealth = GameObject.Find("P2HealthBar").GetComponent("decHealth") as decHealth;
            if (myHealth.startingLife == 0)
                GameManager.loadWinner(true);
        }

        hittable = true;
    }

    IEnumerator waitAnimation(long time)
    {
        yield return new WaitForSeconds(time);
        inAnimation = false;
    }
}
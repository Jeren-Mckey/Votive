using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using System.Runtime.Versioning;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 5f;

    //player movement animations
    public Animator movementAnimator;
    Rigidbody2D myRigidBody;
    private Time animControl;

    public float playerMovement; //takes speed of movement
    public GameObject playerObject;
    public bool isPlayerOne; //holds whether this is player 1 or 2 -a
    public bool isPlayerTwo;
    public Transform attackPt;
    public float attackRange;
    public GameObject player1;
    public GameObject player2; 

    private GameObject spawnPosition; //holds object that contains spawn location -a
    private float distToGround;
    private int playerJumps;
    private float objectHeight;
    private Vector2 screenBounds;
    private int health;
    public RuntimeAnimatorController[] characters;
    private bool isSpecialAttack = false;
    private bool inAnimation = false;
    private bool pauseMovement = false; //flag to stop player movement
    bool flip = false;
    incEnergy myEnergy;


    // test - claire
    void facing(GameObject p1, GameObject p2) {
        float p1X = p1.transform.position.x;
        float p2X = p2.transform.position.x;

        
        if (((p2X - p1X) < -0.01) && !flip)
        {
            p1.GetComponent<SpriteRenderer>().transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            p2.GetComponent<SpriteRenderer>().transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            flip = true;
        }
        else if (((p2X - p1X) > -0.01) && flip) {
            p1.GetComponent<SpriteRenderer>().transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            p2.GetComponent<SpriteRenderer>().transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
       
            flip = false;
        }
        
        //p1.GetComponent<SpriteRenderer>().flipX = flip;
        //p2.GetComponent<SpriteRenderer>().flipX = flip;
    } 
    //***********


    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        if (gameObject.name == player1.name)
            isPlayerOne = true;
        else if (gameObject.name == player2.name)
            isPlayerTwo = true;

        //player movement animations
        movementAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        //myRigidBody.position = Vector3.zero;
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

        playerJumps = 1;
        distToGround = gameObject.GetComponent<Collider2D>().bounds.extents.y;
        //GameManager.playerHitDelegate += spawnPlayer;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,
            Screen.height, Camera.main.transform.position.z));
    }



    // when player is moving, play animation. 
    private void Update()
    {
        // test - claire

        facing(player1, player2);
        //**************

        float horizontalMove;
        if (!GameManager.isPaused)
        {
            if (isPlayerOne)
            {
                movementAnimator.SetFloat("pause", 1.0f);
                horizontalMove = Input.GetAxisRaw("P1Horizontal");
                if (horizontalMove == 0)
                {
                    movementAnimator.SetBool("isRunning", false);
                    movementAnimator.SetBool("isBacking", false);
                }
                else if ((horizontalMove == -1 && !flip) || (horizontalMove != -1 && flip))  // added "&& !flip..." and all after to take into account character being flipped
                {
                    movementAnimator.SetBool("isBacking", true);
                    movementAnimator.SetBool("isRunning", false);                       //Added so that when it flips it doesnt continue running forward
                }
                else
                {
                    movementAnimator.SetBool("isRunning", true);
                    movementAnimator.SetBool("isBacking", false);                       //same reasoning as above
                }

                if (Input.GetButtonDown("P1Jump") && !inAnimation)                      //added inAnimation clause for argument so that is wont jump
                {
                    inAnimation = true;                                                 //added to make jump animation wait 1 second intervals
                    StartCoroutine(waitAnimation(1));                                   //
                    movementAnimator.SetTrigger("isJumping");
                    jump();
                }
                else if (Input.GetButtonDown("P1Fire1") && !inAnimation)
                {
                    inAnimation = true;                                               //commented out to allow spamming
                    StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isKicking");
                    isSpecialAttack = false;                                            
                }
                else if (Input.GetButtonDown("P1Fire2") && !inAnimation)
                {
                    inAnimation = true;                                               //commented out to allow spamming   
                    StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isPunching");
                    isSpecialAttack = false;                                             
                }
                //Added for the special attack input
                else if (Input.GetButtonDown("P1Special") && !inAnimation)
                {
                    incEnergy myEnergy;

                    myEnergy = GameObject.Find("P1EnergyBar").GetComponent("incEnergy") as incEnergy;

                    if (myEnergy.startingEnergy == 6)
                    {
                        //movementAnimator.SetTrigger("isSpecial");                         //commented out since trigger does not exist yet
                        isSpecialAttack = true;                                                 
                    }
                }
            }
            else if (isPlayerTwo)
            {
                movementAnimator.SetFloat("pause", 1.0f);
                horizontalMove = Input.GetAxisRaw("P2Horizontal");
                if (horizontalMove == 0)
                {
                    movementAnimator.SetBool("isRunning", false);
                    movementAnimator.SetBool("isBacking", false);
                }
                else if ((horizontalMove == -1 && !flip) || (horizontalMove != -1 && flip))  // added "&& !flip..." and all after to take into account character being flipped
                {
                    movementAnimator.SetBool("isRunning", true); //-a
                    movementAnimator.SetBool("isBacking", false); //-a
                }
                else
                {
                    movementAnimator.SetBool("isBacking", true); //-a
                    movementAnimator.SetBool("isRunning", false); //-a
                }

                if (Input.GetButtonDown("P2Jump") && !inAnimation)                      //added inAnimation clause for argument so that is wont jump
                {
                    inAnimation = true;                                                 //added to make jump animation wait 1 second intervals
                    StartCoroutine(waitAnimation(1));                                   //
                    movementAnimator.SetTrigger("isJumping");
                    jump();
                }
                else if (Input.GetButtonDown("P2Fire1") && !inAnimation)
                {
                    inAnimation = true;                                               //comment out to allow spamming
                    StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isKicking");
                    isSpecialAttack = false;                                               
                }
                else if (Input.GetButtonDown("P2Fire2") && !inAnimation)
                {
                    inAnimation = true;                                               //comment out to allow spamming
                    StartCoroutine(waitAnimation(1));                                 //
                    movementAnimator.SetTrigger("isPunching");
                    isSpecialAttack = false;                                              
                }
                //Added for the special attack input
                else if (Input.GetButtonDown("P2Special") && !inAnimation)
                {
                    

                    myEnergy = GameObject.Find("P2EnergyBar").GetComponent("incEnergy") as incEnergy;

                    if (myEnergy.startingEnergy == 6)
                    {
                        inAnimation = true;                                                 //added to make jump animation wait 1 second intervals
                        StartCoroutine(waitAnimation(1));
                        movementAnimator.SetTrigger("isSuper");                         //commented out since trigger does not exist yet
                        isSpecialAttack = true;                                           
                    }
                }
            }
            else
                horizontalMove = 0f;

            movementAnimator.SetFloat("speed", horizontalMove);
        }
        else
            movementAnimator.SetFloat("pause", 0.0f);
        
    }



    //************************************************************************attack points 
    

    public GameObject enemy()
    {
        if(isPlayerOne)
        {
            return player2;
        }
        else 
        {
            return player1;
        }
    }
    
    /** Deprecated with new collision changes
    //if player1 or player2's box colliders are in attack range, return true
    public bool inAttackRange() {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPt.position, attackRange);

        if (hitPlayer == enemy().GetComponent<BoxCollider2D>())
        {
            return true;
        }
        return false;

    }
    */

    //draws red circle on attack point
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPt.position, attackRange);
    }
    //**********************************************************************************

    public void attackDamage(bool specialAttack) {                              //added bool to differentiate between special attack and normal attack
        PlayerController other = enemy().GetComponent<PlayerController>();

        if (movementAnimator.GetCurrentAnimatorStateInfo(0).length > .1  && !specialAttack)
        {
            onHit();
            other.isHit();
        }
        else if (movementAnimator.GetCurrentAnimatorStateInfo(0).length > .1 )
        {
            myEnergy.resetEnergy();
            other.isDamaged();
        }


    }

    void FixedUpdate()
    {
        //Player Movement
        if (isPlayerOne && !GameManager.isPaused)
        {
            if (Input.GetKey(KeyCode.D) && !pauseMovement && !flip)
                transform.Translate(Vector3.right * playerMovement * Time.deltaTime);
            else if (Input.GetKey(KeyCode.D) && !pauseMovement && flip)
                transform.Translate(Vector3.left * playerMovement * Time.deltaTime);

            if (Input.GetKey(KeyCode.A) && !pauseMovement && !flip)
                transform.Translate(Vector3.left * playerMovement * Time.deltaTime);
            else if (Input.GetKey(KeyCode.A) && !pauseMovement && flip) 
                transform.Translate(Vector3.right * playerMovement * Time.deltaTime);
        }
        else if(isPlayerTwo && !GameManager.isPaused) {
            if (Input.GetKey(KeyCode.RightArrow) && !pauseMovement && !flip)
                transform.Translate(Vector3.left * playerMovement * Time.deltaTime);
            else if (Input.GetKey(KeyCode.RightArrow) && !pauseMovement && flip)
                transform.Translate(Vector3.right * playerMovement * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftArrow) && !pauseMovement && !flip)
            {
                transform.Translate(Vector3.right * playerMovement * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !pauseMovement && flip)
                transform.Translate(Vector3.left * playerMovement * Time.deltaTime);
        }
    }

    
    /*      //****************************IMPORTANT ORIGINAL WORKING DAMAGE SYSTEM
    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController other = collision.gameObject.GetComponent("PlayerController") as PlayerController;
        if (collision.collider.tag == "Player" && (movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("punch") || movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("kick")
            || movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("solKick") || movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("solPunch"))
            && movementAnimator.GetCurrentAnimatorStateInfo(0).length > .1 && other.hittable && inAttackRange())        //8/13/2020 added inAttackRange()-c
        {
            other.hittable = false;
            //Reset Gravity
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 1; ***************source of bug where if sol damages other player, her jumps become higher

            //Trigger event

            onHit();
            other.isHit();

        }

        else if (collision.collider.tag == "Player" && (movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("special") || movementAnimator.GetCurrentAnimatorStateInfo(0).IsName("solSpecial"))          //****new sol animation state must be named "solSpecial"
        && movementAnimator.GetCurrentAnimatorStateInfo(0).length > .1 && other.hittable && inAttackRange())            //8/13/2020 added inAttackRange()-c
        {
            other.hittable = false;
            other.isDamaged();
        }
    }
    */


    private void jump()
    {
        Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
        myRigidBody.velocity += jumpVelocityToAdd;
    }



    bool isGrounded()
    {
        if ((-1 * (transform.position.y - objectHeight)) >= screenBounds.y) return true;
        else if ((-1 * (transform.position.y + objectHeight)) >= screenBounds.y) return true;
        else return false;
    }



    public void onHit()
    {
        incEnergy myEnergy;

        if (isPlayerOne)
            myEnergy = GameObject.Find("P1EnergyBar").GetComponent("incEnergy") as incEnergy;
        else
            myEnergy = GameObject.Find("P2EnergyBar").GetComponent("incEnergy") as incEnergy;

        myEnergy.increaseEnergy();
    }



    public void isHit()
    {
        GetComponent<AudioSource>().Play();
        movementAnimator.SetBool("isFalling", true);

        StartCoroutine(hitWait());
    }



    public void isDamaged()
    {
        decHealth myHealth;
        GetComponent<AudioSource>().Play();
        movementAnimator.SetBool("isFalling", true);

        if (isPlayerOne)
            myHealth = GameObject.Find("P1HealthBar").GetComponent("decHealth") as decHealth;
        else
            myHealth = GameObject.Find("P2HealthBar").GetComponent("decHealth") as decHealth;

        myHealth.changeLife();
        StartCoroutine(damageWait());
    }



    IEnumerator hitWait()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator damageWait()
    {
        yield return new WaitForSeconds(1);
        decHealth myHealth;
        if (isPlayerOne)
        {
            myHealth = GameObject.Find("P1HealthBar").GetComponent("decHealth") as decHealth;
            if (myHealth.startingLife == 0)
                GameManager.loadWinner(false);
        }
        else
        {
            myHealth = GameObject.Find("P2HealthBar").GetComponent("decHealth") as decHealth;
            if (myHealth.startingLife == 0)
                GameManager.loadWinner(true);
        }

    }



    IEnumerator waitAnimation(long time)
    {
        yield return new WaitForSeconds(time);
        inAnimation = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform[] colliders = collision.transform.GetComponentsInChildren<Transform>();

        bool isIdleCollider = false;
        Debug.Log(collision.collider.gameObject.GetComponent<Collider2D>().tag);
        if (collision.otherCollider.gameObject.GetComponent<Collider2D>().CompareTag("Player") &&
            collision.collider.gameObject.GetComponent<Collider2D>().CompareTag("Player")) isIdleCollider = true;
        
        if (collision.collider.CompareTag("Player") && isIdleCollider == false)
        {
            attackDamage(false);
        }

    }
    

}
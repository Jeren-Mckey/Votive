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
    private bool hittable = true;
    private bool inAnimation = false;
    private bool pauseMovement = false; //flag to stop player movement
    incEnergy myEnergy;
    
  
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
                    jump();
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
                //Added for the special attack input
                else if (Input.GetButtonDown("P1Special") && !inAnimation)
                {
                    incEnergy myEnergy;

                    myEnergy = GameObject.Find("P1EnergyBar").GetComponent("incEnergy") as incEnergy;

                    if (myEnergy.startingEnergy == 6)
                    {
                        //movementAnimator.SetTrigger("isSpecial");                         //commented out since trigger does not exist yet
                        myEnergy.resetEnergy();
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
                    jump();
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
                //Added for the special attack input
                else if (Input.GetButtonDown("P2Special") && !inAnimation)
                {
                    

                    myEnergy = GameObject.Find("P2EnergyBar").GetComponent("incEnergy") as incEnergy;

                    if (myEnergy.startingEnergy == 6)
                    {
                        inAnimation = true;                                                 //added to make jump animation wait 1 second intervals
                        StartCoroutine(waitAnimation(1));
                        movementAnimator.SetTrigger("isSuper");                         //commented out since trigger does not exist yet
                        myEnergy.resetEnergy();
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


    public void attackDamage() {
        PlayerController other = enemy().GetComponent<PlayerController>();

        if (movementAnimator.GetCurrentAnimatorStateInfo(0).length > .1)
        {
            onHit();
            other.isHit();
        }
        else if (movementAnimator.GetCurrentAnimatorStateInfo(0).length > .1 )
        {
            other.isDamaged();
        }


    }

    void FixedUpdate()
    {
        //Player Movement
        if (isPlayerOne && !GameManager.isPaused)
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
        else if(isPlayerTwo && !GameManager.isPaused) {
            if (Input.GetKey(KeyCode.RightArrow) && !pauseMovement)
            {
                transform.Translate(Vector3.left * playerMovement * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !pauseMovement)
            {
                transform.Translate(Vector3.right * playerMovement * Time.deltaTime);
            }
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
            attackDamage();
        }

    }
    

}
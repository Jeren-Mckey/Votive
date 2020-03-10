using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
public class PlayerController : MonoBehaviour
{

    public float playerMovement;
    private bool gravFlipped;
    private float distToGround;
    private int playerJumps;
    private int gravSwitches;
    public GameObject playerObject;
    public GameObject spawnPoint;
    private float objectHeight;
    private Vector2 screenBounds;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        gravFlipped = false;
        playerJumps = 1;
        gravSwitches = 1;
        distToGround = gameObject.GetComponent<Collider2D>().bounds.extents.y;
        GameManager.playerHitDelegate += spawnPlayer;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,
            Screen.height, Camera.main.transform.position.z));
    }

    void FixedUpdate()
    {
        //Player Camera Interaction
        if (isGrounded())
        {
            playerJumps = 1;
            print("playerjumps value is: " + playerJumps);
            //gravSwitches = 1;
           // print("gravswitch value is: " + gravSwitches);
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
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * playerMovement * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * playerMovement * Time.deltaTime);
        }

        //Player Jumps
        if (Input.GetKeyDown(KeyCode.UpArrow) && !gravFlipped && playerJumps > 0 && isGrounded())
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
            playerJumps--;
        }
        //else if (Input.GetKeyDown(KeyCode.UpArrow) && gravFlipped && playerJumps > 0)
        //{
        //    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -4), ForceMode2D.Impulse);
        //    playerJumps--;
        //}

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetBool("isCrouching", true);
            
        }
        
    }
        
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor" || collision.collider.tag == "Object")
        {
            playerJumps = 1;
            gravSwitches = 1;
        }
        else if (collision.collider.tag == "Enemy")
        {
            //Reset Gravity
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

            //Trigger event
            GameManager.OnPlayerHit();
            GameManager.playerHitDelegate -= spawnPlayer;
            Destroy(this.gameObject);
        }
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

    void spawnPlayer()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        if (playerObject != null)
        {
            GameObject go = Instantiate(playerObject, spawnPoint.transform.position, Quaternion.identity);
            go.name = gameObject.name;
        }
    }

    bool isGrounded()
    {
        if ((-1 * (transform.position.y - objectHeight)) >= screenBounds.y) return true;
        else if ((-1 * (transform.position.y + objectHeight)) >= screenBounds.y) return true;
        else return false;
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{ 
    public delegate void OnPlayerHitDelegate();
    public static event OnPlayerHitDelegate playerHitDelegate;
    public static int CurrentLevel = 1;
    public static bool player1won;
    public static bool isPaused;
    public static int player1Char;
    public static bool winnerFound = false;
    public static int background;

    public static void OnPlayerHit()
    {
        playerHitDelegate();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {

    }

    public static void loadLevel()
    {
        SceneManager.LoadScene("Level" + GameManager.CurrentLevel.ToString());
    }

    public static void loadWinner(bool winner)
    {
        player1won = winner;
        SceneManager.LoadScene("Win Screen");
        winnerFound = true;
    }
}

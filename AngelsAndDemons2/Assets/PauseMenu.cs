using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseObject;
    private GameObject controlObject;
    private GameObject winMenuObject;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.isPaused = false;
        pauseObject = GameObject.Find("PausePanel");
        controlObject = GameObject.Find("ControlsImage");
        controlObject.SetActive(false);
        pauseObject.SetActive(GameManager.isPaused);
        winMenuObject = GameObject.Find("WinPanel");
        winMenuObject.SetActive(false);
}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update Called");
        if(Input.GetButtonDown("Pause"))
        {
            if (GameManager.isPaused)
            {
                GameManager.isPaused = false;
                controlObject.SetActive(false);
            }
            else
                GameManager.isPaused = true;

            pauseObject.SetActive(GameManager.isPaused);
            
        }

        if(GameManager.winnerFound)
        {
            GameManager.isPaused = true;
            openWinMenu();
        }
    }

    public void unPause()
    {
        if (GameManager.isPaused)
        {
            GameManager.isPaused = false;
            controlObject.SetActive(false);
        }
        else
            GameManager.isPaused = true;


        pauseObject.SetActive(GameManager.isPaused);
        
    }

    public void openControls()
    {
        controlObject.SetActive(true);
    }

    public void closeControls()
    {
        controlObject.SetActive(false);
    }

    public void openWinMenu()
    {
        winMenuObject.SetActive(true);
    }
}

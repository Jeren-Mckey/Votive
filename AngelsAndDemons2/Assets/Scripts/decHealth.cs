using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class decHealth : MonoBehaviour
{
    public Sprite health5;
    public Sprite health4;
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite health0;

    public int startingLife = 5;

    public void changeLife()
    {
        startingLife--;
        switch (startingLife)
        {
            case 5:
                GetComponent<Image>().sprite = health5;
                break;
            case 4:
                GetComponent<Image>().sprite = health4;
                break;
            case 3:
                GetComponent<Image>().sprite = health3;
                break;
            case 2:
                GetComponent<Image>().sprite = health2;
                break;
            case 1:
                GetComponent<Image>().sprite = health1;
                break;
            case 0:
                GetComponent<Image>().sprite = health0;
                break;
            default:
                break;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

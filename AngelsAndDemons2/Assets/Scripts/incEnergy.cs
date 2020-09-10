using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class incEnergy : MonoBehaviour
{
    public Sprite energy6;
    public Sprite energy5;
    public Sprite energy4;
    public Sprite energy3;
    public Sprite energy2;
    public Sprite energy1;
    //public Sprite energy0;

    public int startingEnergy = 1;

    public void increaseEnergy()
    {
        if (startingEnergy < 6)
        {
            startingEnergy++;
            switch (startingEnergy)
            {
                case 6:
                    GetComponent<Image>().sprite = energy6;
                    break;
                case 5:
                    GetComponent<Image>().sprite = energy5;
                    break;
                case 4:
                    GetComponent<Image>().sprite = energy4;
                    break;
                case 3:
                    GetComponent<Image>().sprite = energy3;
                    break;
                case 2:
                    GetComponent<Image>().sprite = energy2;
                    break;
                case 1:
                    GetComponent<Image>().sprite = energy1;
                    break;
                // case 0:
                //     GetComponent<Image>().sprite = energy0;
                //     break;
                default:
                    break;
            }
        }
    }

    public void resetEnergy()
    {
        startingEnergy = 1;
        GetComponent<Image>().sprite = energy1;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
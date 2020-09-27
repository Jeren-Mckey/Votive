using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        GameObject bg = GameObject.Find("background1");
        GameObject bg2 = GameObject.Find("background2");
        GameObject bg3 = GameObject.Find("background3");
        GameObject bg4 = GameObject.Find("background4");
        switch (GameManager.background)
        {
            case 0:
                break;
            case 1:
                bg.SetActive(false);
                bg2.SetActive(true);
                bg3.SetActive(false);
                bg4.SetActive(false);
                Debug.Log("Test");
                break;
            case 2:
                bg.SetActive(false);
                bg2.SetActive(false);
                bg3.SetActive(true);
                bg4.SetActive(false);
                break;
            case 3:
                bg.SetActive(false);
                bg2.SetActive(false);
                bg3.SetActive(false);
                bg4.SetActive(true);
                break;
            default:
                break;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

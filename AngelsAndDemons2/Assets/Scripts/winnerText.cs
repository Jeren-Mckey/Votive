using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class winnerText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI winText = this.GetComponentInChildren<TextMeshProUGUI>();
        if (GameManager.player1won)
        {
            winText.text = "Player 1 Won!     Play Again?";
        }
        else
        {
            winText.text = "Player 2 Won!     Play Again?";
        }
    }

    // Update is called once per frame
    void Update()
    {
  
    }
}

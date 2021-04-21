using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showPlayerControls : MonoBehaviour
{
    private bool showSol;
    private bool showBlank;
    private GameObject solControl;
    private GameObject blankControl;

    // Start is called before the first frame update
    void Start() {
        showSol = false;
        showBlank = false;
        solControl = GameObject.Find("solControlImage");
        blankControl = GameObject.Find("blankControlImage");

        solControl.SetActive(showSol);
        blankControl.SetActive(showBlank);
    }

    public void solOnOffControls() {
        if (showSol) {
            showSol = false;
            solControl.SetActive(showSol);
        }
        else {
            showSol = true;
            solControl.SetActive(showSol);
        }
    }

    public void blankOnOffControls() {
        if (showBlank) {
            showBlank = false;
            blankControl.SetActive(showBlank);
        }
        else {
            showBlank = true;
            blankControl.SetActive(showBlank);
        }
    }

}

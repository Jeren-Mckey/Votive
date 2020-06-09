using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showControls : MonoBehaviour
{
    private bool show;
    private GameObject controlObject;
    // Start is called before the first frame update
    void Start()
    {
        show = false;
        controlObject = GameObject.Find("ControlsImage");
        controlObject.SetActive(show);
    }

    void Update()
    {

    }

    public void onOffControls()
    {
        if (show)
            offControls();
        else
            onControls();
    }

    public void offControls()
    {
        show = false;
        controlObject.SetActive(show);
    }

    public void onControls()
    {
        show = true;
        controlObject.SetActive(show);
    }
}

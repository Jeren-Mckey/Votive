using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScreen : MonoBehaviour
{
    private GameObject loadObject;
    // Start is called before the first frame update
    void Start()
    {
        loadObject = GameObject.Find("LoadScreen");
        loadObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadNextScene()
    {
        loadObject.SetActive(true);
        StartCoroutine(waitLoad(2));
    }

    IEnumerator waitLoad(long time)
    {
        yield return new WaitForSeconds(time);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void characterChosen(int charNum)
    {
        GameManager.player1Char = charNum;
    }

    public void backgroundChosen(int charNum)
    {
        GameManager.background = charNum;
    }

}

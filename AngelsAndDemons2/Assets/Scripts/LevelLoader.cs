using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int timeToWait = 4;
    int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTime());

        }

    }

    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        loadNextScene();
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

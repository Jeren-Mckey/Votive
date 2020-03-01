using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float waitTime = 4f;
    public float transitionTime = 1f;
    public int nextLevelIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        LoadNextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        /* not using input for opening scene transitions
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextLevel();
        }
        */
    }

    public void LoadNextLevel()
    {
        //StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        StartCoroutine(LoadLevel(nextLevelIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(waitTime);
        transition.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}

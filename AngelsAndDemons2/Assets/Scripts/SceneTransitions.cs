using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions: MonoBehaviour
{
    [SerializeField] int timeToWait = 4;
    public Animator transitionAnim;
    public string sceneName;
    int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        transitionAnim = GetComponent<Animator>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {

            StartCoroutine(LoadScene());
        }
        
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(timeToWait);
        Debug.Log("here");
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("hi again");
        SceneManager.LoadScene(sceneName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

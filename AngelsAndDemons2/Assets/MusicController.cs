using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip gameMus;
    public AudioClip gameMus2;
    public AudioClip gameMus3;
    public AudioClip gameMus4;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource stageAudioSource = GetComponent<AudioSource>();

        switch (GameManager.background)
        {
            case 0:
                stageAudioSource.Play();
                break;
            case 1:
                stageAudioSource.clip = gameMus2;
                stageAudioSource.Play();
                Debug.Log("Test");
                break;
            case 2:
                stageAudioSource.clip = gameMus3;
                stageAudioSource.Play();
                break;
            case 3:
                stageAudioSource.clip = gameMus4;
                stageAudioSource.Play();
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

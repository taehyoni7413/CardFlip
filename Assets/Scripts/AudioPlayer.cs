using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    
    public AudioClip clickSound;

    private AudioSource audioSource;

    private static AudioPlayer instance;

    public static AudioPlayer GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(clickSound);    
    }
}

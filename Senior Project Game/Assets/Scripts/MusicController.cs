using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    private AudioSource music;

    private static MusicController instance;
    public static MusicController Instance { get { return instance; } }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
        music = this.GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (music.isPlaying)
        {
            return;
        }
        music.Play();
    }

    public void StopMusic()
    {
        music.Stop();
    }

    public void SetVolume(float volume)
    {
        music.volume = volume;
    }

    public float GetVolume()
    {
        return music.volume;
    }
}

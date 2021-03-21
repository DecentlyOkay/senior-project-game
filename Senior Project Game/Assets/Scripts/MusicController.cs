using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource music;
    private void Awake()
    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Toggle aimIndicator;
    public Slider volumeSlider;
    private MusicController music;

    private void Awake()
    {
        aimIndicator.isOn = PlayerData.aimIndicatorEnabled;
        music = FindObjectOfType<MusicController>();
        if(music != null)
        {
            volumeSlider.value = music.GetVolume() * volumeSlider.maxValue;
        }
    }
    public void ToggleAimIndicator()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.ToggleAimIndicator(aimIndicator.isOn);
        }
        else
        {
            PlayerData.aimIndicatorEnabled = aimIndicator.isOn;
        }
    }

    public void ChangeVolume()
    {
        if(music != null)
        {
            music.SetVolume(volumeSlider.value / volumeSlider.maxValue);
        }
    }
}

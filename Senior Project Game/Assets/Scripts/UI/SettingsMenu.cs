using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Toggle aimIndicator;

    private void Awake()
    {
        aimIndicator.isOn = PlayerData.aimIndicatorEnabled;
    }
    public void ToggleAimIndicator()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.ToggleAimIndicator(aimIndicator.isOn);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider healthbar;
    public Slider staminabar;
    public Player player;


    public void FixedUpdate()
    {
        healthbar.value = player.health / player.maxHealth;
        staminabar.value = player.stamina / player.maxStamina;
    }

}

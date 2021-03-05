using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider healthbar;
    public Slider staminabar;
    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void FixedUpdate()
    {
        healthbar.value = player.health / player.maxHealth;
        staminabar.value = player.stamina / player.maxStamina;
    }

}

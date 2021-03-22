using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPickup : MonoBehaviour
{
    public int id = 0;

    private void Awake()
    {
        if(id >= PlayerData.gunsUnlocked.Length)
        {
            id = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().SetText(PlayerData.gunNames[id] + " unlocked!");
            PlayerData.gunsUnlocked[id] = true;
            Destroy(this.gameObject);
        }
    }
}

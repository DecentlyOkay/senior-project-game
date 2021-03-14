using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int enemiesRemaining = 0;
    public bool portalActivated = false;

    private Player playerInfo;

    private void Awake()
    {
        this.tag = "GameController";
        playerInfo = GameObject.FindObjectOfType<Player>();
    }
    void FixedUpdate()
    {
        //Debug.Log(enemiesRemaining);
        if(playerInfo != null && !portalActivated && enemiesRemaining == 0)
        {
            portalActivated = true;
            playerInfo.GetComponentInChildren<TextMeshProUGUI>().text = "Continue through\nthe portal!";
            foreach(Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

}

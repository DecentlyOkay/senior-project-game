using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int enemiesRemaining = 0;
    public bool portalActivated = false;
    public Transform closedPortal;
    public Transform openPortal;

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
            playerInfo.SetText("Continue through\nthe portal!");
            closedPortal.gameObject.SetActive(false);
            openPortal.gameObject.SetActive(true);
        }
    }

}

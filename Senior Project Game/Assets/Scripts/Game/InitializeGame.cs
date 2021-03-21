using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    private void Awake()
    {
        PlayerData.Initialize();
    }
}

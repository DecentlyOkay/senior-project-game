using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static bool aimIndicatorEnabled = true;
    public static bool[] gunsUnlocked = new bool[10];
    public static string[] gunNames = { "Pistol", "Shotgun", "AR", "Rocket Launcher", "filler", "filler", "filler", "filler", "filler", "filler" };

    public static int weaponIndex = 0;
    public static void Initialize()
    {
        gunsUnlocked[0] = true;
        //gunsUnlocked[1] = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float nextFireTime = 0f;
    public float fireRate = 100f; //per second

    public abstract void Attack();
}

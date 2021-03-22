using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRotator : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime * 5f);
    }
}

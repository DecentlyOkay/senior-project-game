using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    private Transform target;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = this.GetComponent<Transform>().parent;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, target.position, Time.deltaTime * moveSpeed);
    }
}

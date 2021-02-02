using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public GameObject target;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = this.GetComponent<Transform>().parent;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, target.transform.position, Time.deltaTime * moveSpeed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public GameObject target;

    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = this.GetComponent<Transform>().parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
    }
}

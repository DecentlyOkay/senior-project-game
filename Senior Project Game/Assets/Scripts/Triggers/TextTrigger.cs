using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{

    public string message;
    private bool triggered = false;

    void Start()
    {
        message = message.Replace("\\n", "\n");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            other.gameObject.GetComponent<Player>().SetText(message);
        }
    }
}

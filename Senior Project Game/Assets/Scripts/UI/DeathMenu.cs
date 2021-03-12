using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void Display()
    {
        StartCoroutine(ActivateUI());
    }

    private IEnumerator ActivateUI()
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(true); 
            //bruh
            foreach(Transform childchild in child)
            {
                if (childchild.gameObject.name.Equals("DeathText"))
                {
                    childchild.gameObject.SetActive(true);
                }
                else
                {
                    childchild.gameObject.SetActive(false);
                }
            }
        }
        yield return new WaitForSecondsRealtime(1f);
        foreach (Transform child in this.transform)
        {
            foreach (Transform childchild in child)
            {
                childchild.gameObject.SetActive(true);
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

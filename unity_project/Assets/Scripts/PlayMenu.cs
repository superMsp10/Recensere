using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour, UIState
{
    public Text error;
    public Text Hello;
    public Text Version;
    public GameObject creategame;



    public void EndUI()
    {
        gameObject.SetActive(false);
    }

    public void StartUI()
    {
        gameObject.SetActive(true);
        error.text = "";
        Hello.text = "Hello, " + Persistent.thisPersist.Username;
        Version.text = "Recensere, v" + GameManager.Version;

        if (Persistent.thisPersist.Dev)
        {
            creategame.SetActive(true);
        }
        else
        {
            creategame.SetActive(false);
        }

    }

    public void UpdateUI()
    {
        error.text = "The game is not on right now!";
    }


}

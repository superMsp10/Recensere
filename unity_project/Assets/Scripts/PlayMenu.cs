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


    public string loadSceneName;


    public void EndUI()
    {
        gameObject.SetActive(false);
    }

    public void StartUI()
    {
        gameObject.SetActive(true);
        Hello.text = "Hello, " + Persistent.thisPersist.Username;
        Version.text = "Recensere, v" + GameManager.Version;

    }

    public void UpdateUI()
    {
        throw new NotImplementedException();
    }

    public void changeToPlayMenu()
    {
        SceneManager.LoadScene(loadSceneName);
    }

}

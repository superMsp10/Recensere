using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ErrorUI : MonoBehaviour, UIState
{
    public Text title;
    public Text desciption;
    public string MainMenuScene;

    public void StartUI()
    {
        gameObject.SetActive(true);

    }

    public void EndUI()
    {
        gameObject.SetActive(false);

    }

    public void UpdateUI()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void reloadMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }

    public void openGamePage()
    {
        Application.OpenURL("http://gamejolt.com/games/bbman/94396");
    }

}

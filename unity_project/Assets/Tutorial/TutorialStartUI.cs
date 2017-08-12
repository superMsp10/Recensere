using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialStartUI : MonoBehaviour, UIState
{

    public void StartUI()
    {
        gameObject.SetActive(true);
        GameManager.thisM.paused = true;

    }

    public void EndUI()
    {
        gameObject.SetActive(false);

    }

    public void UpdateUI()
    {
    }

    public void PlayOffline()
    {
        Persistent.thisPersist.offline = true;
        Persistent.thisPersist.autoJoin = true;

        SceneManager.LoadScene(GameManager.thisM.startGameScene);
    }


    
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialLevel : Level
{
    public int currentStage = 0;
    public Transform duplicate;
    public GameObject oldStage;
    //todo: spawn player here
    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!thisM.dead)
        {
            if (thisM.myPlayer.transform.position.y < deathYPos)
            {
                resetStage();
            }
        }

    }

    public override void OnConnected()
    {
        base.OnConnected();
        if (PhotonNetwork.isMasterClient)
        {
            OnLoaded();
        }
        else
        {
            thisM.StartStrucutresSync();

            foreach (SpawnSpot s in spawns)
            {
                s.NetworkInit();
            }
        }
    }

    public override void OnLoaded()
    {
        UIManager.thisM.changeUI(UIManager.thisM.startUI);
        //thisM.spawnPlayer();
    }

    public void StartTutorial()
    {
        UIManager.thisM.changeUI(tileDictionary.thisM.inGameUI);
        foreach (var item in structures)
        {
            item.gameObject.SetActive(false);
        }

        thisM.playerSetup((structures[currentStage] as TutorialStage).ss);
        resetStage();

    }

    public void generateStage(int stage)
    {
        if (oldStage != null)
            Destroy(oldStage);
        oldStage = Instantiate(structures[stage].gameObject, duplicate.position, duplicate.rotation, duplicate);
        var stageComponent = oldStage.GetComponent<TutorialStage>();
        stageComponent.StartStructure();
        oldStage.SetActive(true);
        thisM.myPlayer.spwanPos = stageComponent.ss.transform;
    }

    public void nextStage()
    {
        currentStage++;
        generateStage(currentStage);
        thisM.NetworkEnable(); 
    }
    public void resetStage()
    {
        generateStage(currentStage);
        thisM.NetworkEnable();
    }

    public override void playerDeath()
    {
        resetStage();
    }

}

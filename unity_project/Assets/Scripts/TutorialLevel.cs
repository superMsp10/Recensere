using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialLevel : Level
{
    public GameObject turorialUI;
    //todo: spawn player here
    public new void Start()
    {
        base.Start();
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

    }
}

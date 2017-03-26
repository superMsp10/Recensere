using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TestLevelUI : pauseUI
{
    public GameObject[] cameras;
    GameManager gm;
    int currCam = 0;
    public PlayerStructure testObject;

    void Start()
    {


    }

    public void switchCamera()
    {
        gm = GameManager.thisM;

        if (currCam < cameras.Length)
        {
            gm.ChangeCam(cameras[currCam]);
            currCam++;
        }
        else
        {
            currCam = 0;
            gm.ChangeCam(gm.myPlayer.thisCam);

        }
    }

    bool open = false;
    public void runCode1()
    {
        if (open)
        {
            testObject.closeLid();
        }
        else
        {
        testObject.openLid();

        }
        open = !open;
    }

    public void NetworkIns(string s)
    {
        gm = GameManager.thisM;

        PhotonNetwork.Instantiate(s,
                        gm.myPlayer.transform.position,
                           Quaternion.identity, 0, null);

    }


}

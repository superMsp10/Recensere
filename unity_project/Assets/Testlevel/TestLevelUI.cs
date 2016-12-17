using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TestLevelUI : pauseUI
{
    public GameObject[] cameras;
    GameManager gm;
    int currCam = 0;
    public GameObject testObject;

    void Start()
    {
        if (testObject != null)
            invManager.thisInv.addHoldable(testObject.GetComponent<Holdable>(), 1);
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

}

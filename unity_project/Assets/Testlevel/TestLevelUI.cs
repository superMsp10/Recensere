using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TestLevelUI : pauseUI
{
    public GameObject[] cameras;
    GameManager gm = GameManager.thisM;
    int currCam = 0;
    public Holdable giveItem;

    public void switchCamera()
    {

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

    public void runCode1()
    {
        //PhotonNetwork.player.customProperties["Placed"] = getHashInt(PhotonNetwork.player.customProperties["Kills"]) + 10;
        //PhotonNetwork.player.customProperties["Kills"] = getHashInt(PhotonNetwork.player.customProperties["Placed"]) + 10;
        //PhotonNetwork.player.customProperties["Destroyed"] = getHashInt(PhotonNetwork.player.customProperties["Destroyed"]) + 10;
        //PhotonNetwork.player.customProperties["Deaths"] = getHashInt(PhotonNetwork.player.customProperties["Deaths"]) + 10;
        invManager.thisInv.addHoldable(giveItem, 5);
    }

    int getHashInt(object o)
    {
        return int.Parse(o.ToString());
    }

    public void NetworkIns(string s)
    {
        gm = GameManager.thisM;

        PhotonNetwork.Instantiate(s,
                        gm.myPlayer.transform.position,
                           Quaternion.identity, 0, null);

    }


}

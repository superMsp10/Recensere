using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class TestLevelUI : pauseUI
{
    public GameObject[] cameras;
    GameManager gm;
    int currCam = 0;
    public Structure testStructure;

    void Start()
    {

        testStructure.UpdateStructure(JSONArray.Parse("[{\"Position\":\"(2.0, 2.0, 0.0)\",\"Health\":50,\"PrefabName\":\"defaultFloorTile\",\"Name\":\"defaultFloorTile\"},{\"Position\":\"(0.0, 20.0, 0.0)\",\"Health\":1000,\"PrefabName\":\"defaultFloorTile\",\"Name\":\"defaultFloorTile2\"}]"));
        GameObject g = new GameObject("newStructure", new Structure().GetType());
        g.GetComponent<Structure>().startStructure(JSONArray.Parse("[{\"Position\":\"(2.0, 2.0,5.0)\",\"Health\":50,\"PrefabName\":\"defaultFloorTile\",\"Name\":\"defaultFloorTile\"},{\"Position\":\"(0.0, 0.0, 5.0)\",\"Health\":1000,\"PrefabName\":\"defaultFloorTile\",\"Name\":\"defaultFloorTile2\"}]"));
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

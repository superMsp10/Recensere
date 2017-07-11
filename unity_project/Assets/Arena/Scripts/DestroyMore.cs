using UnityEngine;
using System.Collections;

public class DestroyMore : Objective
{

    int org = -1;
    int needed = 0;
    public override void Initialize()
    {
        org = (int)PhotonNetwork.player.CustomProperties["Destroyed"];
        needed = (int)((new System.Random().NextDouble() + iteration) * 50) + 2;
    }

    public override bool reuseable
    {
        get
        {
            return true;
        }
    }

    public override string description
    {
        get
        {
            return "Destroy " + needed + " more items";
        }
    }

    public override string done
    {
        get
        {
            return "You destroyed " + Mathf.Abs(org - (int)PhotonNetwork.player.CustomProperties["Destroyed"]) + " items";
        }
    }

    public override bool CheckCompleted()
    {

        if ((int)PhotonNetwork.player.CustomProperties["Destroyed"] >= (org + needed))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}

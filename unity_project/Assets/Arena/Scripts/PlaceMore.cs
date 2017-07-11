using UnityEngine;
using System.Collections;

public class PlaceMore : Objective
{
    int org = -1;
    int needed = 0;

    public override void Initialize()
    {
        org = (int)PhotonNetwork.player.CustomProperties["Placed"];
        needed = (int)((new System.Random().NextDouble() + iteration) * 10) + 2;

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
            return "Place " + needed + " more placeable items";
        }
    }

    public override string done
    {
        get
        {
            return "You placed " + Mathf.Abs(org - (int)PhotonNetwork.player.CustomProperties["Placed"]) + " items";
        }
    }

    public override bool CheckCompleted()
    {

        if ((int)PhotonNetwork.player.CustomProperties["Placed"] >= (org + needed))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}

using UnityEngine;
using System.Collections;

public class KillMore : Objective
{

    int org = -1;
    int needed = 0;
    public override void Initialize()
    {
        org = (int)PhotonNetwork.player.CustomProperties["Kills"];
        needed = iteration + 2;
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
            return "Get " + needed + " more kills";
        }
    }

    public override string done
    {
        get
        {
            return "You got " + Mathf.Abs(org - (int)PhotonNetwork.player.CustomProperties["Kills"]) + " kills";
        }
    }

    public override bool CheckCompleted()
    {

        if ((int)PhotonNetwork.player.CustomProperties["Kills"] >= (org + needed))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}

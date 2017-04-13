using UnityEngine;
using System.Collections;

public class KillFirst : Objective
{
    public override string description
    {
        get
        {
            return "Get one kill";
        }
    }

    public override bool reuseable
    {
        get
        {
            return true;
        }
    }

    public override string done
    {
        get
        {
            return "You killed one";
        }
    }

    public override bool CheckCompleted()
    {
        if((int)PhotonNetwork.player.customProperties["Kills"] >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
           
    }

}

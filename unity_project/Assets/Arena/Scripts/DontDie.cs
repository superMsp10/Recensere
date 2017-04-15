using UnityEngine;
using System.Collections;

public class DontDie : Objective
{

    float org = -1;
    float needed = 0;
    float startedTime;
    public override void Initialize()
    {
        org = Time.time;
        needed = iteration * 10 + 30;
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
            return "Dont die for " + needed + " seconds";
        }
    }

    public override string done
    {
        get
        {
            return "You survived for " + (Time.time - org) + " seconds";
        }
    }

    public override bool CheckCompleted()
    {

        if ((Time.time - org) >= needed)
        {
            return true;
        }
        else
        {
            org = Time.time;
            return false;
        }

    }

}

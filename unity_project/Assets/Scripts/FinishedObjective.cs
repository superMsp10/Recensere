using UnityEngine;
using System.Collections;

public class FinishedObjective: Objective
{


    public override string description
    {
        get
        {
            return "All objectives have been successfuly completed!";
        }
    }

    public override string done
    {
        get
        {
            return "No remaining objectives!";
        }
    }

    public override bool CheckCompleted()
    {
        return false;
    }

}


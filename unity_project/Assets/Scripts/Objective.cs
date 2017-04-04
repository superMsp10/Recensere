using UnityEngine;
using System.Collections;

public class Objective
{
    public bool completed = false;

    public Objective()
    {
    }

    public virtual string description
    {
        get
        {
           return "generic objective: do things";
        }
    }

    public virtual string done
    {
        get
        {
            return "completed generic objective";
        }
    }


    public virtual bool CheckCompleted()
    {
        return completed;
    }
}


using UnityEngine;
using System.Collections;

public class Objective
{
    public bool completed = false;
    public int iteration = 0;

    public virtual bool reuseable
    {
        get
        {
            return false;
        }
    }

    public virtual void Initialize()
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
        if (!completed)
        {
            completed = true;
            return true;
        }
        return completed;
    }
}


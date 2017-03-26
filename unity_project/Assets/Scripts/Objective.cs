using UnityEngine;
using System.Collections;

public class Objective
{
    public  string description = "generic objective";
    public  string done = "completed generic objective";


    public virtual bool CheckCompleted()
    {
        return false;
    }
}


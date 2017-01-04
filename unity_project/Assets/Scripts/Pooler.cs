using UnityEngine;
using System.Collections.Generic;

public class Pooler
{
    public List<GameObject> active = new List<GameObject>();
    public List<GameObject> useable = new List<GameObject>();
    public GameObject original;
    protected int max;


    public Pooler(int max, GameObject org)
    {
        original = org;
        this.max = max;
    }

    public virtual GameObject getObject()
    {
        GameObject ret = null;
        if (active.Count >= max)
        {
            ret = active[0];
            active.Remove(ret);
            Poolable p = ret.GetComponent<Poolable>();
            if (p == null)
            {
                Debug.LogError(original.name + " is not poolable");
            }
            else
            {
                p.reset(false);
            }
            useable.Add(ret);

        }
        else if ((useable.Count - 1) < 0)
        {
            //						Debug.Log ("Useable" + useable.Count);
            Poolable p;
            useable.Add(GameObject.Instantiate(original));
            ret = useable[useable.Count - 1];
            ret.name = "ObjectPooled: " + (useable.Count + active.Count).ToString();
            p = ret.GetComponent<Poolable>();
            if (p == null)
            {
                Debug.LogError(original.name + " is not poolable");
            }
            else
            {
                p.reset(false);
            }

            ret = useable[useable.Count - 1];

        }
        else
        {
            ret = useable[useable.Count - 1];

        }



        useable.Remove(ret);
        active.Add(ret);
        if (ret == null)
        {
            Debug.LogError("Poolable object has been destroyed externally; Effects will not work properly");

        }
        else
            ret.GetComponent<Poolable>().reset(true);
        return ret;

    }

    public virtual void disposeObject(Poolable p)
    {
        active.Remove(p.gameobject);
        useable.Add(p.gameobject);
        p.reset(false);
    }
}

using UnityEngine;
using System.Collections.Generic;

public class NetworkPooler : Pooler
{

    public NetworkPooler(int max, GameObject org) : base(max, org)
    {
    }

    public override GameObject getObject()
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
            useable.Add(PhotonNetwork.Instantiate(original.name, Vector3.zero, Quaternion.identity, 0));
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

    public override void disposeObject(Poolable p)
    {
        active.Remove(p.gameobject);
        useable.Add(p.gameobject);
        p.reset(false);
    }
}

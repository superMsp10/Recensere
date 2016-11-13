using UnityEngine;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager thisM;
    private GameObject crackFX;

    [HideInInspector]
    public Pooler
            crackPooler = null;

    public float decalReset;
    public int maxDecals;
    public PhotonView p;

    void Awake()
    {
        if (thisM == null)
            thisM = this;
        else
        {
            Debug.Log("FX Exists");
            enabled = false;
        }
    }

    void Start()
    {
        crackFX = tileDictionary.thisM.hitDecal;
        crackPooler = new Pooler(maxDecals, crackFX);

    }


    public void AddCracksFX(Vector3 normal, Vector3 point, Tile t, float dmgPercent)
    {
        //Debug.Log("Adding cracks FX at Effects Manager");

        if (t.attached.Count < t.limit)
        {
            if (crackFX == null)
            {
                Debug.Log("No crackFX");
                return;
            }
            //Debug.Log("FX within limits");

            float size = Mathf.Lerp(0.1f, t.tileSize, dmgPercent);
            Quaternion hitRotation = Quaternion.FromToRotation(normal, Vector3.forward);
            GameObject g = crackPooler.getObject();
            g.transform.parent = transform;
            g.transform.localScale = new Vector3(size, size, size);

            g.transform.position = point - (normal * 0.001f);
            g.transform.rotation = hitRotation;
            g.GetComponent<Timer>().StartTimer(decalReset);
            t.attach(g);

            p.RPC("SyncCracksFX", PhotonTargets.Others, normal, point, dmgPercent, t.thisStructure.name, t.name);
        }
    }



    [PunRPC]
    public void SyncCracksFX(Vector3 normal, Vector3 point, float dmgPercent, string structureName, string tileName)
    {
        if (!GameManager.thisM.loaded)
            return;
        Debug.Log("received syncracks");

        Tile t = GameManager.thisM.GetTile(structureName, tileName);
        if (t != null)
        {
            if (t.attached.Count < t.limit)
            {
                if (crackFX == null)
                {
                    Debug.Log("No crackFX");
                    return;
                }
                float size = Mathf.Lerp(0.1f, t.tileSize, dmgPercent);
                Quaternion hitRotation = Quaternion.FromToRotation(normal, Vector3.forward);
                GameObject g = crackPooler.getObject();
                g.transform.parent = transform;
                g.transform.localScale = new Vector3(size, size, size);

                g.transform.position = point - (normal * 0.001f);
                g.transform.rotation = hitRotation;
                g.GetComponent<Timer>().StartTimer(decalReset);
                t.attach(g);
            }
        }
        else
        {
            Debug.Log("Tile Add FX Sync Request for Tile: " + tileName + " does not exist in Structure: " + structureName);
        }




    }
}

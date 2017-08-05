using UnityEngine;
using System.Collections.Generic;
using Boomlagoon.JSON;
using System;

public abstract class Level : MonoBehaviour
{
    public GameObject cam;
    public float deathYPos;
    pauseUI pause;

    public GameManager thisM;

    public bool autoSetup = false;

    public SpawnSpot[] spawns;
    public List<Structure> structures;

    public Transform items;
    public Transform StructuresTransform;

    public GameObject spawnStructure;    

    // Use this for initialization
    protected void Start()
    {
        if (autoSetup)
        {
            structures = new List<Structure>(StructuresTransform.GetComponentsInChildren<Structure>());
            foreach (var structure in structures)
            {
                structure.autoSetup();
            }
            spawns = FindObjectsOfType<SpawnSpot>();
        }
        //				cam = FindObjectOfType<Camera> ().gameObject;
        thisM.currLevel = this;
        thisM.ChangeCam(cam);
        pause = tileDictionary.thisM.pauseUI.GetComponent<pauseUI>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!thisM.dead)
        {
            if (thisM.myPlayer.transform.position.y < deathYPos)
            {
                thisM.NetworkDisable();
                StartCoroutine(pause.Respawn(8f));
            }
        }

    }

    public virtual void InitStrucutres(string JSON)
    {
        foreach (JSONValue j in JSONArray.Parse(JSON))
        {
            JSONObject structJSON = j.Obj;
            StructureInit(j.Obj, structures.Find(t => t.name == structJSON.GetString("Name")));
        }
    }

    public virtual void InitStrucutre(JSONObject JSON)
    {
        Structure s = structures.Find(t => t.name == JSON.GetString("Name"));
        StructureInit(JSON, s);
    }

    protected void StructureInit(JSONObject JSON, Structure s)
    {
        if (s == null)
        {
            var insName = JSON.GetString("PrefabName");
            if (insName != null)
            {

                //Debug.Log("Instantiating");
                s = ((GameObject)
                          Instantiate(Resources.Load(insName), JSONObject.StringToVector3(JSON.GetString("Position")), JSONObject.StringToQuaternion(JSON.GetString("Rotation")), StructuresTransform)
                    ).GetComponent<Structure>();
                s.name = JSON.GetString("Name");
            }
            else
            {
                s = CreateStructure(JSON);
            }
        }

        structures.Add(s);
        s.UpdateStructure(JSON.GetArray("Tiles"));
        Debug.Log("Updating " + s.name);
    }

    private Structure CreateStructure(JSONObject tileJSON)
    {
        GameObject g = new GameObject(tileJSON.GetString("Name"), new Structure().GetType());
        g.transform.parent = StructuresTransform;
        Structure s = g.GetComponent<Structure>();
        //s.startStructure(tileJSON.GetArray("Tiles"));
        return s;
    }

    public virtual string getStructuresInit()
    {
        JSONArray j = new JSONArray();
        foreach (Structure s in structures)
        {
            j.Add(s.GenerateJSON());
        }
        return j.ToString();
    }

    //public virtual void generateArena(MapGenerator m)
    //{
    //    //				liveTiles = gen.generateMap (levelStart);
    //    //liveTiles = m.findTiles(levelStart);
    //    lootTiles =GameObject.FindObjectsOfType<LootTile>();

    //    if (PhotonNetwork.isMasterClient)
    //        InvokeRepeating("generateLoot", 0, lootTime);

    //}

    //public virtual void generateArena()
    //{
    //    //Debug.Log("Generating Arena");

    //    MapGenerator gen = new MapGenerator(Map.firstMap);
    //    liveTiles = gen.findTiles(levelStart);
    //    lootTiles = gen.findLootTiles();
    //    if (PhotonNetwork.isMasterClient)
    //        InvokeRepeating("generateLoot", 0, lootTime);
    //}

    public virtual void playerDeath()
    {

    }


    public virtual void OnConnected()
    {
        //Debug.Log ("Connected in GameManager");
        UIManager.thisM.currentUI = null;
    }

    public virtual void OnLoaded()
    {
        thisM.spawnPlayer();
    }

    public JSONArray StructureNames()
    {
        JSONArray ret = new JSONArray();

        foreach (Structure item in structures)
        {
            ret.Add(item.name);
        }
        return ret;
    }

}


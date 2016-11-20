using UnityEngine;
using System.Collections.Generic;
using Boomlagoon.JSON;
using System;

public abstract class Level : MonoBehaviour
{
    public Transform levelStart;
    public Transform items;
    public Transform StructuresTransform;


    public SpawnSpot[] sS;

    public LootTile[] lootTiles;
    public float lootTime;
    public float fuelRate = 2f;

    public List<Structure> structures = new List<Structure>();

    public GameObject cam;

    public GameObject spawnStructure;



    // Use this for initialization
    void Start()
    {
        //				cam = FindObjectOfType<Camera> ().gameObject;
        GameManager.thisM.currLevel = this;
        GameManager.thisM.ChangeCam(cam);
        lootTiles = GameObject.FindObjectsOfType<LootTile>();
        sS = FindObjectsOfType<SpawnSpot>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void InitStrucutres(string JSON)
    {
        foreach (JSONValue j in JSONArray.Parse(JSON))
        {
            JSONObject structJSON = j.Obj;
            Structure s = structures.Find(t => t.name == structJSON.GetString("Name"));
            if (s == null)
            {
                s = CreateStructure(structJSON);
                structures.Add(s);
            }
            s.UpdateStructure(structJSON.GetArray("Tiles"));
            Debug.Log("Updating" + s.name);


        }

        Debug.Log("Updating Loot Tiles");
        foreach (LootTile t in lootTiles)
        {
            t.NetworkInit();
        }
    }

    public virtual void InitStrucutre(JSONObject JSON)
    {
        Structure s = structures.Find(t => t.name == JSON.GetString("Name"));
        if (s == null)
        {
            s = CreateStructure(JSON);
            structures.Add(s);
        }
        s.UpdateStructure(JSON.GetArray("Tiles"));
        Debug.Log("Updating" + s.name);

    }

    private Structure CreateStructure(JSONObject tileJSON)
    {
        GameObject g = new GameObject(tileJSON.GetString("Name"), new Structure().GetType());
        g.transform.parent = StructuresTransform;
        Structure s = g.GetComponent<Structure>();
        //s.startStructure(tileJSON.GetArray("Tiles"));
        return s;
    }

    public virtual string getStrucutresInit()
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

    public void generateLoot()
    {
        Debug.Log("Generated loot");
        foreach (LootTile t in lootTiles)
        {
            t.generateLoot();
        }
    }

    public void OnConnected()
    {
        //Debug.Log ("Connected in GameManager");
        UIManager.thisM.currentUI = null;
        GameManager.thisM.instantiatePlayer();

        if (PhotonNetwork.isMasterClient)
            InvokeRepeating("generateLoot", 0, lootTime);
    }

}


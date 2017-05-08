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



    public List<Structure> structures = new List<Structure>();

    public GameObject cam;

    public GameObject spawnStructure;
    public float deathYPos;
    pauseUI pause;

    public GameManager thisM;

    List<string> deathMessages = new List<string>() { "Went too <color=red>deep</color>", "You got lost in the <size=24><color=black>abyss</color></size>", "You have reached the point of no <b>return</b>",
        "<color=purple><i>Hypnic Jerk</i>, but this time its real</color>", "<color=grey>Free fall: No air resistance just gravity</color>", "Searching for new <color=green>grounds</color> to land on",
        "<size=36>Cant stop the falling by \n Justin Timberlake</size>", "<color=orange>You are falling your physics class</color>" };

    // Use this for initialization
    public void Start()
    {
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

                pause.deathMessage = deathMessages[UnityEngine.Random.Range(0, deathMessages.Count)];
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
            Structure s = structures.Find(t => t.name == structJSON.GetString("Name"));
            if (s == null)
            {
                s = CreateStructure(structJSON);
                structures.Add(s);
            }
            s.UpdateStructure(structJSON.GetArray("Tiles"));
            Debug.Log("Updating" + s.name);


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
        //Debug.Log("Updating" + s.name);

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



    public virtual void OnConnected()
    {
        //Debug.Log ("Connected in GameManager");
        UIManager.thisM.currentUI = null;
        sS = FindObjectsOfType<SpawnSpot>();
    }

    public virtual void OnLoaded()
    {
        thisM.instantiatePlayer();
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


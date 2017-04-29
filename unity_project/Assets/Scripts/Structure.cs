using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class Structure : MonoBehaviour
{
    public List<Tile> tiles = new List<Tile>();
    public List<JSONObject> destroyedTiles = new List<JSONObject>();
    public List<Tile> editedTiles = new List<Tile>();


    public bool autoAddTilesOnStart = false;

    // Use this for initialization
    public void Start()
    {
        if (autoAddTilesOnStart)
        {
            Debug.Log("Auto initializing strucuture for: " + name);
            tiles = new List<Tile>();
            foreach (Tile t in GetComponentsInChildren<Tile>())
            {
                tiles.Add(t);
                t.thisStructure = this;
                t.name = "Tile#" + tiles.Count;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void StartStructure(JSONArray tileDetails)
    {
        Debug.Log("Creating Structure: ");

        foreach (JSONValue j in tileDetails)
        {
            JSONObject tileJSON = j.Obj;
            Tile tile = CreateTile(tileJSON);
            tile.FromJSON(tileJSON);
            tiles.Add(tile);
        }

    }

    public virtual JSONObject GenerateJSON()
    {
        JSONObject ret = new JSONObject();
        //Add structure info
        ret.Add("Name", name);

        //Add tiles info
        JSONArray arr = new JSONArray();
        foreach (Tile t in tiles)
        {
            arr.Add(t.ToJSON());
        }
        foreach (JSONObject j in destroyedTiles)
        {
            arr.Add(j);
        }
        ret.Add("Tiles", arr);

        //Debug.Log(ret.ToString());
        return ret;
    }

    public void UpdateStructure(JSONArray tileDetails)
    {
        foreach (JSONValue j in tileDetails)
        {
            JSONObject tileJSON = j.Obj;
            Tile tile = tiles.Find(t => t.name == tileJSON.GetString("Name"));
            if (tile == null)
            {
                tile = CreateTile(tileJSON);
                tiles.Add(tile);
                tile.FromJSON(tileJSON);
                //Debug.Log("Creating" + tile.name);
            }
            else if (tileJSON.GetBoolean("Destroyed"))
            {
                //Debug.Log("Destroying" + tile.name);
                tile.Destroy(false, false);
            }
            else
            {
                tile.FromJSON(tileJSON);
                //Debug.Log("Updating" + tile.name);
            }
        }

    }

    private Tile CreateTile(JSONObject tileJSON)
    {
        GameObject newTile = (GameObject)GameObject.Instantiate(Resources.Load(tileJSON.GetString("PrefabName")));
        newTile.name = tileJSON.GetString("Name");
        newTile.transform.parent = transform;
        //Debug.Log("Created: " + newTile.name);
        Tile t = newTile.GetComponent<Tile>();
        t.thisStructure = this;
        return t;
    }

    public void DestroyTile(Tile t)
    {
        tiles.Remove(t);
        JSONObject ret = t.ToJSON();
        //ret.Add("Destroyed", true);
        destroyedTiles.Add(ret);

    }

}
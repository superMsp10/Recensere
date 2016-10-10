using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class Structure : MonoBehaviour
{
    public List<Tile> tiles = new List<Tile>();

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void startStructure(JSONArray tileDetails)
    {
        Debug.Log("Creating Structure: ");

        foreach (JSONValue j in tileDetails)
        {
            JSONObject tileJSON = j.Obj;
            Tile tile = CreateTile(tileJSON);
            tile.FromJSON(tileJSON);
        }
    }

    public string GenerateJSON()
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
        ret.Add("Tiles", arr);

        Debug.Log(ret.ToString());
        return ret.ToString();
    }

    public void UpdateStructure(JSONArray tileDetails)
    {
        List<Tile> newTiles = new List<Tile>();
        foreach (JSONValue j in tileDetails)
        {
            JSONObject tileJSON = j.Obj;
            Tile tile = tiles.Find(t => t.name == tileJSON.GetString("Name"));
            if (tile == null)
            {
                tile = CreateTile(tileJSON);
                newTiles.Add(tile);
            }
            tile.FromJSON(tileJSON);
            Debug.Log("Updating" + tile.name);
        }
        tiles.AddRange(newTiles);
    }

    private Tile CreateTile(JSONObject tileJSON)
    {
        GameObject newTile = (GameObject)GameObject.Instantiate(Resources.Load(tileJSON.GetString("PrefabName")));
        newTile.name = tileJSON.GetString("Name");
        newTile.transform.parent = transform;
        Debug.Log("Created: " + newTile.name);
        Tile t = newTile.GetComponent<Tile>();
        return t;
    }
}
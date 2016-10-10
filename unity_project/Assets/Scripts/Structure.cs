using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class Structure : MonoBehaviour
{
    public Tile[] tiles;

    // Use this for initialization
    void Start()
    {
        GenerateJSON();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GenerateJSON()
    {
        JSONObject ret = new JSONObject();
        ret.Add("Name", name);
        foreach (Tile t in tiles)
        {
            ret.Add(t.name, t.ToJSON());
        }
        Debug.Log(ret.ToString());
        return ret.ToString();
    }
}
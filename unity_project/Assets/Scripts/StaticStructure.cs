using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

public class StaticStructure : Structure
{

    public override JSONObject GenerateJSON()
    {
        JSONObject ret = new JSONObject();
        //Add structure info
        ret.Add("Name", name);

        //Add tiles info
        JSONArray arr = new JSONArray();
        foreach (Tile t in editedTiles)
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

}


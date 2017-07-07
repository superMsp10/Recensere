using Boomlagoon.JSON;
using UnityEngine;


public class PlayerStructure : Structure
{
    public Transform spawnPosition;
    public string prefabName;
    public bool isLocal = false;


    public override JSONObject GenerateJSON()
    {
        JSONObject ret = new JSONObject();
        //Add structure info
        ret.Add("Name", name);
        ret.Add("Position", transform.position.ToString());
        ret.Add("Rotation", transform.rotation.ToString());
        ret.Add("PrefabName", prefabName);

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

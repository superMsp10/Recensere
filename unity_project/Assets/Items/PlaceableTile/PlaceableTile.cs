using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class PlaceableTile : Placeable
{
    public Transform graphics;
    public Structure thisStructure;
    public GameManager thisG;


    public override void buttonUP()
    {
        if (thisStructure == null)
            thisStructure = tileDictionary.thisM.playerPlaceableStructure;

        if (thisG == null)
            thisG = GameManager.thisM;

        thisView.RPC("buttonUpBy", PhotonTargets.All, null);

        if (!collided)
        {
            Tile t = ((GameObject)Instantiate(instantiateObject, graphics.position, graphics.rotation, null)).GetComponent<Tile>();
            t.thisStructure = thisStructure;
            t.transform.parent = thisStructure.transform;
            t.name = "Placed Tile: #" + thisStructure.tiles.Count;
            thisStructure.tiles.Add(t);

            JSONObject jStructure = new JSONObject();
            jStructure.Add("Name", thisStructure.name);
            JSONArray jTiles = new JSONArray();
            jTiles.Add(t.ToJSON());
            jStructure.Add("Tiles", jTiles);
            thisG.view.RPC("setStructure",PhotonTargets.Others, jStructure.ToString());

            amount--;

        }


    }
}

using UnityEngine;
using System.Collections;

public class Grenade_Item : Item_Throwable
{
    public Vector3 handRotation;

    [PunRPC]
    protected override void pickedUpBy(int viewID)
    {
        GetComponent<Renderer>().material.color = normal;
        thisPlayer = GameManager.thisM.getPlayerByViewID(viewID);
        _pickable = false;
        r.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer(thisPlayer.handLayer);
        transform.parent = thisPlayer.right_hand;
        transform.position = thisPlayer.right_hand.position;
        transform.localRotation = Quaternion.Euler(handRotation);
        if (!selected)
            gameObject.SetActive(false);
    }

}

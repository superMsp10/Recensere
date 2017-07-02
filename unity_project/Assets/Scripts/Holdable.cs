using UnityEngine;
using System.Collections;

public interface Holdable
{

    Sprite holdUI
    {
        get;
    }
    int stackSize
    {
        get;
    }
    string description
    {
        get;
    }
    bool pickable
    {
        get;
    }
    int amount
    {
        get;
        set;
    }




    bool buttonDown();
    bool buttonUP();

    void onSelect();
    void onDeselect();
    void onPickup();
    void onDrop();
    void onRemove();


    void resetPick();


}

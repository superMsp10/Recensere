using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class invManager : slotCollection, UIState
{

    private GameManager thismanage;
    public static invManager thisInv;

    public UIslot SelectedSlot;
    public int SelectedInt;

    public Color highlighted;
    public Color normal;



    //Mono stuff
    void Awake()
    {

        if (thisInv == null)
            thisInv = this;

    }

    void Start()
    {
        thismanage = GameManager.thisM;
    }

    void Update()
    {
        if (SelectedSlot == null)
        {
            selectSlot(0);
        }
        if (!thismanage.paused)
        {
            if (Input.GetAxisRaw("slotChangeWheel") > 0 || Input.GetKeyDown(KeyCode.E))
            {
                selectSlot(SelectedInt + 1);
            }
            else if (Input.GetAxisRaw("slotChangeWheel") < 0 || Input.GetKeyDown(KeyCode.Q))
            {
                selectSlot(SelectedInt - 1);
            }
            if (SelectedSlot.holding != null)
            {
                if (Input.GetButtonDown("InvSelected"))
                {
                    SelectedSlot.buttonDown();
                }
                if (Input.GetButtonUp("InvSelected"))
                {
                    SelectedSlot.holding.buttonUP();
                }
                if (Input.GetKeyUp(KeyCode.T))
                {
                    SelectedSlot.onDrop();
                }
                //								if (SelectedSlot.holding != null)
                //										SelectedSlot.holding.updateItem ();
            }
        }

    }

    public void randomPickup()
    {
        thismanage.myPlayer.PlayRandomPickup();
        thismanage.myPlayer.thisView.RPC("PlayRandomPickup", PhotonTargets.Others);
    }

    //UIState stuff
    public void StartUI()
    {
        gameObject.SetActive(true);
        GameManager.thisM.paused = false;

    }

    public void EndUI()
    {
        gameObject.SetActive(false);

    }

    public void UpdateUI()
    {
        UIManager.thisM.changeUI(tileDictionary.thisM.pauseUI);

    }

    //slot collection stuff
    public void selectSlot(int i)
    {
        if (i >= 0 && i < slots.Capacity)
        {
            SelectedInt = i;
            if (SelectedSlot != null)
            {
                SelectedSlot.outline.color = normal;
                SelectedSlot.onDeselect();
            }
            SelectedSlot = slots[i];
            SelectedSlot.outline.color = highlighted;
            SelectedSlot.onSelect();

        }
    }

    public void clearInv()
    {
        foreach (UIslot s in slots)
        {
            s.onDrop();

        }

    }

    public override int addHoldable(Holdable h, int amount)
    {

        //		check for existing stacks
        List<UIslot> returnSlots = SlotsWithHoldable(h);
        if (returnSlots.Count > 0)
        {
            foreach (var s in returnSlots)
            {
                int space = h.stackSize - s.amount;
                if (space > amount)
                {
                    s.amount += amount;
                    //										Debug.Log ("Slot collection add holdable item exists \n only changeing amount. Can hold all");
                    randomPickup();
                    return 0;
                }
                else
                {
                    s.amount += space;
                    amount -= space;
                    //										Debug.Log ("Slot collection add holdable item exists \n only changeing amount. Can not hold all");
                    randomPickup();
                }
            }
        }

        //		create new stacks
        foreach (var s in slots)
        {
            if (s.holding == null)
            {
                if (amount > h.stackSize)
                {
                    //										Debug.Log ("Slot collection add holdable creating stack. \n Can not hold all");
                    s.changeHolding(h, h.stackSize);
                    amount -= h.stackSize;
                    randomPickup();

                }
                else
                {
                    //										Debug.Log ("Slot collection add holdable creating stack. \n Can hold all");
                    s.changeHolding(h, amount);
                    randomPickup();

                    return 0;


                }
            }
        }
        //				Debug.Log ("Slot collection add holdable exiting with " + amount + " objects");

        return amount;
    }


}

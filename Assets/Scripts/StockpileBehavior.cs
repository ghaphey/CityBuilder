using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class StockpileBehavior : MonoBehaviour, JCustomMessageTarget
{
    [SerializeField] private GameObject StockpileSlot;
    [SerializeField] private float zOffset = -0.001f;
    [SerializeField] private float halfWidth = 0.5f;

    private GameObject[,] slotObject = null;
    private StockpileSlotBehavior[,] slots = null;
    
    public void SizeStockpile(Vector3 point)
    {
        float x = point.x;
        float y = point.y;
        print("X: " + x);
        print("Y: " + y);
        int n = Mathf.CeilToInt(Mathf.Abs(x));
        int m = Mathf.CeilToInt(Mathf.Abs(y));
        slotObject = new GameObject[n,m];
        slots = new StockpileSlotBehavior[n, m];
        
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                slotObject[i, j] = Instantiate(StockpileSlot, transform, false);
                slots[i, j] = slotObject[i, j].GetComponent<StockpileSlotBehavior>();
                slots[i, j].InitStock();
                if (x >= 0 && y >= 0)
                    slotObject[i, j].transform.localPosition = new Vector3(i, j, zOffset);
                else if (x >= 0 && y < 0)
                    slotObject[i, j].transform.localPosition = new Vector3(i, -j, zOffset);
                else if (x < 0 && y >= 0)
                    slotObject[i, j].transform.localPosition = new Vector3(-i, j, zOffset);
                else if (x < 0 && y < 0)
                    slotObject[i, j].transform.localPosition = new Vector3(-i, -j, zOffset);
            }
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }


    public int DepositItem(int amount, GameObject item)
    {
        if (!DepositSpace(item.name))
            return amount;
        foreach (StockpileSlotBehavior s in slots)
        {
            //check for named item that have capacityleft, then deposit (taking remainder into amount
            if (s.ItemCheck(item.name) && s.CapacityLeft())
            {
                amount = s.DepositItem(amount);
                s.UpdateItem();
            }
            if (amount == 0)
                return 0;
        }
        // if exhausted list for named item, find next null slot and deposit
        foreach (StockpileSlotBehavior s in slots)
        {
            if (s.ItemCheck(null))
            {
                amount = s.DepositItem(amount);
                s.CreateItem(item);
            }
            if (amount == 0)
                return 0;
        }
        //if no additional null slots found, return remainder
        return amount;
    }

    public int TakeItem(int amount, string itemName )
    {
        if (!ItemAvailable(itemName))
            return 0;
        int numleft = 0;
        foreach (StockpileSlotBehavior s in slots)
        {
            if (s.ItemCheck(itemName))
            {
                numleft = s.TakeItem(amount);
                s.UpdateItem();
                if (numleft == amount)
                    return numleft;
                else
                    amount -= numleft;
            }
        }
        return numleft;
    }

    public bool ItemAvailable(string itemType)
    {
        foreach (StockpileSlotBehavior s in slots)
        {
            if (s.ItemCheck(itemType))
                return true;
        }
        return false;
    }

    public bool DepositSpace(string itemType)
    {
        foreach (StockpileSlotBehavior s in slots)
        {
            if (s.ItemCheck(null) || (s.ItemCheck(itemType) && s.CapacityLeft()))
                return true;
        }
        return false;
    }
}

public interface JCustomMessageTarget : IEventSystemHandler
{
    void SizeStockpile(Vector3 point);

}
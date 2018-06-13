using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileSlotBehavior : MonoBehaviour
{
    [SerializeField] private int capacity = 50;

    private Stock currItem;
    void Start()
    {
        currItem = new Stock
        {
            number = 0,
            rTag = null,
            item = null
        };
    }

    // Update is called once per frame
    private void Update()
    {
        if (currItem.number < 0)
        {
            currItem.number = 0;
            currItem.rTag = null;
        }
        else if (currItem.number == 0 && currItem.item != null)
        {
            Destroy(currItem.item);
            currItem.item = null;
            currItem.rTag = null;
        }
        else if (currItem.item != null)
            currItem.item.GetComponentInChildren<TextMesh>().text = currItem.number.ToString();
    }

    private void CreateItem(GameObject item)
    {
        currItem.item = item;
        currItem.rTag = item.name;
        GameObject newItem = Instantiate(item, gameObject.transform, false);
        newItem.transform.localPosition = new Vector3(0.0f, 01.5f, 0.0f);
        // will have to move it AFTER instantiation
    }

    public bool ItemCheck(string name)
    {
        if (name == currItem.rTag)
            return true;
        else
            return false;
    }
    //deposit item, MUST CHECK IF ITEM MATCHES FIRST, diff function
    public int DepositItem(int amount, GameObject newItem)
    {
        if (currItem.item == null)
            CreateItem(newItem);
        int numLeft = amount;
        if (currItem.number >= capacity)
            return numLeft;
        else
        {
            currItem.number += numLeft;
            if (currItem.number > capacity)
            {
                numLeft = capacity - currItem.number;
                currItem.number -= numLeft;
                return numLeft;
            }
            else
                return 0;
        }
    }

    public int TakeItem(int amount)
    {
        int numleft = amount;

        currItem.number -= amount;
        if (currItem.number >= 0)
            return numleft;
        else
        {
            numleft += currItem.number;
            currItem.number = 0;
            return numleft;
        }
    }

    private class Stock
    {
        public string rTag;
        public int number;
        public GameObject item;
    }
}

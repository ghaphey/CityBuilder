using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class StockpileBehavior : MonoBehaviour, JCustomMessageTarget
{
    [SerializeField] private GameObject StockpileSlot;

    private GameObject[,] stockSlots = null;

    private void Start()
    {
    }

    private void Update()
    {
    }
    
    public void SizeStockpile(float x, float z)
    {
        int n = Mathf.RoundToInt(Mathf.Abs(x));
        int m = Mathf.RoundToInt(Mathf.Abs(z));
        stockSlots = new GameObject[n,m];
        
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                stockSlots[i, j] = Instantiate(StockpileSlot, transform, false);
                if (x > 0 && z > 0)
                    stockSlots[i, j].transform.localPosition = new Vector3(i + 0.5f, 0.001f, j + 0.5f);
                else if (x > 0 && z < 0)
                    stockSlots[i, j].transform.localPosition = new Vector3(i + 0.5f, 0.001f, -j - 0.5f);
                /// continue this,
            }
        }

    }


    public int DepositItem(int amount, GameObject newItem)
    {
        return 0;
    }

    public int TakeItem(int amount)
    {
        return 0;
    }

    public bool SpaceAvailable(string itemType)
    {
        return false;
    }
}

public interface JCustomMessageTarget : IEventSystemHandler
{
    void SizeStockpile(float x, float z);

}
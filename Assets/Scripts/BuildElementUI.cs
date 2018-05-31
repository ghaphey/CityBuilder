using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildElementUI : MonoBehaviour {

    [SerializeField] GameObject house;
    [SerializeField] GameObject stockPile;

    private Camera mainCamera;
    private Vector3 rayCastTarget;

    private void Start()
    {
        mainCamera = (GameObject.Find("Main Camera")).GetComponent<Camera>();
    }

    private void Update()
    {
    }


    public void HousePressed ()
    {
        GameObject newHouse = Instantiate(house, new Vector3(0.0f, 0.4f, 0.0f), Quaternion.identity);
        ExecuteEvents.Execute<ICustomMessageTarget>(newHouse, null, (x, y) => x.PlacingBuilding(mainCamera)); 
    }
    public void StockpilePressed()
    {
        GameObject newStockpile = Instantiate(stockPile, new Vector3(0.0f, 0.001f, 0.0f), Quaternion.identity);
        ExecuteEvents.Execute<ICustomMessageTarget>(newStockpile, null, (x, y) => x.PlacingBuilding(mainCamera));
    }

    // TODO: fix bug w/ interface when selecting between stockpile and house, sometimes creates stockpile instead
    // when house clicked
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildElementUI : MonoBehaviour {

    [SerializeField] GameObject house;
    [SerializeField] GameObject stockPile;
    [SerializeField] GameObject selected;
    [SerializeField] GameObject buildings;
    [SerializeField] Button gatherButton;
    [SerializeField] Button stopGatherButton;
    [SerializeField] private Material boundMat;

    private Camera mainCamera;
    private GameObject currentSelected = null;

    private void Start()
    {
        mainCamera = (GameObject.Find("Main Camera")).GetComponent<Camera>();



       /* GameObject boundingBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boundingBox.transform.SetParent(GameObject.Find("Buildings").transform);
        boundingBox.transform.localPosition = new Vector3(0.5f, -10.0f, 0.5f);
        boundingBox.GetComponent<MeshRenderer>().material = boundMat;
        boundingBox.GetComponent<BoxCollider>().isTrigger = true;
        Rigidbody rb = boundingBox.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        boundingBox.name = "BoundingBox";
        boundingBox.tag = "BoundingBox";*/
    }

    private void Update()
    {
        MouseHandler();
    }

    private void MouseHandler()
    {
        if ( Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            // this function generates a particlesystem halo to denote whether an object is selected or not
            // assigns the object clicked to currentSelected
            if (Physics.Raycast(ray, out hit))
            {
                if ( hit.collider.gameObject.tag != "Ground")
                {
                    if (currentSelected != null)
                        Destroy(currentSelected.transform.Find("Selected(Clone)").gameObject);
                    GameObject newSelect = Instantiate(selected, hit.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
                    newSelect.transform.parent = hit.transform;
                    currentSelected = hit.collider.gameObject;
                    ContextMenuController();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (currentSelected != null)
            {
                Destroy(currentSelected.transform.Find("Selected(Clone)").gameObject);
                if (currentSelected.transform.parent.name == "Resources")
                    gatherButton.interactable = false;
                currentSelected = null;
            }
        }
    }

    private void ContextMenuController()
    {
        if (currentSelected != null && currentSelected.transform.parent != null)
        {
            if (currentSelected.transform.parent.name == "Resources")
            {
                gatherButton.interactable = true;
                if (currentSelected.tag == "Gather")
                {
                    gatherButton.interactable = false;
                    stopGatherButton.interactable = true;
                }
                else
                {
                    gatherButton.interactable = true;
                    stopGatherButton.interactable = false;
                }
            }
            else
            {
                gatherButton.interactable = false;
                gatherButton.interactable = false;
            }
        }
    }

    public void HousePressed ()
    {
        GameObject newHouse = Instantiate(house, new Vector3(0.0f, 0.4f, 0.0f), Quaternion.identity, buildings.transform);
        newHouse.name = "House";
        ExecuteEvents.Execute<ICustomMessageTarget>(newHouse, null, (x, y) => x.PlacingBuilding(mainCamera)); 
    }
    public void StockpilePressed()
    {  
        GameObject newStockpile = Instantiate(stockPile, new Vector3(0.0f, 0.001f, 0.0f), Quaternion.identity, buildings.transform);
        newStockpile.name = "Stockpile";
        ExecuteEvents.Execute<ICustomMessageTarget>(newStockpile, null, (x, y) => x.PlacingDynamic(mainCamera));
    }
    public void GatherPressed()
    {
        currentSelected.tag = "Gather";
        gatherButton.interactable = false;
        stopGatherButton.interactable = true;
    }
    public void StopGatherPressed()
    {
        currentSelected.tag = "Untagged";
        gatherButton.interactable = true;
        stopGatherButton.interactable = false;
    }

    // TODO: fix bug w/ interface when selecting between stockpile and house, sometimes creates stockpile instead
    // when house clicked
}

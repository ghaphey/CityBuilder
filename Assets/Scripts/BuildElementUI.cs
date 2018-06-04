using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildElementUI : MonoBehaviour {

    [SerializeField] GameObject house;
    [SerializeField] GameObject stockPile;
    [SerializeField] GameObject selected;
    [SerializeField] GameObject gatherToggle;

    private Camera mainCamera;
    private GameObject currentSelected = null;

    private void Start()
    {
        mainCamera = (GameObject.Find("Main Camera")).GetComponent<Camera>();
    }

    private void Update()
    {
        MouseHandler();
        ContextMenuController();
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
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentSelected.transform.Find("Selected(Clone)").gameObject);
            currentSelected = null;
        }
    }

    private void ContextMenuController()
    {
        if (currentSelected != null)
        {
            if (currentSelected.transform.parent.name == "Resources")
            {
                // must fix toggle to have persistance for objects using "gather" tag
                gatherToggle.SetActive(true);
            }
            else
            {
                gatherToggle.SetActive(false);
            }
        }
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
    public void GatherToggeled()
    {
        if (currentSelected.tag != "Gather")
            currentSelected.tag = "Gather";
        else
            currentSelected.tag = "Untagged";
    }

    // TODO: fix bug w/ interface when selecting between stockpile and house, sometimes creates stockpile instead
    // when house clicked
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownViewer : MonoBehaviour
{
    [SerializeField] private GameObject listItemPrefab;
    [SerializeField] private GameObject ContentPanel;

    // Use this for initialization
    void Start () {
        ConstructBuildingsList();
	}

    // Update is called once per frame
    void Update() {

    }

    public void ChangeViewMode(Text newMode)
    {
        switch ( newMode.text )
        {
            case "Buildings":
                ShowBuildings();
                break;
            case "People":
                ShowPeople();
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }

    private void ShowPeople()
    {
        gameObject.SetActive(true);

    }

    private void ShowBuildings()
    {
        gameObject.SetActive(true);
        ConstructBuildingsList();
        GameObject newItem = null;

        GameObject buildings = GameObject.Find("Buildings");
        print(buildings.transform.childCount);
        for (int i = 0; i < buildings.transform.childCount; i++)
        {
            ObjectController child = buildings.transform.GetChild(i).GetComponent<ObjectController>();
            if (child == null)
                print("oops");
            newItem = Instantiate(listItemPrefab);
            ListItemBehavior controller = newItem.GetComponent<ListItemBehavior>();
            controller.icon = child.icon;
            controller.iconName = child.name;
            newItem.transform.SetParent( ContentPanel.transform, false);
            newItem.transform.localScale = Vector3.one;

        }
       
    }

    private void ConstructBuildingsList ()
    {
        
    }

    private void NewListItem (Texture pic, string name)
    {

    }
}

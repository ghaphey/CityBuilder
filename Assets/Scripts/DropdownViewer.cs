using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownViewer : MonoBehaviour
{
    [SerializeField] private GameObject listItemPrefab;
    [SerializeField] private GameObject contentPanel;
   


    public void ChangeViewMode(Text newMode)
    {
        ClearList();
        switch ( newMode.text )
        {
            case "Buildings":
                ShowBuildings();
                break;
            case "People":
                ShowPeople();
                break;
            case "<none>":
                gameObject.SetActive(false);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }
   
    private void ShowBuildings()
    {
        gameObject.SetActive(true);
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
            controller.text = child.name;
            controller.SetObject(buildings.transform.GetChild(i).gameObject, "Building");
            newItem.transform.SetParent( contentPanel.transform, false);
            newItem.transform.localScale = Vector3.one;

        }
       
    }

    private void ShowPeople()
    {
        gameObject.SetActive(true);
        GameObject newItem = null;

        GameObject people = GameObject.Find("People");
        print(people.transform.childCount);
        for (int i = 0; i < people.transform.childCount; i++)
        {
            PersonController child = people.transform.GetChild(i).GetComponent<PersonController>();
            if (child == null)
                print("oops");
            newItem = Instantiate(listItemPrefab);
            ListItemBehavior controller = newItem.GetComponent<ListItemBehavior>();
            controller.icon = child.icon;
            controller.text = child.PersonTask() + "\n" + child.PersonState();
            controller.SetObject(people.transform.GetChild(i).gameObject, "Person");
            newItem.transform.SetParent(contentPanel.transform, false);
            newItem.transform.localScale = Vector3.one;

        }

    }

    private void ClearList()
    {
        if (contentPanel.transform.childCount == 0)
            return;
        for (int i = contentPanel.transform.childCount; i > 0; i-- )
        {
            Destroy(contentPanel.transform.GetChild(i - 1).gameObject);
        }
    }
   

    private void NewListItem (Texture pic, string name)
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownViewer : MonoBehaviour
{
    [SerializeField] private GameObject listItem;
    [SerializeField] private Texture houseTexture;


    // Use this for initialization
    void Start () {
		
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
        GameObject buildings = GameObject.Find("Buildings");
        for (int i = 0 ; i < buildings.transform.childCount; i++)
        {
            NewListItem(houseTexture, buildings.transform.GetChild(i).name);
            /// need to build the list in an array, then use that list to build the visible data, ensure being parented
            /// to the right object
        }
    }

    private void NewListItem (Texture pic, string name)
    {
        GameObject newItem = Instantiate(listItem, gameObject.transform, false);
        newItem.GetComponent<Text>().text = name;
        newItem.GetComponent<RawImage>().texture = pic;
    }
}

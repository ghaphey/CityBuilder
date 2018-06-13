using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItemBehavior : MonoBehaviour {

    [SerializeField] public Texture icon;
    [SerializeField] public string text;

    private GameObject listObject;
    private string itemType;

    private void Start()
    {
        gameObject.GetComponentInChildren<RawImage>().texture = icon;
        gameObject.GetComponentInChildren<Text>().text = text;

    }

    private void Update()
    {
        switch (itemType)
        {
            case "Building":
                //UpdateBuildingItem();
                break;
            case "Person":
                UpdatePersonItem();
                break;
            default:
                break;
        }
    }

    private void UpdatePersonItem()
    {
        PersonController child = listObject.GetComponent<PersonController>();
        gameObject.GetComponentInChildren<Text>().text = child.PersonTask() + "\n" + child.PersonState();
    }

    public void SetObject(GameObject newObject, string type)
    {
        listObject = newObject;
        itemType = type;
    }
}

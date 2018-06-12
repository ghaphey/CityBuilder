using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItemBehavior : MonoBehaviour {

    [SerializeField] public Texture icon;
    [SerializeField] public string iconName;

    private void Start()
    {
        gameObject.GetComponentInChildren<RawImage>().texture = icon;
        gameObject.GetComponentInChildren<Text>().text = iconName;
    }
}

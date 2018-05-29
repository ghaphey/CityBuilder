using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildElementUI : MonoBehaviour {

    [SerializeField] GameObject house;

    
    public void HousePressed ()
    {
        Instantiate(house, new Vector3(0.0f, 0.48f, 0.0f), Quaternion.identity);
        // Now must figure out how to snap house to mouse, and don't give permanent position until
        // mouse 1 clicked

        // still want to animate button
    }
}

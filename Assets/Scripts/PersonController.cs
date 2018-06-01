using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonController : MonoBehaviour {

    [SerializeField] Transform goal;

    private NavMeshAgent agent;
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = 
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FindStockpile()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Stockpile");
        float distance = list[0].GetComponent<Transform>().;
        float listIndex;
        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 1; i < list.Length; i++)
        {

        }
    }
}

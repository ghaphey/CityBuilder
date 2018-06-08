using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonController : MonoBehaviour
{
    [SerializeField] private float workDistance = 1.0f;
    [SerializeField] private int maxInventorySpace = 20;
    [SerializeField] private int pickupAmount = 5;


    private enum personState { Idle, MovingItem, Building, Moving, Working };
    private enum personTask { Unassigned, Worker, Builder};

    private personState currState = personState.Idle;
    private personTask task = personTask.Unassigned;

    private GameObject goal = null;
    private NavMeshAgent agent;

    private float timeWorked = 0;
    private Inventory myInventory;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 1.0f;

        task = personTask.Worker;
        myInventory = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        switch (task)
        {
            case personTask.Worker:
                workerTask();
                break;
            case personTask.Builder:
                break;
            case personTask.Unassigned:
                break;
        }
//        goal = FindClosestLocation("Gather");
 //       if (goal != null)
 //       {
 //           currState = personState.Moving;
 //           agent.destination = goal.transform.position;
  //      }
    }

    private void workerTask()
    {
        switch(currState)
        {
            case personState.Idle:
                WorkerIdleState();
                break;
            case personState.Moving:
                WorkerMovingState();
                break;
            case personState.Working:
                WorkerWorkingState();
                break;
            case personState.MovingItem:
                WorkerMovingItemState();
                break;
            case personState.Building:
                // not implemented
                break;
        }
    }

    private void WorkerMovingItemState()
    {
        if (goal == null)
        {
            goal = FindClosestStockpile(myInventory.itemType.name);
            if (goal != null)
            {
                agent.destination = goal.transform.position;
                agent.isStopped = false;
            }
            else
                print("no stockpiles");
        }
        else if (agent.remainingDistance <= workDistance)
        {
            agent.isStopped = true;
            if (myInventory.num <= 0)
            {
                goal = null;
                currState = personState.Idle;
            }
            else
                myInventory.num = goal.GetComponent<StockpileBehavior>().DepositItem(myInventory.itemType.name, myInventory.num);

            if (myInventory.num > 0)
                goal = null;
        }
        
    }

    private void WorkerMovingState()
    {
        if (goal == null)
        {
            agent.isStopped = true;
            currState = personState.Idle;
        }
        else if (agent.remainingDistance <= workDistance)
        {
            currState = personState.Working;
        }
        else if (agent.isStopped == true)
            agent.isStopped = false;
    }

    private void WorkerIdleState()
    {
        goal = FindClosestLocation("Gather");
        if (goal != null)
        {
            currState = personState.Moving;
            agent.destination = goal.transform.position;
            agent.isStopped = false;
        }
        else
            print("Nothing to Gather!");
    }

    private void WorkerWorkingState()
    {
        ResourceController resource = goal.GetComponent<ResourceController>();
        if (resource == null)
            print("can't gather not a resource");
        else
        {
            if (goal == null && myInventory.num < maxInventorySpace)
            {
                goal = FindClosestLocation("Gather", myInventory.itemType.name);
                currState = personState.Moving;
                if (goal == null)
                    currState = personState.MovingItem;
                else
                {
                    agent.destination = goal.transform.position;
                    goal = null;
                }
            }
            else if (myInventory.num == maxInventorySpace)
            {
                goal = null;
                currState = personState.MovingItem;
            }
            else if (myInventory.num > maxInventorySpace)
                print("should not have more than max");
            else
            {
                timeWorked += (resource.GetWorkTime() * Time.deltaTime);
                if (timeWorked >= resource.GetWorkTime())
                {
                    if (myInventory.itemType == null)
                        myInventory.itemType = resource.GetResourceType();
                    myInventory.num += resource.TakeResource(pickupAmount);
                    timeWorked = 0.0f;
                }
            }
        }
        
    }

    /// FindClosestLocation
    ///
    /// Finds the closest location via the "FindGameObjectsWithTag" function
    /// location string must match a tag within unity for function to work
    GameObject FindClosestLocation(string location)
    {
        float distance = Mathf.Infinity;
        float curDistance = 0.0f;
        GameObject closest = null;
        GameObject[] list = GameObject.FindGameObjectsWithTag(location);
        if (list.Length <= 0)
        {
            print("No " + location);
            return null;
        }


        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 0; i < list.Length; i++)
        {
            curDistance = Vector3.Distance(transform.position, list[i].transform.position);
            if (curDistance < distance)
            {
                distance = curDistance;
                closest = list[i];
            }
        }
        return closest;
    }

    /// FindClosestLocation (based on resource)
    ///
    /// Finds the closest location via the "FindGameObjectsWithTag" function
    /// location string must match a tag within unity for function to work
    /// and must also match the desired resource type
    GameObject FindClosestLocation(string location, string resourceType)
    {
        float distance = Mathf.Infinity;
        float curDistance = 0.0f;
        GameObject closest = null;
        GameObject[] list = GameObject.FindGameObjectsWithTag(location);
        ResourceController resource = null;
        if (list.Length <= 0)
        {
            print("No " + location);
            return null;
        }


        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 0; i < list.Length; i++)
        {
            resource = list[i].GetComponent<ResourceController>();
            curDistance = Vector3.Distance(transform.position, list[i].transform.position);
            if (curDistance < distance && resource.GetResourceType().name == resourceType)
            {
                // MUST DO SOMETHING HERE TO MAKE IT HAPPEN
                distance = curDistance;
                closest = list[i];
            }
        }
        return closest;
    }

    GameObject FindClosestStockpile(string itemType)
    {
        float distance = Mathf.Infinity;
        float curDistance = 0.0f;
        GameObject closest = null;
        GameObject[] list = GameObject.FindGameObjectsWithTag("Stockpile");
        StockpileBehavior stock = null;
        if (list.Length <= 0)
        {
            return null;
        }


        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 0; i < list.Length; i++)
        {
            stock = list[i].GetComponent<StockpileBehavior>();
            curDistance = Vector3.Distance(transform.position, list[i].transform.position);
            if (curDistance < distance && stock.SpaceAvailable(itemType))
            {
                // MUST DO SOMETHING HERE TO MAKE IT HAPPEN
                distance = curDistance;
                closest = list[i];
            }
        }
        return closest;
    }

    private class Inventory
    {
        public int num;
        public GameObject itemType;

        public Inventory ()
        {
            num = 0;
            itemType = null;
        }
    }
}

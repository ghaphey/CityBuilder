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
    [SerializeField] private personTask assignedTask;
    [SerializeField] public Texture icon;


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

        task = assignedTask;
        myInventory = new Inventory { num = 0, itemType = null } ;
    }
    

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponentInChildren<TextMesh>().text = myInventory.num.ToString();
        switch (task)
        {
            case personTask.Worker:
                WorkerTask();
                break;
            case personTask.Builder:
                BuilderTask();
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


    public string PersonState ()
    {
        return currState.ToString();
    }

    public string PersonTask ()
    {
        return task.ToString();
    }

    private void WorkerTask()
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
            goal = FindClosestStockpile(myInventory.itemType);
            if (goal != null)
            {
                agent.destination = goal.transform.position;
                agent.isStopped = false;
            }
            else
            {
                print("no stockpiles");
                goal = null;
                currState = personState.Idle;
            }
        }
        else if (agent.remainingDistance <= workDistance)
        {
            agent.isStopped = true;
            if (myInventory.num <= 0)
            {
                goal = null;
                myInventory.itemType = null;
                currState = personState.Idle;
            }
            else
                myInventory.num = goal.GetComponent<StockpileBehavior>().DepositItem(myInventory.num, myInventory.itemType);

            if (myInventory.num > 0)
                goal = null;
            else
                myInventory.itemType = null;
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
        goal = FindClosestResource("Gather", null);
        if (goal != null)
        {
            currState = personState.Moving;
            agent.destination = goal.transform.position;
            agent.isStopped = false;
        }
    }

    private void WorkerWorkingState()
    {
        ResourceController resource = null;
        if (goal != null)
            resource = goal.GetComponent<ResourceController>();

        if (resource == null)
        {
            print("can't gather not a resource");
            goal = FindClosestResource("Gather", myInventory.itemType);
            currState = personState.Moving;
            if (goal == null)
                currState = personState.MovingItem;
            else
            {
                agent.destination = goal.transform.position;
            }
        }
        else
        {

            if (myInventory.num == maxInventorySpace)
            {
                goal = null;
                resource.SetOwner(null);
                currState = personState.MovingItem;
            }
            else if (resource.CurrentOwner() == gameObject.name)
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
            else if (resource.CurrentOwner() == null)
                resource.SetOwner(gameObject.name);
            else if ((goal == null || resource.CurrentOwner() != gameObject.name) && myInventory.num < maxInventorySpace)
            {
                goal = FindClosestResource("Gather", myInventory.itemType);
                currState = personState.Moving;
                if (goal == null)
                    currState = personState.MovingItem;
                else
                {
                    agent.destination = goal.transform.position;
                }
            }
            else
            {
                currState = personState.Idle;
                goal = null;
                print("something dun messed up");
            }

        }
        
    }

    private void BuilderTask()
    {
        switch (currState)
        {
            case personState.Idle:
                BuilderIdleState();
                break;
            case personState.Moving:
                BuilderMovingState();
                break;
            case personState.Working:
               // not implemented
                break;
            case personState.MovingItem:
                BuilderMovingItemState();
                break;
            case personState.Building:
                BuilderBuildingState();
                break;
        }
        
    }

    private void BuilderBuildingState()
    {
        ObjectController ObjCont = null;
        if (goal != null)
        {
            ObjCont = goal.GetComponent<ObjectController>();
        }
        if (myInventory.num <= 0 && ObjCont.NeedStone())
        {
            currState = personState.MovingItem;
            goal = null;
            // need to figure out how to request and retreive various stuff
        }
    }

    private void BuilderIdleState()
    {
        goal = FindUnfinishedBuilding();
        if (goal != null)
        {
            currState = personState.Moving;
            agent.destination = goal.transform.position;
            agent.isStopped = false;
        }
    }

    private void BuilderMovingState()
    {
        if (goal == null)
        {
            agent.isStopped = true;
            currState = personState.Idle;
        }
        else if (agent.remainingDistance <= workDistance)
        {
            currState = personState.Building;
        }
        else if (agent.isStopped == true)
            agent.isStopped = false;
    }

    private void BuilderMovingItemState()
    {
        if (goal == null)
        {
            goal = FindClosestLocation("Stockpile");
            if (goal != null)
            {
                agent.destination = goal.transform.position;
                agent.isStopped = false;
            }
            else
            {
                print("no stockpiles");
                goal = null;
                currState = personState.Idle;
            }
        }
        else if (agent.remainingDistance <= workDistance)
        {
            agent.isStopped = true;
            if (myInventory.num <= 0)
            {
                goal = null;
                myInventory.itemType = null;
                currState = personState.Idle;
            }
            else
                myInventory.num = goal.GetComponent<StockpileBehavior>().TakeItem(myInventory.num, myInventory.itemType.name);
            // this ain't gonna work, need to link to which resource is ID'd by building state
            if (myInventory.num > 0)
            {
                currState = personState.Idle;
            }
            else
                goal = null;
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
           // print("No " + location);
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
    GameObject FindClosestResource(string location, GameObject resourceType)
    {
        float distance = Mathf.Infinity;
        float curDistance = 0.0f;
        GameObject closest = null;
        GameObject[] list = GameObject.FindGameObjectsWithTag(location);
        ResourceController resource = null;
        if (list.Length <= 0)
        {
            //print("No " + location);
            return null;
        }


        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 0; i < list.Length; i++)
        {
            resource = list[i].GetComponent<ResourceController>();
            curDistance = Vector3.Distance(transform.position, list[i].transform.position);
            if (curDistance < distance && (resourceType == null || resource.GetResourceType().name == resourceType.name ) && resource.CurrentOwner() == null )
            {
                // MUST DO SOMETHING HERE TO MAKE IT HAPPEN
                distance = curDistance;
                closest = list[i];
            }
        }
        return closest;
    }

    GameObject FindClosestStockpile(GameObject itemType)
    {
        float distance = Mathf.Infinity;
        float curDistance = 0.0f;
        GameObject closest = null;
        GameObject[] list = GameObject.FindGameObjectsWithTag("Stockpile");
        StockpileBehavior stock = null;
        if (list.Length <= 0 || itemType == null)
        {
            return null;
        }


        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 0; i < list.Length; i++)
        {
            stock = list[i].GetComponent<StockpileBehavior>();
            curDistance = Vector3.Distance(transform.position, list[i].transform.position);
            if (curDistance < distance && stock.DepositSpace(itemType.name))
            {
                // MUST DO SOMETHING HERE TO MAKE IT HAPPEN
                distance = curDistance;
                closest = list[i];
            }
        }
        return closest;
    }
    GameObject FindUnfinishedBuilding()
    {
        float distance = Mathf.Infinity;
        float curDistance = 0.0f;
        GameObject closest = null;
        GameObject buildings = GameObject.Find("Buildings");
        ObjectController bCont = null;
        if (buildings.transform.childCount <= 0)
        {
            //print("No " + location);
            return null;
        }


        // NEED TO COMPARE DISTANCE "Vector3.Distance" and determine closest stockpile
        for (int i = 0; i < buildings.transform.childCount; i++)
        {
            bCont = buildings.transform.GetChild(i).GetComponent<ObjectController>();
            curDistance = Vector3.Distance(transform.position, buildings.transform.GetChild(i).position);
            if (curDistance < distance && (bCont.NeedStone() || bCont.NeedWood()))
            {
                // MUST DO SOMETHING HERE TO MAKE IT HAPPEN
                distance = curDistance;
                closest = buildings.transform.GetChild(i).gameObject;
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

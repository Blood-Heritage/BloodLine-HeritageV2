using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Unity.VisualScripting; // Pour simplifier la récupération des objets

public class AIMoveBetweenPoints : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject DesstinationsParent;
    private Transform[] destinationPoints;
    private int currentDestinationIndex = 0;
    public float arrivalThreshold = 0.1f; 

void Start()
{
    agent = GetComponent<NavMeshAgent>();
    
    if (agent == null)
    {
        return;
    }
    
    destinationPoints = DesstinationsParent.GetComponentsInChildren<Transform>()
        .Where(t => t.name.StartsWith("DestinationPoint_"))
        .ToArray();

    if (destinationPoints.Length > 0)
    {
        MoveToNextDestination();
    }
    else
    {
        Debug.LogWarning("çamarchepas");
    }
}


    void Update()
    {
        if (agent.pathPending) return;

    
        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance + arrivalThreshold)
        {
            MoveToNextDestination();
        }
    }

    void MoveToNextDestination()
    {
        if (destinationPoints.Length == 0) return;
        int new_value = Random.Range(0,destinationPoints.Length);
        currentDestinationIndex = destinationPoints.Length;
        
        agent.SetDestination(destinationPoints[new_value].position);
        Debug.Log("Nouvelle destination : " + destinationPoints[new_value].name);
    }
}

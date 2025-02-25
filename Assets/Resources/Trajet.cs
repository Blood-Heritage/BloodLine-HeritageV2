
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class MoveDestination : MonoBehaviourPun {

    public NavMeshAgent agent;
    public Transform goal;

       
    void Start () {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position; 
    }
}
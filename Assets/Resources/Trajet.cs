using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class MoveDestination : MonoBehaviourPun {
    
    public NavMeshAgent agent;
    public Transform goal;

    void Start() {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (goal != null) {
            agent.SetDestination(goal.position);
        } else {
            Debug.LogError("Aucun goal assigné au NPC !");
        }
    }

    void Update() {
        if (goal != null) {
            agent.SetDestination(goal.position);
        }
    }
}

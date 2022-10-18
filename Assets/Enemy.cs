using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        try {
            agent.SetDestination(GameObject.Find("Player").transform.position);
        } catch {
            // No Mesh yet
        }
    }
}

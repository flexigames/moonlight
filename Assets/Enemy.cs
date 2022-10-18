using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private float lastDetectedTime;

    public float coolDown = 5.0f;

    bool hasRandomDestination;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        try {
            if (lastDetectedTime + coolDown < Time.time) {
                WalkToPlayer();
            } else {
                WalkRandomly();
            }
        } catch {
            // No Mesh yet
        }
    }

    void WalkToPlayer() {
        agent.SetDestination(GameObject.Find("Player").transform.position);
        hasRandomDestination = false;
    }

    void WalkRandomly() {
        if (hasRandomDestination && agent.hasPath && agent.remainingDistance > 0.5f) return;

        Vector3 randomDirection = Random.insideUnitSphere * 5;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 5, 1);

        Vector3 finalPosition = hit.position;

        agent.SetDestination(finalPosition);
        hasRandomDestination = true;
    }

    public void OnDetect() {
        lastDetectedTime = Time.time;
    }

    public void OnDetectionLost() {
    }
}

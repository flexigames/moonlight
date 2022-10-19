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
        agent.speed = Game.isDark ? 1.0f : 3.0f;
        try {
            if (IsFollowingPlayer()) {
                WalkToPlayer();
            } else {
                // WalkRandomly();
                agent.isStopped = true;
            }
        } catch {
            // No Mesh yet
        }
    }

    void WalkToPlayer() {
        agent.SetDestination(GameObject.Find("Player").transform.position);
        agent.isStopped = false;
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

    bool IsFollowingPlayer() {
        return true;
        // return !Game.isDark;
        // return lastDetectedTime + coolDown < Time.time;
    }

    public void OnDetect() {
        lastDetectedTime = Time.time;
    }

    public void OnDetectionLost() {
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Destroy(other.gameObject);
            Game.GameOver();
        }
    }
}

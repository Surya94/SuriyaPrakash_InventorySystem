using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAIFollow : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Goalgoing : Agent
{
    public Transform Goal;

    public NavMeshAgent nmAgent;
    public ChaserManager chaserManager;
    Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        startPos = this.transform.localPosition;
    }

    
    private void OnCollisionEnter(Collision other)
    {
        //도망자가 도착점에 도착하면 벌점
        if (other.gameObject.CompareTag("goal"))
        {
            nmAgent.enabled = false;
            chaserManager.EndEpisode(0);
            this.transform.localPosition = startPos;
            nmAgent.enabled = true;
        }

        // 추격자에 잡히면 상점
        else if (other.gameObject.CompareTag("agent"))
        {
            nmAgent.enabled = false;
            chaserManager.EndEpisode(1);
            this.transform.localPosition = startPos;
            nmAgent.enabled = true;
        }

        if (other.collider.tag == "goal" && other.gameObject.GetComponent<BoxCollider>() != null )
        {
            
            SetReward(-3.0f);
        }

    }
    
    private void Update()
    {
        nmAgent.SetDestination(Goal.localPosition);
    }
}

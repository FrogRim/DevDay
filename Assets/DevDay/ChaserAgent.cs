using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
//mlAgent 사용시 포함해야 됨

public class ChaserAgent : Agent
{
    
    Rigidbody rBody;
    public GameObject Target;
    private float stillThreshold = 1f; // 움직임 감지 임계값
    private Vector3 initialPosition;
    public float catchDistance = 1f; // 술래가 도망자를 잡을 수 있는 거리
    public float turnSpeed = 180f; // 에이전트의 회전 속도

    public GameObject viewModel = null;
    
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
    }

    public TraningAreaManager trainingAreaManager = null;

    public override void OnEpisodeBegin()
    {
        //새로운 애피소드 시작시, 다시 에이전트의 포지션의 초기화
        this.transform.localPosition = initialPosition;
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0) //만약 에이전트가 floor 아래로 떨어진 경우 추가 초기화
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

       
    }

    /// <summary>
    /// 강화학습을 위한, 강화학습을 통한 행동이 결정되는 곳
    /// </summary>
    public float forceMultiplier = 10;

    public override void CollectObservations(VectorSensor sensor)
    {
        // 에이전트가 목표물의 상대적인 위치와 방향을 관측합니다.
        Vector3 relativePosition = Target.transform.localPosition - transform.localPosition;
        sensor.AddObservation(relativePosition.normalized);
        sensor.AddObservation(Vector3.Dot(transform.forward, relativePosition.normalized));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        viewModel.transform.LookAt(Target.transform); 
        
        MoveAgent(actionBuffers);

        // 에이전트가 목표물과의 거리에 따라 보상이나 벌점을 받습니다.
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        if (distance < catchDistance)
        {
            // 에이전트가 목표물을 잡으면 최대 보상을 받고 에피소드를 종료합니다.
            SetReward(1.0f);
            EndEpisode();
        }
        else
        {
            // 에이전트가 목표물과 가까워 질수록 상점을 받습니다
            SetReward(0.01f * (1 / distance));
        }
        


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Obsstacles" && collision.gameObject.GetComponent<BoxCollider>() != null)
        {
            // "Obsstacles" 태그를 가지고 있고, BoxCollider 컴포넌트를 가진 GameObject에 충돌한 경우           
            SetReward(-0.05f);
        }



    }


    public void MoveAgent(ActionBuffers actionBuffers)
    {
        // 에이전트는 앞뒤좌우로 움직이거나 회전하는 행동을 합니다.
        Vector3 forwardAmount = transform.forward * actionBuffers.ContinuousActions[0];
        Vector3 rightAmount = transform.right * actionBuffers.ContinuousActions[1];
        //float turnAmount = actionBuffers.ContinuousActions[2];

        rBody.AddForce(forwardAmount + rightAmount, ForceMode.VelocityChange);
        transform.Rotate(transform.up  * turnSpeed * Time.fixedDeltaTime);

        AddReward(-0.01f / MaxStep);
    }

   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


public class PathFinder : Agent
{
    //public int section = 0; // 3가지 상황
    Rigidbody rBody;
    public List<Collider> Obstacles; // 여러 개의 장애물을 관리하기 위한 리스트

    private Vector3 lastPosition; // 이전 프레임에서의 위치
    private float movementThreshold = 2.0f; // 움직임을 감지할 최소 이동 거리
    private float timeThreshold = 1.0f; // 움직임이 없는 것으로 판단할 최대 시간

    private float idleTimer = 0.0f; // 움직임이 없는 시간을 측정할 타이머
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        lastPosition = this.transform.localPosition;
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        
        if (this.transform.localPosition.y < 0) //에이전트가 floor 아래로 떨어진 경우 추가 초기화
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = lastPosition;
        }
        this.transform.localPosition = lastPosition;
        /*
        switch(section)
        {
            case 1:
                this.transform.localPosition = new Vector3(22.0f, 1.0f, 22.0f);
                break;
            case 2:
                this.transform.localPosition = new Vector3(0.0f, 1.0f, 22.0f);
                break;
            case 3:
                this.transform.localPosition = new Vector3(22.0f, 1.0f, 22.0f);
                break;
        }
        */

    }

 
    public override void CollectObservations(VectorSensor sensor)
    {
        
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

       
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    
    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        float minDistanceToObstacle = float.MaxValue;

        //장애물들과의 거리를 측정
        foreach (Collider obstacle in Obstacles)
        {
            float distance = Vector3.Distance(this.transform.localPosition, obstacle.transform.localPosition);
            minDistanceToObstacle = Mathf.Min(minDistanceToObstacle, distance);
        }


        if (minDistanceToObstacle < 1.0f) // 장애물에 부딪힐 때
        {
            SetReward(-0.01f); // 마이너스 보상 부여
            EndEpisode();
        }

        if (distanceToTarget < 1.0f)
        {
            SetReward(0.6f);
            EndEpisode();
        }

        //판 아래로 떨어지면 학습이 종료된다.
        // Fell off platform
        else if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }

    }

    private void Update()
    {
        // 현재 위치와 이전 프레임의 위치 사이의 거리를 계산
        float distance = Vector3.Distance(transform.position, lastPosition);

        if (distance <= movementThreshold)
        {
            // 거리가 이동 임계값 이하라면 움직임이 없는 것으로 간주
            idleTimer += Time.deltaTime;

            if (idleTimer >= timeThreshold)
            {
                SetReward(-0.03f); // 마이너스 보상 부여
            }
        }
        else
        {
            // 움직임이 감지되면 타이머 초기화
            idleTimer = 0.0f;
        }

        // 현재 위치를 이전 위치로 업데이트
        lastPosition = transform.position;
    }

}
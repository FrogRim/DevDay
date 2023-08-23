using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GRollerAgent : Agent
{
    Rigidbody rBody;

    [SerializeField]
    int section;
    public bool catchTarget = false;
    private Vector3 lastPosition; // 이전 프레임에서의 위치
    
    public GameObject GoalPoint;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        lastPosition = this.transform.localPosition;

    }

    public Transform Target;
    public List<Collider> Obstacles; // 여러 개의 장애물을 관리하기 위한 리스트

    // Episode : 하나의 스테이지를 뜻함 1번의 회차 = 1번의 에피소드
    public override void OnEpisodeBegin()
    {
        
        // 새로운 에피소드 시작시, 다시 에이전트의 포지션을 초기화

        if (this.transform.localPosition.y < -0.25) // 바닥 밑으로 떨어졌을 경우
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = lastPosition;

        }


        this.transform.localPosition = lastPosition;


    }


    //강화학습 프로그램에게 관측된 정보를 전달하기
    public override void CollectObservations(VectorSensor sensor)
    {
        //타겟과 에이전트의 포지션을 전달한다

        //if (catchTarget)
        //{
        //    sensor.AddObservation(Target.localPosition); // 타겟
        //}

        sensor.AddObservation(Target.localPosition); // 타겟
        sensor.AddObservation(this.transform.localPosition); // 에이전트

        //현재 에이전트의 이동량을 전달한다.
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

    }

    //강화학습을 위한, 강화학습을 통한 행동이 결정되는 함수
    public float forceMultiplier = 10.0f;

    public GameObject viewModel = null;
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        viewModel.transform.LookAt(Target);
        //보상
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        float distanceToGoal = Vector3.Distance(Target.transform.localPosition, GoalPoint.transform.localPosition);

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


        //에이전트 중 하나가 타겟에 도달할 시 보상점수를 주고, 에피소드 종료
        else if (distanceToTarget < 1.0f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //타겟이 골인지점에 도착하면 마이너스 보상 점수를 주고, 에피소드 종료
        else if (distanceToGoal < 1.0f)
        {
            SetReward(-0.1f);
            EndEpisode();
        }

        //바닥 밑으로 떨어지면 학습종료
        else if (this.transform.localPosition.y < 0.8f)
        {
            EndEpisode();
        }
    }

    // 해당 함수는 코딩 혹은 직접적인 조작을 대비해서 만들어 놓은 예비 함수
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var ContinuousActionsOut = actionsOut.ContinuousActions;

        ContinuousActionsOut[0] = Input.GetAxis("Horizontal");
        ContinuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    
}

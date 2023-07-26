using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class GRollerAgent : Agent
{
    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;

    // Episode : 하나의 스테이지를 뜻함 1번의 회차 = 1번의 에피소드
    public override void OnEpisodeBegin()
    {
        // 새로운 에피소드 시작시, 다시 에이전트의 포지션을 초기화

        if (this.transform.localPosition.y < -0.25) // 바닥 밑으로 떨어졌을 경우
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.05f, 0);

        }

        //타겟의 위치는 에피소드 시작시 랜덤하게 변경시키기
        Target.localPosition = new Vector3(Random.value * 10 - 4, 0.5f, Random.value * 10 - 4);
        
    }


    //강화학습 프로그램에게 관측된 정보를 전달하기
    public override void CollectObservations(VectorSensor sensor)
    {
        //타겟과 에이전트의 포지션을 전달한다
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

        //타겟을 팢을 시 보상점수를 주고, 에피소드 종료
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //바닥 밑으로 떨어지면 학습종료
        else if (this.transform.localPosition.y < -0.25)
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

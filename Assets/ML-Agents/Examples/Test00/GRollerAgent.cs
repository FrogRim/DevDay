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

    // Episode : �ϳ��� ���������� ���� 1���� ȸ�� = 1���� ���Ǽҵ�
    public override void OnEpisodeBegin()
    {
        // ���ο� ���Ǽҵ� ���۽�, �ٽ� ������Ʈ�� �������� �ʱ�ȭ

        if (this.transform.localPosition.y < -0.25) // �ٴ� ������ �������� ���
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.05f, 0);

        }

        //Ÿ���� ��ġ�� ���Ǽҵ� ���۽� �����ϰ� �����Ű��
        Target.localPosition = new Vector3(Random.value * 10 - 4, 0.5f, Random.value * 10 - 4);
        
    }


    //��ȭ�н� ���α׷����� ������ ������ �����ϱ�
    public override void CollectObservations(VectorSensor sensor)
    {
        //Ÿ�ٰ� ������Ʈ�� �������� �����Ѵ�
        sensor.AddObservation(Target.localPosition); // Ÿ��
        sensor.AddObservation(this.transform.localPosition); // ������Ʈ

        //���� ������Ʈ�� �̵����� �����Ѵ�.
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

    }

    //��ȭ�н��� ����, ��ȭ�н��� ���� �ൿ�� �����Ǵ� �Լ�

    public float forceMultiplier = 10.0f;

    public GameObject viewModel = null;
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        viewModel.transform.LookAt(Target);
        //����
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        //Ÿ���� �Y�� �� ���������� �ְ�, ���Ǽҵ� ����
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //�ٴ� ������ �������� �н�����
        else if (this.transform.localPosition.y < -0.25)
        {
            EndEpisode();
        }
    }

    // �ش� �Լ��� �ڵ� Ȥ�� �������� ������ ����ؼ� ����� ���� ���� �Լ�

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var ContinuousActionsOut = actionsOut.ContinuousActions;

        ContinuousActionsOut[0] = Input.GetAxis("Horizontal");
        ContinuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    
}

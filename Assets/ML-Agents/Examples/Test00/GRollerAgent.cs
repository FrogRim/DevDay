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
    private Vector3 lastPosition; // ���� �����ӿ����� ��ġ
    
    public GameObject GoalPoint;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        lastPosition = this.transform.localPosition;

    }

    public Transform Target;
    public List<Collider> Obstacles; // ���� ���� ��ֹ��� �����ϱ� ���� ����Ʈ

    // Episode : �ϳ��� ���������� ���� 1���� ȸ�� = 1���� ���Ǽҵ�
    public override void OnEpisodeBegin()
    {
        
        // ���ο� ���Ǽҵ� ���۽�, �ٽ� ������Ʈ�� �������� �ʱ�ȭ

        if (this.transform.localPosition.y < -0.25) // �ٴ� ������ �������� ���
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = lastPosition;

        }


        this.transform.localPosition = lastPosition;


    }


    //��ȭ�н� ���α׷����� ������ ������ �����ϱ�
    public override void CollectObservations(VectorSensor sensor)
    {
        //Ÿ�ٰ� ������Ʈ�� �������� �����Ѵ�

        //if (catchTarget)
        //{
        //    sensor.AddObservation(Target.localPosition); // Ÿ��
        //}

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

        float distanceToGoal = Vector3.Distance(Target.transform.localPosition, GoalPoint.transform.localPosition);

        float minDistanceToObstacle = float.MaxValue;

        //��ֹ������ �Ÿ��� ����
        foreach (Collider obstacle in Obstacles)
        {
            float distance = Vector3.Distance(this.transform.localPosition, obstacle.transform.localPosition);
            minDistanceToObstacle = Mathf.Min(minDistanceToObstacle, distance);
        }


        if (minDistanceToObstacle < 1.0f) // ��ֹ��� �ε��� ��
        {
            SetReward(-0.01f); // ���̳ʽ� ���� �ο�
            EndEpisode();
        }


        //������Ʈ �� �ϳ��� Ÿ�ٿ� ������ �� ���������� �ְ�, ���Ǽҵ� ����
        else if (distanceToTarget < 1.0f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        //Ÿ���� ���������� �����ϸ� ���̳ʽ� ���� ������ �ְ�, ���Ǽҵ� ����
        else if (distanceToGoal < 1.0f)
        {
            SetReward(-0.1f);
            EndEpisode();
        }

        //�ٴ� ������ �������� �н�����
        else if (this.transform.localPosition.y < 0.8f)
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

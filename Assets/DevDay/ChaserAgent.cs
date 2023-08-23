using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
//mlAgent ���� �����ؾ� ��

public class ChaserAgent : Agent
{
    
    Rigidbody rBody;
    public GameObject Target;
    private float stillThreshold = 1f; // ������ ���� �Ӱ谪
    private Vector3 initialPosition;
    public float catchDistance = 1f; // ������ �����ڸ� ���� �� �ִ� �Ÿ�
    public float turnSpeed = 180f; // ������Ʈ�� ȸ�� �ӵ�

    public GameObject viewModel = null;
    
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
    }

    public TraningAreaManager trainingAreaManager = null;

    public override void OnEpisodeBegin()
    {
        //���ο� ���Ǽҵ� ���۽�, �ٽ� ������Ʈ�� �������� �ʱ�ȭ
        this.transform.localPosition = initialPosition;
        // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0) //���� ������Ʈ�� floor �Ʒ��� ������ ��� �߰� �ʱ�ȭ
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

       
    }

    /// <summary>
    /// ��ȭ�н��� ����, ��ȭ�н��� ���� �ൿ�� �����Ǵ� ��
    /// </summary>
    public float forceMultiplier = 10;

    public override void CollectObservations(VectorSensor sensor)
    {
        // ������Ʈ�� ��ǥ���� ������� ��ġ�� ������ �����մϴ�.
        Vector3 relativePosition = Target.transform.localPosition - transform.localPosition;
        sensor.AddObservation(relativePosition.normalized);
        sensor.AddObservation(Vector3.Dot(transform.forward, relativePosition.normalized));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        viewModel.transform.LookAt(Target.transform); 
        
        MoveAgent(actionBuffers);

        // ������Ʈ�� ��ǥ������ �Ÿ��� ���� �����̳� ������ �޽��ϴ�.
        float distance = Vector3.Distance(transform.position, Target.transform.position);
        if (distance < catchDistance)
        {
            // ������Ʈ�� ��ǥ���� ������ �ִ� ������ �ް� ���Ǽҵ带 �����մϴ�.
            SetReward(1.0f);
            EndEpisode();
        }
        else
        {
            // ������Ʈ�� ��ǥ���� �־������� ������ �޽��ϴ�.
            SetReward(0.01f * (1 / distance));
        }
        


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Obsstacles" && collision.gameObject.GetComponent<BoxCollider>() != null)
        {
            // "Obsstacles" �±׸� ������ �ְ�, BoxCollider ������Ʈ�� ���� GameObject�� �浹�� ���           
            SetReward(-0.05f);
        }



    }


    public void MoveAgent(ActionBuffers actionBuffers)
    {
        // ������Ʈ�� �յ��¿�� �����̰ų� ȸ���ϴ� �ൿ�� �մϴ�.
        Vector3 forwardAmount = transform.forward * actionBuffers.ContinuousActions[0];
        Vector3 rightAmount = transform.right * actionBuffers.ContinuousActions[1];
        //float turnAmount = actionBuffers.ContinuousActions[2];

        rBody.AddForce(forwardAmount + rightAmount, ForceMode.VelocityChange);
        transform.Rotate(transform.up  * turnSpeed * Time.fixedDeltaTime);

        AddReward(-0.01f / MaxStep);
    }

   

}
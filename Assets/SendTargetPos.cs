using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.CommunicatorObjects;
using Unity.MLAgents.Sensors;


public class SendTargetPos : MonoBehaviour
{
    public List<GameObject> objectsAgent;
    public GameObject targetObject; // ������ ������ ��� ������Ʈ
    public float proximityRadius = 2.0f; // ���� �ݰ�


    void Start()
    {
        
    }

    void Update()
    {
        // ��� ������Ʈ���� �Ÿ� ���
        float distance = Vector3.Distance(transform.localPosition, targetObject.transform.localPosition);

        // ���� �ݰ� �ȿ� ������ �� �Լ� ȣ��
        if (distance <= proximityRadius)
        {
            SendTargetPosition();
        }
        
    }

    // ��� ������Ʈ�� ���� ���� �����ϴ� �޼���
    public void SendTargetPosition()
    {
        foreach (GameObject obj in objectsAgent)
        {
            bool script = obj.GetComponent<GRollerAgent>().catchTarget;
            if (script == false)
            {
                script = true;
            }
        }
    }
}

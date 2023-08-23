using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.CommunicatorObjects;
using Unity.MLAgents.Sensors;


public class SendTargetPos : MonoBehaviour
{
    public List<GameObject> objectsAgent;
    public GameObject targetObject; // 색상을 변경할 대상 오브젝트
    public float proximityRadius = 2.0f; // 일정 반경


    void Start()
    {
        
    }

    void Update()
    {
        // 대상 오브젝트와의 거리 계산
        float distance = Vector3.Distance(transform.localPosition, targetObject.transform.localPosition);

        // 일정 반경 안에 들어왔을 때 함수 호출
        if (distance <= proximityRadius)
        {
            SendTargetPosition();
        }
        
    }

    // 모든 오브젝트의 변수 값을 변경하는 메서드
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

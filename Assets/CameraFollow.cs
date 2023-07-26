using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상(캐릭터)의 Transform 컴포넌트
    public Vector3 offset; // 카메라와 대상 사이의 거리(offset)
    public float smoothSpeed = 0.125f; // 카메라 이동의 부드러움 정도

    void LateUpdate()
    {
        // 대상의 위치에 offset을 더한 목표 위치를 계산합니다.
        Vector3 desiredPosition = target.position + offset;

        // 부드러운 이동을 위해 현재 위치와 목표 위치 사이를 보간합니다.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라의 위치를 업데이트합니다.
        transform.position = smoothedPosition;
    }
}


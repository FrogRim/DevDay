using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ���� ���(ĳ����)�� Transform ������Ʈ
    public Vector3 offset; // ī�޶�� ��� ������ �Ÿ�(offset)
    public float smoothSpeed = 0.125f; // ī�޶� �̵��� �ε巯�� ����

    void LateUpdate()
    {
        // ����� ��ġ�� offset�� ���� ��ǥ ��ġ�� ����մϴ�.
        Vector3 desiredPosition = target.position + offset;

        // �ε巯�� �̵��� ���� ���� ��ġ�� ��ǥ ��ġ ���̸� �����մϴ�.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // ī�޶��� ��ġ�� ������Ʈ�մϴ�.
        transform.position = smoothedPosition;
    }
}


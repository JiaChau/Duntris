using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TopDownOrbitCam : MonoBehaviour
{
    [Header("필수")]
    [SerializeField] private Transform target;    // 중심 오브젝트
    [SerializeField] private float radius     = 6f;   // 거리
    [SerializeField] private float height     = 3f;   // 위쪽 오프셋(높이)

    [Header("회전")]
    [SerializeField] private float angularSpeed = 30f; // °/sec (시계방향)

    float currentAngle;   // 누적 각도

    void Start()
    {
        if (!target) { enabled = false; return; }

        // 초기 위치 계산
        UpdatePosition(0f);
    }

    void LateUpdate()
    {
        currentAngle += angularSpeed * Time.deltaTime;
        UpdatePosition(currentAngle);
    }

    void UpdatePosition(float angleDeg)
    {
        // 각도를 라디안으로 변환
        float rad = angleDeg * Mathf.Deg2Rad;

        // 원 궤도 위 위치 계산
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * radius;

        // 높이 적용
        Vector3 camPos = target.position + offset + Vector3.up * height;

        transform.position = camPos;
        transform.LookAt(target);  // 항상 타깃을 바라본다
    }
}

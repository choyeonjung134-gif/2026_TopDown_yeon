using UnityEngine;

public class Bee : MonoBehaviour
{
    [Header("속도 설정")]
    public float baseSpeed = 2f;      // 벌의 기본 속도 (추적할 때는 2~3 정도가 적당해요)
    private float currentSpeed;       // 실제 움직임에 사용할 현재 속도

    private Transform playerTransform; // 쫓아갈 병아리의 위치를 저장할 변수

    void Start()
    {
        currentSpeed = baseSpeed;

        // ⭐ 맵에서 "Player" 태그가 붙은 오브젝트를 찾아서 그 위치를 기억합니다.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            // 병아리가 있는 방향 벡터를 계산합니다.
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // X 기존 코드 (순간이동 방식이라 충돌을 씹음)
            // transform.position += direction * currentSpeed * Time.deltaTime;

            //  [새로운 코드] Rigidbody를 사용해 '물리적으로 밀면서' 직진시킵니다.
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.MovePosition(transform.position + direction * currentSpeed * Time.fixedDeltaTime);
            }
        }
    }

    // 라일락을 먹었을 때 호출될 슬로우 함수 (기존 내용 유지)
    public void SlowDown(float duration)
    {
        CancelInvoke("RestoreSpeed");
        currentSpeed = baseSpeed * 0.4f;
        Invoke("RestoreSpeed", duration);
    }

    void RestoreSpeed()
    {
        currentSpeed = baseSpeed;
    }
}
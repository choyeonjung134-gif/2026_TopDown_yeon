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

    void Update()
    {
        // 쫓아갈 병아리가 존재한다면 타겟을 향해 이동합니다.
        if (playerTransform != null)
        {
            // 병아리가 있는 방향 벡터를 계산합니다.
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // 그 방향으로 현재 속도만큼 매 프레임 이동합니다.
            transform.position += direction * currentSpeed * Time.deltaTime;

            // (선택) 벌이 쫓아가는 방향에 맞춰 좌우 이미지를 뒤집고 싶다면 아래 주석을 해제하세요!
            /*
            if (direction.x > 0) transform.localScale = new Vector3(-1f, 1f, 1f); // 오른쪽 볼 때
            else if (direction.x < 0) transform.localScale = new Vector3(1f, 1f, 1f); // 왼쪽 볼 때
            */
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
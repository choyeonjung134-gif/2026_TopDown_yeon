using UnityEngine;

public class Bee : MonoBehaviour
{
    [Header("속도 설정")]
    public float baseSpeed = 3f;      // 벌의 기본 속도
    private float currentSpeed;       // 실제 움직임에 사용할 현재 속도

    void Start()
    {
        // 시작할 때 현재 속도를 기본 속도로 맞춰둡니다.
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        //  [중요] 기존에 벌이 움직이던 코드에서 'baseSpeed' 대신 'currentSpeed'를 곱해 이동하게 고쳐주세요!
        // 예시: transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
    }

    //  플레이어가 라일락을 먹으면 호출될 함수
    public void SlowDown(float duration)
    {
        // 이미 속도가 느려진 코루틴이 돌고 있을 수 있으므로, 안전하게 리셋 후 실행
        CancelInvoke("RestoreSpeed");

        currentSpeed = baseSpeed * 0.4f; // 원래 속도의 40%로 대폭 감소! (숫자를 낮출수록 더 느려져요)
        Debug.Log(gameObject.name + "가 라일락 향기에 취해 느려졌습니다!");

        // duration(5초) 뒤에 RestoreSpeed 함수를 자동으로 실행해라!
        Invoke("RestoreSpeed", duration);
    }

    // 원래 속도로 복구하는 함수
    void RestoreSpeed()
    {
        currentSpeed = baseSpeed;
        Debug.Log(gameObject.name + "의 느려짐 버프가 해제되었습니다.");
    }
}
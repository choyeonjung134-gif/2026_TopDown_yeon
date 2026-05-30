using System.Collections;       //  1. 이 줄이 반드시 있어야 'IEnumerator' 에러가 사라집니다!
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public float frameTime = 0.15f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;

    [Header("이동 설정")]
    private float originalSpeed; // 원래 속도를 저장할 변수

    [Header("수집한 꽃 개수")]
    public int daisyCount = 0;
    public int lilacCount = 0;
    public int dandelionCount = 0;
    public int nemophilaCount = 0;

    [Header("특성 상태 변수")]
    public bool isMagnetActive = false; // 자석 활성화 여부
    public bool hasShield = false;      // 보호막 활성화 여부

    [Header("체력 설정")]
    public int maxHp = 5;       // 최대 체력
    public int currentHp;       // 현재 체력

    private Vector3 originalScale; // 원래 크기를 저장할 변수

    private Coroutine daisyCoroutine;
    private Coroutine lilacCoroutine;
    private Coroutine dandelionCoroutine;
    private Coroutine nemophilaCoroutine;

    private Vector3 startPosition; // 처음 시작 위치를 기억할 변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent <SpriteRenderer>();

        currentSprites = spriteDown;
        sr.sprite = currentSprites[0];
    }

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            //  정답 로직: 좌우 움직임이 위아래 움직임보다 클 때 (좌우 우선)
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                    ChangeSprites(spriteRight);
                else
                    ChangeSprites(spriteLeft);
            }
            // 위아래 움직임이 좌우 움직임보다 클 때 (위아래 우선)
            else
            {
                if (input.y > 0)
                    ChangeSprites(spriteUp);
                else
                    ChangeSprites(spriteDown);
            }
        }
    }
    private void Update()
    {
        if(input.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            sr.sprite = currentSprites[frameIndex];
            return;
        }
        timer += Time.deltaTime;

        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;

            if (frameIndex >= currentSprites.Length)
                frameIndex = 0;
            sr.sprite = currentSprites[frameIndex];
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites)
            return;

        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }
    void Start()
    {
        // 게임 시작 시 원래 속도와 원래 크기를 기억해 둡니다.
        originalSpeed = moveSpeed;
        originalScale = transform.localScale;

        currentHp = maxHp; // 게임 시작할 때 체력을 5로 가득 채웁니다.

        // 게임이 시작될 때 병아리의 첫 위치(좌표)를 콕 저장해 둡니다.
        startPosition = transform.position;
    }

    
    public void CollectDaisy()
    {
        daisyCount++;
        // 이미 속도 업 코루틴이 돌고 있다면 멈추고 새로 시작 (시간 초기화)
        StopCoroutine("DaisyRoutine");
        StartCoroutine(DaisyRoutine());
    }

    public void CollectLilac()
    {
        lilacCount++;
        StopCoroutine("LilacRoutine");
        StartCoroutine(LilacRoutine());
    }

    public void CollectDandelion()
    {
        dandelionCount++;
        StopCoroutine("DandelionRoutine");
        StartCoroutine(DandelionRoutine());
    }

    public void CollectNemophila()
    {
        nemophilaCount++;
        StopCoroutine("NemophilaRoutine");
        StartCoroutine(NemophilaRoutine());
    }
    IEnumerator DaisyRoutine()
    {
        moveSpeed = originalSpeed + 3f;
        Debug.Log("데이지 파워! 속도 증가");

        yield return new WaitForSeconds(5f);

        moveSpeed = originalSpeed;
        Debug.Log("데이지 버프 종료");
        daisyCoroutine = null; // 끝나면 비워줍니다.
    }
    IEnumerator LilacRoutine()
    {
        Debug.Log("라일락 파워! 주변 벌들을 느리게 만듭니다.");

        // 맵에 있는 모든 벌(Bee) 오브젝트들을 한 번에 다 찾아옵니다!
        Bee[] allBees = FindObjectsOfType<Bee>();

        // 찾아온 벌들에게 전부 "5초 동안 느려져라!" 명령을 내립니다.
        foreach (Bee bee in allBees)
        {
            if (bee != null)
            {
                bee.SlowDown(5f); // 5초 동안 슬로우 부여
            }
        }

        // 플레이어 자체는 무한 루프나 멈춤 없이 5초 동안 얌전히 대기합니다.
        yield return new WaitForSeconds(5f);

        Debug.Log("라일락 버프 종료");
        lilacCoroutine = null;
    }
    IEnumerator DandelionRoutine()
    {
        transform.localScale = originalScale * 0.6f;
        Debug.Log("민들레 파워! 병아리가 작아짐");

        yield return new WaitForSeconds(5f);

        transform.localScale = originalScale;
        Debug.Log("민들레 버프 종료");
        dandelionCoroutine = null;
    }

    // 4. 네모필라 : 5초간 보호막 (또는 적에게 맞을 때까지)
    IEnumerator NemophilaRoutine()
    {
        hasShield = true;
        Debug.Log("네모필라 파워! 보호막 생성");

        yield return new WaitForSeconds(5f);

        if (hasShield)
        {
            hasShield = false;
            Debug.Log("네모필라 보호막 시간 만료로 소멸");
        }
    }

    // ==========================================
    // 추가 기능 구현 함수들
    // ==========================================

    
   

    // 벌이나 장애물에 부딪혔을 때 호출할 데미지 함수 예시
    //  이것만 남겨두세요!
    public void TakeDamage()
    {
        // 만약 네모필라 보호막(Shield)이 켜져 있다면?
        if (hasShield)
        {
            hasShield = false; // 보호막이 대신 깨지고 데미지를 무효화합니다!
            if (nemophilaCoroutine != null) StopCoroutine(nemophilaCoroutine);
            Debug.Log("보호막이 깨져서 병아리가 살았습니다! 체력 유지: " + currentHp);
            return;
        }

        // 보호막이 없다면 실제로 체력을 1 깎습니다.
        currentHp--;
        Debug.Log(" 벌에게 쏘였습니다! 남은 체력: " + currentHp);

        // 체력이 0 이하가 되면 죽습니다.
        if (currentHp <= 0)
        {
            Die();
        }
    }

    // [수정된 코드] 벌과 '통과하며 충돌'했을 때를 감지합니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 물체의 태그가 "Bee" 인지 확인
        if (other.CompareTag("Bee"))
        {
            TakeDamage();
        }
    }

    // 병아리가 죽었을 때 실행될 함수
    void Die()
    {
        Debug.Log("5번 쏘였습니다... 꽃은 그대로 두고 시작 지점으로 돌아갑니다!");

        // 1. 병아리 위치를 처음 시작 위치로 순간이동!
        transform.position = startPosition;

        // 2. 체력을 다시 최대(5)로 가득 채워줍니다.
        currentHp = maxHp;

        // 3. (선택) 만약 작아져 있거나(민들레), 속도가 빨라진(데이지) 상태였다면 원래대로 복구
        moveSpeed = originalSpeed;
        transform.localScale = originalScale;
        hasShield = false;
        isMagnetActive = false;

        // 실행 중이던 모든 5초 버프 타이머들을 안전하게 다 꺼줍니다.
        StopAllCoroutines();
        daisyCoroutine = null;
        lilacCoroutine = null;
        dandelionCoroutine = null;
        nemophilaCoroutine = null;

        // 4. 병아리가 살아났음을 알림
        gameObject.SetActive(true);
        Debug.Log(" 병아리 부활! 현재 체력: " + currentHp);
    }
}

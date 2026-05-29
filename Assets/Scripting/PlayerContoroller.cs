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

    private Vector3 originalScale; // 원래 크기를 저장할 변수

    private Coroutine daisyCoroutine;
    private Coroutine lilacCoroutine;
    private Coroutine dandelionCoroutine;
    private Coroutine nemophilaCoroutine;

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

        // (기존에 작성하신 병아리 이동 및 방향 전환 코드가 여기에 들어갑니다!)

        // 만약 자석이 켜져있다면 주변 꽃을 끌어당깁니다.
        if (isMagnetActive)
        {
            HandleMagnetEffect();
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

    // 라일락 자석 효과 기능
    void HandleMagnetEffect()
    {
        // 주변 3유니티 반경 안에 있는 모든 꽃(Flower)들을 찾습니다.
        Collider2D[] flowers = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D col in flowers)
        {
            // ⭐ [여기 수정] col.gameObject == gameObject (자기자신) 이 아닐 때만 끌어당기도록 조건을 추가합니다!
            if (col.gameObject != gameObject && (col.CompareTag("Flower") || col.GetComponent<Flower>() != null))
            {
                // 꽃만 내 쪽(병아리 쪽)으로 슬며시 끌어당깁니다.
                col.transform.position = Vector3.MoveTowards(col.transform.position, transform.position, Time.deltaTime * 5f);
            }
        }
    }

    // 벌이나 장애물에 부딪혔을 때 호출할 데미지 함수 예시
    public void TakeDamage()
    {
        if (hasShield)
        {
            hasShield = false; // 보호막이 대신 깨지고 데미지 무효화!
            StopCoroutine("NemophilaRoutine"); // 타이머도 꺼줍니다.
            Debug.Log("보호막이 깨져서 병아리가 살았습니다!");
        }
        else
        {
            Debug.Log("보호막이 없어 병아리가 아픕니다! HP 감소");
            // 원래 받았어야 할 데미지 로직 처리 코드가 들어갈 자리
        }
    }
    }

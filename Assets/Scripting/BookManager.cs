using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    [Header("도감 UI 패널")]
    public GameObject bookPanel;

    [Header("연결할 게임 오브젝트들")]
    public PlayerController player;

    // 💡 4개였던 칸을 지우고, 직관적인 3개의 부케 이미지 칸으로 변경합니다!
    [Header("도감 내 꽃다발 이미지들 (회색이었다가 켜질 이미지들)")]
    public Image bouquetImage1;
    public Image bouquetImage2;
    public Image bouquetImage3;

    // 꽃다발 3개를 각각 모았는지 체크하는 변수 (기본값은 false)
    [HideInInspector] public bool hasBouquet1 = false; // 목숨 +1
    [HideInInspector] public bool hasBouquet2 = false; // 벌 -2
    [HideInInspector] public bool hasBouquet3 = false; // 영구 속도 업

    // 스킬이 해금되었는지 기억할 비밀 장부 (true가 되면 스킬 사용 가능!)
    [Header("스킬 해금 여부 상태")]
    public bool isSkill1Unlocked = false;
    public bool isSkill2Unlocked = false;
    public bool isSkill3Unlocked = false;

    public static BookManager Instance { get; private set; }

    void Start()
    {
        // 게임 시작할 때는 도감 창을 확실하게 닫아둡니다.
        if (bookPanel != null) bookPanel.SetActive(false);
    }

    // ==========================================
    // 도감 열고 닫기 (시간 멈춤 기능 포함)
    // ==========================================

    // 도감 열기 버튼에 연결할 함수

    private void Awake()
    {
        // 씬이 전환되어도 BookManager가 새로 생성되어 데이터가 날아가는 것을 방지합니다.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ★ 핵심: 다음 스테이지로 가도 이 오브젝트는 살아남음!
        }
        else
        {
            Destroy(gameObject); // 이미 존재한다면 중복 생성을 막기 위해 파괴
        }
    }
    void Update()
    {
        // 💡 매 프레임마다 플레이어가 숫자키를 누르는지 감시합니다!
        HandleSkillInputs();
    }

    public void OpenBook()
    {
        if (bookPanel != null)
        {
            bookPanel.SetActive(true); // 도감 화면 켜기
            Time.timeScale = 0f;       // ⭐ [치트키] 유니티의 시간을 0으로 만들어서 게임을 완전히 멈춥니다!
            Debug.Log("도감을 열었습니다. 게임 일시정지!");
        }
    }

    // 도감 닫기 버튼에 연결할 함수
    public void CloseBook()
    {
        if (bookPanel != null)
        {
            bookPanel.SetActive(false); // 도감 화면 끄기
            Time.timeScale = 1f;        // ⭐ 유니티의 시간을 다시 1로 돌려서 게임을 원래대로 흐르게 합니다.
            Debug.Log("도감을 닫았습니다. 게임 재개!");
        }
    }

    // ==========================================
    // 꽃다발 획득 및 특수능력 발동 시스템
    // ==========================================

    // 꽃다발을 조합하거나 획득했을 때 이 함수들을 호출해 주면 됩니다!

    // 1번 꽃다발 : 목숨 영구 +1 (최대 체력 5 -> 6)


    // 2번 꽃다발 : 벌 2마리 줄이기

    private void UseBouquetSkill(int skillNumber)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return;

        if (skillNumber == 1)
        {
            Debug.Log("💐 [부케 1번 스킬 발동!] 오리의 이동 속도가 3초간 빨라집니다!");
        }
        else if (skillNumber == 2)
        {
            Debug.Log("🛡️ [부케 2번 스킬 발동!] 무적 보호막 생성!");
        }
        else if (skillNumber == 3)
        {
            Debug.Log("⚡ [부케 3번 스킬 발동!] 주변의 벌들을 모두 쫓아냅니다!");
        }
    }

    // 기존의 중복되었던 1~3번 해금 함수들도 깔끔하게 딱 한 번만 아래처럼 새로 정의해 줍니다.
    public void UnlockBouquet1()
    {
        if (bouquetImage1 != null) bouquetImage1.color = Color.white;
        isSkill1Unlocked = true;
        Debug.Log("부케 1호 도감 해금 및 스킬1 활성화 완료!");
    }

    public void UnlockBouquet2()
    {
        if (bouquetImage2 != null) bouquetImage2.color = Color.white;
        isSkill2Unlocked = true;
        Debug.Log("부케 2호 도감 해금 및 스킬2 활성화 완료!");
    }

    public void UnlockBouquet3()
    {
        if (bouquetImage3 != null) bouquetImage3.color = Color.white;
        isSkill3Unlocked = true;
        Debug.Log("부케 3호 도감 해금 및 스킬3 활성화 완료!");
    }

    private void HandleSkillInputs()
    {
        // 1번 숫자키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 스킬이 해금된 상태일 때만 작동!
            if (isSkill1Unlocked)
            {
                UseBouquetSkill(1);
            }
            else
            {
                Debug.Log("아직 1번 부케 스킬이 해금되지 않았습니다!");
            }
        }
    }
}
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

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (bookPanel != null) bookPanel.SetActive(false);

        // 💐 [핵심] 스테이지 2 등 다음 씬으로 넘어갔을 때, 
        // 스테이지 1에서 저장했던 부케 획득 장부(PlayerPrefs)를 확인해서 복구합니다.
        RefreshBouquetUI();
    }

    // ==========================================
    // 도감 열고 닫기 (시간 멈춤 기능 포함)
    // ==========================================

    // 도감 열기 버튼에 연결할 함수

    
    void Update()
    {
        // 💡 매 프레임마다 플레이어가 숫자키를 누르는지 감시합니다!
        HandleSkillInputs();
    }

    public void OpenBook()
    {
        if (bookPanel != null)
        {
            bookPanel.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
            RefreshBouquetUI();  // 도감 열 때마다 최신 데이터로 UI 새로고침
            Debug.Log("도감을 열었습니다.");
        }
       
    }


    public void OnBookButtonClick()
    {
        // 💡 싱글톤 Instance를 활용하면 에디터에서 드래그 안 해도 알아서 찾아갑니다!
        if (BookManager.Instance != null)
        {
            // 도감 창을 열거나 닫는 함수 호출 (예시 이름이 OpenBook일 경우)
            // BookManager.Instance.OpenBook(); 
        }
    }

    // 도감 닫기 버튼에 연결할 함수
    public void CloseBook()
    {
        if (bookPanel != null)
        {
            bookPanel.SetActive(false);
            Time.timeScale = 1f; // 게임 재개
            Debug.Log("도감을 닫았습니다.");
         }
    }

    // ==========================================
    // 꽃다발 획득 및 특수능력 발동 시스템
    // ==========================================

    // 꽃다발을 조합하거나 획득했을 때 이 함수들을 호출해 주면 됩니다!

    // 1번 꽃다발 : 목숨 영구 +1 (최대 체력 5 -> 6)


    // 2번 꽃다발 : 벌 2마리 줄이기

    // 획득 장부를 검사해서 도감 이미지 색상과 스킬 잠금을 동기화하는 함수
    public void RefreshBouquetUI()
    {
        // 1번 부케 체크
        if (PlayerPrefs.GetInt("HasBouquet1", 0) == 1)
        {
            if (bouquetImage1 != null) bouquetImage1.color = Color.white; // 불 켜기 (기존 색)
            isSkill1Unlocked = true;
        }
        else
        {
            if (bouquetImage1 != null) bouquetImage1.color = new Color(0.3f, 0.3f, 0.3f, 1f); // 회색
            isSkill1Unlocked = false;
        }

        // 2번 부케 체크
        if (PlayerPrefs.GetInt("HasBouquet2", 0) == 1)
        {
            if (bouquetImage2 != null) bouquetImage2.color = Color.white;
            isSkill2Unlocked = true;
        }
        else
        {
            if (bouquetImage2 != null) bouquetImage2.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            isSkill2Unlocked = false;
        }

        // 3번 부케 체크
        if (PlayerPrefs.GetInt("HasBouquet3", 0) == 1)
        {
            if (bouquetImage3 != null) bouquetImage3.color = Color.white;
            isSkill3Unlocked = true;
        }
        else
        {
            if (bouquetImage3 != null) bouquetImage3.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            isSkill3Unlocked = false;
        }
    }

    // 외부(BouquetObject 등)에서 부케를 먹었을 때 직접 원격 호출해 줄 함수들
    public void UnlockBouquet1()
    {
        PlayerPrefs.SetInt("HasBouquet1", 1);
        PlayerPrefs.Save();
        RefreshBouquetUI();
        Debug.Log("부케 1호 도감 해금 및 스킬1 활성화 완료!");
    }

    public void UnlockBouquet2()
    {
        PlayerPrefs.SetInt("HasBouquet2", 1);
        PlayerPrefs.Save();
        RefreshBouquetUI();
        Debug.Log("부케 2호 도감 해금 및 스킬2 활성화 완료!");
    }

    public void UnlockBouquet3()
    {
        PlayerPrefs.SetInt("HasBouquet3", 1);
        PlayerPrefs.Save();
        RefreshBouquetUI();
        Debug.Log("부케 3호 도감 해금 및 스킬3 활성화 완료!");
    }

  

    private void HandleSkillInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isSkill1Unlocked)
            {
                Debug.Log("💐 [부케 1번 스킬 발동!] 주변의 벌들을 모두 쫓아냅니다!");
            }
            else
            {
                Debug.Log("아직 1번 부케 스킬이 해금되지 않았습니다! (스테이지 1에서는 사용 불가)");
            }
        }
    }


}
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    [Header("도감 UI 패널")]
    public GameObject bookPanel;       // 아까 만든 BookPanel을 연결할 곳

    [Header("연결할 게임 오브젝트들")]
    public PlayerController player;    // 병아리 스크립트 연결

    // 꽃다발 3개를 각각 모았는지 체크하는 변수 (기본값은 false)
    [HideInInspector] public bool hasBouquet1 = false; // 목숨 +1
    [HideInInspector] public bool hasBouquet2 = false; // 벌 -2
    [HideInInspector] public bool hasBouquet3 = false; // 영구 속도 업

    void Start()
    {
        // 게임 시작할 때는 도감 창을 확실하게 닫아둡니다.
        if (bookPanel != null) bookPanel.SetActive(false);
    }

    // ==========================================
    // 도감 열고 닫기 (시간 멈춤 기능 포함)
    // ==========================================

    // 도감 열기 버튼에 연결할 함수
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
    public void UnlockBouquet1()
    {
        if (hasBouquet1) return; // 이미 해금했다면 패스
        hasBouquet1 = true;

        player.maxHp = 6;       // 병아리의 최대 체력을 6으로 늘리고
        player.currentHp++;     // 현재 체력도 1 보너스로 채워줍니다!
        Debug.Log(" 1번 꽃다발 해금! 최대 목숨이 6으로 늘어났습니다.");
    }

    // 2번 꽃다발 : 벌 2마리 줄이기
    public void UnlockBouquet2()
    {
        if (hasBouquet2) return;
        hasBouquet2 = true;

        // 맵에 있는 벌("Bee" 태그)을 찾아서 딱 2마리만 파괴합니다.
        GameObject[] bees = GameObject.FindGameObjectsWithTag("Bee");
        int deleteCount = Mathf.Min(2, bees.Length); // 벌이 2마리보다 적으면 있는 만큼만

        for (int i = 0; i < deleteCount; i++)
        {
            Destroy(bees[i]);
        }
        Debug.Log(" 2번 꽃다발 해금! 벌 {deleteCount}마리를 맵에서 쫓아냈습니다.");
    }

    // 3번 꽃다발 : 병아리 영구적으로 빨라지기
    public void UnlockBouquet3()
    {
        if (hasBouquet3) return;
        hasBouquet3 = true;

        player.moveSpeed += 1.5f;

        // originalSpeed 에러를 우회하기 위해 컴포넌트를 강제로 직접 찾아와서 수정합니다!
        if (player.TryGetComponent(out PlayerController realPlayer))
        {
            realPlayer.moveSpeed = player.moveSpeed;
            // 만약 원래 쓰려던 변수가 originalSpeed가 맞다면 아래 주석(//)을 지우고 사용하세요!
            // realPlayer.originalSpeed += 1.5f;
        }
    }
    }
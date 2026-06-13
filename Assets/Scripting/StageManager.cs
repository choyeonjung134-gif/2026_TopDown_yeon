using UnityEngine;
using TMPro; // UI 텍스트(TextMeshPro)를 제어하기 위해 꼭 필요합니다!

public class StageManager : MonoBehaviour
{
    [Header("각 꽃의 현재 획득 개수")]
    public int daisyCount = 0;
    public int dandelionCount = 0;
    public int lilacCount = 0;
    public int nemophilaCount = 0;

    [Header("클리어에 필요한 목표 개수")]
    public int targetCount = 5; // 예시로 5개씩 모으면 클리어되게 설정

    [Header("화면 왼쪽 끝에 배치할 TextMeshPro 텍스트들")]
    public TextMeshProUGUI daisyText;
    public TextMeshProUGUI dandelionText;
    public TextMeshProUGUI lilacText;
    public TextMeshProUGUI nemophilaText;

    [Header("다 모으면 나타날 꽃다발 오브젝트")]
    public GameObject bouquetObject;

    void Start()
    {
        // 게임 시작 시 텍스트 UI를 0개 상태로 초기화
        UpdateFlowerUI();

        // 시작할 때는 꽃다발을 잠시 꺼둡니다.
        if (bouquetObject != null) bouquetObject.SetActive(false);
    }

    // 꽃을 먹었을 때 외부 스크립트(Flower 등)에서 호출할 함수
    public void AddFlower(string flowerType)
    {
        if (flowerType == "Daisy") daisyCount++;
        else if (flowerType == "Dandelion") dandelionCount++;
        else if (flowerType == "Lilac") lilacCount++;
        else if (flowerType == "Nemophila") nemophilaCount++;

        // 숫자가 올랐으니 UI 글자도 새로고침
        UpdateFlowerUI();

        // 모든 종류의 꽃을 목표 개수만큼 다 모았는지 체크
        CheckStageClear();
    }

    // 화면의 글자들을 예쁘게 업데이트해 주는 함수
    void UpdateFlowerUI()
    {
        if (daisyText != null) daisyText.text = $"Daisy : {daisyCount} / {targetCount}";
        if (dandelionText != null) dandelionText.text = $"Dandelion : {dandelionCount} / {targetCount}";
        if (lilacText != null) lilacText.text = $"Lilac : {lilacCount} / {targetCount}";
        if (nemophilaText != null) nemophilaText.text = $"Nemophila : {nemophilaCount} / {targetCount}";
    }

    // 꽃을 다 모았는지 검사하는 함수
    void CheckStageClear()
    {
        if (daisyCount >= targetCount &&
            dandelionCount >= targetCount &&
            lilacCount >= targetCount &&
            nemophilaCount >= targetCount)
        {
            Debug.Log("모든 꽃을 수집했습니다! 중앙에 꽃다발이 나타납니다.");
            if (bouquetObject != null)
            {
                bouquetObject.SetActive(true); // 아까 만든 꽃다발 출현!
            }

        }


        // 꽃다발에 닿아서 다음 씬으로 넘어가기 바로 직전에 이 코드가 실행되어야 합니다!
        if (BookManager.Instance != null)
        {
            // 1스테이지라면 1번 부케 해금, 2스테이지라면 2번 부케 해금!
            BookManager.Instance.UnlockBouquet1();
        }

        // 그 이후에 다음 씬으로 넘어가는 코드 실행
        // SceneManager.LoadScene("Stage2");
    }
    // StageManager.cs 파일 내부 맨 아래 (71번째 줄 괄호 닫히기 직전)에 추가
    public void OnBookButtonClick()
    {
        // 1. 현재 Stage2 씬의 Canvas 안에 살아있는 "Book" 패널을 직접 찾습니다.
        // (하이어라키에 있는 'Book' 오브젝트의 정확한 이름을 큰따옴표 안에 적어주셔야 합니다!)
        GameObject localBookPanel = GameObject.Find("Canvas").transform.Find("Book")?.gameObject;

        if (localBookPanel != null)
        {
            // 2. 만약 꺼져있다면 도감 패널을 켭니다!
            localBookPanel.SetActive(true);

            // 3. 도감이 열렸으므로 게임의 시간을 일시정지 상태로 만듭니다.
            Time.timeScale = 0f;
            Debug.Log(" [StageManager] 현재 스테이지의 도감 패널을 원격으로 열었습니다! 시간 정지.");
        }
        else
        {
            Debug.LogError(" Canvas 밑에 있는 'Book' 오브젝트를 찾을 수 없습니다. 이름을 확인해 주세요!");
        }
    }
}
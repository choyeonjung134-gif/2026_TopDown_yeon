using UnityEngine;
using UnityEngine.SceneManagement; // 스테이지 전환을 위해 필요합니다!

public class BouquetObject : MonoBehaviour
{
    [Header("해금할 꽃다발 번호 (1, 2, 3 중 선택)")]
    public int bouquetNumber = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어(오리)가 꽃다발에 닿았을 때만 작동
        if (other.CompareTag("Player"))
        {
            // 1. 게임 매니저를 찾아 도감 기능을 해금합니다.
            BookManager bookManager = FindObjectOfType<BookManager>();
            if (bookManager != null)
            {
                bookManager.UnlockBouquet1();
                PlayerPrefs.SetInt("HasBouquet1", 1); // 👈 [핵심] 이제 스테이지2부터 도감에서 유색으로 활성화됩니다!
            }

            // 💐 스테이지 2를 깨고 꽃다발 2번을 획득했을 때 처리
            else if (bouquetNumber == 2)
            {
                bookManager.UnlockBouquet2();
                PlayerPrefs.SetInt("HasBouquet2", 1);
            }
            // 💐 스테이지 3을 깨고 꽃다발 3번을 획득했을 때 처리
            else if (bouquetNumber == 3)
            {
                bookManager.UnlockBouquet3();
                PlayerPrefs.SetInt("HasBouquet3", 1);
            }
        }

            Debug.Log("{bouquetNumber}번 꽃다발 획득! 다음 스테이지로 이동합니다.");

            // 2. 다음 스테이지(씬)로 이동합니다.
            // 빌드 세팅에 등록된 현재 씬의 다음 인덱스 씬을 불러옵니다.
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // 만약 다음 씬이 빌드 세팅에 존재한다면 전환
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("마지막 스테이지입니다! 처음 스테이지로 돌아갑니다.");
                SceneManager.LoadScene(0); // 혹은 엔딩 크레딧 씬 이름 입력
            }

            // 3. 먹은 꽃다발 오브젝트는 파괴
            Destroy(gameObject);
        }
    
}
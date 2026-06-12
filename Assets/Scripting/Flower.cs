using UnityEngine;

public class Flower : MonoBehaviour
{
    // 유니티 인스펙터에서 꽃 종류를 고를 수 있게 만듭니다.
    public enum FlowerType { Daisy, Lilac, Dandelion, Nemophila }
    [Header("꽃 종류 선택")]
    public FlowerType flowerType;

    // [Header]를 써주면 유니티 인스펙터 창에서 글자를 직접 타이핑할 수 있게 됩니다!
    [Header("이 꽃의 종류를 적어주세요 (Daisy, Dandelion, Lilac, Nemophila)")]
    public string flowerTypes = "Daisy"; // 기본값은 Daisy로 설정

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 오브젝트가 플레이어인지 확인 (Tag가 Player여야 합니다)
        if (other.CompareTag("Player"))
        {
            // 플레이어의 PlayerController 컴포넌트를 가져옵니다.
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // 꽃 종류에 따라 플레이어에게 능력을 부여합니다.
                switch (flowerType)
                {
                    case FlowerType.Daisy:
                        player.CollectDaisy();
                        break;
                    case FlowerType.Lilac:
                        player.CollectLilac();
                        break;
                    case FlowerType.Dandelion:
                        player.CollectDandelion();
                        break;
                    case FlowerType.Nemophila:
                        player.CollectNemophila();
                        break;
                }
                if (other.CompareTag("Player"))
                {
                    // 1. 방금 만든 StageManager를 찾습니다.
                    StageManager stageManager = FindObjectOfType<StageManager>();
                    if (stageManager != null)
                    {
                        // 2. 이 꽃 오브젝트의 이름이나 지정된 문자열을 넘겨줍니다.
                        // 인스펙터에서 고른 FlowerType 변수(Dandelion, Lilac 등)의 이름을 그대로 문자로 바꿔서 전달합니다!
                        stageManager.AddFlower(flowerType.ToString());
                    }

                    // 3. 먹었으니 꽃은 사라짐
                    Destroy(gameObject);
                }

            }

        }

    }

}
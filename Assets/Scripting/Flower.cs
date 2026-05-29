using UnityEngine;

public class Flower : MonoBehaviour
{
    // 유니티 인스펙터에서 꽃 종류를 고를 수 있게 만듭니다.
    public enum FlowerType { Daisy, Lilac, Dandelion, Nemophila }
    [Header("꽃 종류 선택")]
    public FlowerType flowerType;

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

                // 능력을 주고 난 꽃은 화면에서 삭제합니다.
                Destroy(gameObject);
            }
        }
    }
}
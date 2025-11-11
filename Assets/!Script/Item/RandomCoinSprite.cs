using UnityEngine;

public class RandomCoinSprite : MonoBehaviour
{
    public Sprite[] coinSprites; // 3種類のスプライト
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
      
        if (coinSprites != null && coinSprites.Length > 0)
        {
            spriteRenderer.sprite = coinSprites[Random.Range(0, coinSprites.Length)];
        }
    }
}

using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField, Header("スクロール速度")]
    private float _speed = 2f;

    private float _height;

    void Start()
    {
        // 背景スプライトの高さを計算（スケールも含める）
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        _height = sr.bounds.size.y;
    }

    void Update()
    {
        _Scroll();
    }

    private void _Scroll()
    {
        // ワールド座標で下方向に移動
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);

        // 画面下に出たら一番上に再配置
        if (transform.position.y <= -_height)
        {
            transform.position += new Vector3(0, _height * 3f, 0);
            // ← 3枚分上に戻す
        }
    }
}
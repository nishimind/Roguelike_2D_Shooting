using UnityEngine;

public class ExplodeBullet : BulletBase
{
    [Header("移動速度（初速）")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("減速量(毎秒)")]
    [SerializeField] private float deceleration = 2f;

    [Header("左右ゆらゆら")]
    [SerializeField] private float wobbleAmplitude = 0.3f;
    [SerializeField] private float wobbleFrequency = 4f;

    [Header("爆発までの距離")]
    [SerializeField] private float explodeDistance = 5f;

    [Header("最大寿命(秒) 保険用")]
    [SerializeField] private float maxLifeTime = 3f;

    // ▼ 爆発後に生成される下向き弾 ▼
    [Header("爆発後に落ちる弾のプレハブ")]
    [SerializeField] private GameObject downBulletPrefab;

    [Header("ランダムに生成する弾の数")]
    [SerializeField] private int downBulletCount = 12;

    [Header("出現範囲（X,Y）（爆心地からの±方向）")]
    [SerializeField] private Vector2 downAreaSize = new Vector2(4f, 2f);

    [Header("下に落ちる弾のベーススピード")]
    [SerializeField] private float downBulletSpeed = 5f;

    [Header("下弾のふんわり散らばり")]
    [SerializeField] private float downSideDriftRange = 0.6f; // 横方向のばらつき

    [Header("下弾のゆらゆら設定")]
    [SerializeField] private float downWobbleAmplitude = 0.3f;
    [SerializeField] private float downWobbleFrequency = 2f;

    private float _time;
    private float _traveled;
    private float _currentSpeed;
    private BulletDamage _damage;
    private bool _exploded = false;

    protected override void Initialize()
    {
        ResetState();
        _damage = GetComponent<BulletDamage>();
    }

    /// <summary>プール再利用時のリセット</summary>
    public void ResetState()
    {
        _time = 0f;
        _traveled = 0f;
        _currentSpeed = moveSpeed;
        _exploded = false;
    }

    protected override void Update()
    {
        if (_exploded) return;

        _time += Time.deltaTime;

        // 減速
        _currentSpeed -= deceleration * Time.deltaTime;
        if (_currentSpeed < 0f) _currentSpeed = 0f;

        // 左右ゆらゆら
        float wobble = Mathf.Sin(_time * wobbleFrequency) * wobbleAmplitude;
        Vector3 forward = transform.up;
        Vector3 side = transform.right * wobble;

        Vector3 dir = (forward + side).normalized;

        // 移動
        float move = _currentSpeed * Time.deltaTime;
        transform.position += dir * move;
        _traveled += Mathf.Abs(move);

        // 距離 or 時間で爆発
        if (_traveled >= explodeDistance || _time >= maxLifeTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (_exploded) return;
        _exploded = true;

        Vector3 center = transform.position;

        // ▼ランダムに「下方向ふんわり弾」を生成
        if (downBulletPrefab != null && downBulletCount > 0)
        {
            SpawnDownRain(center);
        }

        // 親弾をプールに返却
        var myDamage = GetComponentInChildren<BulletDamage>();
        if (myDamage != null && myDamage.originPrefab != null)
        {
            BulletPool.Instance.Release(myDamage.originPrefab, this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 爆心地周辺にランダム配置して、
    /// 「下方向ベース＋ふんわりゆらゆら」で落ちる弾を生成
    /// </summary>
    private void SpawnDownRain(Vector3 center)
    {
        float halfW = downAreaSize.x * 0.5f;
        float halfH = downAreaSize.y * 0.5f;

        for (int i = 0; i < downBulletCount; i++)
        {
            // 爆心地(center)を基準にランダムな位置に出現
            float offsetX = Random.Range(-halfW, halfW);
            float offsetY = Random.Range(-halfH, halfH);

            Vector3 spawnPos = center + new Vector3(offsetX, offsetY, 0f);

            // 回転はとりあえずそのまま（見た目優先なら調整してOK）
            Quaternion rot = Quaternion.identity;

            GameObject bullet = BulletPool.Instance.Get(
                downBulletPrefab,
                spawnPos,
                rot
            );

            bullet.transform.position = spawnPos;
            bullet.transform.rotation = rot;

            // ダメージ（少し弱めに）
            var dmg = bullet.GetComponent<BulletDamage>();
            if (dmg != null && _damage != null)
            {
                dmg.damage = _damage.damage * 0.5f;
            }

            // ★ 横方向のベースドリフト（ふんわり散らばり）
            float side = Random.Range(-downSideDriftRange, downSideDriftRange);

            // ★ ゆらゆら付きの移動コンポーネントがあれば初期化
            var mover = bullet.GetComponent<DownWobbleMover>();
            if (mover != null)
            {
                mover.Init(
                    downBulletSpeed,
                    downWobbleAmplitude,
                    downWobbleFrequency,
                    side
                );
            }
            else
            {
                // 予備：コンポーネントが無い場合はただのふんわり下向き
                var rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = new Vector2(side, -1f).normalized;
                    rb.velocity = dir * downBulletSpeed;
                }
            }
        }
    }
}
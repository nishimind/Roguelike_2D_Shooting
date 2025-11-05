using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
  

    [SerializeField, Header("死亡時effect")]
    protected GameObject deadEffect;

    [Header("ダメージ演出")]
    [SerializeField, Header("点滅時間(秒)")]
    private float damageTime = 0.5f;
    [SerializeField, Header("点滅周期(秒)")]
    private float damageCycle = 0.1f;

    protected SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool isDamage;

    protected virtual void Start()
    {
       
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        DamageBlink();
    }

    public virtual void TakeDamage(int damage)
    {
      
    }

    protected virtual void StartBlink()
    {
        isDamage = true;
        damageTimeCount = 0;
    }

    // 共通：点滅処理
    protected virtual void DamageBlink()
    {
        if (!isDamage) return;
       
        damageTimeCount += Time.deltaTime;
        float value = Mathf.Repeat(damageTimeCount, damageCycle);
        spriteRenderer.enabled = value >= damageCycle * 0.5f;
       // Debug.Log("Blink");
        if (damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.enabled = true;
            isDamage = false;
        }
    }

    // 共通：回復処理


    // 派生クラスで具体的な死の挙動を定義する
    protected abstract  void Die();
}

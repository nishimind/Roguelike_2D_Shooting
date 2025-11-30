using UnityEngine;
using System.Threading;

public abstract class AttackPatternSO : ScriptableObject
{public bool isPlyerBullet = false;
    public abstract void Shoot(Vector3 position, int angle, GameObject bulletPrefab, float damage);
   
    public CancellationToken token = CancellationToken.None;

    public void SetToken(CancellationToken t)
    {
        token = t;
    }

}

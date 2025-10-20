using UnityEngine;

public class OptionShooter : MonoBehaviour
{
    private float shootCount;

    private void FixedUpdate()
    {
        

        //’e‚ðŒ‚‚Âˆ—‚ðˆÚ“®‚³‚¹‚Ü‚µ‚½, ’e”­ŽËˆ—‚ðŒy‚¢•ûŽ®‚ÉC³
        shootCount += Time.deltaTime;
        if (shotPressed && shootCount >= _shootTime)
        {
            if (_bulletPooler != null)
            {
                GameObject bullet = _bulletPooler.Get(transform.position, transform.rotation);
                bullet.GetComponent<BulletDamage>().damage = bullletPower;
                shootCount = 0f;
            }
        }

    }
}

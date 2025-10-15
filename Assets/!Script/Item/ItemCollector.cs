using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [Header("‹z‚¢‚İ”ÍˆÍ")]
    public float collectRadius = 2.5f;
  
   /* private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }*/
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFloatMotion item = other.GetComponent<ItemFloatMotion>();
        if (item != null)
        {
            item.Collect(transform); // ƒvƒŒƒCƒ„[‚É‹z‚¢‚Şˆ—‚ğŒÄ‚Ô
        }
    }
   
}

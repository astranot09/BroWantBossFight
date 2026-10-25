using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float bulletTime = 5f;
    [SerializeField] private bool normalBullet = true;

    [Header("Kaboom")]
    [SerializeField] private float kaboomRange = 1f;
    [SerializeField] private LayerMask iDamageableLayer;
    [SerializeField] private GameObject kaboomEffectPrefab;
    [SerializeField] private bool isKaboom = false;

    private void Start()
    {
        StartCoroutine(CountDownDestroy());
    }

    public void SetUpBullet(int dmg, bool normal)
    {
        damage = dmg;
        normalBullet = normal;

        StartCoroutine(CountDownDestroy());
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }

    IEnumerator CountDownDestroy()
    {
        yield return new WaitForSeconds(bulletTime);
        if(isKaboom)
            DamageArea();
        Destroy(gameObject);
    }

    private void DamageArea()
    {
        Instantiate(kaboomEffectPrefab, transform.position, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, kaboomRange, iDamageableLayer);

        foreach(Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                hit.GetComponent<IDamageable>().TakeDamage(damage);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, kaboomRange);
        Gizmos.color = Color.yellow;
    }
}

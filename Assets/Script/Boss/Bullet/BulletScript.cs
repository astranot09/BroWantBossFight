using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private int damage = 2;

    [SerializeField] private float bulletLife = 5f;
    [SerializeField] private float bulletSpeed;

    [Header("Kaboom")]
    [SerializeField] private float kaboomRange = 1f;
    [SerializeField] private LayerMask iDamageableLayer;
    [SerializeField] private GameObject kaboomEffectPrefab;
    [SerializeField] private bool isKaboom = false; //If false = bullet hell

    private Vector2 spawnPoint;
    private Transform playerTransform;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        StartCoroutine(CountDownDestroy());
    }

    private void Update()
    {
        if (!isKaboom)
        {
            transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
        }
        else
        {
            if (playerTransform != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, bulletSpeed * Time.deltaTime);

                // Opsional: Memutar arah moncong peluru menghadap player
                Vector2 direction = (Vector2)playerTransform.position - (Vector2)transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            }
        }
    }
    public void SetUpBullet(int dmg, float speed, bool normal)
    {
        damage = dmg;
        bulletSpeed = speed;
        isKaboom = !normal;

        StartCoroutine(CountDownDestroy());
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamageable>().TakeDamage(damage);
            if (isKaboom)
                DamageArea();
            Destroy(gameObject);
        }
    }

    IEnumerator CountDownDestroy()
    {
        yield return new WaitForSeconds(bulletLife);
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

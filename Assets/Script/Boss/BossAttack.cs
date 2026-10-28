using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BossAttack : MonoBehaviour
{
    [SerializeField] private GameObject missliePrefab;
    [SerializeField] private int indexAttack = -1;

    [Header("Bullet Hell Setting")]
    [SerializeField] private int bulletHellDamage = 1;
    [SerializeField] private float bulletHellSpeed = 5f;
    [SerializeField] private float firingRateBulletHell = 0.1f;
    [SerializeField] private float bulletHellDuration = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    private float fireTimer = 0f;
    private float durationTimer = 0f;


    [Header("Bullet Follow Setting")]
    [SerializeField] private int bulletFollowDamage = 2;
    [SerializeField] private float bulletFollowSpeed = 1.5f;
    [SerializeField] private int maxBulletFollowSpawn = 3;
    [SerializeField] private float bulletFollowSpawnDelay = 2f;

    [Header("Reference")]
    [SerializeField] private BossScript bossScript;

    private void Update()
    {
        switch (indexAttack)
        {
            case 0:
                BulletHellTime();
                break;
        }
    }

    public void RandomSpecialAttack()
    {
        indexAttack = Random.Range(0, 2);

        switch (indexAttack)
        {
            case 0:
                // BulletHell
                fireTimer = 0f;
                durationTimer = 0f;
                break;

            case 1:
                // Summon 3 misil pelacak
                StartCoroutine(SpawnBulletFollowCoroutine());
                break;
        }
    }

    private void BulletHellTime()
    {
        transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + rotationSpeed * Time.deltaTime);

        fireTimer += Time.deltaTime;
        durationTimer += Time.deltaTime;

        if (fireTimer >= firingRateBulletHell)
        {
            FireBulletHell();
            fireTimer = 0f;
        }

        if (durationTimer >= bulletHellDuration)
        {
            ChangeStateToIdle();
        }
    }

    private void FireBulletHell()
    {
        GameObject spawnBullet = Instantiate(missliePrefab, transform.position, Quaternion.identity);
        spawnBullet.GetComponent<BulletScript>().SetUpBullet(bulletHellDamage, bulletHellSpeed, true);
        spawnBullet.transform.rotation = transform.rotation;
    }

    IEnumerator SpawnBulletFollowCoroutine()
    {
        for (int i = 0; i < maxBulletFollowSpawn; i++)
        {
            yield return new WaitForSeconds(bulletFollowSpawnDelay);
            FireBulletFollow();
        }
        ChangeStateToIdle();
        
    }
    private void FireBulletFollow()
    {
        GameObject spawnBullet = Instantiate(missliePrefab, transform.position, Quaternion.identity);
        spawnBullet.GetComponent<BulletScript>().SetUpBullet(bulletHellDamage, bulletHellSpeed, false);
        spawnBullet.transform.rotation = transform.rotation;
    }

    private void ChangeStateToIdle()
    {
        indexAttack = -1;
        bossScript.SwitchStateBoss(BossState.Idle);
    }
}

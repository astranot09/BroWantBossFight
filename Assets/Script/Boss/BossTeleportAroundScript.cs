using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTeleportAroundScript : MonoBehaviour
{
    [SerializeField] private int maxTeleport = 3;
    private int currentTeleport;

    [SerializeField] private float bossTeleportAroundTimer = 10f;
    private float timer;

    [SerializeField] private float checkRadius = 3f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private List<Transform> transformLocations = new();
    private int currentLocationIndex = -1;
    [SerializeField] private GameObject teleportEffect;

    [SerializeField] private BossScript bossScript;

    private Vector2 originalLocation;

    public void TeleportInit()
    {
        originalLocation = transform.parent.position;
        TeleportToNextLocation();
    }

    private void Update()
    {
        if (bossScript.CurrentState == BossState.Escape)
        {
            timer += Time.deltaTime;
            if (timer >= bossTeleportAroundTimer)
            {
                BackToIdle();
                return;
            }

            CheckPlayerRange();
        }
    }

    private void CheckPlayerRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, checkRadius, playerLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                if (currentTeleport >= maxTeleport)
                {
                    BackToIdle();
                    return;
                }

                TeleportToNextLocation();
                break;
            }
        }
    }

    private void TeleportToNextLocation()
    {
        if (transformLocations.Count == 0) return;

        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, transformLocations.Count);
        }
        while (randomIndex == currentLocationIndex);

        Instantiate(teleportEffect, transform.position, Quaternion.identity);
        currentLocationIndex = randomIndex;
        transform.parent.position = transformLocations[randomIndex].position;
        Instantiate(teleportEffect, transform.position, Quaternion.identity);

        currentTeleport++;
    }

    private void BackToIdle()
    {
        bossScript.SwitchStateBoss(BossState.Idle);
        timer = 0f;
        currentTeleport = 0;
        transform.parent.position = originalLocation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

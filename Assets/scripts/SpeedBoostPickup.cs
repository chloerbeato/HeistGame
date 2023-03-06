using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    public float boostDuration = 5f;
    public float boostMultiplier = 24f;
    public AudioClip pickupSound;
    public float rotationSpeed = 100f; // new variable for rotation speed

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpeedBoost(other.gameObject));
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }

    IEnumerator SpeedBoost(GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        float originalSpeed = playerMovement.speed;
        playerMovement.speed = boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        playerMovement.speed = originalSpeed;

        Destroy(gameObject);
    }

    // rotate the pickup around its y-axis
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}

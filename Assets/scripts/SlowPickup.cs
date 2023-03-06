using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPickup : MonoBehaviour
{
    public float boostDuration = 5f;
    public float slowBoost = 7f;
    public AudioClip pickupSound;
    public float rotationSpeed = 100f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpeedDecrease(other.gameObject));
             AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }
    }

    IEnumerator SpeedDecrease(GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        float originalSpeed = playerMovement.speed;
        playerMovement.speed = slowBoost;

        yield return new WaitForSeconds(boostDuration);

        playerMovement.speed = originalSpeed;
        Destroy(gameObject);
    }
    
    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}


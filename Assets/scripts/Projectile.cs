using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float destroyDelay = 1.5f;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private AudioClip hitSound;
    public GameObject impactEffect;

    private Rigidbody rb;
    private float timer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = transform.forward * projectileSpeed;
        timer = Time.time + destroyDelay;
    }

    private void Update()
    {
        if (Time.time >= timer)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerNavMesh"))
        {
            other.GetComponent<PlayerNavMesh>().Stun(stunDuration);
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
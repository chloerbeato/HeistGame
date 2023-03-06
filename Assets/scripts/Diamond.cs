using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Diamond : MonoBehaviour
{
    public float winTime = 90f;
    private float timer = 0f;
    private bool hasWon = false;
    public GameObject timerUI;
    public float rotateSpeed = 50f;
    public AudioClip loseMusicClip; // add lose music clip
    private AudioSource audioSource; // add audio source variable

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // get the audio source component
        audioSource.loop = false; // set the audio source to not loop
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().Win();
            hasWon = true;
        }
    }

    private void Update()
    {
        if (!hasWon)
        {
            timer += Time.deltaTime;
            if (timerUI != null)
            {
                timerUI.GetComponent<TMPro.TextMeshProUGUI>().text = "Time: " + Mathf.Round(timer);         
            }
            if (timer >= winTime)
            {
                Debug.Log("Time's up!");
                hasWon = true;
                audioSource.clip = loseMusicClip; // set the audio source clip to the lose music
                audioSource.Play(); // play the lose music
                SceneManager.LoadScene(3);
            }
            // Add rotation to the diamond
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }
    }
}

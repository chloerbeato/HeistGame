using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public int health = 3; // add health variable
    public bool isPaused = false; // add pause variable
    public GameObject projectilePrefab; // add reference to projectile prefab
    public AudioSource mainMusicSource;
    public AudioClip winSound;
    public AudioClip loseSound;
    public float winTime = 90f;
    private float timer = 0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // get reference to audio source component
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= winTime)
        {
            mainMusicSource.Stop(); // stop the main music
           // mainMusicSource.PlayOneShot(loseSound); // play the lose sound effect
        }

        if (!isPaused) // check if game is paused
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if (Input.GetButtonDown("Fire1")) // check for projectile input
            {
                FireProjectile(); // fire projectile
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // check for pause input
        {
            isPaused = !isPaused; // toggle pause state

            if (isPaused)
            {
                Time.timeScale = 0f; // pause time
            }
            else
            {
                Time.timeScale = 1f; // unpause time
            }
        }
    }
    
     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerNavMesh")) // check if collided with PlayerNavMesh
        {
            health--; // decrease health by 1
            if (health <= 0) // if health reaches 0 or less
            {
                speed = 0f;
                Debug.Log("Game over"); // display game over message
                mainMusicSource.Stop(); // stop the main music
                mainMusicSource.PlayOneShot(loseSound); // play the lose sound effect
                SceneManager.LoadScene(3);
            }
        }
        else if (other.gameObject.CompareTag("Projectile")) // check if collided with Projectile
        {
            PlayerNavMesh playerNavMesh = other.GetComponentInParent<PlayerNavMesh>();
            if (playerNavMesh != null)
            {
                playerNavMesh.Stun(3f); // trigger stun effect on guard
            }
        }
        else if (other.gameObject.CompareTag("Diamond")) // check if collided with Diamond
        {
            Win(); // trigger win condition
        }
    }
    
    public void Win()
    {
         Debug.Log("You win!"); // display win message
        mainMusicSource.Stop(); // stop the main music
        mainMusicSource.PlayOneShot(winSound); // play the win sound effect
       
        SceneManager.LoadScene(2);
    }

    void FireProjectile()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        Rigidbody projectileRigidbody = projectileObject.GetComponent<Rigidbody>();
        projectileRigidbody.AddForce(transform.forward * 500f);
    }
}

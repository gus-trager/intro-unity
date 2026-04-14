using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource coinAudioSource;
    private AudioSource damageAudioSource;
    private Animator animator;

    public float speed = 5f;

    private Vector2 movement;
    private int lastKnownLives;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        AudioSource[] audioSources = GetComponents<AudioSource>();

        if (audioSources.Length > 0)
        {
            coinAudioSource = audioSources[0];
        }

        if (audioSources.Length > 1)
        {
            damageAudioSource = audioSources[1];
        }

        if (rb != null)
        {
            rb.freezeRotation = true;
        }

        lastKnownLives = GameController.currentLives;
    }

    void Update()
    {
        if (GameController.currentLives < lastKnownLives)
        {
            if (damageAudioSource != null)
            {
                damageAudioSource.Play();
            }
        }

        lastKnownLives = GameController.currentLives;

        if (GameController.gameOver)
        {
            movement = Vector2.zero;

            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }

            return;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        if (animator != null)
        {
            animator.SetBool("isMoving", movement != Vector2.zero);
        }

        if (movement != Vector2.zero)
        {
            UpdateFacing(moveHorizontal, moveVertical);
        }
    }

    void FixedUpdate()
    {
        if (GameController.gameOver)
            return;

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void UpdateFacing(float moveHorizontal, float moveVertical)
    {
        if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical))
        {
            if (moveHorizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f); // direita
            }
            else if (moveHorizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180f); // esquerda
            }
        }
        else
        {
            if (moveVertical > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90f); // cima
            }
            else if (moveVertical < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90f); // baixo
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameController.gameOver)
            return;

        if (other.CompareTag("Coletavel"))
        {
            if (coinAudioSource != null)
            {
                coinAudioSource.Play();
            }

            GameController.Collect();
            Destroy(other.gameObject);
        }
    }
}
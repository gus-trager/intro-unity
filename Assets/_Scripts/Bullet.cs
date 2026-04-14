using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;

    private Vector2 direction;
    private float speed;

    public void Initialize(Vector2 newDirection, float newSpeed)
    {
        direction = newDirection.normalized;
        speed = newSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (GameController.gameOver)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameController.gameOver)
            return;

        if (other.CompareTag("Player"))
        {
            GameController.TakeDamage(1);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Enemy") || other.CompareTag("Coletavel"))
            return;

        Destroy(gameObject);
    }
}
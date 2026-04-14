using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 2f;

    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (GameController.gameOver)
            return;

        if (player == null)
            return;

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = player.position;

        Vector2 direction = (targetPosition - currentPosition).normalized;

        UpdateFacing(direction.x, direction.y);

        Vector2 newPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            speed * Time.deltaTime
        );

        transform.position = newPosition;
    }

    void UpdateFacing(float dirX, float dirY)
    {
        if (Mathf.Abs(dirX) > Mathf.Abs(dirY))
        {
            if (dirX > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (dirX < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
        }
        else
        {
            if (dirY > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (dirY < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
        }
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameController.gameOver)
            return;

        if (other.CompareTag("Player"))
        {
            GameController.TakeDamage(1);
        }
    }
}
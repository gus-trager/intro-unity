using UnityEngine;

public class StationaryShooter : MonoBehaviour
{
    public GameObject bulletPrefab;

    [Header("Sprites")]
    public Sprite pistolSprite;
    public Sprite akSprite;

    [Header("Configuração inicial")]
    public float fireInterval = 1.2f;
    public float bulletSpeed = 8f;
    public float spawnDistance = 0.7f;

    [Header("Modo Pistola")]
    public float pistolFireInterval = 1.2f;
    public float pistolBulletSpeed = 8f;

    [Header("Modo AK")]
    public float akFireInterval = 0.45f;
    public float akBulletSpeed = 10f;

    [Header("Rotação")]
    public float rotationOffset = 0f;

    private Transform player;
    private float fireTimer;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        fireTimer = fireInterval;
    }

    void OnEnable()
    {
        fireTimer = fireInterval;
    }

    void Update()
    {
        if (GameController.gameOver)
            return;

        if (player == null || bulletPrefab == null)
            return;

        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

        RotateTowardsPlayer(direction);

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot(direction);
            fireTimer = fireInterval;
        }
    }

    void RotateTowardsPlayer(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }

    void Shoot(Vector2 direction)
    {
        Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;

        GameObject bulletObject = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Initialize(direction, bulletSpeed);
        }
    }

    public void SetPistolMode()
    {
        fireInterval = pistolFireInterval;
        bulletSpeed = pistolBulletSpeed;

        if (spriteRenderer != null && pistolSprite != null)
        {
            spriteRenderer.sprite = pistolSprite;
        }

        fireTimer = fireInterval;
    }

    public void SetAKMode()
    {
        fireInterval = akFireInterval;
        bulletSpeed = akBulletSpeed;

        if (spriteRenderer != null && akSprite != null)
        {
            spriteRenderer.sprite = akSprite;
        }

        fireTimer = fireInterval;
    }
}
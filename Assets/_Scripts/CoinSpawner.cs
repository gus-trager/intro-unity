using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public EnemyChase[] enemies;
    public Transform player;

    [Header("Shooter")]
    public StationaryShooter shooterEnemy;

    [Header("Quantidade e dificuldade")]
    public int coinsPerWave = 4;
    public float speedIncreasePerWave = 0.5f;

    [Header("Área de spawn")]
    public Vector2 spawnMin;
    public Vector2 spawnMax;

    [Header("Segurança do spawn")]
    public float minDistanceFromPlayer = 2f;
    public float minDistanceFromEnemies = 1.5f;
    public int maxSpawnTries = 30;

    private int completedWaves = 0;

    void Start()
    {
        GameController.Init();

        if (shooterEnemy != null)
        {
            shooterEnemy.gameObject.SetActive(false);
        }

        SpawnWave();
    }

    void Update()
    {
        if (GameController.gameOver)
            return;

        if (GameController.IsWaveComplete())
        {
            completedWaves++;

            IncreaseEnemySpeed();
            HandleShooterProgression();
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        if (coinPrefab == null)
            return;

        int spawned = 0;
        int tries = 0;

        while (spawned < coinsPerWave && tries < coinsPerWave * maxSpawnTries)
        {
            tries++;

            Vector2 randomPosition = new Vector2(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y)
            );

            if (!IsPositionValid(randomPosition))
                continue;

            Instantiate(coinPrefab, randomPosition, Quaternion.identity);
            spawned++;
        }

        GameController.SetActiveCoins(spawned);
    }

    bool IsPositionValid(Vector2 position)
    {
        if (player != null)
        {
            if (Vector2.Distance(position, player.position) < minDistanceFromPlayer)
                return false;
        }

        if (enemies != null)
        {
            foreach (EnemyChase enemy in enemies)
            {
                if (enemy != null)
                {
                    if (Vector2.Distance(position, enemy.transform.position) < minDistanceFromEnemies)
                        return false;
                }
            }
        }

        if (shooterEnemy != null && shooterEnemy.gameObject.activeInHierarchy)
        {
            if (Vector2.Distance(position, shooterEnemy.transform.position) < minDistanceFromEnemies)
                return false;
        }

        Collider2D hit = Physics2D.OverlapCircle(position, 0.3f);
        if (hit != null)
        {
            if (!hit.CompareTag("Player") && !hit.CompareTag("Enemy"))
            {
                return false;
            }
        }

        return true;
    }

    void IncreaseEnemySpeed()
    {
        foreach (EnemyChase enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.IncreaseSpeed(speedIncreasePerWave);
            }
        }
    }

    void HandleShooterProgression()
    {
        if (shooterEnemy == null)
            return;

        if (completedWaves == 1)
        {
            shooterEnemy.gameObject.SetActive(true);
            shooterEnemy.SetPistolMode();
        }

        if (completedWaves == 2)
        {
            shooterEnemy.SetAKMode();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(
            (spawnMin.x + spawnMax.x) / 2f,
            (spawnMin.y + spawnMax.y) / 2f,
            0f
        );
        Vector3 size = new Vector3(
            spawnMax.x - spawnMin.x,
            spawnMax.y - spawnMin.y,
            0f
        );

        Gizmos.DrawWireCube(center, size);
    }
}
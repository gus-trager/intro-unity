using UnityEngine;

public static class GameController
{
    public static float tempoInicial = 30f;
    public static float tempoRestante;
    public static float tempoSobrevivido;

    public static int score;
    public static int finalScore;

    public static int scorePorMoeda = 10;
    public static float bonusTempoPorMoeda = 5f;

    public static int maxLives = 3;
    public static int currentLives;

    public static float damageInvulnerabilityDuration = 0.7f;
    private static float damageCooldownRemaining;

    private static int moedasAtivas;

    public static bool gameOver { get; private set; }

    public static void Init()
    {
        tempoRestante = tempoInicial;
        tempoSobrevivido = 0f;
        score = 0;
        finalScore = 0;
        moedasAtivas = 0;
        currentLives = maxLives;
        damageCooldownRemaining = 0f;
        gameOver = false;
    }

    public static void UpdateTime(float deltaTime)
    {
        if (gameOver)
            return;

        if (damageCooldownRemaining > 0f)
        {
            damageCooldownRemaining -= deltaTime;
        }

        tempoSobrevivido += deltaTime;
        tempoRestante -= deltaTime;

        if (tempoRestante <= 0f)
        {
            tempoRestante = 0f;
            EndGame();
        }
    }

    public static void SetActiveCoins(int quantidade)
    {
        moedasAtivas = quantidade;
    }

    public static void Collect()
    {
        if (gameOver)
            return;

        score += scorePorMoeda;
        tempoRestante += bonusTempoPorMoeda;

        moedasAtivas--;

        if (moedasAtivas < 0)
        {
            moedasAtivas = 0;
        }
    }

    public static bool IsWaveComplete()
    {
        return moedasAtivas <= 0;
    }

    public static void TakeDamage(int amount = 1)
    {
        if (gameOver)
            return;

        if (damageCooldownRemaining > 0f)
            return;

        currentLives -= amount;

        if (currentLives < 0)
        {
            currentLives = 0;
        }

        damageCooldownRemaining = damageInvulnerabilityDuration;

        if (currentLives <= 0)
        {
            EndGame();
        }
    }

    public static void DefeatByEnemy()
    {
        TakeDamage(1);
    }

    private static void EndGame()
    {
        gameOver = true;
        finalScore = score;
    }
}
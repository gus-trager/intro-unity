using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;

    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text finalTimeText;

    public Image[] heartImages;

    private bool panelShown = false;

    void Start()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }

        UpdateHearts();
    }

    void Update()
    {
        if (!GameController.gameOver)
        {
            GameController.UpdateTime(Time.deltaTime);

            if (timerText != null)
            {
                timerText.text = "Tempo: " + GameController.tempoRestante.ToString("F1") + "s";
            }

            if (scoreText != null)
            {
                scoreText.text = "Score: " + GameController.score;
            }

            UpdateHearts();
            return;
        }

        if (!panelShown)
        {
            panelShown = true;

            if (timerText != null)
            {
                timerText.gameObject.SetActive(false);
            }

            if (scoreText != null)
            {
                scoreText.gameObject.SetActive(false);
            }

            HideHearts();

            if (endGamePanel != null)
            {
                endGamePanel.SetActive(true);
            }

            if (finalScoreText != null)
            {
                finalScoreText.text = "Score Final: " + GameController.finalScore;
            }

            if (finalTimeText != null)
            {
                finalTimeText.text = "Tempo sobrevivido: " + GameController.tempoSobrevivido.ToString("F1") + "s";
            }
        }
    }

    void UpdateHearts()
    {
        if (heartImages == null)
            return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                heartImages[i].gameObject.SetActive(i < GameController.currentLives);
            }
        }
    }

    void HideHearts()
    {
        if (heartImages == null)
            return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
            {
                heartImages[i].gameObject.SetActive(false);
            }
        }
    }
}
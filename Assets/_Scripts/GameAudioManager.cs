using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip gameOverClip;

    private bool gameOverHandled = false;

    void Update()
    {
        if (!gameOverHandled && GameController.gameOver)
        {
            gameOverHandled = true;

            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.Stop();
            }

            if (sfxSource != null && gameOverClip != null)
            {
                sfxSource.PlayOneShot(gameOverClip);
            }
        }
    }
}
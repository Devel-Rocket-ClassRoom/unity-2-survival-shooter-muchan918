using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public CanvasGroup gameOverUI;
    private int score = 0;
    public bool IsGameOver { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = $"Score: {score}";
        gameOverUI.gameObject.SetActive(false);
    }

    public void AddScore(int add)
    {
        if (IsGameOver)
            return;

        score += add;
        scoreText.text = $"Score: {score}";
    }

    public void SetActiveGameOverUI(bool active)
    {
        if (active)
        {
            gameOverUI.enabled = true;
            StartCoroutine(FadeIn());
        }
        else
        {
            gameOverUI.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeIn()
    {
        gameOverUI.alpha = 0f;
        gameOverUI.gameObject.SetActive(true);

        while (gameOverUI.alpha < 1f)
        {
            gameOverUI.alpha += Time.deltaTime/2;  
            yield return null;
        }

        gameOverUI.alpha = 1f;
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject gameOverScreen;

    private Text _scoreText;
    private int _score;

    private void Start()
    {
        _scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
        _score = 0;
    }

    public void GameOver() {
        gameOverScreen.SetActive(true);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IncreaseScore(int toAdd)
    {
        _score += toAdd;
        _scoreText.text = _score.ToString();
    }
}

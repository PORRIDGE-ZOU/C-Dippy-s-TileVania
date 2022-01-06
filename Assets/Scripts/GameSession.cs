using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 10;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;


    int coinScore = 0000;

    void Awake()
    {

        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = coinScore.ToString();
    }

    public void ProcessPlayerDeath()
    {

        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSessions();
        }

    }

    public void PickUpOneCoin(int pointForOneCoin)
    {
        coinScore += pointForOneCoin;
        scoreText.text = coinScore.ToString();
    }

    void TakeLife()
    {
        playerLives -= 1;
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current);
        livesText.text = playerLives.ToString();
    }

    void ResetGameSessions()
    {
        FindObjectOfType<ScenePersist>().ResetGamePersist();
        coinScore = 0000;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
        scoreText.text = coinScore.ToString();
    }
}

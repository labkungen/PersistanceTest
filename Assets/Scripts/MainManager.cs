using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text highscoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        UpdateHighscoreText();



        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    void UpdateHighscoreText()
    {
        int currentHighscore =  GameManager.instance.highscores[0];
        string currentHighscoreName = GameManager.instance.highscoreNames[0];

        if (m_Points > currentHighscore)
        {
            currentHighscore = m_Points;
            currentHighscoreName = GameManager.instance.playerName;
        }

        highscoreText.text = "Best Score: " + currentHighscoreName + " : " + currentHighscore;
    }


    void UpdateHighScoreData()
    {
        //check current game vs highscores
        int[] highscores = GameManager.instance.highscores;

        if (m_Points > highscores[0])
        {
            Debug.Log("Current highscore");
            //pushdown current
            GameManager.instance.highscoreNames[1] = GameManager.instance.highscoreNames[0];
            GameManager.instance.highscores[1] = GameManager.instance.highscores[0];

            //set new high
            GameManager.instance.highscoreNames[0] = GameManager.instance.playerName;
            GameManager.instance.highscores[0] = m_Points;
        }
        else if (m_Points > highscores[1])
        {
            Debug.Log("Beat 2nd highscore");
            //pushdown current
            GameManager.instance.highscoreNames[2] = GameManager.instance.highscoreNames[1];
            GameManager.instance.highscores[2] = GameManager.instance.highscores[1];

            //set new high
            GameManager.instance.highscoreNames[1] = GameManager.instance.playerName;
            GameManager.instance.highscores[1] = m_Points;
        }
        else if (m_Points > highscores[2])
        {
            Debug.Log("Beat 3rd highscore");
            GameManager.instance.highscoreNames[2] = GameManager.instance.playerName;
            GameManager.instance.highscores[2] = m_Points;
        }

        GameManager.instance.SaveHighscores();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        UpdateHighscoreText();
    }

    public void GameOver()
    {
        UpdateHighScoreData();
        string highscores = "\nHighscores"+"\n---------"+"\n1. " + GameManager.instance.highscoreNames[0] + " : " + GameManager.instance.highscores[0]
            + "\n2. " + GameManager.instance.highscoreNames[1] + " : " + GameManager.instance.highscores[1]
            + "\n3. " + GameManager.instance.highscoreNames[2] + " : " + GameManager.instance.highscores[2];

        string gameOverText = "GAME OVER\n" + highscores + "\nPress Space to Restart (Ecs > menu)";

        GameOverText.gameObject.GetComponent<Text>().text = gameOverText;

        m_GameOver = true;
        GameOverText.SetActive(true);
        
    }
}

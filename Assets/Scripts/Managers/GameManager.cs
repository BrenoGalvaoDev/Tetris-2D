using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int height = 20;
    public static int width = 10;

    public ScriptableObjects scriptableObject;

    public int scoreDifficulty;
    public float difficulty = 1;

    public bool isGameOver;
    public bool isPauseGame = false;

    public int score = 0;

    public Text currentScoreText;
    public Text lastScoreText;
    public Text maxScoreText;

    public GameObject gameOverPanel;
    public GameObject pauseGamePanel;

    public List<GameObject> backgrounds;

    #region Audio References
    public List<AudioClip> audioList = new List<AudioClip>();
    public AudioSource audioSource;
    public float volumeSound;
    public Slider soundSlider;

    public AudioSource deleteSound;
    #endregion

    public static Transform[,] grid = new Transform[width, height];

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            score = 0;
        }

        backgrounds[Random.Range(0, backgrounds.Count - 1)].SetActive(true);
        PlaySound();
        if (PlayerPrefs.GetFloat("Volume") > 0)
        {
            audioSource.volume = PlayerPrefs.GetFloat("Volume");
            soundSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            audioSource.volume = 1;
        }
    }

    private void Update()
    {
        UpdateScore();

        if(scoreDifficulty > 300)
        {
            scoreDifficulty -= 300;
            difficulty += 0.5f;
        }

        pauseGamePanel.SetActive(isPauseGame);
        if (!audioSource.isPlaying)
        {
            PlaySound();
        }
    }

    #region Grid Manager

    public bool InsideTheGrid(Vector2 position)
    {
        return ((int)position.x >= 0 && (int)position.x < width && (int)position.y >= 1);
    }

    public Vector2 RoundNumber(Vector2 number)
    {
        return new Vector2(Mathf.Round(number.x), Mathf.Round(number.y));
    }

    public void UpdateGrid(PartsMovement partTetro)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    if(grid[x, y].parent == partTetro.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach(Transform piece in partTetro.transform)
        {
            Vector2 position = RoundNumber(piece.position);
            if(position.y < height)
            {
                grid[(int)position.x, (int)position.y] = piece;
            }
        }
    }

    public Transform PositionTransformGrid(Vector2 position)
    {
        if(position.y > height - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)position.x, (int)position.y];
        }
    }

    public bool FullLine(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void DestroySquare(int y)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);

            grid[x, y] = null;
        }
    }

    public void MoveLineBellow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if(grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllLine(int y)
    {
        for(int i = y; i < height; i++)
        {
            MoveLineBellow(i);
        }
    }

    public void DeleteLine()
    {
        for(int y = 0;  y < height; y++)
        {
            if (FullLine(y))
            {
                DestroySquare(y);
                MoveAllLine(y + 1);
                y--;
                score += 100;
                scoreDifficulty += 100;
                deleteSound.Play();
            }
        }
    }

    public bool AboveGrid(PartsMovement tetroPiece)
    {
        for(int x = 0; x < width; x++)
        {
            foreach(Transform square in tetroPiece.transform)
            {
                Vector2 position = RoundNumber(square.position);
                if(position.y > height - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

    public void UpdateScore()
    {
        currentScoreText.text = scriptableObject.currentScore.ToString();
        lastScoreText.text = scriptableObject.lastScore.ToString();
        maxScoreText.text = scriptableObject.maxScore.ToString();
        
        scriptableObject.currentScore = score;

        if(score > scriptableObject.maxScore)
        {
            scriptableObject.maxScore = score;
            PlayerPrefs.SetFloat("MaxScore", scriptableObject.maxScore);
        }
        if (isGameOver)
        {
            GameOver();
            scriptableObject.lastScore = score;
            scriptableObject.currentScore = 0;
            PlayerPrefs.SetFloat("LastScore", scriptableObject.lastScore);
        }
    }

    #region Functions
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void PauseGame()
    {
        isPauseGame = !isPauseGame;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        scriptableObject.lastScore = score;
        scriptableObject.currentScore = 0;
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
        scriptableObject.lastScore = score;
        scriptableObject.currentScore = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
        scriptableObject.lastScore = score;
        scriptableObject.currentScore = 0;
    }

    public void PlaySound()
    {
        audioSource.clip = audioList[Random.Range(0, audioList.Count - 1)];
        audioSource.Play();
    }

    public void SoundVolume()
    {
        volumeSound = soundSlider.value;
        audioSource.volume = volumeSound;
        PlayerPrefs.SetFloat("Volume", volumeSound);
    }

    #endregion
}
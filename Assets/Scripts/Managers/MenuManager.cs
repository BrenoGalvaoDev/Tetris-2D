using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public ScriptableObjects scriptableObjects;

    public Text maxScoreText;
    public Text lastScoreText;

    public List<AudioClip> audioList = new List<AudioClip>();
    public AudioSource audioSource;
    public float volumeSound;
    public Slider soundSlider;
    
    private void Start()
    {
        if(PlayerPrefs.GetFloat("MaxScore") > 0)
        {
            scriptableObjects.maxScore = PlayerPrefs.GetFloat("MaxScore");
            scriptableObjects.lastScore = PlayerPrefs.GetFloat("LastScore");
        }
        maxScoreText.text = scriptableObjects.maxScore.ToString();
        lastScoreText.text = scriptableObjects.lastScore.ToString();
        PlaySound();
        if(PlayerPrefs.GetFloat("Volume") > 0)
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
        if (!audioSource.isPlaying)
        {
            PlaySound();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
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
}

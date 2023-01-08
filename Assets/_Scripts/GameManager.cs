using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class GameManager : Singleton<GameManager>
{
    public bool playing = true; 

    public Transform startPosition;
    public TextMeshProUGUI objectiveText; 
    public bool AllowInput { get { return !paused && playing; } }
    public bool paused; 

    public GameObject player;
    public GameObject pause;
    public GameObject gameOver;
    public GameObject ending;
    public TextMeshProUGUI endingText; 

    public PlayerController playerController;

    public float timeInRange = 0f;

    public CanvasGroup endTitleGroup;

    public TextMeshProUGUI tutorialText;

    public bool GameOver;

    public AudioSource globalSource;
    public AudioClip playerDeathClip;

    public CombineHarvester combineHarvester;

    public Material retroShader; 

    public IEnumerator Start()
    {
        player.SetActive(true); 
        pause.SetActive(false);
        gameOver.SetActive(false);
        ending.SetActive(false);

        playing = false;
        yield return new WaitForSeconds(1f);
        ScreenFader.Instance.Fade(0, 2f);
        yield return new WaitForSeconds(2f);
        playing = true;

        StartCoroutine(GameRoutine());
    }

    public IEnumerator GameRoutine()
    {
        while (true)
        {
            yield return null; 
        }

        yield return new WaitForSeconds(4f);

        //Return to main menu
        SceneManager.LoadScene("Menu");
    }

    public void Pause()
    {
        if (playing && !GameOver)
        {
            if (!paused)
            {
                paused = true;
                Time.timeScale = 0f;
            }
            else
            {
                paused = false;
                Time.timeScale = 1f;
            }

            player.SetActive(!paused);
            pause.SetActive(paused);
        }
    }

    [Button]
    public void KillPlayer()
    {
        GameOver = true; 
        player.SetActive(false);
        pause.SetActive(false);
        gameOver.SetActive(true); 

        globalSource.PlayOneShot(playerDeathClip);

        //AudioManager.Instance.SnapAudioClose(4); 
    }
     
    public void TriggerEnding(int endingNumber) 
    {
        combineHarvester.PowerDown();

        switch (endingNumber)
        {
            case 1:
                endingText.text = "ENDING 1/3 - NOT TODAY";
                break;
            case 2:
                endingText.text = "ENDING 2/3 - ESCAPE";
                break;
            case 3:
                endingText.text = "ENDING 3/3 - DESTROY";
                break;
            default:
                endingText.text = "ENDING ?/3 - THIS ISN'T SUPPOSED TO EXIST?";
                break;
        }
          

        GameOver = true; 
        player.SetActive(false);
        pause.SetActive(false);
        gameOver.SetActive(false);
        ending.SetActive(true);

        ScreenFader.Instance.Fade(1, 3f);  
    }

    public void Update() 
    { 
        retroShader.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}

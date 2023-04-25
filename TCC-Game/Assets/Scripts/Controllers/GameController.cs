using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //AUDIO
    [field: SerializeField] private AudioSource _audioSource { get; set; }

    //DIFFICULTY
    [field: SerializeField] public int Difficult { get; set; }

    [field: SerializeField] public int AddDifficult { get; set; }
    public static GameController GameControllerInstance { get; private set; }

    private bool _isPaused { get; set; }

    private void Awake()
    {
        if (GameControllerInstance != null)
        {
            Debug.LogWarning("Mais de um DungeonController na Cena");
            Destroy(gameObject);
        }
        else
        {
            GameControllerInstance = this;
            DontDestroyOnLoad(GameControllerInstance);
        }

    }

    private void Start()
    {
        _isPaused = false;
    }
    public void PlayBGM(AudioClip audio)
    {
        _audioSource.clip = audio;
        _audioSource.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("Ui"));
    }

    public void PauseGame()
    {
        if (_isPaused)
        {
            Time.timeScale = 1;
            _isPaused = false;
            UiController.UiInstance.MenuBtnOff();
        }
        else
        {
            Time.timeScale = 0;
            _isPaused = true;
            UiController.UiInstance.MenuBtnOn();
        }

    }

}

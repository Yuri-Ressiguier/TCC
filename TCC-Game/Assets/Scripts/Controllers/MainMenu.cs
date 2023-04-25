using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameController.GameControllerInstance.StartGame();
    }

    public void QuitGame()
    {
        GameController.GameControllerInstance.QuitGame();
    }
}

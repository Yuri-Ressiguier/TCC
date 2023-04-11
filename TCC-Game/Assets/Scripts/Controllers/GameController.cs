using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [field: SerializeField] public int Difficult { get; set; }

    [field: SerializeField] public int AddDifficult { get; set; }
    public static GameController GameControllerInstance { get; private set; }

    private void Awake()
    {
        if (GameControllerInstance != null)
        {
            Debug.LogWarning("Mais de um DungeonController na Cena");
        }
        GameControllerInstance = this;
    }

}

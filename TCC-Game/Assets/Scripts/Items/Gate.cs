using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    [field: SerializeField] public int ToScene { get; set; }
    [field: SerializeField] public Vector2 PlayerPoint { get; set; }

    public Vector2 NextMap()
    {
        SceneManager.LoadScene(ToScene);
        return PlayerPoint;
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Music : MonoBehaviour
{
    [field: SerializeField] private AudioClip _bgmMusic;
    private GameController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = FindObjectOfType<GameController>();
        _controller.PlayBGM(_bgmMusic);
    }


}

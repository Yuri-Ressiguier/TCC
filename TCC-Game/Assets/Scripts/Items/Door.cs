using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [field: SerializeField] private Sprite _doorOpened { get; set; }
    [field: SerializeField] private GameObject _nextRoom { get; set; }
    [field: SerializeField] private GameObject _actualRoom { get; set; }
    [field: SerializeField] public Vector2 PlayerPoint { get; set; }

    private Collider2D _doorCollider;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _doorCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Vector2 NexRoom()
    {
        _nextRoom.SetActive(true);
        _actualRoom.SetActive(false);
        return PlayerPoint;
    }

    public void Open()
    {
        _doorCollider.isTrigger = true;
        _spriteRenderer.sprite = _doorOpened;
    }

    
}

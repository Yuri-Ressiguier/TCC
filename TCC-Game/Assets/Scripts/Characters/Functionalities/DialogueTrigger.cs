using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [field: SerializeField] private GameObject _visualCue { get; set; }

    [field: SerializeField] private TextAsset _inkJson { get; set; }

    [field: SerializeField] private bool _playerInRange;


    private void Awake()
    {
        _playerInRange = false;
        _visualCue.SetActive(false);
    }

    private void Update()
    {
        if (_playerInRange)
        {
            _visualCue.SetActive(true);

        }
        else
        {
            _visualCue.SetActive(false);
        }
    }

    public void Dialogue(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            Debug.Log(_playerInRange);
            if (_playerInRange)
            {
                
                DialogueManager.DialogueInstance.EnterDialogueMode(_inkJson);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("entrou");
            _playerInRange = true;
            UiController.UiInstance.BtnInteract.gameObject.SetActive(true);
            UiController.UiInstance.BtnAttack.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("SAIU");
            _playerInRange = false;
            UiController.UiInstance.BtnInteract.gameObject.SetActive(false);
            UiController.UiInstance.BtnAttack.gameObject.SetActive(true);
        }
    }
}

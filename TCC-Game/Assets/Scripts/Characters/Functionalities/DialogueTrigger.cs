using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [field: SerializeField] private GameObject _visualCue { get; set; }

    [field: SerializeField] private TextAsset _inkJson { get; set; }

    [field: SerializeField] private bool _speaker { get; set; }

    protected bool _canDialogue { get; set; }



    private void Awake()
    {
        _visualCue.SetActive(false);
        _canDialogue = true;
    }



    public virtual void Dialogue()
    {
        if (_canDialogue || _speaker)
        {
            _canDialogue = DialogueController.DialogueInstance.EnterDialogueMode(_inkJson);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && (_canDialogue || _speaker))
        {

            _visualCue.SetActive(true);
            UiController.UiInstance.BtnInteract.gameObject.SetActive(true);
            UiController.UiInstance.BtnAttack.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            UiController.UiInstance.BtnInteract.gameObject.SetActive(false);
            UiController.UiInstance.BtnAttack.gameObject.SetActive(true);
            _visualCue.SetActive(false);
        }
    }
}

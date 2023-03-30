using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    private Story _currentStory;
    public bool DialogueIsPlaying;

    public static DialogueManager DialogueInstance { get; private set; }

    private void Awake()
    {
        if (DialogueInstance != null)
        {
            Debug.LogWarning("Mais de um Dialogue Manager na Cena");
        }
        DialogueInstance = this;
    }

    private void Start()
    {
        UiController.UiInstance.DialoguePanel.SetActive(false);
        DialogueIsPlaying = false;
    }


    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (!DialogueIsPlaying)
        {
            _currentStory = new Story(inkJSON.text);
            DialogueIsPlaying = true;
            UiController.UiInstance.DialoguePanel.SetActive(true);
        }


        if (_currentStory.canContinue)
        {
            UiController.UiInstance.DialogueText.text = _currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void ExitDialogueMode()
    {
        UiController.UiInstance.DialoguePanel.SetActive(false);
        DialogueIsPlaying = false;
        UiController.UiInstance.DialogueText.text = "";
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            Debug.Log(choice.text + " - " + choice.index);
            UiController.UiInstance.BtnChoices[index].gameObject.SetActive(true);
            UiController.UiInstance.TxtChoices[index].text = choice.text;
            index++;
        }

    }


    public void MakeChoice(int choiceIndex)
    {

        Debug.Log(choiceIndex);
        _currentStory.ChooseChoiceIndex(choiceIndex);
        if (_currentStory.canContinue)
        {
            UiController.UiInstance.DialogueText.text = _currentStory.Continue();
            foreach (var item in UiController.UiInstance.BtnChoices)
            {
                item.gameObject.SetActive(false);
            }
        }

    }
}

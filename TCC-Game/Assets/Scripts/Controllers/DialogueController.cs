using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Linq;

public class DialogueController : MonoBehaviour
{
    private Story _currentStory;
    public bool DialogueIsPlaying;

    public static DialogueController DialogueInstance { get; private set; }

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


    public bool EnterDialogueMode(TextAsset inkJSON)
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
            return true;
        }
        else
        {
            ExitDialogueMode();
            return false;
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
        UiController.UiInstance.BtnInteract.gameObject.SetActive(false);

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            UiController.UiInstance.BtnChoices[index].gameObject.SetActive(true);
            UiController.UiInstance.TxtChoices[index].text = choice.text;
            index++;
        }

    }


    public void MakeChoice(int choiceIndex)
    {
        UiController.UiInstance.BtnInteract.gameObject.SetActive(true);
        _currentStory.ChooseChoiceIndex(choiceIndex);
        if (_currentStory.canContinue)
        {
            string txt = _currentStory.Continue();
            UiController.UiInstance.DialogueText.text = txt;
            string[] arr = txt.Split(" ");
            string str = new string(arr[arr.Length - 1].Where(c => char.IsLetter(c)).ToArray());
            if (str.Equals("fortes"))
            {
                GameController.GameControllerInstance.Difficult += GameController.GameControllerInstance.AddDifficult;
            }
            else if (str.Equals("fracos"))
            {
                GameController.GameControllerInstance.Difficult -= GameController.GameControllerInstance.AddDifficult;
            }
            foreach (var item in UiController.UiInstance.BtnChoices)
            {
                item.gameObject.SetActive(false);
            }
        }

    }
}

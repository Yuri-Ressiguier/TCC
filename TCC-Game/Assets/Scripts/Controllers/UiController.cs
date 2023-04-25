using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    //UI

    //TXT
    [field: SerializeField] public TextMeshProUGUI TxtCoins { get; set; }
    [field: SerializeField] public TextMeshProUGUI TxtHealthPotions { get; set; }
    [field: SerializeField] public TextMeshProUGUI TxtEnergyWarning { get; set; }
    [field: SerializeField] public TextMeshProUGUI TxtLvl { get; set; }
    [field: SerializeField] public TextMeshProUGUI DialogueText { get; set; }
    [field: SerializeField] public TextMeshProUGUI[] TxtChoices { get; set; }

    //BTN
    [field: SerializeField] public Button BtnHealthPotion { get; set; }
    [field: SerializeField] public Button BtnChange { get; set; }
    [field: SerializeField] public Button BtnDefense { get; set; }
    [field: SerializeField] public Button BtnInterrupt { get; set; }
    [field: SerializeField] public Button BtnAttack { get; set; }
    [field: SerializeField] public Button BtnInteract { get; set; }
    [field: SerializeField] public GameObject[] BtnChoices { get; set; }
    [field: SerializeField] public Button BtnMenu { get; set; }

    //Outros
    [field: SerializeField] private Sprite ImgMelee { get; set; }
    [field: SerializeField] private Sprite ImgRanged { get; set; }
    [field: SerializeField] public Image LifeBar { get; set; }
    [field: SerializeField] public Image EnergyBar { get; set; }
    [field: SerializeField] public Image ExpBar { get; set; }
    [field: SerializeField] public GameObject DialoguePanel { get; set; }

    public static UiController UiInstance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (UiInstance == null)
        {
            Debug.Log("UI É Nula");
            UiInstance = this;
        }
        else
        {
            Debug.Log("UI É Destruida");
            Destroy(gameObject);
        }
        //GCInstance = this;

        BtnInteract.gameObject.SetActive(false);
    }

    private void Start()
    {
        ExpBar.fillAmount = 0;
        TxtChoices = new TextMeshProUGUI[BtnChoices.Length];
        int index = 0;
        foreach (GameObject choice in BtnChoices)
        {
            TxtChoices[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        BtnMenu.gameObject.SetActive(false);
    }

    public void ChangeAttackImg(bool melee)
    {
        if (melee)
        {
            BtnAttack.GetComponent<Image>().sprite = ImgMelee;
        }
        else
        {
            BtnAttack.GetComponent<Image>().sprite = ImgRanged;
        }

    }

    public void EnergyWarning()
    {
        TxtEnergyWarning.alpha = 255;
        StartCoroutine("FadeOutDelay");
    }

    //COroutines 

    IEnumerator FadeOutDelay()
    {
        yield return new WaitForSeconds(1);
        TxtEnergyWarning.alpha = 0;
    }

    public void MenuBtnOn()
    {
        BtnMenu.gameObject.SetActive(true);
    }

    public void MenuBtnOff()
    {
        BtnMenu.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        GameController.GameControllerInstance.BackToMenu();
    }
}

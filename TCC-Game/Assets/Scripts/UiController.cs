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

    //BTN
    [field: SerializeField] public Button BtnHealthPotion { get; set; }
    [field: SerializeField] public Button BtnChange { get; set; }
    [field: SerializeField] public Button BtnDefense { get; set; }
    [field: SerializeField] public Button BtnInterrupt { get; set; }
    [field: SerializeField] public Button BtnAttack { get; set; }

    //Outros
    [field: SerializeField] private Sprite ImgMelee { get; set; }
    [field: SerializeField] private Sprite ImgRanged { get; set; }
    [field: SerializeField] public Image LifeBar { get; set; }
    [field: SerializeField] public Image EnergyBar { get; set; }
    [field: SerializeField] public Image ExpBar { get; set; }

    public static UiController UiInstance;

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
    }

    private void Start()
    {
        ExpBar.fillAmount = 0;
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

}

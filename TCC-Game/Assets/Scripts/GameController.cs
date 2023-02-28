using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //UI

    //TXT
    [field: SerializeField] public TextMeshProUGUI TxtCoins { get; set; }
    [field: SerializeField] public TextMeshProUGUI TxtHealthPotions { get; set; }
    [field: SerializeField] public TextMeshProUGUI TxtEnergyWarning { get; set; }

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

    public static GameController GCInstance;

    private void Awake()
    {
        GCInstance = this;
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

    public void EnergyWarming()
    {
        TxtEnergyWarning.alpha = 255;
        StartCoroutine("FadeOutDelay");
    }

    IEnumerator FadeOutDelay()
    {
        yield return new WaitForSeconds(1);
        TxtEnergyWarning.alpha = 0;
    }

}

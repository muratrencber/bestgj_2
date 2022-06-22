using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OtomatScreen : MonoBehaviour
{
    [SerializeField] GameObject selectionScreen, bakiyeScreen;
    [SerializeField] TextMeshProUGUI promptText, bakiyeText, secimText;
    Otomat otomat;

    public void Set(Otomat o){
        otomat = o;
    }
    public void Prompt(){
        Prompt(Configs.OtomatConfigs.Properties[(int)otomat.CurrentState].prompt, otomat.Deposit > 0, otomat.Deposit > 0);
    }
    void Prompt(string promptString, bool showSelection = true, bool showDeposit = true)
    {
        selectionScreen.SetActive(showSelection);
        bakiyeScreen.SetActive(showDeposit);
        if(showDeposit) bakiyeText.text = otomat.Deposit.ToString();
        if(showSelection) secimText.text = otomat.CurrentInput;

        promptText.text = promptString;
    }
}

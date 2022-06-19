using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Otomat : DayEntity
{
    [SerializeField] GameObject selectionScreen, bakiyeScreen;
    [SerializeField] TextMeshProUGUI promptText, bakiyeText, secimText; 
    [SerializeField] PlayerStorage pstrg;
    [SerializeField] Transform objectSpawnPosition;
    [SerializeField] Transform coinSpawnPosition;
    public GameObject[] items;

    string currNum = "";
    public int deposit;
    string prompText;
    private void Start()
    {
        Prompt();
    }

    void Prompt()
    {
        selectionScreen.SetActive(deposit > 0);
        bakiyeScreen.SetActive(deposit > 0);
        if(deposit > 0){
            promptText.text = "LÜTFEN SEÇİM YAPINIZ";
            bakiyeText.text = deposit.ToString();
            secimText.text = currNum;
        } else{
            promptText.text = "BAŞLAMAK İÇİN MADENİ PARA ATINIZ";
        }
    }

    public void enterLira(){
        deposit++;
        Prompt();
    }

    public void enterItem(int number)
    {
        if (currNum.Length == 0 && number == 0)
        {
            return;
        }
        currNum += number.ToString();
        prompText += number.ToString();
        Prompt();
        if (currNum.Length == 2)
        {
            GiveItem(int.Parse(currNum));
            currNum = "";
        }
    }
    void GiveItem(int no) {
        currNum = "";
        if ((no - 9) > items.Length)
        {
            Prompt();
            return;
        }
        if(items[no -10].GetComponent<Obje>().price > deposit){
            Prompt();
            return;
        }
        Obje obj = items[no -10].GetComponent<Obje>();
        GameObject instance = Instantiate(items[no-10], objectSpawnPosition);
        instance.transform.localPosition = Vector3.zero;
        instance.GetComponent<DraggableObject>().enabled = false;
        instance.GetComponent<InvObject>().enabled = true;
        instance.GetComponent<InvObject>().Set(pstrg, 1);

        deposit -= obj.price;
        Prompt();
    }
    void PromptAtScreen(string str)
    {
        Debug.Log(str);
    }
    public override void OnDayEnded()
    {
        throw new System.NotImplementedException();
    }
    public override void OnDayStarted()
    {
        throw new System.NotImplementedException();
    }


}

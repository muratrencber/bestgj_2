using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otomat : DayEntity
{
    public GameObject[] items;

    string currNum = "";
    string prompText;
    private void Start()
    {
        prompToDefault();
    }

    void prompToDefault()
    {
        prompText = "Numara Giriniz:";
    }

    public void enterItem(int number)
    {
        if (currNum.Length == 0 && number == 0)
        {
            return;
        }
        currNum += number.ToString();
        prompText += number.ToString();
        if (currNum.Length == 2)
        {
            GiveItem(int.Parse(currNum));
            currNum = "";
        }
    }
    void GiveItem(int no) {
        if ((no - 9) > items.Length)
        {
            PromptAtScreen("Geçersiz Numara");
            return;
        }
        Debug.Log(items[no-10].name);
        GiveChange();
    }
    void PromptAtScreen(string str)
    {
        Debug.Log(str);
    }
    void GiveChange() {


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

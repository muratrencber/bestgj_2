using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Automata.Modding;

[System.Serializable]
public class CursorLoadProperties{
    public string imageKey;
    public Vector2 offsets;
}

[System.Serializable]
public class DayAndMoney{
    public int day;
    public int money;
}

[System.Serializable]
public class DailyMoneyGraph : ICustomField{
    const string PREFAB_PATH = "UI/ModFields/MoneyGraph";
    const int DEFAULT_MONEY = 5;
    public bool discreteInterpolation;
    public List<DayAndMoney> data = new List<DayAndMoney>();

    public int Evaluate(int day){
        if(data.Count == 0) return DEFAULT_MONEY;
        else if (data.Count == 1) return data[0].money;
        for(int i = 0; i < data.Count - 1; i++){
            DayAndMoney left = data[i];
            DayAndMoney right = data[i];
            if(day == left.day) return left.money;
            if(day == right.day) return right.money;
            if(day > left.day && day < right.day) return Mathf.RoundToInt(GetValueInBetween(left, right, day));
        }
        return DEFAULT_MONEY;
    }

    public int GetValueInBetween(DayAndMoney left, DayAndMoney right, int targetDay){
        if(!discreteInterpolation) return left.money;
        float x1 = left.day; float x2 = right.day; float x = targetDay;
        float y1 = left.money; float y2 = right.money;
        float ratio = (x - x1) / (x2 - x1);
        float result = y1 + (ratio * (y2 - y1));
        return Mathf.RoundToInt(result);
    }

    public void Draw(InspectorCreationInfo icinf){
        UIGraph instance = GameObject.Instantiate(Resources.Load<GameObject>(PREFAB_PATH), icinf.container).GetComponent<UIGraph>();
        InspectorCreator.CreateField(icinf.ChangeContainer(instance.toggleContainer), "discreteInterpolation", this);
        InspectorCreator.CreateField(icinf.ChangeContainer(instance.itemsContainer), "data", this);
        icinf.container.GetComponent<RectTransform>().RedrawCSFUpwards();
        instance.Set(data.Select((dnm) => dnm.day).ToArray(), data.Select((dnm) => dnm.money).ToArray(), discreteInterpolation);

    }
}

[System.Serializable]
public class GameProperties : Configurable
{
    [SerializeField] int dayCount = 6;
    [SerializeField] float dayMinutes = 5;
    [SerializeField] DailyMoneyGraph moneyGraph;
    [SerializeField] List<UIPrompt.Command> startLines = new List<UIPrompt.Command>();
    [SerializeField] List<UIPrompt.Command> endLines = new List<UIPrompt.Command>();
    [SerializeField] int wonIndex, lostIndex;
    [SerializeField] CursorLoadProperties defaultCursor, travelCursor, lookCursor, interactCursor, giveMoneyCursor;

}

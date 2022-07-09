using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CursorLoadProperties{
    public string imageKey;
    public Vector2 offsets;
}

[System.Serializable]
public class GameProperties : Configurable
{
    [System.Serializable]
    struct DailyMoney{
        public int day;
        public int money;
    }
    [SerializeField] int dayCount = 6;
    [SerializeField] float dayMinutes = 5;
    [SerializeField] List<DailyMoney> dailyMoney = new List<DailyMoney>();
    [SerializeField] bool discreteInterpolation = false;
    [SerializeField] List<UIPrompt.Command> startLines = new List<UIPrompt.Command>();
    [SerializeField] List<UIPrompt.Command> endLines = new List<UIPrompt.Command>();
    [SerializeField] int wonIndex, lostIndex;
    [SerializeField] CursorLoadProperties defaultCursor, travelCursor, lookCursor, interactCursor, giveMoneyCursor;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DayEntity : MonoBehaviour
{
    public abstract void OnDayStarted();
    public abstract void OnDayEnded();
}

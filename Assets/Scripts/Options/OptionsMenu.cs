using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] protected Transform container;
    protected List<Checker> checkers = new List<Checker>();
    [SerializeField] protected bool redrawOnEnable = false;

    void Start(){
        CreateMenu();
    }

    void OnEnable(){
        if(redrawOnEnable) CreateMenu();
    }

    void Update(){
        checkers.ForEach((w) => w.Update());
    }

    protected virtual void CreateMenu(){}
}

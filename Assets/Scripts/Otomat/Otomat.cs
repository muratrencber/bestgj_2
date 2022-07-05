using UnityEngine;

public class Otomat : DayEntity
{
    public string CurrentInput {get{return currNum;}}
    public int Deposit {get{return deposit;}}
    public State CurrentState {get{return currentState;}}
    public enum State{
        NO_MONEY,
        MONEY,
        MONEY_ONE_INPUT,
        PROCESSING_INPUT,
        INVALID_NUMBER,
        NOT_ENOUGH_MONEY,
        GIVING_ITEM,
        GIVING_CHANGE
    }
    [SerializeField] protected OtomatScreen screen;
    protected State currentState = State.NO_MONEY;
    protected string currNum = "";
    public int deposit;

    private void Awake(){
        Initialize();
    }

    protected virtual void Initialize(){
        screen.Set(this);
        screen.Prompt();
    }

    public bool CanAcceptMoney(){
        switch(currentState){
            case State.NO_MONEY: return true;
            case State.MONEY: return true;
            case State.MONEY_ONE_INPUT: return true;
            case State.PROCESSING_INPUT: return false;
            case State.INVALID_NUMBER: return false;
            case State.NOT_ENOUGH_MONEY: return false;
            case State.GIVING_ITEM: return false;
            case State.GIVING_CHANGE: return false;
        }
        return false;
    }

    public bool CanAcceptInput(){
        switch(currentState){
            case State.NO_MONEY: return false;
            case State.MONEY: return true;
            case State.MONEY_ONE_INPUT: return true;
            case State.PROCESSING_INPUT: return false;
            case State.INVALID_NUMBER: return false;
            case State.NOT_ENOUGH_MONEY: return false;
            case State.GIVING_ITEM: return false;
            case State.GIVING_CHANGE: return false;
        }
        return false;
    }

    public virtual void ReceiveMoney(){}
    public virtual void PressKey(int number){}

}

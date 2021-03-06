using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOtomat : Otomat
{
    [SerializeField] Transform rowContainer, columnContainer, objectSpawnPosition, coinSpawnPosition;
    [SerializeField] Transform slider, handle;
    [SerializeField] Vector2 handleSpeed;
    [SerializeField] OtomatItemsGenerator itemsGenerator;

    protected override void Initialize(){
        itemsGenerator.Create(this);
        base.Initialize();
    }

    public override void PressKey(int number)
    {
        if(!CanAcceptInput()) return;
        currNum += number.ToString();
        if(currentState == State.MONEY) currentState = State.MONEY_ONE_INPUT;
        else if(currentState == State.MONEY_ONE_INPUT){
            StartCoroutine(GiveItem());
        }
        screen.Prompt();
    }

    IEnumerator GiveItem(){
        OtomatConfigs oc = Configs.OtomatConfigs;
        currentState = State.PROCESSING_INPUT;
        screen.Prompt();
        yield return new WaitForSeconds(oc.Properties[(int)currentState].duration);
        int number = int.Parse(currNum);
        if(number < oc.StartNumber || number >= oc.StartNumber + oc.ItemKeys.Length){
            currentState = State.INVALID_NUMBER;
            screen.Prompt();
            yield return new WaitForSeconds(oc.Properties[(int)currentState].duration);
            currNum = "";
            currentState = State.MONEY;
            screen.Prompt();
        } else {
            string selectedKey = oc.ItemKeys[number - oc.StartNumber];
            ItemConfigs.ItemProperties itemProps = Configs.ItemConfigs.ItemDictionary[selectedKey];
            if(itemProps.price > deposit){
                currentState = State.NOT_ENOUGH_MONEY;
                screen.Prompt();
                yield return new WaitForSeconds(oc.Properties[(int)currentState].duration);
                currNum = "";
                currentState = State.MONEY;
                screen.Prompt();
            } else {
                currentState = State.GIVING_ITEM;
                screen.Prompt();
                Vector2 targetPos = GetPosition(number, true);
                Vector2 durationsFirst = new Vector2(Mathf.Abs(targetPos.x - slider.transform.position.x) / handleSpeed.x,
                                                Mathf.Abs(targetPos.y - handle.transform.position.y) / handleSpeed.y);
                yield return MoveTransform(slider, targetPos, durationsFirst.x, false, true);
                yield return MoveTransform(handle, targetPos, durationsFirst.y, true, false);
                Vector2 durationsSecond = new Vector2(Mathf.Abs(objectSpawnPosition.position.x - slider.transform.position.x) / handleSpeed.x,
                                                Mathf.Abs(objectSpawnPosition.position.y - handle.transform.position.y) / handleSpeed.y);
                yield return new WaitForSeconds(1);
                yield return MoveTransform(handle, objectSpawnPosition.position, durationsSecond.y, true, false);
                yield return MoveTransform(slider, objectSpawnPosition.position, durationsSecond.x, false, true);
                Debug.Log("itemGiven");
                currentState = State.GIVING_CHANGE;
                screen.Prompt();
                yield return GiveChange();
                currNum = "";
                currentState = State.NO_MONEY;
                screen.Prompt();
            }
        }
        yield return null;
    }

    IEnumerator MoveTransform(Transform t, Vector3 targetPosition, float duration, bool limitX = false, bool limitY = false){
        Vector3 tEnd = new Vector3(limitX ? t.position.x : targetPosition.x, limitY ? t.position.y : targetPosition.y, t.position.z);
        Vector3 tStart = t.position;
        float timer = 0;
        while(timer < duration){
            timer += Time.deltaTime;
            t.position = Vector3.Lerp(tStart, tEnd, timer / duration);
            yield return null;
        }
        t.position = tEnd;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator GiveChange(){
        Debug.Log("giving change...");
        PlayerStorage.instance.AddCoin(deposit);
        deposit = 0;
        yield return null;
    }

    public Vector3 GetPosition(int number, bool centered = false) => GetPosition((number - Configs.OtomatConfigs.StartNumber)%Configs.OtomatConfigs.SizeX,
                                                                            (number - Configs.OtomatConfigs.StartNumber)/Configs.OtomatConfigs.SizeY, centered);

    public Vector3 GetPosition(int x, int y, bool centered = false){
        float xPos = columnContainer.GetChild(x).position.x;
        float yPos = rowContainer.GetChild(y).position.y;
        Vector3 finalPosition = new Vector3(xPos, yPos, columnContainer.position.z);
        if(centered){
            float height = Mathf.Abs(rowContainer.GetChild(0).position.y - rowContainer.GetChild(1).position.y);
            finalPosition += Vector3.up * height * .5f;
        }
        return finalPosition;
    }
}

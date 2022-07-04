using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPrompt : MonoBehaviour
{
    public static bool Busy {get {return isBusy;}}
    static bool isBusy;
    static UIPrompt instance;
    [System.Serializable]
    public class Command{
        public string text = "";
        public float textDuration = 1;
        public float betweenWait = 0.1f;
        public float bgFadeDuration = 0.1f;
        public float textFadeDuration = 0.1f;
        public string colorHex = "";
        public Color textColor = Color.white;
        public System.Action onFinished;
    }
    [SerializeField] Image blackness;
    [SerializeField] TextMeshProUGUI text;

    Queue<Command> commands = new Queue<Command>();

    void Awake(){
        instance = this;
    }

    public static void AddCommand(Command c){
        instance.commands.Enqueue(c);
    }

    public static void StartEvaluating(){
        if(isBusy)
            return;
        instance.StartCoroutine(instance.Evaluate());
    }

    IEnumerator Evaluate(){
        int commandCount = 0;
        text.text = "";
        isBusy = true;
        while(commands.Count > 0){
            Color finalColor = Color.black;
            Color startColor = blackness.color;
            float timer = 0;
            text.text = "";
            Command c = commands.Dequeue();
            if(commandCount == 0){
                finalColor = Color.black;
                startColor = blackness.color;
                timer = 0;
                while(timer < c.bgFadeDuration){
                    timer += Time.deltaTime;
                    blackness.color = Color.Lerp(startColor, finalColor, timer / c.bgFadeDuration);
                    yield return null;
                }
                blackness.color = finalColor;
                yield return new WaitForSeconds(c.betweenWait);
            }
            if(c.text != ""){
                finalColor = c.textColor;
                startColor = c.textColor;
                startColor.a = 0;
                text.text = c.text;
                timer = 0;
                while(timer < c.textFadeDuration){
                    timer += Time.deltaTime;
                    text.color = Color.Lerp(startColor, finalColor, timer / c.textFadeDuration);
                    yield return null;
                }
                float waitTime = 0.1f;
                yield return new WaitForSeconds(waitTime);
                startColor = finalColor;
                finalColor.a = 0;
                timer = 0;
                while(timer < c.textFadeDuration){
                    timer += Time.deltaTime;
                    text.color = Color.Lerp(startColor, finalColor, timer / c.textFadeDuration);
                    yield return null;
                }
                yield return new WaitForSeconds(c.betweenWait);
            }
            c.onFinished?.Invoke();
            if(commands.Count == 0){
                finalColor = new Color(0,0,0,0);
                startColor = blackness.color;
                timer = 0;
                while(timer < c.bgFadeDuration){
                    timer += Time.deltaTime;
                    blackness.color = Color.Lerp(startColor, finalColor, timer / c.bgFadeDuration);
                    yield return null;
                }
                blackness.color = finalColor;
            }
            commandCount++;
            yield return null;
        }
        
        isBusy = false;
    }
}

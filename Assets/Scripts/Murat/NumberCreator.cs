using UnityEngine;

public class NumberCreator : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] renderers;
    [SerializeField] int number;
    [SerializeField] bool setItself = true;

    void Awake(){
        if(setItself)
            SetForNumber(number);
    }

    public void SetForNumber(int number){
        string num = number.ToString();
        int len = Mathf.Min(num.Length, renderers.Length);
        for(int i = 0; i < len; i++){
            Sprite s = Resources.Load<Sprite>("Numbers/"+num[i]);
            renderers[i].sprite = s;
        }
    }
}

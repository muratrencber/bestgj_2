using UnityEngine;

public class NumberCreator : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] renderers;
    [SerializeField] int number;

    void Awake(){
        SetForNumber();
    }

    public void SetForNumber(){
        string num = number.ToString();
        int len = Mathf.Min(num.Length, renderers.Length);
        for(int i = 0; i < len; i++){
            Sprite s = Resources.Load<Sprite>("Numbers/"+num[i]);
            renderers[i].sprite = s;
        }
    }
}

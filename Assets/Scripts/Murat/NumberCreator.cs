using UnityEngine;

public class NumberCreator : MonoBehaviour
{
    [SerializeField] int number;
    [SerializeField] bool setItself = true;

    void Awake(){
        if(setItself)
            SetForNumber(number);
    }

    public void SetForNumber(int number, float spacing = 0) => SetForNumber(transform, number, spacing);

    public static void SetForNumber(Transform t, int number, float spacing){
        string num = number.ToString();
        GameObject[] objects = new GameObject[num.Length];
        float totalWidth = 0;
        for(int i = 0; i < num.Length; i++){
            GameObject g = t.CreateEmptyChild();
            SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("Numbers/"+num[i]);
            totalWidth += sr.bounds.extents.x;
            objects[i] = g;
        }
        totalWidth += (spacing * (objects.Length - 1));
        float startX = -totalWidth * .5f;
        for(int i = 0; i < num.Length; i++){
            GameObject o = objects[i];
            SpriteRenderer sr = o.GetComponent<SpriteRenderer>();
            o.transform.localPosition += Vector3.right * (startX + sr.bounds.extents.x);
            startX += sr.bounds.extents.x * 2 + spacing;
        }
    }
}

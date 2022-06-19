using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    public int totalTurn;
    [SerializeField]
    private GameObject cardBase1, cardBase2;
    private Vector3 cardBase1Pos, cardBase2Pos;
    [SerializeField]
    GameObject cardPrefab;
    float speed;
    void Start()
    {
        totalTurn = 0;
        speed = 10;
        cardBase1Pos = cardBase1.transform.position;
        cardBase2Pos = cardBase2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameObject.FindGameObjectWithTag("card") && totalTurn < 3) {
            StartCoroutine(GenerateCards());
            //Debug.Log(totalTurn);
        }
        
    }
    IEnumerator GenerateCards() {

        GameObject tmp1 = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        GameObject tmp2 = Instantiate(cardPrefab, transform.position, Quaternion.identity);

        while((tmp1.transform.position - cardBase1Pos).magnitude >= 0.001f && (tmp2.transform.position - cardBase2Pos).magnitude >= 0.001f) {
            tmp1.transform.position = Vector3.Lerp(tmp1.transform.position, cardBase1Pos, Time.deltaTime * speed);
            tmp2.transform.position = Vector3.Lerp(tmp2.transform.position, cardBase2Pos, Time.deltaTime * speed);
            yield return null;
        }

        int tmp1Num = tmp1.GetComponent<Card>().point;
        int tmp2Num = tmp2.GetComponent<Card>().point;

        if(tmp1Num > tmp2Num) {

            int tmp = tmp1Num;
            tmp1.GetComponent<Card>().point = tmp2Num;
            tmp2.GetComponent<Card>().point = tmp;;
        }
        
        totalTurn++;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    int totalTurn;
    [SerializeField]
    private GameObject cardBase1, cardBase2;
    [SerializeField]
    GameObject cardPrefab;
    void Start()
    {
        totalTurn = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(GenerateCards());
        }
        
    }
    IEnumerator GenerateCards() {

        GameObject tmp1 = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        

        GameObject tmp2 = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        while((tmp1.transform.position - cardBase1.transform.position).magnitude >= 0.001f && (tmp2.transform.position - cardBase2.transform.position).magnitude >= 0.001f) {
            tmp1.transform.position = Vector3.Lerp(tmp1.transform.position, cardBase1.transform.position, Time.deltaTime);
            tmp2.transform.position = Vector3.Lerp(tmp2.transform.position, cardBase2.transform.position, Time.deltaTime);
            yield return null;
        }
        
        yield return null;
    }
}

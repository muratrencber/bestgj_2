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
    IEnumerable GenerateCards() {

        Instantiate(cardPrefab, Vector3.Lerp(transform.position, cardBase1.transform.position, Time.deltaTime), Quaternion.identity);
        Instantiate(cardPrefab, Vector3.Lerp(transform.position, cardBase2.transform.position, Time.deltaTime), Quaternion.identity);
        yield return null;
    }
}

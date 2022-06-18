using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    int point;

    void Start() {

        point = Random.Range(1, 12);
    }

}

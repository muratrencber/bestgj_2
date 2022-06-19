using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int point;

    void Start() {

        point = Random.Range(1, 12);
    }

}

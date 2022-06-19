using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
3 -> 6
2 -> 4
1.5 -> 3
2.5 5
1.5 3
1 2
4.5 9
5 9
3.5 7
4 8
3 6
2.5 5
3.6 7
*/
public class Obje : MonoBehaviour
{
    public Types[] types;
    public Items item;
    public string objeName;
    public int price;
}

public enum Types
{
    tatli, 
    soguk, 
    sicak, 
    tuzlu, 
    aci, 
    eksi, 
    doyurucu,
    yiyecek,
    aburCubur, 
    kremali, 
    kitir, 
    baharatli,
    biskuvi, 
    cikolatali,
    icecek,
    esya,
    teknoloji, 
    spor, 
    sirin, 
    oyuncak, 
    alet
}

public enum Items
{

KOLA,
SODA,
MUZLU_SUT,
SU, 
GAZOZ, 
CANPARE,
PROBIS,
TUTKU,
CIZIVIC,
CUBUK_KRAKER,
DORITOS_SHOTS,
CHEETOS_SARI_UZUN,
GONG,
TWISTER,
MILKA,
KINDER,
ULKER_CIKOLATALI_GOFRET,
KARAM,
TADIM_BAR,
HOSBES,
DIDO,
ALBENI_TANE_TANE,
ETI_CIN,
HALLEY,
HARBY
}

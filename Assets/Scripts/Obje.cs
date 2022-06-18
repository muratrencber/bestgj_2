using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obje : MonoBehaviour
{
    public Types[] types;
    public Items item;
    float price;
    public int otomatNumber;
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

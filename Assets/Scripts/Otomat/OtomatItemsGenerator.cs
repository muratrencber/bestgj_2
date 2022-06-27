using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtomatItemsGenerator : MonoBehaviour
{
    [SerializeField] float numbersYOffset, itemYOffset, numbersXoffset, noXOffset, numbersSpacing, numbersScaling;
    [SerializeField] float rotation;
    [SerializeField] int copyCount;
    [SerializeField] float itemScaling, itemRotation;
    [SerializeField] GameObject numberCreatorPrefab;

    public void Create(Otomat o){
        OtomatConfigs oc = Configs.OtomatConfigs;
        for(int i = 0; i < oc.ItemKeys.Length; i++){
            ItemConfigs.ItemProperties ip = Configs.ItemConfigs.ItemDictionary[oc.ItemKeys[i]];
            Vector3 pos = o.GetPosition(i + oc.StartNumber);
            pos.z = transform.position.z;
            Vector3 bottomPosition = pos + Vector3.up * itemYOffset;
            Vector3 pricePosition = pos + Vector3.up * numbersYOffset + Vector3.right * numbersXoffset;
            Vector3 noPosition = pricePosition + Vector3.right * noXOffset;

            GameObject newObject = transform.CreateEmptyChild();
            GameObject spritesObject = newObject.transform.CreateEmptyChild();
            GameObject priceObject = newObject.transform.CreateEmptyChild();
            GameObject noObject = newObject.transform.CreateEmptyChild();

            for(int j = 0; j < copyCount; j++){
                GameObject item = ItemWorldObjectCreator.CreateItem(ip.itemKey, spritesObject.transform, itemScaling, itemRotation);
                SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
                pos.z = item.transform.position.z;
                item.transform.position = bottomPosition + Vector3.up * sr.bounds.extents.y;
                item.transform.RotateAround(item.transform.position, Vector3.forward, Random.Range(-rotation, rotation));
                item.transform.localPosition += Vector3.forward * 0.01f* j;
            }

            priceObject.transform.localScale = priceObject.transform.localScale * numbersScaling;
            noObject.transform.localScale = noObject.transform.localScale * numbersScaling;
            priceObject.transform.position = pricePosition;
            noObject.transform.position = noPosition;
            NumberCreator.SetForNumber(priceObject.transform, ip.price, numbersSpacing);
            NumberCreator.SetForNumber(noObject.transform, i + oc.StartNumber, numbersSpacing);
        }
    }
}

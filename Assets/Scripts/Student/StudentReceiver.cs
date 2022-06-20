using UnityEngine;

public class StudentReceiver : ItemReceiver
{
    [SerializeField] Student student;
    public override void TakeItem(GameObject itemObject)
    {
        student.OnReceivedItem(itemObject);
        PlayerStorage.instance.RemoveItem(itemObject.GetComponent<Obje>().item, 1);
        DestroyImmediate(itemObject);
    }
}

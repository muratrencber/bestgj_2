using UnityEngine;

public class Lid : MonoBehaviour
{
    [SerializeField] bool startOpen, isOpen, checkChildren;
    [SerializeField] Transform openTransform, closedTransform, targetTransform, childTransform;
    [SerializeField] float speed;

    public void Set(bool value){
        isOpen = value;
    }

    public void Toggle(){
        isOpen = !isOpen;
    }

    void Awake(){
        Set(startOpen);
    }

    void Update(){
        if(checkChildren)
            isOpen = childTransform.childCount > 0;
        Vector3 targetPosition = isOpen ? openTransform.position : closedTransform.position;
        targetPosition.z = targetTransform.position.z;
        targetTransform.position = Vector3.Lerp(targetTransform.position, targetPosition, Time.deltaTime * speed);
    }
    
}

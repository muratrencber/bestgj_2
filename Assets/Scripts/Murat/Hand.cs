using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] Camera target;

    float defaultCameraSize;
    Vector3 defaultScale;

    void Start(){
        defaultCameraSize = target.orthographicSize;
        defaultScale = transform.localScale;
    }

    void Update(){
        float height = target.orthographicSize;
        float width = height * target.aspect;
        Vector3 pos = new Vector3(target.transform.position.x - width, target.transform.position.y - height, transform.position.z);
        Vector3 scale = (target.orthographicSize / defaultCameraSize) * defaultScale;
        transform.position = pos;
        transform.localScale = scale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    static CameraControls instance;

    [SerializeField] bool enableZoom, enableLookahead;
    [SerializeField] float zoomSize, normalSize;
    [SerializeField] float lookaheadStartDistance, maxLookahead;
    [SerializeField] float moveSpeed, zoomSpeed;
    [SerializeField] Camera cam;
    float adjustedLADistance {get{
        return cam.orthographicSize / normalSize * lookaheadStartDistance;
    }}
    float adjustedMaxLA {get{
        return normalSize / cam.orthographicSize * maxLookahead;
    }}
    bool lookingAhead, zooming;
    Vector3 targetPosition;
    void Awake(){
        instance = this;
    }
    public static void Move(Vector3 newPosition){
        instance.transform.position = newPosition;
    }

    void Update(){
        if(enableLookahead)
            LookaheadUpdate();
        zooming = enableZoom && Input.GetMouseButton(1);
        Vector3 t = lookingAhead ? targetPosition : Vector3.zero;
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, t, Time.deltaTime * moveSpeed);
        float targetSize = zooming ? zoomSize : normalSize;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }
    Vector2 GetFromTransform(Vector2 screenPosition) => transform.InverseTransformPoint(cam.ScreenToWorldPoint(screenPosition));

    void LookaheadUpdate(){
        Vector2 fromVec = GetFromTransform(Input.mousePosition);
        if(lookingAhead){
            if(fromVec.magnitude < adjustedLADistance){
                lookingAhead = false;
            } else {
                targetPosition = fromVec.normalized * Mathf.Min(fromVec.magnitude -  adjustedLADistance, adjustedMaxLA);
            }
        } else if(fromVec.magnitude >= adjustedLADistance)
            lookingAhead = true;
    }
}

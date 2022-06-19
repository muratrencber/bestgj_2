using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    static CameraControls instance;
    public static bool InLook {get {return instance.inLook;}}

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
    bool lookingAhead, zooming, inLook;
    Vector3 targetPosition;
    LookArea lArea;
    void Awake(){
        instance = this;
    }
    public static void Move(Vector3 newPosition){
        newPosition.z = instance.transform.position.z;
        instance.transform.position = newPosition;
    }

    public static void SetLook(LookArea area){
        instance.lArea = area;
        instance.inLook = true;
    }

    public static void DisableLook(){
        instance.lArea?.EndedLook();
        instance.lArea = null;
        instance.inLook = false;
    }

    void Update(){
        if(enableLookahead)
            LookaheadUpdate();
        zooming = enableZoom && Input.GetMouseButton(1);
        Vector3 t = lookingAhead ? targetPosition : Vector3.zero;
        t = inLook ? transform.InverseTransformPoint(lArea.newCameraPosition.position) : t;

        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, t, Time.deltaTime * moveSpeed);
        float targetSize = zooming ? zoomSize : normalSize;
        targetSize = inLook ? lArea.newCameraSize : targetSize;
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

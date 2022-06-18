using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] bool enableZoom, enableLookahead;
    [SerializeField] float zoomSize, normalSize;
    [SerializeField] float lookaheadStartDistance, maxLookahead;
    [SerializeField] float moveSpeed, zoomSpeed;
    [SerializeField] Camera cam;
    bool lookingAhead, zooming;
    Vector3 savedPosition;
    Vector3 targetPosition;
    Vector2 startMousePosition;

    void Update(){
        if(enableLookahead)
            LookaheadUpdate();
        zooming = enableZoom && Input.GetMouseButton(1);
        Vector3 t = lookingAhead ? targetPosition : Vector3.zero;
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, t, Time.deltaTime * moveSpeed);
        float targetSize = zooming ? zoomSize : normalSize;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }

    Vector2 GetCameraSize() => new Vector2(cam.orthographicSize * 2,  cam.orthographicSize * 2 * cam.aspect);
    Vector2 GetFromTransform(Vector2 screenPosition) => transform.InverseTransformPoint(cam.ScreenToWorldPoint(screenPosition));

    void LookaheadUpdate(){
        Vector2 fromVec = GetFromTransform(Input.mousePosition);
        if(lookingAhead){
            if(fromVec.magnitude < lookaheadStartDistance){
                lookingAhead = false;
            } else {
                targetPosition = fromVec.normalized * Mathf.Min(fromVec.magnitude -  lookaheadStartDistance, maxLookahead);
            }
        } else if(fromVec.magnitude >= lookaheadStartDistance)
            lookingAhead = true;
    }
}

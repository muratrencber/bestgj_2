using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    static CameraControls instance;
    public static bool InLook {get {return instance.inLook;}}

    [SerializeField] bool enableZoom;
    [SerializeField] float zoomSize, normalSize;
    [SerializeField] float moveSpeed, zoomSpeed;
    [SerializeField] Camera cam;
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

        Vector3 t = inLook ? transform.InverseTransformPoint(lArea.newCameraPosition.position) : Vector3.zero;
        t.z = cam.transform.localPosition.z;
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, t, Time.deltaTime * moveSpeed);
        float targetSize = inLook ? lArea.newCameraSize : normalSize;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }
}

using UnityEngine;

public class DraggableObject : InteractableBase
{
    public static bool IsDragging {get {return dragging;}}
    static bool dragging;
    [SerializeField] float lerpSpeed;
    [SerializeField] Vector2 offset;
    
    Transform parent;
    Vector3 savedPosition;
    protected override string EvaluateCursor()
    {
        return "drag";
    }

    protected override bool EvaluateAvailability()
    {
        return enabled && (!dragging || thisDragging);
    }

    bool thisDragging;
    void Start(){
        savedPosition = transform.localPosition;
        parent = transform.parent;
    }

    public override void OnCursorDown()
    {
        transform.SetParent(null);
        dragging = true;
        thisDragging = true;
    }

    public override void OnCursorUp()
    {
        if(thisDragging){
            transform.SetParent(parent);
            dragging = false;
            thisDragging = false;

            ItemReceiver[] receivers = GameObject.FindObjectsOfType<ItemReceiver>();
            foreach(ItemReceiver rec in receivers){
                if(rec.Check(gameObject, GetComponent<Obje>().item)){
                    rec.TakeItem(gameObject);
                    return;
                }
            }
        }
    }

    public override void OnChangedInteractable()
    {
        if(thisDragging){
            transform.SetParent(parent);
            dragging = false;
            thisDragging = false;
        }
    }

    void Update(){
        Vector3 targetPosition = thisDragging ? Camera.main.ScreenToWorldPoint(Input.mousePosition) + (Vector3)offset : parent.TransformPoint(savedPosition);
        targetPosition.z = transform.position.z;
        if(!thisDragging)
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        else
            transform.position = targetPosition;
    }
}

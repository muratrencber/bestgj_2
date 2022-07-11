using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class UIGraph : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    [SerializeField] RectTransform bottomLeft, topRight;
    [SerializeField] Transform xAxis, yAxis;
    public Transform toggleContainer, itemsContainer;

    int[] x; int[] y;
    int minX; int maxX;
    int minY; int maxY;

    public void Set(int[] xValues, int[] yValues, bool interpolate){
        x = xValues; y = yValues;
        SetAxes();
        Redraw(interpolate);
    }

    void SetAxes(){
        SetXAxis();
        SetYAxis();
    }
    void SetXAxis()=>SetAxisText(xAxis, x, ref minX, ref maxX);
    void SetYAxis()=>SetAxisText(yAxis, y, ref minY, ref maxY);

    void SetAxisText(Transform container, int[] values, ref int min, ref int max){
        TextMeshProUGUI[] texts = container.GetComponentsInChildren<TextMeshProUGUI>();
        if(values == null || values.Length == 0){
            min = 0; max = 0;
            foreach(TextMeshProUGUI tmp in texts) tmp.text = "";
            return;
        }
        
        float distance = values[values.Length - 1] - values[0];
        if(distance == 0) distance = 1;
        float margins = distance * 0.25f;
        int margin = Mathf.CeilToInt(margins);
        min = values[0] - margin; max = values[values.Length - 1] + margin;

        float textCount = texts.Length + 1;
        int last = min;
        for(int i = 0; i < texts.Length; i+=1){
            int current = Mathf.RoundToInt(Mathf.Lerp(min, max, (float)(i + 1) / textCount));
            if(current == last) texts[i].text = "";
            else {
                last = current;
                texts[i].text = current.ToString();
            }
        }
    }

    public void Redraw(bool interpolate){
        int pointCount = x != null && y != null ?  x.Length * 2 + 2 : 0;
        lr.positionCount = pointCount;
        if(pointCount == 0) return;
        
        int totalIndex = 0;
        lr.SetPosition(totalIndex++, new Vector3(bottomLeft.localPosition.x, GetPoint(0).y));
        lr.SetPosition(totalIndex++, GetPoint(0));

        for(int i = 0; i < x.Length - 1; i++){
            int startIndex = i;
            int endIndex = interpolate ? i + 1 : i;
            
            lr.SetPosition(totalIndex++, GetPoint(startIndex));
            lr.SetPosition(totalIndex++, GetPoint(endIndex));
        }

        lr.SetPosition(totalIndex++, GetPoint(x.Length - 1));
        lr.SetPosition(totalIndex++, new Vector3(topRight.localPosition.x, GetPoint(x.Length - 1).y));
    }
     

    public Vector3 GetPoint(int index){
        float widthInt = maxX - minX;
        float heightInt = maxY - minY;

        float xPosInt = x[index]; float yPosInt = y[index];
        float xPosNormalized = xPosInt / widthInt;
        float yPosNormalized = yPosInt / heightInt;
        
        Vector2 size = topRight.localPosition - bottomLeft.localPosition;
        float xPos = size.x * xPosNormalized;
        float yPos = size.y * yPosNormalized;

        return bottomLeft.localPosition + new Vector3(xPos, yPos);
    }
}

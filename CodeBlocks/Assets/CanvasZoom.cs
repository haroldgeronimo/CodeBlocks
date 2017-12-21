using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasZoom : MonoBehaviour {
    RectTransform imageToZoom;
    CanvasGroup canvas;
    float maxScale = 2;
    float minScale = .4f;
    float zoomSpeed = 0.002f;
    float currentScale;
    float zoomMag = 0;  
     Text ZoomText;
    void Start()
    {
        imageToZoom = this.GetComponent<RectTransform>();
        currentScale = imageToZoom.rect.height;
       // ZoomText = GetComponentInChildren<Text>();
    }
    void Update()
    {
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        if(Input.touchCount == 2)
        {
           // canvas.blocksRaycasts = true;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudediff = touchDeltaMag - prevTouchDeltaMag;
           zoomMag = deltaMagnitudediff * zoomSpeed;
       
            Zoom(Mathf.Clamp(zoomMag,-1,1));
            //canvas.blocksRaycasts = false;
        }
     //   ZoomText.text = zoomMag.ToString();
    }

   public void Zoom(float increment)
    {
        currentScale += increment;
        if (currentScale >= maxScale)
        {
            currentScale = maxScale;
        }
        else if (currentScale <= minScale)
        {
            currentScale = minScale;
        }
        imageToZoom.localScale = new Vector2(currentScale, currentScale);
    }
}

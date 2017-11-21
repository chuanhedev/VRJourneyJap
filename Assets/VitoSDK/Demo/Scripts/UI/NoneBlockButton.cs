using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class NoneBlockButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler {


	public void OnPointerDown(PointerEventData data)
    {
        VitoPluginPlayVideo.instance.VideoSliderDown();
    }

    public void OnPointerUp(PointerEventData data)
    {
        VitoPluginPlayVideo.instance.VideoSliderUp();
    }
    public void OnDrag(PointerEventData data)
    {
        VitoPluginPlayVideo.instance.VideoSliderDown();
    }
}

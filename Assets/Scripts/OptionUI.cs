using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 答题按钮交互
/// </summary>
public class OptionUI : MonoBehaviour
{
    VitoVRInteractiveItem interactiveItem;
    RefreshPanoData refreshPanoData;
    Image optionImage;
    Color defColor;

    void Awake()
    {
        Find();
        Init();
    }

    public void OnExit()
    {
        optionImage.color = defColor;
    }

    public void OnEnter()
    {
        optionImage.color = new Color(defColor.r, defColor.b, defColor.g, defColor.a + 0.25f);
    }

    public void OnClick()
    {
        if (interactiveItem) interactiveItem.Click();

        if (refreshPanoData) refreshPanoData.Click();
    }

    void Find()
    {
        optionImage = GetComponent<Image>();
        interactiveItem = GetComponent<VitoVRInteractiveItem>();
        refreshPanoData = GetComponent<RefreshPanoData>();
    }

    void Init()
    {
        defColor = optionImage.color;
    }
}

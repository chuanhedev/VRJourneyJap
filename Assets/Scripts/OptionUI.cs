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
    Transform label;

    void Awake()
    {
        Find();
        Init();
    }

    public void OnExit()
    {
        Debug.Log("OnExit");

        if (optionImage) optionImage.color = defColor;

        if (label) label.gameObject.SetActive(false);
    }

    public void OnEnter()
    {
        Debug.Log("OnEnter");

        if (optionImage) optionImage.color = new Color(defColor.r, defColor.b, defColor.g, defColor.a + 0.25f);

        if (label && !label.gameObject.activeSelf) label.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        Debug.Log("OnClick");

        if (interactiveItem) interactiveItem.Click();

        if (refreshPanoData) refreshPanoData.Click();

        if (label)
        {
            FacadeManager._instance.RequestUpdatePano(label.GetComponent<Label>().goPanoScene);
        }
    }

    void Find()
    {
        optionImage = GetComponent<Image>();
        interactiveItem = GetComponent<VitoVRInteractiveItem>();
        refreshPanoData = GetComponent<RefreshPanoData>();
        label = transform.Find("Label");
    }

    void Init()
    {
        if (optionImage) defColor = optionImage.color;
    }
}

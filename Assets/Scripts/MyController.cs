using Pvr_UnitySDKAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// pico手柄
/// </summary>
public class MyController : MonoBehaviour
{
    RaycastHit raycastHit;
    Transform controller;
    Transform dot;
    Transform direction;
    List<OptionUI> optionUIList = new List<OptionUI>();

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Find();
#endif
    }

    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Ray ray = new Ray(controller.position, (dot.position - controller.position).normalized);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        Ray ray = new Ray(transform.position, transform.forward);
#endif

        bool isEnter = Physics.Raycast(ray, out raycastHit, 50);
        if (isEnter)
        {
            OptionUI optionUI = raycastHit.transform.GetComponent<OptionUI>();
            if (dot) dot.position = raycastHit.point;
            if (optionUI)
            {
                if (!optionUIList.Contains(optionUI)) optionUIList.Add(optionUI);
                CheckOptionUIs(optionUI, false);

#if UNITY_ANDROID && !UNITY_EDITOR
                if (Controller.UPvr_GetKeyDown(Pvr_KeyCode.TOUCHPAD)) optionUI.OnClick();
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                if (Input.GetMouseButtonUp(0)) optionUI.OnClick();
#endif

            }
            else
            {
                CheckOptionUIs(null, true);
            }
        }
        else
        {
            CheckOptionUIs(null, true);
            if (dot) dot.position = controller.position + direction.forward * 3;
        }
    }

    /// <summary>
    /// 设置答题button
    /// </summary>
    /// <param name="optionUI"></param>
    /// <param name="isResetAll">是否还原所有</param>
    void CheckOptionUIs(OptionUI optionUI, bool isResetAll)
    {
        if (optionUIList.Count == 0) return;

        for (int i = 0; i < optionUIList.Count; i++)
        {
            if (optionUIList[i])
            {
                if (isResetAll)
                {
                    optionUIList[i].OnExit();
                }
                else
                {
                    if (optionUIList[i].transform.name == optionUI.transform.name) optionUIList[i].OnEnter();
                    else optionUIList[i].OnExit();
                }
            }
        }

        if (isResetAll) optionUIList.Clear();
    }

    void Find()
    {
        controller = transform.Find("controller/ppcontroller");
        dot = transform.Find("dot");
        direction = transform.Find("direction");
    }
}

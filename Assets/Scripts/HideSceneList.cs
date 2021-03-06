﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideSceneList : MonoBehaviour
{
    Scrollbar scrollbar;
    Transform content;
    GameObject sceneList;
    GameObject child4;
    GameObject child5;
    GameObject load;

    void Awake()
    {
        load = transform.Find("Load").gameObject;
        sceneList = transform.Find("HostUI/SceneList").gameObject;
        content = transform.Find("HostUI/SceneList/Scroll View/Viewport/Content");
        scrollbar = transform.Find("HostUI/SceneList/Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        Debug.Log(content.name);
    }

    public void OnRefreshClick()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        load.SetActive(true);

        yield return new WaitForSeconds(Random.Range(3, 6));

        if (PlayerPrefs.GetInt("Refresh1") != 1)
        {
            if (child4) child4.SetActive(true);
            PlayerPrefs.SetInt("Refresh1", 1);
        }
        else if (PlayerPrefs.GetInt("Refresh2") != 1)
        {
            if (child5) child5.SetActive(true);
            PlayerPrefs.SetInt("Refresh2", 1);
        }
        yield return new WaitForSeconds(0.2f);
        scrollbar.value = 1;
        load.SetActive(false);
        StartCoroutine(ScrollBarValue());
    }

    IEnumerator ScrollBarValue()
    {
        yield return null;
        scrollbar.value = Mathf.Lerp(scrollbar.value, 0, 0.2f);
        if (scrollbar.value < 0.05f)
        {
            scrollbar.value = 0;
            StopCoroutine(ScrollBarValue());
        }
        else
        {
            StartCoroutine(ScrollBarValue());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.SetInt("Refresh1", 0);
            PlayerPrefs.SetInt("Refresh2", 0);
        }

        if (sceneList.activeSelf)
        {
            child4 = content.GetChild(4).gameObject;
            child5 = content.GetChild(5).gameObject;
            if (PlayerPrefs.GetInt("Refresh1") != 1)
                if (child4) child4.SetActive(false);
            if (PlayerPrefs.GetInt("Refresh2") != 1)
                if (child5) child5.SetActive(false);
        }
    }
}

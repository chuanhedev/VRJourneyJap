using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// pico Log
/// </summary>
public class MyPicoLog : MonoBehaviour
{
    static Text log1;
    static Text log2;

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        log1 = transform.Find("Head/Log1").GetComponent<Text>();
        log2 = transform.Find("Head/Log2").GetComponent<Text>();
#endif
    }

    public static void SetLog1(string content)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        log1.text = content;
#endif
    }

    public static void SetLog2(string content)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        log2.text = content;
#endif
    }
}

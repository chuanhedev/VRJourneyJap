using UnityEngine;
using System.Collections;
using LitJson;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;
public class VitoSDKConfig2 :MonoBehaviour
{
    public static VitoSDKConfig2 instance { get; private set; }
    public EVrType type = EVrType.EVT_NONE;

    void Awake()
    {
        instance = this;
    }
}
using UnityEngine;
using System.Collections;

/// <summary>
/// this class is deprecated 
/// </summary>
public class VitoVRHead : MonoBehaviour {
    
    public static VitoVRHead instance { get; private set; }
    [HideInInspector]
    public Transform mTransform;
    void Awake()
    {
        instance = this;
        mTransform = transform;
    }
    
}

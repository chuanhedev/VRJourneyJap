using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    public string goPanoScene;

    void OnEnable()
    {
        transform.LookAt(Vector3.zero);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pano_Box : MonoBehaviour
{
    public string firstScene;

    void OnEnable()
    {
        FacadeManager._instance.UpdatePano(firstScene, false, true);
    }
}

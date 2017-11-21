using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshPanoData : MonoBehaviour
{

    public void Click()
    {
        FacadeManager._instance.GetRefreshPanoData();
    }
}

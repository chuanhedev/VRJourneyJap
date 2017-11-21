using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class OculusVRMountCheck : MonoBehaviour {
    /// <summary>
    /// Occurs when an HMD is put on the user's head.
    /// </summary>
    public static event Action HMDMounted;

    /// <summary>
    /// Occurs when an HMD is taken off the user's head.
    /// </summary>
    public static event Action HMDUnmounted;


    private bool isUserPresent;
    private bool _wasUserPresent;

#if GEARVR
    void Update()
    {
        isUserPresent = true;// OVRPlugin.userPresent;

        if (_wasUserPresent && !isUserPresent)
        {
            try
            {
                Debug.Log("摘下头盔");
                if (HMDUnmounted != null)
                    HMDUnmounted();
            }
            catch (Exception e)
            {
                Debug.LogError("Caught Exception: " + e);
            }
        }

        if (!_wasUserPresent && isUserPresent)
        {
            try
            {

                Debug.Log("带上头盔");
                if (HMDMounted != null)
                    HMDMounted();
            }
            catch (Exception e)
            {
                Debug.LogError("Caught Exception: " + e);
            }
        }

        _wasUserPresent = isUserPresent;
    }

#endif


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileDeviceInput
{
    /// <summary>
    /// 手指按下瞬间
    /// </summary>
    /// <returns></returns>
    /// 

    private static bool isIVRDown = false;

    public static bool GetTouchDown()
    {
#if IVR  //Idealens
        if (IVRInput.GetKeyDown())
        {
            isIVRDown = true;
            return true;
        }
        else
        {
            return false;
        }

#elif DPN  //Deepoon
        return Input.GetMouseButtonDown(0);
#else //GearVR
        return Input.GetButtonDown("Fire1");
#endif
    }

    /// <summary>
    /// 手指抬起
    /// </summary>
    /// <returns></returns>
    public static bool GetTouchUp()
    {
#if IVR  //Idealens
        if (IVRInput.GetKeyUp())
        {
            isIVRDown = false;
            return true;
        }
        else
        {
            return false;
        }
#elif DPN  //Deepoon
        return Input.GetMouseButtonUp(0);
#else //GearVR

        return Input.GetButtonUp("Fire1");
#endif
    }

    /// <summary>
    /// 手指一直按下
    /// </summary>
    /// <returns></returns>
    public static bool GetTouch()
    {
#if IVR  //Idealens
        return isIVRDown;
#elif DPN  //Deepoon
        return Input.GetMouseButton(0);
#else //GearVR
        return Input.GetButton("Fire1");
#endif
    }

    /// <summary>
    /// 手指按下的位置
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetTouchPos()
    {
#if IVR  //Idealens
        return new Vector2(IVRInput.mousePosition.y, IVRInput.mousePosition.x);
#elif DPN  //Deepoon
         return Input.mousePosition;
#else //GearVR
        return Input.mousePosition;
#endif
    }

    /// <summary>
    /// 面板上的返回键
    /// </summary>
    /// <returns></returns>
    public static bool IsCancleDown()
    {
#if IVR
        return IVRInput.IsHomeButtonDown();
#else
        return Input.GetButtonDown("Cancel");
#endif
    }


}

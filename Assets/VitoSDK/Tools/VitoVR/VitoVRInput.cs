using UnityEngine;
using System.Collections;
using System;
using VR = UnityEngine.VR;
#if HTCVIVE
using Valve.VR;
#endif
/// <summary>
/// 自定义的虚拟按键
/// </summary>
public enum VirtualKey : ulong
{
    TriggerLeft = 1,  //htc、Oculus左手压杆
    TriggerRight = 2,//htc、Oculus右手压杆
    GripLeft = 4,//htc、Oculus左手Grip(握键)
    GripRight = 8,//htc、Oculus右手Grip(握键)
    TouchPadLeft = 16, //htc左手触摸板
    TouchPadRight = 32, //htc右手触摸板
    TouchPad = 64,  //GearVR、Idealens、deepoon触摸板
}

/// <summary>
/// HTC、Oculus、GearVR、Idealens、Deepoon现已加入豪华套餐
/// </summary>
public class VitoVRInput : SingleInstance<VitoVRInput>
{
    //Swipe directions
    public enum SwipeDirection
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

#region Action

    /// <summary>
    /// HTC：滑动抬起时触发
    /// Oculus：无
    /// Mobile：滑动抬起时触发
    /// </summary>
    public event Action<SwipeDirection> OnSwipeOnce;
    /// <summary>
    /// HTC：滑动未抬起时触发
    /// Oculus：无
    /// Mobile：滑动未抬起时触发
    /// </summary>
    public event Action<SwipeDirection> OnSwipeFrame;
    /// <summary>
    /// HTC、Oculus：按下左右手柄trigger键
    /// Mobile：点击面板(抬起)
    /// </summary>
    public event Action OnClick;
    /// <summary>
    /// HTC、Oculus：两次按下左右手柄trigger键
    /// Mobile：两次点击面板(抬起)
    /// </summary>
    public event Action OnDoubleClick;
    /// <summary>
    /// HTC、Oculus：松开左右手柄trigger键
    /// Mobile：点击面板抬起后
    /// </summary>
    public event Action OnUp;
    /// <summary>
    /// HTC、Oculus：按住左右手柄trigger键
    /// Mobile：按压面板时
    /// </summary>
    public event Action OnDown;
    /// <summary>
    /// HTC：按住左右手柄menu键
    /// Oculus：无
    /// Mobile：按压返回时
    /// </summary>
    public event Action OnCancel;

    /// <summary>
    /// HTCVIVE：松开左手trigger键
    /// </summary>
    public event Action OnLeftTriggerUp;
    /// <summary>
    /// HTCVIVE：松开左手trigger键
    /// </summary>
    public event Action OnRightTriggerUp;
    /// <summary>
    /// HTCVIVE：按住左手trigger键
    /// </summary>
    public event Action OnLeftTriggerDown;
    /// <summary>
    /// HTCVIVE：按住左手trigger键
    /// </summary>
    public event Action OnRightTriggerDown;
    /// <summary>
    /// HTCVIVE：松下左手trigger键
    /// </summary>
    public event Action OnLeftTriggerClick;
    /// <summary>
    /// HTCVIVE：松下左手trigger键
    /// </summary>
    public event Action OnRightTriggerClick;
    /// <summary>
    /// HTCVIVE：按下左手trigger键程度
    /// </summary>
    public event Action<float> OnLeftTrigger;
    /// <summary>
    /// HTCVIVE：按下右手trigger键程度
    /// </summary>
    public event Action<float> OnRightTrigger;

    #endregion

    public GameObject htcHandLeft;
    public GameObject htcHandRight;
#if HTCVIVE
    private SteamVR_TrackedObject trackedObj_left;
    private SteamVR_TrackedObject trackedObj_right;
#endif
    public bool isUseDoubleClick;

    [SerializeField]
    private float m_DoubleClickTime = 0.3f;    //The max time allowed between double clicks
    [SerializeField]
    private float m_SwipeWidthPer = 0.3f;         //The width of a swipe

    private Vector2 m_MouseDownPosition, m_MouseDownPosition_left, m_MouseDownPosition_right;       // The screen position of the mouse when Fire1 is pressed.
    private Vector2 m_MouseUpPosition, m_MouseUpPosition_left, m_MouseUpPosition_right;         // The screen position of the mouse when Fire1 is released.
    private float m_LastMouseUpTime;                            // The time when Fire1 was last released.
    private float m_LastHorizontalValue;                        // The previous value of the horizontal axis used to detect keyboard swipes.
    private float m_LastVerticalValue;                          // The previous value of the vertical axis used to detect keyboard swipes.

    public float DoubleClickTime { get { return m_DoubleClickTime; } }

    private bool mIsLeftDown = false;
    private bool mIsRightDown = false;


    private ulong mDeviceState; //当前设备的状态
    private ulong mPreDeviceState; //前一帧设备的状态
    private Vector2[] mTouchPos = new Vector2[2];
    private Vector2[] mPreTouchPos = new Vector2[2];
    private Vector2[] mTouchOffset = new Vector2[2];
    private float[] mTriggerValue = new float[2];
    private int mPreFrameCount = -1;

    void Awake()
    {

#if HTCVIVE
        if(htcHandLeft==null)
        {
            htcHandLeft = VRSwitchCameraRig.instance.mLeftHand.gameObject;
        }
        if(htcHandRight==null)
        {
            htcHandRight = VRSwitchCameraRig.instance.mRightHand.gameObject;
        }
        trackedObj_left = htcHandLeft.GetComponent<SteamVR_TrackedObject>();
        trackedObj_right = htcHandRight.GetComponent<SteamVR_TrackedObject>();
#endif
    }


    #region 手柄震动
    /// <summary>
    /// 手柄震动，左手：-1，右手：1，XBOX手柄或者头盔：0
    /// </summary>
    /// <param name="index"></param>
    public void ShackHands(int index, float HTCshack = 300f)
    {
#if HTCVIVE
        switch (index)
        {
            case -1:
                switch (Global.eVrType)
                {
                    case EVrType.EVT_HTC_Vive:
                        CVRSystem system = OpenVR.System;
                        if (system != null)
                        {
                            EVRButtonId buttonId = EVRButtonId.k_EButton_SteamVR_Touchpad;
                            var axisId = (uint)buttonId - (uint)EVRButtonId.k_EButton_Axis0;
                            system.TriggerHapticPulse((uint)trackedObj_left.index, axisId, (char)HTCshack);
                        }
                        break;
                    case EVrType.EVT_Oculus_Rift:
#if OCULUSRIFT
                        OVRInput.SetControllerVibration(1f, 1, OVRInput.Controller.LTouch);
#endif
                        Invoke("EndVibrationL", 0.25f);
                        break;
                }
                break;
            case 1:
                switch (Global.eVrType)
                {
                    case EVrType.EVT_HTC_Vive:
                        CVRSystem system = OpenVR.System;
                        if (system != null)
                        {
                            EVRButtonId buttonId = EVRButtonId.k_EButton_SteamVR_Touchpad;
                            var axisId = (uint)buttonId - (uint)EVRButtonId.k_EButton_Axis0;
                            system.TriggerHapticPulse((uint)trackedObj_right.index, axisId, (char)HTCshack);
                        }
                        break;
                    case EVrType.EVT_Oculus_Rift:
                        Invoke("EndVibrationR", 0.25f);
#if OCULUSRIFT
                        OVRInput.SetControllerVibration(1f, 1, OVRInput.Controller.RTouch);
#endif
                        break;
                }
                break;
            case 0:
                break;
        }
#endif
    }
    void EndVibrationL()
    {
        EndVibration(-1);
    }
    void EndVibrationR()
    {
        EndVibration(1);
    }
    void EndVibration(int index)
    {
#if OCULUSRIFT
        switch (index)
        {
            case -1:

                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
                break;
            case 1:
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
                break;
        }
#endif
    }
#endregion

    public bool GetKeyDown(VirtualKey vk) { LocalUpdate(); return (mDeviceState & (ulong)vk) != 0 && (mPreDeviceState & (ulong)vk) == 0; }
    public bool GetKeyUp(VirtualKey vk) { LocalUpdate(); return (mDeviceState & (ulong)vk) == 0 && (mPreDeviceState & (ulong)vk) != 0; }
    public bool GetKey(VirtualKey vk) { LocalUpdate(); return (mDeviceState & (ulong)vk) != 0; }
    public Vector2 GetTouchPos(VirtualKey vk)
    {
        LocalUpdate();
        switch (vk)
        {
            case VirtualKey.TouchPadLeft:
                return mTouchPos[0];
            case VirtualKey.TouchPadRight:
                return mTouchPos[1];
            case VirtualKey.TouchPad:
                return mTouchPos[0];
            default:
                return mTouchPos[0];
        }
    }
    public Vector2 GetTouchOffset(VirtualKey vk)
    {
        LocalUpdate();
        switch (vk)
        {
            case VirtualKey.TouchPadLeft:
                return mTouchOffset[0];
            case VirtualKey.TouchPadRight:
                return mTouchOffset[1];
            case VirtualKey.TouchPad:
                return mTouchOffset[0];
            default:
                return mTouchOffset[0];
        }
    }
    public float GetTriggerValue(VirtualKey vk)
    {
        LocalUpdate();
        switch (vk)
        {
            case VirtualKey.TriggerLeft:
                return mTriggerValue[0];
            case VirtualKey.TriggerRight:
                return mTriggerValue[1];
            default:
                return 0;
        }
    }

    private void Update()
    {
        LocalUpdate();

#if HTCVIVE
        CheckHTCVive();
#elif OCULUSRIFT
         CheckOculus();
#elif GEARVR
        CheckMobile();
#else
        CheckMobile();
#endif
    }

    private void LocalUpdate()
    {
        if (Time.frameCount != mPreFrameCount)
        {
            mPreFrameCount = Time.frameCount;
            mPreDeviceState = mDeviceState;
            mPreTouchPos = mTouchPos;
#if HTCVIVE
            SteamVR_Controller.Device device_left = SteamVR_Controller.Input((int)trackedObj_left.index);
            SteamVR_Controller.Device device_right = SteamVR_Controller.Input((int)trackedObj_right.index);
            if (device_left.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                mTriggerValue[0] = device_left.GetState().rAxis1.x;
                mDeviceState &= (ulong)VirtualKey.TriggerLeft;
            }
            if (device_right.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                mTriggerValue[1] = device_right.GetState().rAxis1.x;
                mDeviceState &= (ulong)VirtualKey.TriggerRight;
            }
            if (device_left.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                mDeviceState &= (ulong)VirtualKey.GripLeft;
            }
            if (device_right.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                mDeviceState &= (ulong)VirtualKey.GripRight;
            }
            if (device_left.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                mDeviceState &= (ulong)VirtualKey.TouchPadLeft;
            }
            if (device_right.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                mDeviceState &= (ulong)VirtualKey.TouchPadRight;
            }
            if(device_left.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                mTouchPos[0] = device_left.GetAxis();
            }
            if(device_right.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                mTouchPos[1] = device_right.GetAxis();
            }



#elif OCULUSRIFT
            if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                mDeviceState &= (ulong)VirtualKey.TriggerLeft;
            }
            if(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                mDeviceState &= (ulong)VirtualKey.TriggerRight;
            }
            if(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
            {
                mDeviceState &= (ulong)VirtualKey.GripLeft;
            }
            if(OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                mDeviceState &= (ulong)VirtualKey.GripRight;
            }
#else
            if (MobileDeviceInput.GetTouch())
            {
                mDeviceState &= (ulong)VirtualKey.TouchPad;
                mTouchPos[0] = MobileDeviceInput.GetTouchPos();
            }
#endif

        }
    }

    private void CheckHTCVive()
    {
#if HTCVIVE
        SteamVR_Controller.Device device_left = SteamVR_Controller.Input((int)trackedObj_left.index);
        SteamVR_Controller.Device device_right = SteamVR_Controller.Input((int)trackedObj_right.index);

        //trigger
        if (device_left.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (OnDown != null)
                OnDown();
            if (OnLeftTriggerDown != null)
                OnLeftTriggerDown();
            if (OnLeftTrigger != null)
                OnLeftTrigger(device_left.GetPrevState().rAxis1.x);

        }
        if (device_left.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (isUseDoubleClick)
            {
                if (Time.time - m_LastMouseUpTime < m_DoubleClickTime)
                {
                    if (OnDoubleClick != null)
                        OnDoubleClick();
                }
                else
                {
                    if (OnClick != null)
                        OnClick();
                    if (OnLeftTriggerClick != null)
                        OnLeftTriggerClick();
                }
            }
            else
            {
                if (OnClick != null)
                    OnClick();
                if (OnLeftTriggerClick != null)
                    OnLeftTriggerClick();
            }
        }

        if (device_left.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (OnLeftTriggerUp != null)
                OnLeftTriggerUp();
            if (OnLeftTrigger != null)
                OnLeftTrigger(0);
            if (OnUp != null)
                OnUp();
        }

        if (device_right.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (OnRightTrigger != null)
                OnRightTrigger(device_right.GetPrevState().rAxis1.x);
            if (OnDown != null)
                OnDown();
            if (OnRightTriggerDown != null)
                OnRightTriggerDown();
        }
        if (device_right.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (isUseDoubleClick)
            {
                if (Time.time - m_LastMouseUpTime < m_DoubleClickTime)
                {
                    if (OnDoubleClick != null)
                        OnDoubleClick();
                }
                else
                {
                    if (OnClick != null)
                        OnClick();
                    if (OnRightTriggerClick != null)
                        OnRightTriggerClick();
                }
            }
            else
            {
                if (OnClick != null)
                    OnClick();
                if (OnRightTriggerClick != null)
                    OnRightTriggerClick();
            }
        }
        else if (device_right.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (OnRightTrigger != null)
                OnRightTrigger(0);
            if (OnRightTriggerUp != null)
                OnRightTriggerUp();
            if (OnUp != null)
                OnUp();
        }

        //cancel
        if (device_left.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            if (OnCancel != null)
                OnCancel();
        if (device_right.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            if (OnCancel != null)
                OnCancel();

        //swipe
        SwipeDirection swipe = SwipeDirection.NONE;
        if (device_left.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            m_MouseDownPosition_left = device_left.GetAxis();
        }
        if (device_left.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            m_MouseUpPosition_left = device_left.GetAxis();
            swipe = DetectSwipe(m_MouseDownPosition_left, m_MouseUpPosition_left);

            if (OnSwipeFrame != null)
                OnSwipeFrame(swipe);
        }
        if (device_left.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            m_MouseUpPosition_left = device_left.GetAxis();
            swipe = DetectSwipe(m_MouseDownPosition_left, m_MouseUpPosition_left);

            if (OnSwipeOnce != null)
                OnSwipeOnce(swipe);
        }


        if (device_right.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            m_MouseDownPosition_right = device_right.GetAxis();
        }
        if (device_right.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            m_MouseUpPosition_right = device_right.GetAxis();
            swipe = DetectSwipe(m_MouseDownPosition_right, m_MouseUpPosition_right);
            if (OnSwipeFrame != null)
                OnSwipeFrame(swipe);
        }
        if (device_right.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            m_MouseUpPosition_right = device_right.GetAxis();
            swipe = DetectSwipe(m_MouseDownPosition_right, m_MouseUpPosition_right);
            if (OnSwipeOnce != null)
                OnSwipeOnce(swipe);
        }
#endif
    }

    private void CheckOculus()
    {
#if OCULUSRIFT
        float v = 0;
        if (OnLeftTrigger != null)
        {
            v = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            if (v > 0.9f)
            {
                if (!mIsLeftDown)
                {
                    mIsLeftDown = true;
                    if (OnDown != null) OnDown();
                    if (OnLeftTriggerDown != null) OnLeftTriggerDown();
                }
            }
            else
            {
                if (mIsLeftDown)
                {
                    mIsLeftDown = false;
                    if (OnUp != null) OnUp();
                    if (OnClick != null) OnClick();
                    if (OnLeftTriggerUp != null) OnLeftTriggerUp();
                    if (OnLeftTriggerClick != null) OnLeftTriggerClick();
                }
            }
            OnLeftTrigger(v);
        }

        if (OnRightTrigger != null)
        {
            v = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
            if (v > 0.9f)
            {
                if (!mIsRightDown)
                {
                    mIsRightDown = true;
                    if (OnDown != null) OnDown();
                    if (OnRightTriggerDown != null) OnRightTriggerDown();
                }
            }
            else
            {
                if (mIsRightDown)
                {
                    mIsRightDown = false;
                    if (OnUp != null) OnUp();
                    if (OnClick != null) OnClick();
                    if (OnRightTriggerUp != null) OnRightTriggerUp();
                    if (OnRightTriggerClick != null) OnRightTriggerClick();
                }
            }
            OnRightTrigger(v);
        }
#endif
    }

    private void CheckMobile()
    {
        SwipeDirection swipe = SwipeDirection.NONE;
        if (MobileDeviceInput.GetTouchDown())
        {
            m_MouseDownPosition = MobileDeviceInput.GetTouchPos();
            if (OnDown != null)
                OnDown();
        }
        //OnSwipeOnce
        if (MobileDeviceInput.GetTouchUp())
        {
            m_MouseUpPosition = MobileDeviceInput.GetTouchPos();
            swipe = DetectSwipe(m_MouseDownPosition, m_MouseUpPosition);
            if (OnSwipeOnce != null)
                OnSwipeOnce(swipe);
        }
        //OnSwipeFrame
        if (MobileDeviceInput.GetTouch())
        {
            m_MouseUpPosition = MobileDeviceInput.GetTouchPos();// new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            swipe = DetectSwipe(m_MouseDownPosition, m_MouseUpPosition);
            if (OnSwipeFrame != null)
                OnSwipeFrame(swipe);
        }
        //OnDoubleClick OnClick
        if (MobileDeviceInput.GetTouchUp())
        {
            if (OnUp != null)
                OnUp();
            if (isUseDoubleClick)
            {
                if (Time.time - m_LastMouseUpTime < m_DoubleClickTime)
                {
                    if (OnDoubleClick != null)
                        OnDoubleClick();
                }
                else
                {
                    if (OnClick != null)
                        OnClick();
                }
                m_LastMouseUpTime = Time.time;

            }
            else
            {
                if (OnClick != null)
                    OnClick();
            }
        }
        //cancel
        if (MobileDeviceInput.IsCancleDown())
        {
            if (OnCancel != null)
                OnCancel();
            //Application.Quit();
        }
    }

    private SwipeDirection DetectSwipe(Vector2 m_MouseDownPosition, Vector2 m_MouseUpPosition)
    {
        Vector2 mSwipeValue = m_MouseUpPosition - m_MouseDownPosition;
        Vector2 swipeData = (mSwipeValue).normalized;
        bool swipeIsVertical = Mathf.Abs(swipeData.x) < m_SwipeWidthPer ;
        bool swipeIsHorizontal = Mathf.Abs(swipeData.y) < m_SwipeWidthPer;

        if (swipeData.y > 0f && swipeIsVertical)
            return SwipeDirection.UP;
        if (swipeData.y < 0f && swipeIsVertical)
            return SwipeDirection.DOWN;
        if (swipeData.x > 0f && swipeIsHorizontal)
            return SwipeDirection.RIGHT;
        if (swipeData.x < 0f && swipeIsHorizontal)
            return SwipeDirection.LEFT;
        return SwipeDirection.NONE;
    }

    private SwipeDirection DetectKeyboardEmulatedSwipe()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 mSwipeValue = new Vector2(horizontal, vertical);
        bool noHorizontalInputPreviously = Mathf.Abs(m_LastHorizontalValue) < float.Epsilon;
        bool noVerticalInputPreviously = Mathf.Abs(m_LastVerticalValue) < float.Epsilon;

        m_LastHorizontalValue = horizontal;
        m_LastVerticalValue = vertical;
        if (vertical > 0f && noVerticalInputPreviously)
            return SwipeDirection.UP;
        if (vertical < 0f && noVerticalInputPreviously)
            return SwipeDirection.DOWN;
        if (horizontal > 0f && noHorizontalInputPreviously)
            return SwipeDirection.RIGHT;
        if (horizontal < 0f && noHorizontalInputPreviously)
            return SwipeDirection.LEFT;
        return SwipeDirection.NONE;
    }
}

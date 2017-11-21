using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 射线检测
/// </summary>
public class VitoBaseRaycaster : MonoBehaviour
{

    public event Action<RaycastHit> OnRaycasthit;
    public bool mIsLeft = false;
    public bool mIsDebug = false;
    [SerializeField]
    private Transform mOriginalPoint;
    [SerializeField]
    private LayerMask mExclusionLayers;
    [SerializeField]
    private bool mShowDebugRay;
    [SerializeField]
    private float mDebugRayLength = 5f;
    [SerializeField]
    private float mDebugRayDuration = 1f;
    [SerializeField]
    private float mRayLength = 50000f;
    [SerializeField]
    private VitoVRInput mVRInput;

    [SerializeField]
    private LineRenderer mRayLine;

    [SerializeField]
    private VitoVRReticleStatic mReticle_static;
    [SerializeField]
    private VitoVRReticle mReticle;
    private VitoVRInteractiveItem mCurrentInteractible;
    private VitoVRInteractiveItem mLastInteractible;

    public VitoVRInteractiveItem CurrentInteractible
    {
        get { return mCurrentInteractible; }
    }

    private void OnEnable()
    {
        mVRInput.OnClick += HandleClick;
        mVRInput.OnDoubleClick += HandleDoubleClick;
        mVRInput.OnUp += HandleUp;
        mVRInput.OnDown += HandleDown;
        if (mIsLeft)
        {
            mVRInput.OnLeftTriggerDown += HandleTriggerDown;
            mVRInput.OnLeftTriggerUp += HandleTriggerUp;
            mVRInput.OnLeftTriggerClick += HandleTriggerClick;
        }
        else
        {
            mVRInput.OnRightTriggerDown += HandleTriggerDown;
            mVRInput.OnRightTriggerUp += HandleTriggerUp;
            mVRInput.OnRightTriggerClick += HandleTriggerClick;
        }
    }

    private void OnDisable()
    {
        mVRInput.OnClick -= HandleClick;
        mVRInput.OnDoubleClick -= HandleDoubleClick;
        mVRInput.OnUp -= HandleUp;
        mVRInput.OnDown -= HandleDown;

        if (mIsLeft)
        {
            mVRInput.OnLeftTriggerDown -= HandleTriggerDown;
            mVRInput.OnLeftTriggerUp -= HandleTriggerUp;
            mVRInput.OnLeftTriggerClick -= HandleTriggerClick;
        }
        else
        {
            mVRInput.OnRightTriggerDown -= HandleTriggerDown;
            mVRInput.OnRightTriggerUp -= HandleTriggerUp;
            mVRInput.OnRightTriggerClick -= HandleTriggerClick;
        }
    }


    private void Awake()
    {
        if (mOriginalPoint == null)
        {
            mOriginalPoint = transform;
        }
        if (mRayLine != null)
        {
            mRayLine.useWorldSpace = false;
            mRayLine.SetPosition(0, Vector3.zero);
        }
    }

    private void Update()
    {
        RayCast();
    }
    private void RayCast()
    {
        Ray ray = new Ray(mOriginalPoint.position, mOriginalPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, mRayLength,~(mExclusionLayers|LayerMask.NameToLayer("Ignore Raycast"))))
        {
            //Debug.Log(hit.collider.gameObject.name);
            //if (mIsDebug && VitoVRInput.GetButtonUp("Fire1"))
            //{
            //    GameObject go = hit.collider.gameObject;
            //    string goPath = go.name;
            //    while (go.transform.parent != null)
            //    {
            //        go = go.transform.parent.gameObject;
            //        goPath = go.name + "/" + goPath;
            //    }
            //    Debug.Log("raycastHit:" + goPath);
            //}
            VitoVRInteractiveItem interactible = hit.collider.GetComponent<VitoVRInteractiveItem>();
            mCurrentInteractible = interactible;
            if (interactible && interactible != mLastInteractible)
            {
                interactible.Over();
                if (mIsLeft)
                {
                    interactible.OverLeft();
                }
                else
                {
                    interactible.OverRight();
                }
            }

            if (interactible != mLastInteractible)
                DeactiveLastInteractible();
            mLastInteractible = interactible;

            if (mReticle)
                mReticle.SetPosition(hit);
            if (mReticle_static)
                mReticle_static.SetPosition(hit);

            if (OnRaycasthit != null)
            {
                OnRaycasthit(hit);
            }

            if (mShowDebugRay)
            {
                Debug.DrawLine(mOriginalPoint.position, hit.point, Color.red);
            }
            if (mRayLine != null)
            {
                mRayLine.SetPosition(0, Vector3.zero);
                mRayLine.SetPosition(1,mOriginalPoint.InverseTransformDirection( hit.point-mOriginalPoint.position));
            }
        }
        else
        {
            //Debug.Log("out");
            //Debug.Log(mLastInteractible);
            DeactiveLastInteractible();
            mCurrentInteractible = null;

            if (mRayLine != null)
            {
                mRayLine.SetPosition(0, Vector3.zero);
                mRayLine.SetPosition(1, new Vector3(0,0,5000));
            }

            if (mReticle)
                mReticle.SetPosition();
            if (mReticle_static)
                mReticle_static.SetPosition();
        }

    }

    private void DeactiveLastInteractible()
    {
        //Debug.Log(mLastInteractible);
        //Debug.Log(mCurrentInteractible);
        if (mLastInteractible == null)
            return;
        mLastInteractible.Out();
        if (mIsLeft)
        {
            //if (mCurrentInteractible != null)
                mLastInteractible.OutLeft();
        }
        else
        {
            //if (mCurrentInteractible != null)
                mLastInteractible.OutRight();
        }

        mLastInteractible = null;
    }

    private void HandleTriggerUp()
    {
        if (mCurrentInteractible != null)
        {
            if (mIsLeft)
            {
                if (mCurrentInteractible != null)
                    mCurrentInteractible.UpLeft();
            }
            else
            {
                if (mCurrentInteractible != null)
                    mCurrentInteractible.UpRight();
            }

        }

    }
    private void HandleTriggerDown()
    {
        if (mIsLeft)
        {
            if (mCurrentInteractible != null)
                mCurrentInteractible.DownLeft(mReticle);
        }
        else
        {
            if (mCurrentInteractible != null)
                mCurrentInteractible.DownRight(mReticle);
        }
    }
    private void HandleTriggerClick()
    {
        if (mIsLeft)
        {
            if (mCurrentInteractible != null)
                mCurrentInteractible.ClickLeft();
        }
        else
        {
            if (mCurrentInteractible != null)
                mCurrentInteractible.ClickRight();
        }
    }

    private void HandleUp()
    {
        if (mCurrentInteractible != null)
            mCurrentInteractible.Up();
    }

    private void HandleDown()
    {
        if (mCurrentInteractible != null)
            mCurrentInteractible.Down(mReticle);
    }

    private void HandleClick()
    {
        if (mCurrentInteractible != null)
            mCurrentInteractible.Click();
    }

    private void HandleDoubleClick()
    {
        if (mCurrentInteractible != null)
            mCurrentInteractible.DoubleClick();
    }

}

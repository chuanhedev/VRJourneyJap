using UnityEngine;
using System.Collections;

public class VitoVRRotatableItemByAxis : MonoBehaviour
{
    public enum HandType
    {
        left,
        right,
        both,
    }
    public enum Axis
    {
        x,
        y,
        z,
    }

    public VitoVRInteractiveItem mInteractiveItem;
    public Transform mPivot;
    public HandType mHandType = HandType.both;
    public Axis mAxis = Axis.y;

    private bool useLeft, useRight, isRightFirst, isLeftFirst = true;
    private Vector3 mLastPos;
    private Quaternion mTargetQ;

    private void Awake()
    {
#if HTCVIVE
        if(GetComponent<CapsuleCollider>()!=null)
        {
            GetComponent<CapsuleCollider>().enabled = true;
        }
#else
        if(GetComponent<CapsuleCollider>()!=null)
        {
            GetComponent<CapsuleCollider>().enabled = false;
        }
#endif
        if (mInteractiveItem == null)
            mInteractiveItem = GetComponent<VitoVRInteractiveItem>();

        if (mPivot == null)
            mPivot = transform;
    }

    private void Start()
    {
        switch (mHandType)
        {
            case HandType.left:
                useLeft = true;
                break;
            case HandType.right:
                useRight = true;
                break;
            case HandType.both:
                useLeft = true;
                useRight = true;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (useLeft)
            RotatoLeft();
        if (useRight)
            RotatoRight();
    }

    private void RotatoLeft()
    {
        if (mInteractiveItem.mReticleLeft != null)
        {
            if (isLeftFirst)
            {
                mLastPos = mInteractiveItem.mReticleLeft.mReticleTransform.position;
                mTargetQ = mPivot.rotation;
                isLeftFirst = false;
            }
            else
            {
                Vector3 mPos = mInteractiveItem.mReticleLeft.mReticleTransform.position;
                Vector3 from = mLastPos - mPivot.position;
                Vector3 to = mPos - mPivot.position;
                switch (mAxis)
                {
                    case Axis.x:
                        from.x = to.x = 0;
                        break;
                    case Axis.y:
                        from.y = to.y = 0;
                        break;
                    case Axis.z:
                        from.z = to.z = 0;
                        break;
                    default:
                        break;
                }

                Quaternion q = Quaternion.FromToRotation(from, to);
                mTargetQ = q * mTargetQ;

                mPivot.rotation = Quaternion.Lerp(mPivot.rotation, mTargetQ, 0.15f);// q * mPivot.rotation;
                mLastPos = mPos;
            }
        }
        else
            isLeftFirst = true;
    }

    private void RotatoRight()
    {
        if (mInteractiveItem.mReticleRight != null)
        {
            if (isRightFirst)
            {
                mLastPos = mInteractiveItem.mReticleRight.mReticleTransform.position;
                mTargetQ = mPivot.rotation;
                isRightFirst = false;
            }
            else
            {
                Vector3 mPos = mInteractiveItem.mReticleRight.mReticleTransform.position;
                Vector3 from = mLastPos - mPivot.position;
                Vector3 to = mPos - mPivot.position;
                switch (mAxis)
                {
                    case Axis.x:
                        from.x = to.x = 0;
                        break;
                    case Axis.y:
                        from.y = to.y = 0;
                        break;
                    case Axis.z:
                        from.z = to.z = 0;
                        break;
                    default:
                        break;
                }
                Quaternion q = Quaternion.FromToRotation(from, to);
                mTargetQ = q * mTargetQ;


                mPivot.rotation = Quaternion.Lerp(mPivot.rotation, mTargetQ, 0.15f);// q * mPivot.rotation;
                mLastPos = mPos;
            }
        }
        else
            isRightFirst = true;
    }
}

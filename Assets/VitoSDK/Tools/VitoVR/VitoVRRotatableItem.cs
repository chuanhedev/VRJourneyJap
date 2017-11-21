using UnityEngine;
using System.Collections;

public class VitoVRRotatableItem : MonoBehaviour {
    public VitoVRInteractiveItem mInteractiveItem;
    public Transform mPivot;

    private bool isFirst = true;
    private bool isRightFirst = true;
    private bool isLeftFirst = true;
    private Vector3 mLastPos; 
    private void Awake()
    {
        if (mInteractiveItem == null)
            mInteractiveItem = GetComponent<VitoVRInteractiveItem>();

        if (mPivot == null)
            mPivot = transform;
    }

    Quaternion mTargetQ;



    private void Update()
    {
        VitoVRReticle tempReticle = null;

        tempReticle = mInteractiveItem.mReticleRight;


        if (tempReticle != null)
        {
            if (isLeftFirst)
            {
                mLastPos = tempReticle.mReticleTransform.position;
                mTargetQ = mPivot.rotation;
                isLeftFirst = false;
            }
            else
            {
                Vector3 mPos = tempReticle.mReticleTransform.position;
                Quaternion q = Quaternion.FromToRotation(mLastPos - mPivot.position, mPos - mPivot.position);
                mTargetQ = q * mTargetQ;

                mPivot.rotation = Quaternion.Lerp(mPivot.rotation, mTargetQ, 0.15f);// q * mPivot.rotation;
                mLastPos = mPos;
            }
            //Rotete( isLeftFirst,mInteractiveItem.mReticleLeft);
            //return;
        }
        else
        {
            isLeftFirst = true;
        }

        //if (mInteractiveItem.mReticleRight != null)
        //{
        //    Rotete( isRightFirst, mInteractiveItem.mReticleRight);
        //    return;
        //}
        //else
        //{
        //    isRightFirst = true;
        //}
        //if(mInteractiveItem.mReticle!=null)
        //{
        //    Rotete( isFirst,mInteractiveItem.mReticle);
        //    return;
            
        //}else
        //{
        //    isFirst = true;
        //}
    }


    private void Rotete( bool isFirst, VitoVRReticle reticle)
    {
        if (isFirst)
        {
            mLastPos = mInteractiveItem.mReticle.mReticleTransform.position;
            mTargetQ = mPivot.rotation;
            isFirst = false;
        }
        else
        {
            Vector3 mPos = mInteractiveItem.mReticle.mReticleTransform.position;
            Quaternion q = Quaternion.FromToRotation(mLastPos - mPivot.position, mPos - mPivot.position);
            mTargetQ = q * mTargetQ;

            mPivot.rotation = Quaternion.Lerp(mPivot.rotation, mTargetQ, 0.12f);// q * mPivot.rotation;
            mLastPos = mPos;
        }
    }



}

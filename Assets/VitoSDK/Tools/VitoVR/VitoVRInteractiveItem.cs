using UnityEngine;
using System.Collections;
using System;
public class VitoVRInteractiveItem : MonoBehaviour {

    public event Action OnOver;
    public event Action OnOut;
    public event Action OnClick;
    public event Action OnDoubleClick;
    public event Action OnUp;
    public event Action OnDown;

    public event Action OnLeftOver;
    public event Action OnLeftOut;
    public event Action OnLeftClick;
    public event Action OnLeftUp;
    public event Action OnLeftDown;

    public event Action OnRightOver;
    public event Action OnRightOut;
    public event Action OnRightClick;
    public event Action OnRightUp;
    public event Action OnRightDown;


    [HideInInspector] public VitoVRReticle mReticle;
    [HideInInspector]
    public VitoVRReticle mReticleLeft;
    [HideInInspector]
    public VitoVRReticle mReticleRight;


    protected bool mIsOver;
    public bool IsOver
    {
        get { return mIsOver; }
    }
    
    public void OverLeft()
    {
        if (OnLeftOver != null) OnLeftOver();
    }

    public void OverRight()
    {
        if (OnRightOver != null) OnRightOver();
    }
    public void OutLeft()
    {
        mReticleLeft = null;
        if (OnLeftOut != null) OnLeftOut();
    }
    public void OutRight()
    {
        mReticleRight = null;
        if (OnRightOut != null) OnRightOut();
    }
    public void ClickLeft()
    {
        if (OnLeftClick != null) OnLeftClick();
    }

    public void ClickRight()
    {
        if (OnRightClick != null) OnRightClick();
    }

    public void UpLeft()
    {
        mReticleLeft = null;
        if (OnLeftUp != null) OnLeftUp();
    }
    public void UpRight()
    {
        mReticleRight = null;
        if (OnRightUp != null) OnRightUp();
    }
    public void DownLeft(VitoVRReticle reticle)
    {
        mReticleLeft = reticle;
        if (OnLeftDown != null) OnLeftDown();
    }
    public void DownRight(VitoVRReticle reticle)
    {
        mReticleRight = reticle;
        if (OnRightDown != null) OnRightDown();
    }

    public void Over()
    {
        mIsOver = true;
        if (OnOver != null)
            OnOver();
    }
    

    public void Out()
    {
        mIsOver = false;
        mReticle = null;
        if (OnOut != null)
            OnOut();
    }


    public void Click()
    {
        if (OnClick != null)
            OnClick();
    }


    public void DoubleClick()
    {
        if (OnDoubleClick != null)
            OnDoubleClick();
    }

    public void Up()
    {
        mReticle = null;
        if (OnUp != null)
            OnUp();
    }


    public void Down(VitoVRReticle reticle=null)
    {
        mReticle = reticle;
        if (OnDown != null)
            OnDown();
    }




}

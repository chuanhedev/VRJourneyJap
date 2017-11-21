using UnityEngine;
using System.Collections;

public class VitoVRReticle : MonoBehaviour {

    public static VitoVRReticle instance { get; private set; }
    /// <summary>
    /// 當前紅點是否顯示
    /// </summary>
    public static bool enableReticle
    {
        
         get {  return instance!=null?  instance.mReticleTransform.gameObject.activeSelf:false; }
          set
        {
            if (instance!=null)
            {
                instance.mReticleTransform.gameObject.SetActive(value);
            }
            
        }
    }
    /// <summary>
    /// gearVR下為true,視綫位于頭部
    /// </summary>
    public bool mIsHead = false;
    /// <summary>
    /// 默認情況下小紅點是否可見
    /// </summary>
    public bool mDefaultEnable = true;
    [SerializeField]
    private float mDefaultDistance = 5f;
    [SerializeField]
    private bool mUseNormal;
    [SerializeField]
    private SpriteRenderer mImage;
    [SerializeField]
    public Transform mReticleTransform;
    [SerializeField]
    private Transform mOriginalPoint;
    [SerializeField]
    private bool mIsLeft;

    [SerializeField]
    private Animation mAnim;

    public VitoVRInput mInput;

    private Vector3 mOriginalScale;
    private Quaternion mOriginalRotation;


    public bool UseNormal
    {
        
        get
        {
            return mUseNormal;
        }
        set
        {
            mUseNormal = value;
        }
    }
    public Transform ReticleTransform { get { return mReticleTransform; } }


    private void Awake()
    {
        if(mIsHead)
        {
            if (instance != null && instance.gameObject != gameObject)
            {
                Destroy(instance.gameObject);
            }
            instance = this;
            
        }
        if(mDefaultEnable)
        {
            mReticleTransform.gameObject.SetActive(true);
        }else
        {
            mReticleTransform.gameObject.SetActive(false);
        }
        // Store the original scale and rotation.
        mOriginalScale = mReticleTransform.localScale;
        mOriginalRotation = mReticleTransform.localRotation;
    }
    
    private void OnEnable()
    {
        if(mIsLeft)
        {
            mInput.OnLeftTrigger += OnTrigger;
        }else
        {
            //Debug.Log("onRight");
            mInput.OnRightTrigger += OnTrigger;
        }
       
    }
    private void OnDisable()
    {
        if (mIsLeft)
        {
            mInput.OnLeftTrigger -= OnTrigger;
        }
        else
        {
            mInput.OnRightTrigger -= OnTrigger;
        }
    }


    public void OnTrigger(float v)
    {
        //Debug.Log(v);
        //if(v>0)
        {
            //Debug.Log(v);
            mAnim.clip.SampleAnimation(mAnim.gameObject,v);
        }
    }

    public void Hide()
    {
        mImage.enabled = false;
    }


    public void Show()
    {
        mImage.enabled = true;
    }

    public bool effectEX = false;
    public float effectSpeed = 10;
    // This overload of SetPosition is used when the the VREyeRaycaster hasn't hit anything.
    public void SetPosition()
    {
        // Set the position of the reticle to the default distance in front of the camera.
        mReticleTransform.position = mOriginalPoint.position + mOriginalPoint.forward * mDefaultDistance;
        if (effectEX)
        {
            mReticleTransform.localScale = Vector3.Slerp(mReticleTransform.localScale, mOriginalScale * mDefaultDistance, Time.unscaledDeltaTime * effectSpeed);
        }
        else
        {
            // Set the scale based on the original and the distance from the camera.
            mReticleTransform.localScale = mOriginalScale * mDefaultDistance;
        }
        // The rotation should just be the default.
        mReticleTransform.localRotation = mOriginalRotation;
    }


    // This overload of SetPosition is used when the VREyeRaycaster has hit something.
    public void SetPosition(RaycastHit hit)
    {
        mReticleTransform.position = hit.point;
        if (effectEX)
        {
            mReticleTransform.localScale = Vector3.Slerp(mReticleTransform.localScale, mOriginalScale * hit.distance * 3, Time.unscaledDeltaTime * effectSpeed);
        }
        else
        {
            mReticleTransform.localScale = mOriginalScale * hit.distance;
        }


        // If the reticle should use the normal of what has been hit...
        if (mUseNormal)
            // ... set it's rotation based on it's forward vector facing along the normal.
            mReticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        else
            // However if it isn't using the normal then it's local rotation should be as it was originally.
            mReticleTransform.localRotation = mOriginalRotation;
    }


    
}

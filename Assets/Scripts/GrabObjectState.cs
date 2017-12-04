using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectState : MonoBehaviour
{
    [HideInInspector]
    public ObjectGripState objectGripState = ObjectGripState.None;
    [SerializeField]
    Transform parent;
    Rigidbody _rigidbody;
    public bool IsHandTrigger { get; set; }
    bool isMaster;

    public Transform Parent
    {
        get
        {
            return parent;
        }
    }

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (VitoPlugin.CT == CtrlType.Admin) return;

        if (VitoPlugin.isMaster && !isMaster)
        {
            isMaster = true;
            _rigidbody.MovePosition(transform.position + Vector3.up * 0.01f);
        }
        else
        {
            if (!VitoPlugin.isMaster)
                isMaster = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        RigidHand rigidHand = other.transform.GetComponentInParent<RigidHand>();

        if (rigidHand)
        {
            IsHandTrigger = true;
        }
        else
        {
            return;
        }
        if (objectGripState != ObjectGripState.None) return;
        if (FacadeManager._instance.vitoMode == VitoMode.Ctrl) return;

        HandGrabController handGrabController = rigidHand.GetComponentInChildren<HandGrabController>();
        AddForceWithHand(handGrabController.HandVelocity);
        //Debug.Log("AddForceForHand-------");
    }

    void OnTriggerExit(Collider other)
    {
        RigidHand rigidHand = other.transform.GetComponentInParent<RigidHand>();

        if (rigidHand)
        {
            IsHandTrigger = false;
        }
    }

    void AddForceWithHand(Vector3 handVelocity)
    {
        _rigidbody.AddForce(handVelocity * _rigidbody.mass * 30, ForceMode.Force);
    }

    /// <summary>
    /// trigger
    /// </summary>
    /// <param name="isTrigger"></param>
    public void SetTrigger(bool isTrigger)
    {
        Collider[] colliders = GetComponents<Collider>();

        if (colliders == null) return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].isTrigger = isTrigger;
        }
        // Debug.Log("SetTrigger-------");
    }

    /// <summary>
    /// 重置物体
    /// </summary>
    public void ResetObject()
    {
        transform.rotation = parent.rotation;
        transform.position = parent.position;
    }

    void Init()
    {
        if (!_rigidbody)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.drag = 4;
        }
        if (VitoPlugin.CT == CtrlType.Admin)
        {
            _rigidbody.isKinematic = true;
        }

        gameObject.layer = LayerMask.NameToLayer("GrabObject");
    }
}

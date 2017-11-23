using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectState : MonoBehaviour
{
    [HideInInspector] public ObjectGripState objectGripState = ObjectGripState.None;
    Rigidbody _rigidbody;
    Vector3 defPos;
    Quaternion defRotate;

    public Vector3 DefPos
    {
        get
        {
            return defPos;
        }

        set
        {
            defPos = value;
        }
    }

    public Quaternion DefRotate
    {
        get
        {
            return defRotate;
        }

        set
        {
            defRotate = value;
        }
    }

    void Awake()
    {
        Init();
    }

    void OnTriggerEnter(Collider other)
    {
        if (objectGripState != ObjectGripState.None) return;

        RigidHand rigidHand = other.transform.GetComponentInParent<RigidHand>();
        if (!rigidHand) return;

        HandGrabController handGrabController = rigidHand.GetComponentInChildren<HandGrabController>();
        AddForceForHand(handGrabController.HandVelocity);

        //Debug.Log("AddForceForHand-------");
    }

    void AddForceForHand(Vector3 handVelocity)
    {
        _rigidbody.AddForce(handVelocity * _rigidbody.mass * 30, ForceMode.Force);
    }

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

    void Init()
    {
        defPos = transform.position;
        defRotate = transform.rotation;
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.drag = 4;
        gameObject.layer = LayerMask.NameToLayer("GrabObject");
    }
}

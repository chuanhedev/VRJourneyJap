using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectState : MonoBehaviour
{
    [HideInInspector] public ObjectGripState objectGripState = ObjectGripState.None;
    Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        gameObject.layer = LayerMask.NameToLayer("GrabObject");
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
}

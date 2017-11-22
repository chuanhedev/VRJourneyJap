using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public enum ObjectGripState
{
    None,
    Right,
    Left
}

public class HandGrabController : MonoBehaviour
{
    Hand hand;
    HandManager handManager;
    bool isGrab;//是否抓取

    void Awake()
    {
        handManager = GetComponent<HandManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hand = handManager._hand;

        if (hand != null)
        {
            float grabAngel = hand.PinchStrength;
            if (grabAngel > 0.65f)
            {
                isGrab = true;
                //Debug.Log(grabAngel + " " + hand.IsRight);
            }
            else
            {
                isGrab = false;
            }

        }
    }

    /// <summary>
    /// 拇指
    /// </summary>
    public Finger Thumb
    {
        get
        {
            if (hand != null)
            {
                return hand.Fingers[0];
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 手速度
    /// </summary>
    public Vector3 HandVelocity
    {
        get
        {
            return FingersVelocity();
        }
    }

    public bool IsGrab
    {
        get
        {
            return isGrab;
        }

        set
        {
            isGrab = value;
        }
    }

    public Hand Hand
    {
        get
        {
            return hand;
        }

        set
        {
            hand = value;
        }
    }

    /// <summary>
    /// 手指速度
    /// </summary>
    /// <returns></returns>
    private Vector3 FingersVelocity()
    {
        if (hand != null)
        {
            Vector3 velocity = Vector3.zero;
            List<Finger> fingers = hand.Fingers;
            foreach (Finger finger in fingers)
            {
                velocity += finger.TipVelocity.ToVector3();
            }
            return (hand.PalmVelocity.ToVector3() + velocity) / 6;
        }
        else
        {
            return Vector3.zero;
        }
    }
}

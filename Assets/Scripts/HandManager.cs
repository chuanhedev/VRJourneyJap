using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    LeapProvider leapProvider;
    [SerializeField] bool isRight;

    private void Awake()
    {
        leapProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    public Hand _hand
    {
        get
        {
            return GetHand();
        }
    }

    Hand GetHand()
    {
        Hand hand;
        Frame frame = leapProvider.CurrentFrame;
        if (frame == null) return null;

        //伸出去的第一只手是0
        if (isRight)
        {
            if (frame.Hands[0].IsRight)
            {
                hand = frame.Hands[0];
            }
            else
            {
                hand = frame.Hands[1];
            }
        }
        else
        {
            if (frame.Hands[0].IsLeft)
            {
                hand = frame.Hands[0];
            }
            else
            {
                hand = frame.Hands[1];
            }
        }
        return hand;
    }
}

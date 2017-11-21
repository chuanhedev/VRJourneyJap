using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace com.vito.plugin.demo
{
    [RequireComponent(typeof(Animator))]
    public class HostUIAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string onEnterAnim;
        public string onExitAnim;

        public Animator mAnimator;
        void OnEnable()
        {
            mAnimator = GetComponent<Animator>();
        }
        public void OnPointerEnter(PointerEventData data)
        {
            mAnimator.Rebind();
            mAnimator.Play(onEnterAnim, 0);
        }
        public void OnPointerExit(PointerEventData data)
        {
            mAnimator.Rebind();
            mAnimator.Play(onExitAnim, 0);
        }

        // Use this for initialization
        void Start()
        {

        }

    }

}


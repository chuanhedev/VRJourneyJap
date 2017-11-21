using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace com.vito.plugin.demo
{
    [RequireComponent(typeof(Toggle))]
    public class HostToggleWithContent : MonoBehaviour
    {
        private Toggle mToggle;
        public Transform mContent;

        private void Awake()
        {
            mToggle = GetComponent<Toggle>();
            if(mToggle)
                mToggle.onValueChanged.AddListener(OnValueChange);
        }
        void Start()
        {
            OnValueChange(mToggle.isOn);
        }
        private void OnDestroy()
        {
            if(mToggle)
                mToggle.onValueChanged.RemoveListener(OnValueChange);
        }
        public void SetOffToggle()
        {
            if (mToggle)
            mToggle.isOn = false;
        }
        void OnValueChange(bool isOn)
        {
            if(isOn)
            {
               
                //mToggle.graphic.gameObject.SetActive(true);
                if(mContent)
                    mContent.gameObject.SetActive(true);
            }else
            {
                //mToggle.graphic.gameObject.SetActive(false);
                if (mContent)
                    mContent.gameObject.SetActive(false);
            }
        }



    }
}


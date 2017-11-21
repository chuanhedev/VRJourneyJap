using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace com.vito.plugin.demo
{
    [RequireComponent(typeof(Toggle))]
    public class HostToggle : MonoBehaviour
    {
        private Toggle mToggle;

        private void Awake()
        {
            mToggle = GetComponent<Toggle>();
            if (mToggle)
                mToggle.onValueChanged.AddListener(OnValueChange);
        }
        private void OnDestroy()
        {
            if (mToggle)
                mToggle.onValueChanged.RemoveListener(OnValueChange);
        }
        void OnValueChange(bool isOn)
        {
            if(justUI)
            {                
                justUI = false;
                return;
            }
            HostUIManager.instance.OnToggleValueChange(this.transform.name,isOn);
        }
        private bool justUI = false;
        public void SetToggleValue(bool isOn)
        {
            if(mToggle)
            {
                if(mToggle.isOn!=isOn)
                {
                    justUI = true;
                    mToggle.isOn = isOn;
                    
                }
                
            }
            
        }


    }
}

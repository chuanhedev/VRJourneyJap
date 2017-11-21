using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace com.vito.plugin.demo
{
    public class CustomCmdBtn : MonoBehaviour
    {
        public static bool isNGUI = false;

        void OnEnable()
        {
            {
                GetComponent<Button>().onClick.AddListener(OnUGUIBtnClick);
            }
        }
        void OnDisable()
        {
            {
                GetComponent<Button>().onClick.RemoveListener(OnUGUIBtnClick);
            }
        }

        void OnUGUIBtnClick()
        {
            VitoPlugin.RequestActionEvent(this.gameObject.name);
        }
        // Use this for initialization
        void Start()
        {

        }
    }

}


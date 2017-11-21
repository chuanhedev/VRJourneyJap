using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.vito.plugin.demo
{
    public class SyncClientObjTest : MonoBehaviour
    {

        public bool move;
        public bool rotate;
        public bool scale;

        private Vector3 moveValue;
        private Vector3 rotateValue;
        private Vector3 scaleValue;
        // Use this for initialization
        void Start()
        {
            moveValue = Random.insideUnitSphere;
            rotateValue = Random.insideUnitSphere * 30;
            scaleValue = Random.insideUnitSphere;
        }

        void Update()
        {
            if (VitoPlugin.ClientHasPermission)
            {
                if (move)
                {
                    transform.position += moveValue * Time.deltaTime;
                    if (transform.position.sqrMagnitude > 100)
                    {
                        moveValue = -moveValue;
                        transform.position += moveValue * Time.deltaTime;
                    }
                }
                if (rotate)
                {
                    transform.eulerAngles += Time.deltaTime * rotateValue;
                }
                if (scale)
                {
                    transform.localScale += scaleValue * Time.deltaTime;
                    if (transform.localScale.sqrMagnitude > 8)
                    {
                        scaleValue = -scaleValue;
                        transform.localScale += scaleValue * Time.deltaTime;
                    }
                }
            }
        }
    }
}
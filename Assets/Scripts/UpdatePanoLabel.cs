using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePanoLabel
{

    public void UpdateLabel(GameObject pano_LabelGo, string panoPath)
    {

        string panoCity = panoPath.Substring(0, panoPath.IndexOf('/'));
        string panoLocal = panoPath.Substring(panoPath.IndexOf('/') + 1);

        Debug.Log(panoCity + " " + panoLocal);

        Transform panoCityTransform = pano_LabelGo.transform.Find(panoCity);
        panoCityTransform.gameObject.SetActive(true);

        for (int i = 0; i < panoCityTransform.childCount; i++)
        {
            if (panoCityTransform.GetChild(i).name == panoLocal)
            {
                panoCityTransform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                panoCityTransform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

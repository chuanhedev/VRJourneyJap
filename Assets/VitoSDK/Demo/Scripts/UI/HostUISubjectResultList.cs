using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostUISubjectResultList : MonoBehaviour {
    HostUISubjectListData data;
    private void OnEnable()
    {
        GetComponent<Text>().text = data.ListTitle;
        HostUISubjectResultManager.instance.ShowList(data);
    }
}

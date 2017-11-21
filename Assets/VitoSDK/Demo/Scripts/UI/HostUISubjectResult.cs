using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostUISubjectResult : MonoBehaviour {
    public HostUISubjectData data;
    public void ShowResult()
    {
        HostUISubjectResultManager.instance.ShowResult(data);
    }
}

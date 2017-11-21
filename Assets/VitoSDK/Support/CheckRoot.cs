using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class CheckRoot : SingleInstance<CheckRoot> {
    public string username;
    public string password;
    public bool isSuccess = false;

    public override void OnAwkae()
    {
        base.OnAwkae();
        isSuccess = false;
    }

    // Use this for initialization
    IEnumerator Start () {
        WWW www = new WWW("http://121.40.93.137:8888/CheckRoot.txt");
        yield return www;
        //Debug.Log("CheckRoot txt :" + www.text);
        if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text))
        {
            JsonData jd = JsonMapper.ToObject(www.text);
            if (jd.Contains(username) && (string)jd[username] == password)
            {
                isSuccess = true;
            }
        }
        if (isSuccess)
        {

        }
        else
        {
            Application.Quit();
        }
        yield break;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

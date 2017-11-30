using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    List<Quaternion> qList = new List<Quaternion>();
    int userId;
    string scene;
    int i = 0;

    void Start()
    {
        //UserInfoData userInfoData = UserInfoManager.instance.getHistoryUserData("2c4552f8145bc7909196bdb433ad0ac7");
        Debug.Log(VitoPlugin.DeviceID);

        SpeechController speechController = SpeechController.instance;

        string[] files = Directory.GetFiles(Application.persistentDataPath + "/SpeechData");

        for (int i = 0; i < files.Length; i++)
        {
            // transform.rotation = qList[i];
            Debug.Log(files[i]);
        }
        speechController.GetRecordData(files[0], out userId, out scene, out qList);
        Debug.Log(files[0]);
        Debug.Log(scene);
        Debug.Log(userId);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(transform.rotation);

        transform.rotation = Quaternion.Lerp(transform.rotation, qList[i], 0.12f);
        i++;
        Debug.Log(qList[i]);

    }
}

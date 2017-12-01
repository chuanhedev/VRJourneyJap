using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayBackController : MonoBehaviour
{
    public static PlayBackController instance;
    List<Quaternion> qList = new List<Quaternion>();
    List<GameObject> micStudentItemGoList = new List<GameObject>();
    string diviceId;
    string scene;
    int readCount = 0;
    Transform head;
    bool isPlayBack;
    AudioSource audioSource;
    [SerializeField]
    HostUIManager hostUIManager;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    StartPlayBack();
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    StopPlayBack();
        //}
    }

    void FixedUpdate()
    {
        if (VitoPlugin.CT == CtrlType.Player) return;
        if (!head) return;
        if (!isPlayBack) return;
        if (readCount >= qList.Count) return;

        head.rotation = Quaternion.Lerp(head.rotation, qList[readCount], 0.12f);
        readCount++;
    }

    public void InitData(Transform head, AudioSource audioSource)
    {
        this.head = head;
        this.audioSource = audioSource;
    }

    public bool StartPlayBack(string deviceId)
    {
        try
        {
            if (!head || !audioSource)
            {
                Debug.Log("未初始化InitData");
                return false;
            }

            StopAllCoroutines();

            LoadSpeechJsonData(deviceId);

            LoadMic(deviceId);
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void PausePlayBack()
    {
        isPlayBack = false;
        if (audioSource.isPlaying) audioSource.Pause();
    }

    /// <summary>
    /// 继续
    /// </summary>
    public void ContinuePlayBack()
    {
        isPlayBack = true;
        if (!audioSource.isPlaying) audioSource.Play();
    }

    public void StopPlayBack()
    {
        isPlayBack = false;
        hostUIManager.MicToggleEnable(false);
    }

    void LoadSpeechJsonData(string deviceId)
    {
        readCount = 0;
        string jsonDataPath = @"D:\server\speech\" + deviceId + ".txt";
        SpeechController.instance.GetRecordData(jsonDataPath, out diviceId, out scene, out qList);
    }

    void LoadMic(string deviceId)
    {
        //string[] files = Directory.GetFiles(@"D:\server\upload");
        string jsonDataPath = @"D:\server\upload\" + deviceId + ".wav";
        StartCoroutine(LoadAudio(jsonDataPath));
    }

    IEnumerator LoadAudio(string recordPath)
    {
        WWW www = new WWW("file://" + recordPath);
        yield return www;
        AudioClip audioClip = www.GetAudioClip();
        audioSource.clip = audioClip;
        audioSource.Play();
        isPlayBack = true;
    }

    /// <summary>
    /// 实例化学生录音列表
    /// </summary>
    /// <param name="studentMicListItem"></param>
    public void CreateStudentItem(GameObject studentMicListItem, Transform studentMicListContent)
    {
        try
        {
            string diviceId;
            string scene;
            List<Quaternion> qList = new List<Quaternion>();
            string[] files = Directory.GetFiles(@"D:\server\speech");

            ClearStudentItem();

            for (int i = 0; i < files.Length; i++)
            {
                SpeechController.instance.GetRecordData(files[i], out diviceId, out scene, out qList);
                GameObject micStudentItemGo = Instantiate(studentMicListItem, studentMicListContent);
                MicStudentItem micStudentItem = micStudentItemGo.GetComponent<MicStudentItem>();
                micStudentItem.InitData(diviceId, scene, qList);
                micStudentItemGoList.Add(micStudentItemGo);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    /// <summary>
    /// 清除学生列表
    /// </summary>
    void ClearStudentItem()
    {
        for (int i = 0; i < micStudentItemGoList.Count; i++)
        {
            Destroy(micStudentItemGoList[i]);
        }
        micStudentItemGoList.Clear();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayBackController : MonoBehaviour
{
    public static PlayBackController instance;
    List<Quaternion> qList = new List<Quaternion>();
    List<string> micFileNameList = new List<string>();
    Dictionary<string, GameObject> micStudentItemGoDict = new Dictionary<string, GameObject>();
    string diviceId;
    string panoPath;
    string micTimer;
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

    void FixedUpdate()
    {
        if (VitoPlugin.CT == CtrlType.Player) return;
        if (!head) return;
        if (!isPlayBack) return;
        if (readCount >= qList.Count)
        {
            StopPlayBack(); return;
        }

        head.rotation = Quaternion.Lerp(head.rotation, qList[readCount], 0.1f);
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
        readCount = 0;
        hostUIManager.MicToggleEnable(false);
    }

    void LoadSpeechJsonData(string deviceId)
    {
        readCount = 0;
        string jsonDataPath = @"D:\server\speech\" + deviceId + ".txt";
        SpeechController.instance.GetRecordData(jsonDataPath, out diviceId, out panoPath, out micTimer, out qList);
    }

    void LoadMic(string deviceId)
    {
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
    /// 更新录音列表
    /// </summary>
    /// <param name="studentMicListItem"></param>
    /// <param name="studentMicListContent"></param>
    /// <returns></returns>
    public IEnumerator UpdateStudentItem(GameObject studentMicListItem, Transform studentMicListContent)
    {
        yield return 0;

        try
        {
            string diviceId = "";
            string panoPath = "";
            string micTimer = "";
            string[] files = Directory.GetFiles(@"D:\server\speech");

            int filesCount = files.Length;
            int studentItemCount = micStudentItemGoDict.Count;
            if (filesCount > studentItemCount)
            {
                //增加
                for (int i = 0; i < files.Length; i++)
                {
                    if (!micStudentItemGoDict.ContainsKey(files[i]))
                    {
                        GameObject micStudentItemGo = Instantiate(studentMicListItem, studentMicListContent);

                        micStudentItemGoDict.Add(files[i], micStudentItemGo);
                    }
                }
            }
            else if (filesCount < studentItemCount)
            {
                //删减
                List<string> removeKeyList = new List<string>();

                foreach (string key in micStudentItemGoDict.Keys)
                {
                    bool exist = false;
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (key == files[i])
                        {
                            //有文件
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        removeKeyList.Add(key);
                    }
                }

                for (int i = 0; i < removeKeyList.Count; i++)
                {
                    Destroy(micStudentItemGoDict[removeKeyList[i]]);
                    micStudentItemGoDict.Remove(removeKeyList[i]);
                }
            }
            UpdateStudentItemData(diviceId, panoPath, micTimer);
        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    void UpdateStudentItemData(string diviceId, string panoPath, string micTimer)
    {
        foreach (string studentItemFile in micStudentItemGoDict.Keys)
        {
            SpeechController.instance.GetRecordData(studentItemFile, out diviceId, out panoPath, out micTimer, out qList);
            micStudentItemGoDict[studentItemFile].GetComponent<MicStudentItem>().InitData(diviceId, panoPath, micTimer, qList);
        }
    }
}

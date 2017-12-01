using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MicRecorder : MonoBehaviour
{
    //[SerializeField] Text text;
	private AudioClip clip;
	[HideInInspector]
	public string userName;
	[HideInInspector]
	public string serverUrl;
//	[HideInInspector]
//	public string serverFolder = "audio";
	[HideInInspector]
	public string localFolder = "audio";
    //录音的采样率
	const int samplingRate = 8000;
	[HideInInspector]
	public bool recording = false;
	[HideInInspector]
	public bool busy = false;
	[HideInInspector]
	public string uploadError;
	[HideInInspector]
	public string uploadResponse;

	public void Start(){
		localFolder = Application.persistentDataPath + "/" + localFolder;
	}
    // private TimerInfo timerInfo;
    /// <summary>
    /// 开始录音
    /// </summary>
    public void StartRecord()
    {
		if (recording)
			return;
		recording = true;
		string[] micDevices = Microphone.devices;
        if (micDevices.Length == 0)
        {
            //Util.Log("没有找到录音组件");
            //UpdateMessage("没有找到录音组件");
            return;
        }

        //Util.Log("录音时长为30秒");
        //UpdateMessage("录音时长为30秒");
        Microphone.End(null);//录音前先停掉录音，录音参数为null时采用的是默认的录音驱动

        try
        {
            clip = Microphone.Start(null, false, 300, samplingRate);

        }
        catch (Exception e)
        {
			Debug.Log("开始录音错误");
        }
    }


	public void Cancel(){
		try
		{
			Microphone.End(null);
		}
		catch (Exception e)
		{
			
		}
		recording = false;
		busy = false;
	}

    /// <summary>
    /// 停止录音
    /// </summary>
	public IEnumerator StopRecord()
    {
		if (recording && !busy) {
			busy = true;
            //TimerManager.StopTimerEvent(timerInfo);
            // TimerManager.RemoveTimerEvent(timerInfo);
            // index = 0;
            bool err = false;
            try
            {
                int audioLength;
                int position = Microphone.GetPosition(null);
                var soundData = new float[clip.samples * clip.channels];
                clip.GetData(soundData, 0);
                var newData = new float[position * clip.channels];
                //Copy the used samples to a new array
                for (int i = 0; i < newData.Length; i++)
                {
                    newData[i] = soundData[i];
                }
                var newClip = AudioClip.Create(clip.name,
                                  position,
                                  clip.channels,
                                  clip.frequency,
                                  false,
                                  false);
                newClip.SetData(newData, 0);        //Give it the data from the old clip

                //Replace the old clip
                AudioClip.Destroy(clip);
                clip = newClip;
            } catch (Exception e)
            {
                Debug.Log(e.StackTrace);
                err = true;
            }
            if (!err)
            {
                yield return SaveRecord();
                uploadError = MicUtils.error;
                uploadResponse = MicUtils.text;
            }

            recording = false;
            busy = false;
		} else {
			uploadError = "";
			uploadResponse = MicUploadResponse.Cancelled;
			recording = false;
			busy = false;
		}
    }

    /// <summary>
    /// 播放录音
    /// </summary>
    public void PlayRecord()
    {
        StopRecord();
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }

    /// <summary>
    /// 保存录音
    /// </summary>
	public IEnumerator SaveRecord()
    {
		string filename = DateTime.Now.ToString("yyMMddHHmmss") + ".wav";
		if (!string.IsNullOrEmpty (userName)) {
            //filename = userName + "_" + filename;
            filename = userName + ".wav";// + "_" + filename;
        }
		MicUtils.Save(clip, localFolder + "/" + filename);
		yield return MicUtils.Upload(localFolder + "/" + filename, serverUrl);
    }
}


public static class MicUploadResponse{
	public static string Cancelled = "cancelled";
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Pvr_UnitySDKAPI;

public enum MicControllerState{
	Idle,
	Ready,
	Recording,
	PostRecording
}

public class MicController : MonoBehaviour {
    //public Text message;
    public static MicController instance;
	public Text hint;
	public Text txtTimer;
	[HideInInspector]
	public MicControllerState state;
	private MicRecorder micRecorder;
	private float timer;
	private bool _enabled = true;
	private GameObject content;
	private bool txtTimerEnabled = true;
	public string serverPort = "8026";

	void Awake(){
        instance = this;
        micRecorder = GetComponent<MicRecorder> ();
		content = transform.GetChild (0).gameObject;
		SetState (MicControllerState.Idle);

        userName = VitoPlugin.UserId.ToString();
	}

	public string userName{
		get{
			return micRecorder.userName;
		}
		set{
			micRecorder.userName = value;
		}
	}

	void KeyDown(){
		if (state == MicControllerState.Idle) {
			micRecorder.serverUrl = "http://" + ConnectionClientConfig.logicServerIp + ":" + serverPort + "/fileupload";
			micRecorder.userName = ConnectionClientConfig.UserId.ToString ();
			timer = Time.time;
		}
	}

	void KeyHold(){
		if (state == MicControllerState.Idle) {
			if (timer != -1 && Time.time - timer > 1) {
				SetState (MicControllerState.Ready);
				timer = Time.time;
			}
		} else if (state == MicControllerState.Ready) {
			if (Time.time - timer > 3) {
				SetState (MicControllerState.Recording);
				//timer = Time.time;
			} else if (Time.time - timer > 2) {
				showImage (1);
			} else if (Time.time - timer > 1) {
				showImage (2);
			}
		} else if (state == MicControllerState.Recording) {
			UpdateTimer ();
		}
	}

	void KeyUp(){
		if (state == MicControllerState.Recording) {
			//timer = Time.time;
			txtTimerEnabled = false;
			hint.text = "录制完成，开始上传";
			StartCoroutine (StopRecord ());
		} else if(state == MicControllerState.Ready){
			SetState (MicControllerState.Idle);
		}
	}

	IEnumerator StopRecord(){
		yield return micRecorder.StopRecord ();
		Debug.Log (micRecorder.uploadError + " " + micRecorder.uploadResponse);
		if (string.IsNullOrEmpty (micRecorder.uploadError)) {
			showImage ("Done");
			hint.text = "上传成功";
		} else if (micRecorder.uploadError.Contains ("timeout")) {
			showImage ("Error");
			hint.text = "请求超时，请检查网络";
		} else {
			showImage ("Error");
			hint.text = "连接失败，请检查网络";
		}
		SetState (MicControllerState.PostRecording);
	}

	private void showImage(int t){
		content.GetChildByName ("Count").ShowChildByName (t.ToString ());
	}

	private void showImage(string s){
		content.GetChildByName ("Count").ShowChildByName (s);
	}

	void SetState(MicControllerState s){
		state = s;
		if (state == MicControllerState.Idle) {
			timer = -1;
			txtTimer.gameObject.SetActive (false);
			//hint.text = "wait";
			content.SetActive (false);
		}else if (state == MicControllerState.Ready) {
			content.SetActive (true);
			UI3DFollowCamera.instance.ResetPos();
			txtTimer.gameObject.SetActive (false);
            hint.text = "准备录音";
			showImage (3);
		}else if (state == MicControllerState.Recording) {
			timer = Time.time;
			txtTimerEnabled = true;
			txtTimer.gameObject.SetActive (true);
			micRecorder.StartRecord ();
			showImage (4);
			hint.text = "正在录音...松开即停止";
		}else if (state == MicControllerState.PostRecording) {
			StartCoroutine (waitAndGotoIdle (1));
		}

	}

	void UpdateTimer(){
		if (txtTimerEnabled) {
			int t = Mathf.FloorToInt (Time.time - timer);
			txtTimer.text = Mathf.FloorToInt (t / 60f).ToString () + ":" + (t % 60).ToString ("00");
		}
	}

	private IEnumerator waitAndGotoIdle(float t){
		yield return new WaitForSeconds (t);
		SetState (MicControllerState.Idle);
	}

	public bool enabled{
		get{
			return _enabled;
		}
		set{
			if(_enabled != value){
                _enabled = value;
                if (!_enabled)
                {
                    if (state == MicControllerState.Recording)
                    {
                        micRecorder.Cancel();
                    }
                    SetState(MicControllerState.Idle);
                }
                else
                {
					SetState(MicControllerState.Idle);
                }
            }
		}
	}

	// Update is called once per frame
	void Update () {
		if (!enabled)
			return;
		if (Controller.UPvr_GetKeyDown (Pvr_KeyCode.TOUCHPAD) || Input.GetKeyDown(KeyCode.A)) {
			//Debug.Log ("key down");
			KeyDown ();
		}

		if (Controller.UPvr_GetKey (Pvr_KeyCode.TOUCHPAD) || Input.GetKey(KeyCode.A)) {
			//Debug.Log ("key hold");
			KeyHold ();
		}

		if (Controller.UPvr_GetKeyUp (Pvr_KeyCode.TOUCHPAD) || Input.GetKeyUp(KeyCode.A)) {
			//Debug.Log ("key up");
			KeyUp ();
		}
	}
}

public static class Extention
{
    public static T ToEnum<T>(this string value)
    {
        return (T) Enum.Parse(typeof(T), value, true);
    }

	public static void ShowChildByName(this GameObject o, string name){
		o.SetActive (name != "null");
		for (int i = 0; i < o.transform.childCount; i++) {
			GameObject obj = o.transform.GetChild(i).gameObject;
			obj.SetActive (obj.name == name);
		}
	}


	public static GameObject GetChildByName(this GameObject o, string name){
		for (int i = 0; i < o.transform.childCount; i++) {
			GameObject obj = o.transform.GetChild(i).gameObject;
			if(obj.name == name)
				return obj;
		}
		return null;
	}
}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Text;
using System.IO;
using System;

public class NetItem{
	public string url { get; set; }
	public string json { get; set; }
	public WWWForm form { get; set; }
	public GameObject backGo { get; set; }
    public GameObject targetObj { get; set; }
	public string backFun { get; set; }
	public string parameter { get; set; }
}

public class MyWWW{
	public WWW www { get; set; }
	private string _text;
	private Texture texture;
	public string text { 
		get {
			if(www!=null && string.IsNullOrEmpty(_text))
				return www.text;
			else
				return _text;
		}
		set{
			_text = value;
		} 
	}
	public Texture tex { 
		get {
			if(www!=null)
				return www.texture;
			else
				return null;
		}
		set{
			     texture = value;
		} 
	}
	public Texture[] texs { get; set; }
	public NetItem netItem { get; set; }
}
public delegate void NetDelegate(MyWWW www);
public class NetManager : MonoBehaviour {

    public static NetManager instance { get; set; }
	private  float downloadIntervalTime = 0.2f;

	private float downloadDelay = 0;

	private bool isdownloading = false;
	private List<NetItem> downloadQueue = new List<NetItem>();

	private void Awake ()
	{
        instance = this;
	}
	void Update () {
		if (isdownloading)
			return;
		downloadDelay += Time.unscaledDeltaTime;
		if (downloadDelay > downloadIntervalTime&& downloadQueue.Count > 0) {
			isdownloading = true;
			downloadDelay = 0;
			StartDownloadRequest(downloadQueue[0],null);
			downloadQueue.RemoveAt(0);
		}
	}


    /// <summary>
    /// 把文件下载请求加入下载队列
    /// </summary>
    /// <param name="netitem"></param>
	public void EnqueDownloadItem(NetItem netitem)
	{
		downloadQueue.Add (netitem);
	}

    /// <summary>
    /// 开始执行文件下载请求
    /// </summary>
    /// <param name="netitem"></param>
    /// <param name="callback">下载结束的回掉函数</param>
	public void ProcessDownloadItem(NetItem netitem,NetDelegate callback)
	{
		StartCoroutine(IStartDownloadRequest(netitem, callback));
	}

	void StartDownloadRequest(NetItem netitem,NetDelegate netDeleagte)
	{
		try {
			StartCoroutine(IStartDownloadRequest(netitem,netDeleagte));
		} catch (System.Exception ex) {
			Debug.LogException(ex);
		}

	}
	IEnumerator IStartDownloadRequest(NetItem netitem,NetDelegate netDeleagte)
	{
		MyWWW mw = new MyWWW ();
		mw.netItem = netitem;

		WWW www = null;
        if (netitem.form!=null)
		{
			www = new WWW (netitem.url,netitem.form);
		}
		else if(!string.IsNullOrEmpty(netitem.json))
		{
			byte[] bytes= Encoding.UTF8.GetBytes(netitem.json);
			www = new WWW (netitem.url,bytes);
		}
		else
		{
			www = new WWW (netitem.url);
		}
		yield return www;
		if(string.IsNullOrEmpty (www.error)) {
			mw.www = www;
		}
		else
		{
			string errorlog = "url:"+  netitem.url + " error:" + www.error;
		}
		if (netitem.backGo != null && !string.IsNullOrEmpty(netitem.backFun))
		{
			netitem.backGo.SendMessage(netitem.backFun, mw);
		}
		if(netDeleagte!=null)
			netDeleagte(mw);
		isdownloading = false;
	}

}

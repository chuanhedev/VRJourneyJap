using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
public class DataCache  {


    //public static Dictionary<int, SnakeSkin> skinMap = new Dictionary<int, SnakeSkin>();
    //public static Dictionary<int, SnakeSkin> skinNormalMap = new Dictionary<int, SnakeSkin>();
    //private static List<SnakeSkin> skinList = new List<SnakeSkin>();

    //public static Dictionary<int, Serverconfig> serverConfigMap = new Dictionary<int, Serverconfig>();
    //private static List<Serverconfig> serverConfinList = new List<Serverconfig>();

	public static void LoadData(string name,string content){
        switch(name)
        {
            //case "SnakeSkin":
            //    skinList = JsonConvert.DeserializeObject<List<SnakeSkin>>(content);
            //    foreach ( var item in skinList)
            //    {
            //        if(item.type==1)
            //        {
            //            skinNormalMap.Add(item.skinid,item);
            //        }
            //        skinMap.Add(item.skinid, item);
                   
            //    }
            //    break;
            //case "Serverconfig":
            //    serverConfinList = JsonConvert.DeserializeObject<List<Serverconfig>>(content);
            //    foreach(var item in serverConfinList)
            //    {
            //        serverConfigMap.Add(item.id,item);
            //    }
            //    break;
        }
	}




    private static Dictionary<string, GameObject> prefabPool = new Dictionary<string, GameObject>();
    private static Dictionary<string, AudioClip> soundPool = new Dictionary<string, AudioClip>();




    public static GameObject  loadPrefab(string prefabName)
    {
        if(prefabPool.ContainsKey(prefabName))

        {
            return prefabPool[prefabName];
        }else
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/"+prefabName);
            if(obj!=null)
            {
                prefabPool.Add(prefabName, obj);
                return obj;
            }
        }
        return null;
    }
    public static AudioClip  loadSound(string soundName)
    {
        if(soundPool.ContainsKey(soundName))
        {
            return soundPool[soundName];
        }
        else
        {
            AudioClip clip = Resources.Load<AudioClip>("Sound/"+soundName);
            if(clip!=null)
            {

                soundPool.Add(soundName,clip);
                return clip;
            }
        }
        return null;
    }
	// Use this for initialization
	void Start () {
				
	}
	

}

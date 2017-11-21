using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HostUISceneOptions : MonoBehaviour {

    private Dictionary<string, GameObject> sceneOptions = new Dictionary<string, GameObject>();
    public List<GameObject> optionList;
    private bool isOn = false;
    void Awake()
    {
        if(optionList!=null)
        {
            for(int i=0;i<optionList.Count;i++)
            {
                sceneOptions.Add(optionList[i].name,optionList[i]);
            }
        }
    }

    public  void  OnLevelWasLoaded()
    {
        foreach(var go in sceneOptions.Values)
        {
            go.SetActive(false);
        }
        if(isOn)
        {
            ShowOptions();
        }
    }

    public void ShowOptions()
    {
        isOn = true;
        string sceneName=UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        #if !UNITY_ANDROID
        DebugHealper.Log("ShowScene: "+sceneName+" option");
#endif
        if(sceneOptions.ContainsKey(sceneName))
        {
            sceneOptions[sceneName].SetActive(true);
        }
    }


    public void HideOptions()
    {
        isOn = false;
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneOptions.ContainsKey(sceneName))
        {
            sceneOptions[sceneName].SetActive(false);
        }
    }



}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;


    [ExecuteInEditMode]
    public class HostUIGetSubjectData : MonoBehaviour
    {
        void Awake()
        {

            mainDatas = new Dictionary<int, HostUI_SubjectData>();
            for (int i = 0; i < subjectDataList.Count; i++)
            {
                mainDatas.Add(subjectDataList[i].id, subjectDataList[i]);
            }
        }
        [SerializeField]
        private List<HostUI_SubjectData> subjectDataList;
        public List<HostUI_SubjectData> _SubjectDataList
        {
            get
            {
                return subjectDataList;
            }
        }

        [SerializeField]
        private Dictionary<int, HostUI_SubjectData> mainDatas;

        public HostUI_SubjectData getSubjectData(int id)
        {
            HostUI_SubjectData returnData = null;
            if (mainDatas == null)
            {
                mainDatas = new Dictionary<int, HostUI_SubjectData>();
                for (int i = 0; i < subjectDataList.Count; i++)
                {
                    mainDatas.Add(subjectDataList[i].id, subjectDataList[i]);
                }
            }
            mainDatas.TryGetValue(id, out returnData);
            return returnData;
        }

    public void SetSubjectData(HostUI_SubjectData[] data)
    {
        mainDatas = new Dictionary<int, HostUI_SubjectData>();
        for (int i = 0; i < data.Length; i++)
        {
            mainDatas.Add(data[i].id, data[i]);
        }
    }


#if UNITY_EDITOR
    public TextAsset jsonFile;


        [ContextMenu("ReadJsonFile")]
        public void ReadJsonFile()
        {
            if (jsonFile != null)
            {
                string jsonContent = jsonFile.text;
                subjectDataList = JsonConvert.DeserializeObject<List<HostUI_SubjectData>>(jsonContent);
                mainDatas = new Dictionary<int, HostUI_SubjectData>();
                for (int i = 0; i < subjectDataList.Count; i++)
                {
                    mainDatas.Add(subjectDataList[i].id, subjectDataList[i]);
                }
            }
        }
#endif

    }

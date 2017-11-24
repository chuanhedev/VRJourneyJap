using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablewareManager : MonoBehaviour
{
    public static TablewareManager _instance;
    public int Grade { get; set; }
    [SerializeField] GameObject tabelware;
    [HideInInspector] public List<GameObject> addedGrabObjectList = new List<GameObject>();

    void Awake()
    {
        _instance = this;
    }

    void OnEnable()
    {
        tabelware.SetActive(false);
    }

    public void CheckGrade()
    {
        tabelware.SetActive(true);
    }

    public void End()
    {
        Grade = 0;
        addedGrabObjectList.Clear();
        tabelware.SetActive(false);
    }
     
    public void AddGrade(int value)
    {
        Grade += value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeController : MonoBehaviour
{
    [SerializeField] Animator gradeAnim;
    Text gradeText;
    GameObject bg_pass;
    GameObject bg_NoPass;
    TablewareManager tablewareManager;
    public bool IsShowing { get; set; }

    void Awake()
    {
        Find();
    }

    void Find()
    {
        gradeText = gradeAnim.transform.Find("GradeText").GetComponent<Text>();
        bg_pass = gradeAnim.transform.Find("Bg_Pass").gameObject;
        bg_NoPass = gradeAnim.transform.Find("Bg_NoPass").gameObject;
        tablewareManager = GetComponent<TablewareManager>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        ShowGrade();
    //    }
    //}

    public void HideGrade()
    {
        if (IsShowing) return;
        gradeAnim.SetTrigger("HideGrade");
    }

    public void ShowGrade()
    {
        if (IsShowing) return;

        IsShowing = true;
        tablewareManager.CheckGrade();
        StartCoroutine(ShowGradeNum());
    }

    IEnumerator ShowGradeNum()
    {
        yield return new WaitForSeconds(1f);

        if (tablewareManager.Grade >= 60)
        {
            bg_pass.SetActive(true);
            bg_NoPass.SetActive(false);
        }
        else
        {
            bg_pass.SetActive(false);
            bg_NoPass.SetActive(true);
        }
        gradeText.text = tablewareManager.Grade + "分";
        gradeAnim.SetTrigger("ShowGrade");
        tablewareManager.End();
        yield return new WaitForSeconds(3f);
        IsShowing = false;
        HideGrade();
    }
}

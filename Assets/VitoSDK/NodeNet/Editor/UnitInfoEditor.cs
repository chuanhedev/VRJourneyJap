using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UnitInfo))]
public class UnitInfoEditor : Editor
{
    private UnitInfo m_Target;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    void OnEnable()
    {
        if (Application.isPlaying)
            return;
        this.m_Target = (UnitInfo)target;
        UnitInfo[] units = Object.FindObjectsOfType<UnitInfo>();
        int index = 0;
        for (int i = 0; i < units.Length; i++)
        {
            UnitInfo unitinfo = units[i];
            if (unitinfo.UnitType != EUnitType.EUT_LocalPlayer && unitinfo.UnitType != EUnitType.EUT_OtherPlayer)
            {
                unitinfo.UnitID = index;
                index++;
            }
        }
    }
}

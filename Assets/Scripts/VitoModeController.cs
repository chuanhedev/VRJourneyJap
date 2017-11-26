using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitoModeController : MonoBehaviour
{
    public static VitoModeController _instance;
    public static Action<string> Act_ChangeMode;
    [SerializeField] Toggle free;
    [SerializeField] Toggle ctrl;

    void Awake()
    {
        _instance = this;
        RegisterChangeMode();
        // Debug.Log(VitoMode.Free == (VitoMode)Enum.Parse(typeof(VitoMode), "Free"));
    }

    void OnEnable()
    {
        Act_ChangeMode += ChangeMode;
        free.onValueChanged.AddListener(OnFreeChange);
        ctrl.onValueChanged.AddListener(OnCtrlChange);
    }

    void OnDisable()
    {
        Act_ChangeMode -= ChangeMode;
        free.onValueChanged.RemoveListener(OnFreeChange);
        ctrl.onValueChanged.RemoveListener(OnCtrlChange);
    }

    void OnFreeChange(bool isOn)
    {
        if (!isOn) return;

        RequestChangeMode(Enum.GetName(typeof(VitoMode), VitoMode.Free));
    }

    void OnCtrlChange(bool isOn)
    {
        if (!isOn) return;
        RequestChangeMode(Enum.GetName(typeof(VitoMode), VitoMode.Ctrl));
    }

    void ChangeMode(string msg)
    {
        FacadeManager._instance.vitoMode = (VitoMode)Enum.Parse(typeof(VitoMode), msg);
    }

    public void RequestChangeMode(string mode)
    {
        Rpc_UpdatePano(mode);
    }

    void Rpc_UpdatePano(string msg)
    {
        if (!VitoPlugin.IsNetMode)
        {
            Cmd_ChangeMode(msg);
        }
        else
        {
            VitoPlugin.RequestActionEvent("ChangeMode", msg);
        }
    }

    void Cmd_ChangeMode(string msg)
    {
        if (Act_ChangeMode != null)
        {
            Act_ChangeMode(msg);
        }
    }

    void RegisterChangeMode()
    {
        VitoPlugin.RegisterActionEvent("ChangeMode", (string actionName, string parameter, string deviceid) =>
        {
            Cmd_ChangeMode(parameter);
        });
    }
}

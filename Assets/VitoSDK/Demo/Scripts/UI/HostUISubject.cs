using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostUISubject : MonoBehaviour {
    public HostUISubjectData data;
    public HostUISubject ShowSubject(InputField subTitle, InputField content, InputField optionA, InputField optionB, InputField optionC, InputField optionD)
    {
        if (data != null)
        {
            subTitle.text = data.Title;
            print(data.Title);
            content.text = data.Content;
            optionA.text = data.OptionA;
            optionB.text = data.OptionB;
            optionC.text = data.OptionC;
            optionD.text = data.OptionD;
        }
        return this;
    }
    public void WriteSubject(InputField title, InputField content, InputField optionA, InputField optionB, InputField optionC, InputField optionD)
    {
        data.Title = title.text;
        data.Content = content.text;
        data.OptionA = optionA.text;
        data.OptionB = optionB.text;
        data.OptionC = optionC.text;
        data.OptionD = optionD.text;
    }
}

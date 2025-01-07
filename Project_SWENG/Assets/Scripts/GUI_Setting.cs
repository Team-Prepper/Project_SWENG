using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EHTool.LangKit;
using EHTool.UIKit;

public class GUI_Setting : GUIPopUp
{

    public void SetLanguage(string lang)
    {
        LangManager.Instance.ChangeLang(lang);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;
using LangSystem;

public class GUI_Setting : GUIPopUp
{

    public void SetLanguage(string lang)
    {
        StringManager.Instance.ChangeLang(lang);

    }

}

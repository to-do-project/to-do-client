using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PW : UI_SignUp
{

    enum Buttons
    {
        next_btn,
        AuthCheck_btn,
        Back_btn,
    }

    enum Texts
    {
        Info_txt,
        Etitle_txt,
        Ptitle_txt,
        Atitle_txt,
        EmailCheck_txt,
        PW_txt,
        PWCheck_txt,
        AuthCheck_txt,
    }

    enum InputFields
    {
        Email_inputfield,
        PW_inputfield,
        PWCheck_inputfield,
        Auth_inputfield,

    }

    enum View
    {
        InfoView,
        EmailView,
        AuthView,
        PWView,
    }
}

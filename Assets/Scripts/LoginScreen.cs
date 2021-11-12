using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    public GameObject SigninScreen;
    public GameObject SignupScreen;
    public GameObject MainScreen;
    public string Texts;

    public void OpenSignin()
    {
        SigninScreen.SetActive(true);
        SignupScreen.SetActive(false);
        MainScreen.SetActive(false);
    }
    public void OpenSignup()
    {
        SigninScreen.SetActive(false);
        SignupScreen.SetActive(true);
        MainScreen.SetActive(false);
    }
    public void OpenMain()
    {
        SigninScreen.SetActive(false);
        SignupScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    public void EnterGame()
    {

        TouchScreenKeyboard.Open(Texts, TouchScreenKeyboardType.NamePhonePad);
    }
}

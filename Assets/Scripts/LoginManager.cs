using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class LoginManager : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject registerPanel;
    public InputField usernameField;
    public InputField passwordField;
    public Toggle rememberToggle;
    public Button signInButton;
    public Button registerButton;
    private GameManager mGameManager;
    private const bool debugMode = true;

    // Respostas do Servidor
    private const string _LoginAccepted = "1"; // Login bem-sucedido
    private const string _UserRejected = "2"; // Usuário bloqueado
    private const string _UserNonExistent = "3"; // Usuário não existente
    private const string _WrongPassword = "4"; // Senha incorreta
    private const string _PasswordRecover = "5"; // Recuperação de senha

    public void Awake()
    {
        if (GameObject.FindGameObjectWithTag(Tags.GameController) != null)
        {
            mGameManager = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameManager>();
        }
    }

    void Start()
    {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
    }

    public void SignInButtonPressed()
    {
        if (debugMode)
        {
            mGameManager.LoadLevel(Levels.worldScene);
        }
        if (usernameField.text == "")
        {
            ShowLog("Usuário não inserido!");
        }
        else if (passwordField.text == "")
        {
            ShowLog("Senha não inserida!");
        }
        else
        {
            StartCoroutine(CheckLogin(usernameField.text, passwordField.text));
        }
    }

    public void RegisterButtonPressed()
    {
        
    }

    string PasswordHash(string password, string salt)
    {
        return "Test"; //128 char long
    }

    IEnumerator CheckLogin(string user, string password)
    {
        // Bloqueia Input
        LoginInterfaceEnabled(false);
        //Inicia form
        WWWForm form = new WWWForm();
        //form.AddField("user_hash", Tools.CryptSha512(user)); // hash do usuario (separado da senha caso necessite de recuperação)
        form.AddField("pass_hash", PasswordHash(password, user)); // hash da senha (com salt para evitar 2 senhas com hashs iguais)
        form.AddField("device_hash", SystemInfo.deviceUniqueIdentifier); // Identificador do Aparelho
        WWW webRequest = new WWW(mGameManager.databaseUrl + "login.php", form);
        yield return webRequest;

        if (webRequest.error != null)
        {
            ShowLog(webRequest.error);
        }
        else
        {
            switch (webRequest.text)
            {
                case _LoginAccepted:
                    break;
                case _PasswordRecover:
                    break;
                case _UserNonExistent:
                    ShowLog("Usuário inexistente.");
                    LoginInterfaceEnabled(true);
                    break;
                case _UserRejected:
                    ShowLog("Usuário bloqueado.");
                    LoginInterfaceEnabled(true);
                    break;
                case _WrongPassword:
                    ShowLog("Senha incorreta.");
                    LoginInterfaceEnabled(true);
                    passwordField.text = "";
                    break;
                default:
                    LoginInterfaceEnabled(true);
                    break;
            }
        }
    }

    void LoginInterfaceEnabled(bool value)
    {
        usernameField.interactable = value;
        passwordField.interactable = value;
        rememberToggle.interactable = value;
        signInButton.interactable = value;
        registerButton.interactable = value;
    }

    void ShowLog(string message)
    {
        
    }
}

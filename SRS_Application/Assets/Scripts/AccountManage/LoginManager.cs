using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button signUp;
    public Button logIn;
    public Button changeActiveKey;

    public InputField userName;
    public InputField password;

    public GameObject warningText;

    public static Account user = new Account();

    bool login = false;

    void Start()
    {
        signUp.onClick.AddListener(SignUp);
        logIn.onClick.AddListener(LogIn);
        changeActiveKey.onClick.AddListener(ChangeScene);

        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame

    void SignUp()
    {
        SceneManager.LoadScene("Signup");
    }

    void LogIn()
    {
        string username = userName.text;
        string pwd = password.text;

        if (!BinarySerializer.HasSaved(username))
        {
            warningText.gameObject.SetActive(true);
            return;
        }
        else user = BinarySerializer.Load<Account>(username);

        if (pwd != user.password)
        {
            warningText.gameObject.SetActive(true);
            user = null;
            return;
        }

        login = true;

        PlayerPrefs.SetString("cur_broker_uri", user.broker);
        PlayerPrefs.SetString("cur_access_token", user.accessToken);
        PlayerPrefs.SetString("cur_pwd_access_token", user.passwordToken);

        PlayerPrefs.SetString("cur_user", username);

        SceneManager.LoadScene("main_system");
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("ChangeActiveKey");
    }
}

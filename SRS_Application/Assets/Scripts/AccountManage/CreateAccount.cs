using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateAccount : MonoBehaviour
{
    // Start is called before the first frame update
    public Button signUp;
    public List<InputField> input;
    public Text warning;
    public Button returnLogin;

    void Start()
    {
        signUp.onClick.AddListener(SignUp);
        returnLogin.onClick.AddListener(ReturnLogin);
    }

    // Update is called once per frame

    void SignUp()
    {
        warning.gameObject.SetActive(false);

        string userName = input[0].text;
        string brokerID = input[1].text;
        string accessToken = input[2].text;
        string password = input[4].text;
        string repassword = input[5].text;
        string passwordToken = input[3].text + "a";

        foreach (InputField field in input)
        {
            if (field.text == "")
            {
                warning.text = "Please fill in all your information";
                warning.gameObject.SetActive(true);
                return;
            }
        }

        if (BinarySerializer.HasSaved(userName))
        {
            warning.text = "Username already exist";
            warning.gameObject.SetActive(true);
            return;
        }

        if (password != repassword)
        {
            warning.text = "Password doesn't match";
            warning.gameObject.SetActive(true);
            return;
        }

        Account account = new Account(userName, brokerID, accessToken, password, passwordToken);
        BinarySerializer.Save<Account>(account, userName);
        //Debug.Log(userName);
        SceneManager.LoadScene("Login");
    }
    void ReturnLogin()
    {
        SceneManager.LoadScene("Login");
    }
}

[System.Serializable]
public class Account
{
    public string userName;
    public string broker;
    public string accessToken;
    public string password;
    public string passwordToken;

    public Account()
    {

    }

    public Account(string userName, string brokerID, string accessToken, string password, string passwordToken)
    {
        this.userName = userName;
        this.broker = brokerID;
        this.accessToken = accessToken;
        this.password = password;
        this.passwordToken = passwordToken;
    }
}





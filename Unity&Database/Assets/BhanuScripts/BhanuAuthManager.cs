using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BhanuAuthManager : MonoBehaviour 
{
	bool m_isShowingRegistration = false;
	WWWForm m_form;

	[SerializeField] GameObject m_joinGameButtonObj , m_mainMenuObj , m_signInButtonObj , m_submitButtonObj , m_swapRegistrationButtonObj;
	[SerializeField] GameObject m_emailFieldObj , m_passwordFieldObj , m_passwordAgainFieldObj;
	[SerializeField] Text m_email , m_messageToUser , m_password , m_passwordAgain , m_swapButton;

	void Start() 
	{
		m_messageToUser.text = "";
		JoinPanel();
	}

	void Update() 
	{
		
	}

	public IEnumerator SignInRoutine()
	{
		//Debug.Log("Sign In Routine Before return");

		string email = m_email.text;
		string password = m_password.text;

		m_form = new WWWForm();
		m_form.AddField("email" , email);
		m_form.AddField("password" , password);

		WWW www = new WWW("http://localhost:8888/SignIn.php" , m_form);
		yield return www;

		//Debug.Log("Sign In Routine After return");

		if(string.IsNullOrEmpty(www.error)) 
		{
			//m_messageToUser.text = "Sir Bhanu, DB Connected :)";

			if(www.text.Contains("invalid")) 
			{
				m_messageToUser.text = "Sir Bhanu, Invalid Email or Password :(";
			}

			else
			{
				m_messageToUser.text = "Sir Bhanu, Sign In Success :)";	
			}
		} 
		else 
		{
			m_messageToUser.text = "Sir Bhanu, DB Connection Failed :(";
		}
	}

	public void JoinGameTapped()
	{
		m_messageToUser.text = "Joining...";
		StartCoroutine("SignInRoutine");
	}

	public void JoinPanel()
	{
		m_joinGameButtonObj.SetActive(true);
		m_passwordAgainFieldObj.SetActive(false);
		m_submitButtonObj.SetActive(false);
	}

	public void RegisterButtonTapped()
	{
		m_messageToUser.text = "Registering...";
	}

	public void SignInButtonTapped()
	{
		m_messageToUser.text = "Signing In...";
		//StartCoroutine("SignInRoutine");
	}

	public void SignInPanel()
	{
		m_passwordAgainFieldObj.SetActive(false);
		m_signInButtonObj.SetActive(true);
		m_submitButtonObj.SetActive(false);
	}

	public void SwapSignUpSignIn()
	{
		if(m_isShowingRegistration) 
		{
			m_isShowingRegistration = false;
			m_joinGameButtonObj.SetActive(true);
			m_passwordAgainFieldObj.SetActive(false);
			m_submitButtonObj.SetActive(false);
			m_swapButton.text = "Sign Up";
		} 
		else 
		{
			m_isShowingRegistration = true;	
			m_joinGameButtonObj.SetActive(false);
			m_passwordAgainFieldObj.SetActive(true);
			m_submitButtonObj.SetActive(true);
			m_swapButton.text = "Sign In";
		}
	}
}

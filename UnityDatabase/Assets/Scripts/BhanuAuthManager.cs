using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BhanuAuthManager : MonoBehaviour 
{
	WWWForm m_form;

	[SerializeField] GameObject m_signInFormObj , m_signUpFormObj;
	[SerializeField] Text m_emailField , m_messageToUser , m_passwordField , m_reEnterPasswordField , m_toggle;

	void Start() 
	{
		
	}

	public IEnumerator SignInRoutine()
	{
		string emailForPHP = m_emailField.text;
		string passwordForPHP = m_passwordField.text;

		m_form = new WWWForm();
		m_form.AddField("emailFromUnity" , emailForPHP);
		m_form.AddField("passwordFromUnity" , passwordForPHP);

		WWW www = new WWW("http://localhost:8888/sign_in.php" , m_form);
		yield return www;

		if(string.IsNullOrEmpty(www.error)) 
		{
			if(www.text.Contains("invalid email or password"))
			{
				m_messageToUser.text = "Invalid Username And/Or Password :(";
				m_messageToUser.color = Color.red;
			}

			else if(www.text.Contains("No Errors"))
			{
				m_messageToUser.text = "Sign In Successful :)";
				m_messageToUser.color = Color.green;
			}
		} 
		else 
		{
			m_messageToUser.text = "Unable to connect to the Database :(";
			m_messageToUser.color = Color.red;
		}
	}

	public IEnumerator SignUpRoutine()
	{
		yield return 0;
	}
		
	public void SignInSwap()
	{
		m_signInFormObj.SetActive(true);
		m_signUpFormObj.SetActive(false);
	}

	public void SignUpSwap()
	{
		m_signInFormObj.SetActive(false);
		m_signUpFormObj.SetActive(true);
	}

	public void TappedSignIn()
	{
		m_messageToUser.text = "Signing In...";
		m_messageToUser.color = Color.yellow;
		StartCoroutine("SignInRoutine");
	}

	public void TappedSignUp()
	{
		m_messageToUser.text = "Signing Up...";
		m_messageToUser.color = Color.yellow;
	}

	public void ToggleSignInSignUp()
	{
		if(m_toggle.text == "Sign Up") 
		{
			m_toggle.text = "Sign In";
			SignUpSwap();
		}

		else if(m_toggle.text == "Sign In") 
		{
			m_toggle.text = "Sign Up";
			SignInSwap();
		}
	}
}

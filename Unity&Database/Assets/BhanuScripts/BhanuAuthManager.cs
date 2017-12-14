using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BhanuAuthManager : MonoBehaviour 
{
	[SerializeField] GameObject m_joinGameButtonObj , m_mainMenuObj , m_registerButtonObj , m_swapRegistrationButtonObj;
	[SerializeField] GameObject m_emailFieldObj , m_passwordFieldObj , m_passwordAgainFieldObj;
	[SerializeField] Text m_emailText , m_passwordText , m_passwordAgainText;

	void Start() 
	{
		LogInPanel();
	}

	void Update() 
	{
		
	}

	public void LogInButtonTapped()
	{
		
	}

	public void LogInPanel()
	{
		m_joinGameButtonObj.SetActive(true);
		m_registerButtonObj.SetActive(false);
		m_passwordAgainFieldObj.SetActive(false);
	}

	public void RegisterButtonTapped()
	{
		
	}

	public void SwapSignUpSignIn()
	{
		
	}
}

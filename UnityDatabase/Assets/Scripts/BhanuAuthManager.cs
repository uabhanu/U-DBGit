using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BhanuAuthManager : MonoBehaviour 
{
	[SerializeField] GameObject m_signInFormObj , m_signUpFormObj;
	[SerializeField] Text m_toggleText;

	void Start() 
	{
		
	}

	public IEnumerator SignInRoutine()
	{
		yield return 0;
	}

	public IEnumerator SignUpRoutine()
	{
		yield return 0;
	}
		
	public void SignIn()
	{
		m_signInFormObj.SetActive(true);
		m_signUpFormObj.SetActive(false);
	}

	public void SignUp()
	{
		m_signInFormObj.SetActive(false);
		m_signUpFormObj.SetActive(true);
	}

	public void TappedJoin()
	{

	}

	public void TappedSubmit()
	{
		
	}

	public void ToggleSignInSignUp()
	{
		if(m_toggleText.text == "Sign Up") 
		{
			m_toggleText.text = "Sign In";
			SignUp();
		}

		else if(m_toggleText.text == "Sign In") 
		{
			m_toggleText.text = "Sign Up";
			SignIn();
		}
	}
}

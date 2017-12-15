using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public class User
{
	public bool success;
	public string error;
	public string email;
	public int id;
	// feel free to add userName....
}

[Serializable]
public class PlayerGear
{
	public string pathHead;
	public string pathShoulders;
	public string pathRightHand;
	public string pathOffHand;
}

[Serializable]
public class PlayerBackpack
{
	public string pathSlot1;
	public string pathSlot2;
	public string pathSlot3;
	public string pathSlot4;
	public string pathSlot5;
	public string pathSlot6;
}

public class AuthenticationManager : MonoBehaviour {

	public string urlLogin = "action_login.php";
	public string urlRegister = "action_register.php";
	public GameObject mainMenu;
	public GameObject buttonJoinGame;
	public GameObject buttonSwapRegistration;
	public GameObject buttonRegister;

	public GameObject fieldEmailAddress;
	public GameObject fieldPassword;
	public GameObject fieldReenterPassword;

	public Text textEmail;

	public InputField inputPassword;
	public InputField inputReenterPassword;

	public Text textSwapButton;
	public Text textFeedback;

	WWWForm form;

	public bool showRegistration = false;

	// Use this for initialization
	void Start () {
		textFeedback.text = "";
		ManagerReferences refs = GameObject.Find ("Manager").GetComponent<ManagerReferences> ();
		urlLogin = refs.serverAddress + urlLogin;
		urlRegister = refs.serverAddress + urlRegister;


		displayLoginPanel ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void displayLoginPanel() {
		buttonRegister.SetActive (false);
		fieldReenterPassword.SetActive (false);
	}


	public void SwapSignupSignin() {
		if (showRegistration) {
			showRegistration = false;
			buttonJoinGame.SetActive (true);
			buttonRegister.SetActive (false);
			fieldReenterPassword.SetActive (false);
			textSwapButton.text = "SignUp";


		} else {
			showRegistration = true;
			buttonJoinGame.SetActive (false);
			buttonRegister.SetActive (true);
			fieldReenterPassword.SetActive (true);
			textSwapButton.text = "SignIn";

		}
	}

	public void RegisterButtonTapped() {
		// todo
		textFeedback.text = "Processing registration...";
		StartCoroutine ("RequestUserRegistration");
	}

	public void LoginButtonTapped() {
		textFeedback.text = "Logging in...";
		StartCoroutine ("RequestLogin");


	}

	public IEnumerator RequestLogin() {
		
		string email = textEmail.text;
		string password = inputPassword.text;

		form = new WWWForm ();
		form.AddField ("email", email);
		form.AddField ("password", password);

		WWW w = new WWW (urlLogin, form);
		yield return w;

		if (string.IsNullOrEmpty (w.error)) {
			User user = JsonUtility.FromJson<User> (w.text);
			if (user.success == true) 
			{
				if (user.error != "") 
				{
					textFeedback.text = user.error;
				} else 
				{
					textFeedback.text = "login successful.";
					ProcessPlay (user);
				}
			} else {
				textFeedback.text = "An error occured";
			}

			// todo: launch the game (player)
		} else {
			// error
			textFeedback.text = "An error occured.";
		}
	}

	public IEnumerator RequestUserRegistration(){
		string email = textEmail.text;
		string password = inputPassword.text;
		string reenterPassword = inputReenterPassword.text;

		if (password.Length < 8) {
			textFeedback.text = "password needs to be at least 8 characters long.";
			yield break;
		}
		if(password != reenterPassword) {
			textFeedback.text = "password do not match";
			yield break;
		}

		form = new WWWForm();
		form.AddField("email", email);
		form.AddField("password", password);

		WWW w = new WWW(urlRegister, form);
		yield return w;

		if (string.IsNullOrEmpty (w.error)) {
			User user = JsonUtility.FromJson<User> (w.text);
			if (user.success == true) {
				if (user.error != "") {
					textFeedback.text = user.error;
				} else {
					textFeedback.text = "registration successful.";
					ProcessPlay (user);
				}
			} else {
				textFeedback.text = "An error occured";
			}
		}
		
	}

	public void ProcessPlay(User user){
		ManagerReferences refs = GameObject.Find ("Manager").GetComponent<ManagerReferences> ();
		refs.user = user;
		GetComponent<NetworkManagerMainMenu> ().JoinGame ();
	}

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ManagerReferences : NetworkBehaviour {


	public GameObject StatsUI;
	public GameObject StatsItemUI;
	public Text playerStatsTextStrength;
	public Text playerStatsTextDexterity;
	public Text playerStatsTextIntelligence;
	public Text playerStatsTextDamage;

	public GameObject localPlayer;

	public Button ButtonInventory;
	public GameObject backpack;
	public GameObject SkillBar;

	public GameObject mainMenu;
	public GameObject buttonDisconnect;

	public float playerScale = 15.0f; // different characters have different sizes, i.e goblin, dward, human, elf, etc...
	// Use this for initialization
	void Start () {
		//NetworkLobbyManager.singleton.StartServer ();
		//NetworkLobbyManager.singleton.StartClient();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LocalPlayerSpawned() {
		ButtonInventory.gameObject.SetActive (true);
		SkillBar.gameObject.SetActive (true);
	}

}

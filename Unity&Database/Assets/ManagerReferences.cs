using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ManagerReferences : NetworkBehaviour
{

	public string serverAddress = "http://localhost:8888/";
	public User user;
	public string urlGetUserGear = "action_get_player_gear.php";
	public string urlSetUserGear = "action_set_player_gear.php";
	public string urlSetUserBackpack = "action_set_player_backpack.php";

	public GameObject StatsUI;
	public GameObject StatsItemUI;
	public Text playerStatsTextStrength;
	public Text playerStatsTextDexterity;
	public Text playerStatsTextIntelligence;
	public Text playerStatsTextDamage;
	public GameObject panelEquippedItems;

	public GameObject localPlayer;

	public Button ButtonInventory;
	public GameObject backpack;
	public GameObject SkillBar;

	public GameObject mainMenu;
	public GameObject buttonDisconnect;

	public float playerScale = 15.0f;
	// different characters have different sizes, i.e goblin, dward, human, elf, etc...
	// Use this for initialization
	void Start ()
	{
		//NetworkLobbyManager.singleton.StartServer ();
		//NetworkLobbyManager.singleton.StartClient();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void LocalPlayerSpawned ()
	{
		ButtonInventory.gameObject.SetActive (true);
		SkillBar.gameObject.SetActive (true);
		StartCoroutine ("RetrievePlayerGear");
		StartCoroutine ("RetrievePlayerBackpack");
	}

	public IEnumerator RetrievePlayerGear ()
	{
		WWWForm form = new WWWForm ();
		form.AddField ("userid", user.id.ToString ());

		WWW w = new WWW (serverAddress + urlGetUserGear, form);
		yield return w;

		if (string.IsNullOrEmpty (w.error)) {
			PlayerGear gear = JsonUtility.FromJson<PlayerGear> (w.text);
			// todo: equip the gear...
			EquipPlayerGear (gear);
		}
	}

	public IEnumerator RetrievePlayerBackpack()
	{
		WWWForm form = new WWWForm ();
		form.AddField ("userid", user.id.ToString ());

		WWW w = new WWW (serverAddress + urlGetUserGear, form);
		yield return w;

		if (string.IsNullOrEmpty (w.error)) {
			PlayerBackpack backpack = JsonUtility.FromJson<PlayerBackpack> (w.text);
			PutItemsInBackpack (backpack);
		}
	}

	public void EquipPlayerGear (PlayerGear gear)
	{
		EquipItem (gear.pathHead, "ItemSlotHead");
		EquipItem (gear.pathShoulders, "ItemSlotShoulders");
		EquipItem (gear.pathRightHand, "ItemSlotRightHand");
		EquipItem (gear.pathOffHand, "ItemSlotLeftHand");
	}

	public void EquipItem(string itemOnGroundPath, string slotName) {

		GameObject objectOnGround = Instantiate (Resources.Load (itemOnGroundPath)) as GameObject;
		ItemGroundScript groundScript = objectOnGround.GetComponent<ItemGroundScript> ();
		string imageSlotString = groundScript.NameOfSlotImage;

		GameObject imageSlotHead = Instantiate (Resources.Load (imageSlotString)) as GameObject;
		imageSlotHead.transform.parent = panelEquippedItems.transform.Find (slotName);

		ItemAttributes itemAttributes = objectOnGround.GetComponent<ItemAttributes> ();
		ItemAttributes attributesForImageSlot = imageSlotHead.AddComponent<ItemAttributes> ();
		attributesForImageSlot.cloneAttributesFrom (itemAttributes);

		Attributes attributes = new Attributes ();
		attributes.cloneAttributesFrom (itemAttributes);

		string itemPath = imageSlotHead.GetComponent<DragDropScript> ().pathOfItemToEquip;
		string itemPathB = imageSlotHead.GetComponent<DragDropScript> ().pathOfItemToEquip2;

		if (slotName == "ItemSlotHead") {
			localPlayer.GetComponent<EquipItemScript> ().CmdEquipItemOnHead (itemPath, localPlayer.name, attributes);
		} else if (slotName == "ItemSlotRightHand") {
			localPlayer.GetComponent<EquipItemScript> ().CmdEquipItemOnRightHand (itemPath, localPlayer.name, attributes);
		} else if (slotName == "ItemSlotLeftHand") {
			localPlayer.GetComponent<EquipItemScript> ().CmdEquipItemOnLeftHand (itemPath, localPlayer.name, attributes);
		}  else if (slotName == "ItemSlotShoulders") {
			localPlayer.GetComponent<EquipItemScript> ().CmdEquipLeftShoulderWith (itemPath, localPlayer.name, attributes);
			localPlayer.GetComponent<EquipItemScript> ().CmdEquipRightShoulderWith (itemPathB, localPlayer.name, attributes);
		}

		Destroy (objectOnGround);
	}

	public void RequestStorePlayerGear ()
	{
		Invoke ("RequestStorePlayerGearWithDelay", 2.0f);

	}

	public void RequestStorePlayerGearWithDelay ()
	{
		StartCoroutine ("RequestStorePlayerGearRoutine");
	}

	public IEnumerator RequestStorePlayerGearRoutine ()
	{
		WWWForm form = new WWWForm ();

		form.AddField ("userId", user.id);

		// HEAD
		GameObject slotHead = panelEquippedItems.transform.Find ("ItemSlotHead").gameObject;
		string pathHead = "";
		if (slotHead != null) {
			if (slotHead.transform.childCount > 0) {
				GameObject imageObject = slotHead.transform.GetChild (0).gameObject;
				pathHead = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("head", pathHead);

		// RIGHT HAND
		GameObject slotRightHand = panelEquippedItems.transform.Find ("ItemSlotRightHand").gameObject;
		string pathRightHand = "";
		if (slotRightHand != null) {
			if (slotRightHand.transform.childCount > 0) {
				GameObject imageObject = slotRightHand.transform.GetChild (0).gameObject;
				pathRightHand = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("rightHand", pathRightHand);

		// LEFT HAND
		GameObject slotLeftHand = panelEquippedItems.transform.Find ("ItemSlotLeftHand").gameObject;
		string pathLeftHand = "";
		if (slotLeftHand != null) {
			if (slotLeftHand.transform.childCount > 0) {
				GameObject imageObject = slotLeftHand.transform.GetChild (0).gameObject;
				pathLeftHand = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}

		}
		form.AddField ("leftHand", pathLeftHand);


		// SHOULDER PADS
		GameObject slotShoulders = panelEquippedItems.transform.Find ("ItemSlotShoulders").gameObject;
		string pathShoulders = "";
		if (slotShoulders != null) {
			if (slotShoulders.transform.childCount > 0) {
				GameObject imageObject = slotShoulders.transform.GetChild (0).gameObject;
				pathShoulders = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("shoulders", pathShoulders);

		WWW w = new WWW (serverAddress + urlSetUserGear, form);
		yield return w;

		if (string.IsNullOrEmpty (w.error)) {
			// we have successfully stored the updated gear
		}



	}




	public void PutItemInBackpack(string itemOnGroundPath, string slotName) {

		GameObject objectOnGround = Instantiate (Resources.Load (itemOnGroundPath)) as GameObject;
		ItemGroundScript groundScript = objectOnGround.GetComponent<ItemGroundScript> ();
		string imageSlotString = groundScript.NameOfSlotImage;

		GameObject imageSlotHead = Instantiate (Resources.Load (imageSlotString)) as GameObject;
		imageSlotHead.transform.parent = panelEquippedItems.transform.Find (slotName);

		ItemAttributes itemAttributes = objectOnGround.GetComponent<ItemAttributes> ();
		ItemAttributes attributesForImageSlot = imageSlotHead.AddComponent<ItemAttributes> ();
		attributesForImageSlot.cloneAttributesFrom (itemAttributes);

		Attributes attributes = new Attributes ();
		attributes.cloneAttributesFrom (itemAttributes);

		Destroy (objectOnGround);
	}

	public void PutItemsInBackpack (PlayerBackpack backpack)
	{
		PutItemInBackpack (backpack.pathSlot1, "ItemSlot1");
		PutItemInBackpack (backpack.pathSlot2, "ItemSlot2");
		PutItemInBackpack (backpack.pathSlot3, "ItemSlot3");
		PutItemInBackpack (backpack.pathSlot4, "ItemSlot4");
		PutItemInBackpack (backpack.pathSlot5, "ItemSlot5");
		PutItemInBackpack (backpack.pathSlot6, "ItemSlot6");

	}


	public void RequestStorePlayerBackpack(){
		Invoke ("RequestStorePlayerBackpackWithDelay", 2.0f);
	}
	public void RequestStorePlayerBackpackWithDelay(){
		StartCoroutine ("RequestStorePlayerBackpackRoutine");
	}

	public IEnumerator RequestStorePlayerBackpackRoutine ()
	{
		WWWForm form = new WWWForm ();
		form.AddField ("userId", user.id);

		// slot 1
		GameObject slot1 = panelEquippedItems.transform.Find ("ItemSlot1").gameObject;
		string path1 = "";
		if (slot1 != null) {
			if (slot1.transform.childCount > 0) {
				GameObject imageObject = slot1.transform.GetChild (0).gameObject;
				path1 = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("slot1", path1);

		// slot 2
		GameObject slot2 = panelEquippedItems.transform.Find ("ItemSlot2").gameObject;
		string path2 = "";
		if (slot2 != null) {
			if (slot2.transform.childCount > 0) {
				GameObject imageObject = slot2.transform.GetChild (0).gameObject;
				path2 = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("slot2", path2);

		// slot 3
		GameObject slot3 = panelEquippedItems.transform.Find ("ItemSlot3").gameObject;
		string path3 = "";
		if (slot3 != null) {
			if (slot3.transform.childCount > 0) {
				GameObject imageObject = slot3.transform.GetChild (0).gameObject;
				path3 = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("slot3", path3);


		// slot 4
		GameObject slot4 = panelEquippedItems.transform.Find ("ItemSlot4").gameObject;
		string path4 = "";
		if (slot4 != null) {
			if (slot4.transform.childCount > 0) {
				GameObject imageObject = slot4.transform.GetChild (0).gameObject;
				path4 = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("slot4", path4);


		// slot 5
		GameObject slot5 = panelEquippedItems.transform.Find ("ItemSlot5").gameObject;
		string path5 = "";
		if (slot5 != null) {
			if (slot5.transform.childCount > 0) {
				GameObject imageObject = slot5.transform.GetChild (0).gameObject;
				path5 = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("slot5", path5);


		// slot 6
		GameObject slot6 = panelEquippedItems.transform.Find ("ItemSlot6").gameObject;
		string path6 = "";
		if (slot6 != null) {
			if (slot6.transform.childCount > 0) {
				GameObject imageObject = slot6.transform.GetChild (0).gameObject;
				path6 = imageObject.GetComponent<DragDropScript> ().pathToGroundItem;
			}
		}
		form.AddField ("slot6", path6);


		WWW w = new WWW (serverAddress + urlSetUserBackpack, form);
		yield return w;

		if (string.IsNullOrEmpty (w.error)) {
			// we have successfully stored the updated gear
		}



	}




}

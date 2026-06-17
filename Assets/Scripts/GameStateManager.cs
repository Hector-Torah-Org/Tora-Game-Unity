using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ItemData coin;
    [SerializeField] private ItemData unlockObject1;
    [SerializeField] private ItemData axe;
    [SerializeField] private ItemData bucket;

    [SerializeField] private ApiConnection apiConnection;

    public static GameStateManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SaveToServer()
    {
        if (apiConnection == null)
        {
            Debug.LogError("GameStateManager: ApiConnection not assigned.");
            return;
        }

        string saveString = CreateSaveString();
        Debug.Log("Saving gameState: " + saveString);

        StartCoroutine(apiConnection.UpdateGameState(saveString));
    }



    private void RemoveItemFromChest(string chestId, ItemData item)
    {
        SceneData sceneData = null;

        List<ItemStack> contents = SceneManager.Instance.GetChestContents(chestId, null);

        for (int i = contents.Count - 1; i >= 0; i--)
        {
            if (contents[i] != null && contents[i].item == item)
            {
                contents.RemoveAt(i);
                return;
            }
        }
    }
    string inventoryGameState = "012";

    string sceneV1GameState = "00210"; // Village 1
    string sceneP1GameState = "00"; // Path 1
    string sceneF1GameState = ""; // Forest 1
    string sceneFRGameState = ""; // Forest Ravine
    string sceneH1GameState = "0"; // House 1
    string sceneH2GameState = ""; // House 2
    string sceneH3GameState = "0"; // House 3
    string sceneC1GameState = "0000"; // Cave 1
    string sceneRIVGameState = "0"; // River
    string sceneP2GameState = "0"; // Path 2

    string sceneC3GameState = "0"; // Cave 3
    string sceneC2GameState = ""; // Cave 2
    string sceneCTGameState = "0"; // Cave Tunnel
    string sceneV3GameState = "0000"; // Village 3
    string sceneVCGameState = "00"; // Village Church
    string sceneF3GameState = "0"; // Forest 3
    string sceneFHGameState = "00"; // Forest House
    string sceneFBGameState = ""; // Foresthouse Basement (Will add a lot of barrels)
    string sceneFBTGameState = "0"; // Foresthouse Basement Tile
    string sceneGSGameState = "0"; // Gate Scene
    string sceneR1GameState = "0"; // Ruin 1
    string sceneD1GameState = "0"; // Dungeon 1


    void Start()
    {
        Debug.Log("in GameStateManager");

        ApplyLoadedState();
    }

    private char DoorState(string doorId)
    {
        return SceneManager.Instance.IsDoorOpen(doorId) ? '1' : '0';
    }

    private char InteractableState(string interactableId)
    {
        return SceneManager.Instance.IsInteractableUsed(interactableId) ? '1' : '0';
    }

    private char ChestState(string chestId, ItemData expectedItem)
    {
        bool isOpen = SceneManager.Instance.IsChestOpen(chestId);

        if (!isOpen)
            return '0';

        if (expectedItem == null)
            return '1';

        bool stillContainsItem = SceneManager.Instance.ChestContainsItem(chestId, expectedItem);

        return stillContainsItem ? '1' : '2';
    }





    public string CreateSaveString()
    {
        CreateSaveStrings();

        return string.Join("|",
            inventoryGameState,
            sceneV1GameState,
            sceneP1GameState,
            sceneF1GameState,
            sceneFRGameState,
            sceneH1GameState,
            sceneH2GameState,
            sceneH3GameState,
            sceneC1GameState,
            sceneRIVGameState,
            sceneP2GameState,
            sceneC3GameState,
            sceneC2GameState,
            sceneCTGameState,
            sceneV3GameState,
            sceneVCGameState,
            sceneF3GameState,
            sceneFHGameState,
            sceneFBGameState,
            sceneFBTGameState,
            sceneGSGameState,
            sceneR1GameState,
            sceneD1GameState
        );
    }






    public void LoadGameStateString(string gameState)
    {
        if (string.IsNullOrEmpty(gameState))
        {
            Debug.LogWarning("No gameState received. Using default strings.");
            return;
        }

        string[] parts = gameState.Split('|');

        if (parts.Length < 23)
        {
            Debug.LogError("Invalid gameState. Expected 23 parts, got " + parts.Length);
            return;
        }

        inventoryGameState = parts[0];
        sceneV1GameState = parts[1];
        sceneP1GameState = parts[2];
        sceneF1GameState = parts[3];
        sceneFRGameState = parts[4];
        sceneH1GameState = parts[5];
        sceneH2GameState = parts[6];
        sceneH3GameState = parts[7];
        sceneC1GameState = parts[8];
        sceneRIVGameState = parts[9];
        sceneP2GameState = parts[10];

        sceneC3GameState = parts[11];
        sceneC2GameState = parts[12];
        sceneCTGameState = parts[13];
        sceneV3GameState = parts[14];
        sceneVCGameState = parts[15];
        sceneF3GameState = parts[16];
        sceneFHGameState = parts[17];
        sceneFBGameState = parts[18];
        sceneFBTGameState = parts[19];
        sceneGSGameState = parts[20];
        sceneR1GameState = parts[21];
        sceneD1GameState = parts[22];

        Debug.Log(parts);

        ApplyLoadedState();
    }




    public void CreateSaveStrings()
    {
        sceneV1GameState =
            "" + DoorState("door_0_A")
               + DoorState("door_0_B")
               + ChestState("chest_0_A", coin)
               + ChestState("chest_0_B", coin)
               + InteractableState("well_trigger1");

        sceneP1GameState =
            "" + DoorState("door_1_A")
               + ChestState("chest_1_A", null);          // TODO: replace null with real item

        sceneF1GameState =
            "";

        sceneFRGameState =
            "";

        sceneH1GameState =
            "" + ChestState("chest_4_A", null);          // TODO: replace null with real item

        sceneH2GameState =
            "";

        sceneH3GameState =
            "" + InteractableState("axe_trigger1");

        sceneC1GameState =
            "" + ChestState("chest_7_A", null)           // TODO: replace null with real item
               + ChestState("chest_7_B", null)
               + ChestState("chest_7_C", unlockObject1)
               + ChestState("chest_7_D", null);

        sceneRIVGameState =
            "" + ChestState("chest_8_A", null);

        sceneP2GameState =
            "" + DoorState("door_9_A");

        sceneC3GameState =
            "" + InteractableState("lever_trigger");

        sceneC2GameState =
            "";

        sceneCTGameState =
            "" + InteractableState("pickaxe_trigger");

        sceneV3GameState =
            "" + DoorState("door_13_A")
               + DoorState("door_13_B")
               + ChestState("chest_13_A", coin)
               + ChestState("chest_13_B", coin);

        sceneVCGameState =
            "" + ChestState("chest_14_A", null)          // TODO: replace null with real item
               + ChestState("chest_14_B", null);

        sceneF3GameState =
            "" + DoorState("door_15_A");

        sceneFHGameState =
            "" + DoorState("door_16_A")
               + ChestState("chest_16_A", null);         // TODO: replace null with real item

        sceneFBGameState =
            "";

        sceneFBTGameState =
            "" + InteractableState("key_trigger");

        sceneGSGameState =
            "" + ChestState("chest_19_A", coin);

        sceneR1GameState =
            "" + ChestState("chest_20_A", null);         // TODO: replace null with real item

        sceneD1GameState =
            "" + ChestState("chest_21_A", null);         // TODO: replace null with real item

        /*
        Debug.Log("Inventory: " + inventoryGameState);

        Debug.Log("V1: " + sceneV1GameState);
        Debug.Log("P1: " + sceneP1GameState);
        Debug.Log("F1: " + sceneF1GameState);
        Debug.Log("FR: " + sceneFRGameState);
        Debug.Log("H1: " + sceneH1GameState);
        Debug.Log("H2: " + sceneH2GameState);
        Debug.Log("H3: " + sceneH3GameState);
        Debug.Log("C1: " + sceneC1GameState);
        Debug.Log("RIV: " + sceneRIVGameState);
        Debug.Log("P2: " + sceneP2GameState);

        Debug.Log("C3: " + sceneC3GameState);
        Debug.Log("C2: " + sceneC2GameState);
        Debug.Log("CT: " + sceneCTGameState);
        Debug.Log("V3: " + sceneV3GameState);
        Debug.Log("VC: " + sceneVCGameState);
        Debug.Log("F3: " + sceneF3GameState);
        Debug.Log("FH: " + sceneFHGameState);
        Debug.Log("FB: " + sceneFBGameState);
        Debug.Log("FBT: " + sceneFBTGameState);
        Debug.Log("GS: " + sceneGSGameState);
        Debug.Log("R1: " + sceneR1GameState);
        Debug.Log("D1: " + sceneD1GameState);
        */
    }






    private void ApplyLoadedState()
    {
        //================================================================= I N V E N T O R Y ====================================================================

        foreach (char i in inventoryGameState)
        {
            switch (i)
            {
                case '0':
                    inventory.AddItem(new ItemStack
                    {
                        item = coin,
                        amount = 1
                    });
                    break;

                case '1':
                    inventory.AddItem(new ItemStack
                    {
                        item = unlockObject1,
                        amount = 1
                    });
                    break;

                case '2':
                    inventory.AddItem(new ItemStack
                    {
                        item = axe,
                        amount = 1
                    });
                    break;

                case '3':
                    inventory.AddItem(new ItemStack
                    {
                        item = bucket,
                        amount = 1
                    });
                    break;
            }
        }


        //----------------------------------------------------------------- S C E N E   V 1 --------------------------------------------------------------------0

        switch (sceneV1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_0_A", true);                                    // Set door_0_A to open
                break;
        }

        switch (sceneV1GameState[1])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_0_B", true);                                    // Set door_0_B to open
                break;
        }

        switch (sceneV1GameState[2])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_0_A", true);                                  // Set chest_0_A to open, contains coin (default state)
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_0_A", true);
                RemoveItemFromChest("chest_0_A", coin);                                                 // Set chest_0_A to open, does not contain coin (remove it)
                break;
        }

        switch (sceneV1GameState[3])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_0_B", true);                                  // Set chest_0_B to open, contains coin (default state)
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_0_B", true);
                RemoveItemFromChest("chest_0_B", coin);                                                 // Set chest_0_B to open, does not contain coin (remove it)
                break;
        }

        switch (sceneV1GameState[4])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("well_trigger1", true);                       // Set Well to Used
                break;

            case '2':
                SceneManager.Instance.SetInteractableUsed("well_trigger1", true);
                SceneManager.Instance.SetSceneEventState("scene3_special_used", true);
                SceneManager.Instance.SetSceneBackgroundIndex(3, 1);                                    // Set Well to Used, change the scene as if the bucket was used (currently not yet defined)
                break;

        }

        //----------------------------------------------------------------- S C E N E   P 1 --------------------------------------------------------------------1

        switch (sceneP1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_1_A", true);                                    // Set door_1_A to open
                break;

        }

        switch (sceneP1GameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_1_A", true);                                  // Set chest_1_A to open, contains item (whenever I use just plain and simple "item", i mean that its not yet settled what item is in here)
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_1_A", true);
                //RemoveItemFromChest("chest_1_A", coin); //not a coin                                  // Set chest_1_A to open, does not contain item
                break;

        }

        //----------------------------------------------------------------- S C E N E   F 1 --------------------------------------------------------------------2

        //----------------------------------------------------------------- S C E N E   F R --------------------------------------------------------------------3

        //----------------------------------------------------------------- S C E N E   H 1 --------------------------------------------------------------------4

        switch (sceneH1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_4_A", true);                                  // Set chest_4_A to open, contains item                              
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_4_A", true);
                //RemoveItemFromChest("chest_1_A", coin); //not a coin                                  // Set chest_4_A to open, does not contain item
                break;

        }

        //----------------------------------------------------------------- S C E N E   H 2 --------------------------------------------------------------------5

        //----------------------------------------------------------------- S C E N E   H 3 --------------------------------------------------------------------6

        switch (sceneH3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("axe_trigger1", true);                        // Set Axe to used
                break;

            case '2':
                SceneManager.Instance.SetInteractableUsed("axe_trigger1", true);
                SceneManager.Instance.SetSceneEventState("scene9_special_used", true);
                SceneManager.Instance.SetSceneBackgroundIndex(9, 1);                                    // Set Axe to used, change scene 9 as if axe was used (spawn the arrow, change the scene background id to 1)
                break;

        }

        //----------------------------------------------------------------- S C E N E   C 1 --------------------------------------------------------------------7

        switch (sceneC1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_7_A", true);                                  // Set chest_7_A to open, contains coin (default state)
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_7_A", true);
                //RemoveItemFromChest("chest_7_A", coin); //not a coin                                  // Set chest_7_A to open, does NOT contain coin (remove it)
                break;
        }

        switch (sceneC1GameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_7_B", true);                                  // Set chest_7_B to open
                break;

        }

        switch (sceneC1GameState[2])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_7_C", true);                                  // Set chest_7_C to open, contains Unlock_Object_1 (default state)
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_7_C", true);
                RemoveItemFromChest("chest_7_C", unlockObject1);                                        // Set chest_7_C to open, does NOT contain Unlock_Object_1 (remove it)
                break;
        }

        switch (sceneC1GameState[3])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_7_D", true);                                  // Set chest_7_D to open
                break;

        }

        //----------------------------------------------------------------- S C E N E   R I V --------------------------------------------------------------------8

        switch (sceneRIVGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_8_A", true);                                  // Set chest_8_A to open
                break;
        }

        //----------------------------------------------------------------- S C E N E   P 2 --------------------------------------------------------------------9

        switch (sceneP2GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_9_A", true);                                    // Set door_9_A to open
                break;
        }

        //----------------------------------------------------------------- S C E N E   C 3 --------------------------------------------------------------------10

        switch (sceneC3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("lever_trigger", true);                      // Set lever_trigger to used
                break;
        }

        //----------------------------------------------------------------- S C E N E   C 2 --------------------------------------------------------------------11


        //----------------------------------------------------------------- S C E N E   C T --------------------------------------------------------------------12

        switch (sceneCTGameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("pickaxe_trigger", true);                    // Set pickaxe_trigger to used
                break;
        }

        //----------------------------------------------------------------- S C E N E   V 3 --------------------------------------------------------------------13

        switch (sceneV3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_13_A", true);                                  // Set door_13_A to open
                break;
        }

        switch (sceneV3GameState[1])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_13_B", true);                                  // Set door_13_B to open
                break;
        }

        switch (sceneV3GameState[2])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_13_A", true);                                // Set chest_13_A to open, contains coin
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_13_A", true);
                RemoveItemFromChest("chest_13_A", coin);                                               // Set chest_13_A to open, does not contain coin
                break;
        }

        switch (sceneV3GameState[3])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_13_B", true);                                // Set chest_13_B to open, contains coin
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_13_B", true);
                RemoveItemFromChest("chest_13_B", coin);                                               // Set chest_13_B to open, does not contain coin
                break;
        }

        //----------------------------------------------------------------- S C E N E   V C --------------------------------------------------------------------14

        switch (sceneVCGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_14_A", true);                                // Set chest_14_A to open, contains item
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_14_A", true);
                //RemoveItemFromChest("chest_14_A", coin); //not a coin                                // Set chest_14_A to open, does not contain item
                break;
        }

        switch (sceneVCGameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_14_B", true);                                // Set chest_14_B to open
                break;
        }

        //----------------------------------------------------------------- S C E N E   F 3 --------------------------------------------------------------------15

        switch (sceneF3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_15_A", true);                                  // Set door_15_A to open
                break;
        }

        //----------------------------------------------------------------- S C E N E   F H --------------------------------------------------------------------16

        switch (sceneFHGameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_16_A", true);                                  // Set door_16_A to open
                break;
        }

        switch (sceneFHGameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_16_A", true);                                // Set chest_16_A to open, contains item
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_16_A", true);
                //RemoveItemFromChest("chest_16_A", coin); //not a coin                                // Set chest_16_A to open, does not contain item
                break;
        }

        //----------------------------------------------------------------- S C E N E   F B --------------------------------------------------------------------17


        //----------------------------------------------------------------- S C E N E   F B T --------------------------------------------------------------------18

        switch (sceneFBTGameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("key_trigger", true);                        // Set key_trigger to used
                break;
        }

        //----------------------------------------------------------------- S C E N E   G S --------------------------------------------------------------------19

        switch (sceneGSGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_19_A", true);                                // Set chest_19_A to open, contains coin
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_19_A", true);
                RemoveItemFromChest("chest_19_A", coin);                                               // Set chest_19_A to open, does not contain coin
                break;
        }

        //----------------------------------------------------------------- S C E N E   R 1 --------------------------------------------------------------------20

        switch (sceneR1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_20_A", true);                                // Set chest_20_A to open, contains item
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_20_A", true);
                //RemoveItemFromChest("chest_20_A", coin); //not a coin                                // Set chest_20_A to open, does not contain item
                break;
        }

        //----------------------------------------------------------------- S C E N E   D 1 --------------------------------------------------------------------21

        switch (sceneD1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_21_A", true);                                // Set chest_21_A to open, contains item
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_21_A", true);
                //RemoveItemFromChest("chest_21_A", coin); //not a coin                                // Set chest_21_A to open, does not contain item
                break;
        }





        SceneManager.Instance.ReloadCurrentScene();
    }
}
using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ItemData coin;
    [SerializeField] private ItemData unlockObject1;
    [SerializeField] private ItemData axe;
    [SerializeField] private ItemData bucket;
    [SerializeField] private ItemData lever;
    [SerializeField] private ItemData key;
    [SerializeField] private ItemData sword;
    [SerializeField] private ItemData cursed_coin;
    [SerializeField] private ItemData rope;
    [SerializeField] private ItemData bridge_restorer;
    [SerializeField] private ItemData crowbar;
    [SerializeField] private ItemData tog;
    [SerializeField] private ItemData tod;
    [SerializeField] private ItemData tol;
    [SerializeField] private ItemData stone_tablet;
    [SerializeField] private ItemData scroll;
    [SerializeField] private ItemData letter;

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
        List<ItemStack> contents = SceneManager.Instance.GetChestContents(chestId, null);
        if (contents == null) return;

        for (int i = contents.Count - 1; i >= 0; i--)
        {
            if (contents[i] != null && contents[i].item == item)
            {
                contents.RemoveAt(i);
                return;
            }
        }
    }

    private void RemoveOneItemFromChest(string chestId, ItemData item)
    {
        List<ItemStack> contents = SceneManager.Instance.GetChestContents(chestId, null);
        if (contents == null) return;

        for (int i = contents.Count - 1; i >= 0; i--)
        {
            if (contents[i] != null && contents[i].item == item)
            {
                if (contents[i].amount > 1)
                    contents[i].amount--;
                else
                    contents.RemoveAt(i);
                return;
            }
        }
    }

    private int ChestItemAmount(string chestId, ItemData item)
    {
        List<ItemStack> contents = SceneManager.Instance.GetChestContents(chestId, null);
        if (contents == null) return 0;

        int amount = 0;
        foreach (ItemStack stack in contents)
        {
            if (stack != null && stack.item == item)
                amount += stack.amount;
        }
        return amount;
    }

    string inventoryGameState = "";

    string sceneV1GameState = "00000"; // Village 1
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
    string sceneF3GameState = "00"; // Forest 3
    string sceneFHGameState = "00"; // Forest House
    string sceneFBGameState = ""; // Foresthouse Basement
    string sceneFBTGameState = "0"; // Foresthouse Basement Tile
    string sceneGSGameState = "0"; // Gate Scene
    string sceneR1GameState = ""; // Ruin 1
    string sceneD1GameState = "0"; // Dungeon 1
    string sceneF2GameState = ""; // Forest 2
    string sceneV2GameState = "0000"; // Village 2
    string sceneH4GameState = "0"; // House 4
    string sceneWHGameState = "0"; // Wild Hut
    string sceneD2GameState = "0"; // Dungeon 2
    string sceneD3GameState = "0"; // Dungeon 3
    string sceneR3GameState = ""; // Ruin 3
    string sceneWHIGameState = "00"; // Wild Hut Interior
    string sceneG1GameState = ""; // Graveyard 1
    string sceneG2GameState = ""; // Graveyard 2
    string sceneG3GameState = "0"; // Graveyard 3
    string sceneF4GameState = "0"; // Forest 4
    string sceneGCGameState = "0"; // Great Clearing
    string sceneFoLGameState = "0"; // Fountain of Life
    string sceneR2GameState = ""; // Ruin 2
    string sceneGRD1GameState = "0"; // Great Ruin Door 1
    string sceneGRD2GameState = "0"; // Great Ruin Door 2
    string sceneH5GameState = "0"; // House 5
    string sceneH6GameState = "0000"; // House 6
    string sceneH7GameState = "0"; // House 7

    void Start()
    {
        Debug.Log("in GameStateManager");
        ApplyLoadedState();
    }

    private char DoorState(string doorId) => SceneManager.Instance.IsDoorOpen(doorId) ? '1' : '0';
    private char InteractableState(string interactableId) => SceneManager.Instance.IsInteractableUsed(interactableId) ? '1' : '0';

    private char ChestState(string chestId, ItemData expectedItem)
    {
        bool isOpen = SceneManager.Instance.IsChestOpen(chestId);
        if (!isOpen) return '0';
        if (expectedItem == null) return '1';
        return SceneManager.Instance.ChestContainsItem(chestId, expectedItem) ? '1' : '2';
    }

    public string CreateSaveString()
    {
        Debug.Log("CreateSaveString active");
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
            sceneD1GameState,
            sceneF2GameState,
            sceneV2GameState,
            sceneH4GameState,
            sceneWHGameState,
            sceneD2GameState,
            sceneD3GameState,
            sceneR3GameState,
            sceneWHIGameState,
            sceneG1GameState,
            sceneG2GameState,
            sceneG3GameState,
            sceneF4GameState,
            sceneGCGameState,
            sceneFoLGameState,
            sceneR2GameState,
            sceneGRD1GameState,
            sceneGRD2GameState,
            sceneH5GameState,
            sceneH6GameState,
            sceneH7GameState
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
        if (parts.Length < 43)
        {
            Debug.LogError("Invalid gameState. Expected 43 parts, got " + parts.Length);
            Debug.LogError("Received gameState: " + gameState);
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
        sceneF2GameState = parts[23];
        sceneV2GameState = parts[24];
        sceneH4GameState = parts[25];
        sceneWHGameState = parts[26];
        sceneD2GameState = parts[27];
        sceneD3GameState = parts[28];
        sceneR3GameState = parts[29];
        sceneWHIGameState = parts[30];
        sceneG1GameState = parts[31];
        sceneG2GameState = parts[32];
        sceneG3GameState = parts[33];
        sceneF4GameState = parts[34];
        sceneGCGameState = parts[35];
        sceneFoLGameState = parts[36];
        sceneR2GameState = parts[37];
        sceneGRD1GameState = parts[38];
        sceneGRD2GameState = parts[39];
        sceneH5GameState = parts[40];
        sceneH6GameState = parts[41];
        sceneH7GameState = parts[42];

        ApplyLoadedState();
    }

    public void CreateSaveStrings()
    {
        Debug.Log("CreateSaveStrings active");

        sceneV1GameState = "" + DoorState("door_0_A") + DoorState("door_0_B") + ChestState("chest_0_A", coin) + ChestState("chest_0_B", coin) + InteractableState("well_trigger1");
        sceneP1GameState = "" + DoorState("door_1_A") + ChestState("chest_1_A", coin);
        sceneF1GameState = "";
        sceneFRGameState = "";
        sceneH1GameState = "" + ChestState("chest_4_A", coin);
        sceneH2GameState = "";
        sceneH3GameState = "" + InteractableState("axe_trigger2");
        sceneC1GameState = "" + ChestState("chest_7_B", null) + ChestState("chest_7_C", unlockObject1);
        sceneRIVGameState = "" + ChestState("chest_8_A", coin);
        sceneP2GameState = "" + DoorState("door_9_A");
        sceneC3GameState = "" + InteractableState("lever_trigger");
        sceneC2GameState = "";
        sceneCTGameState = "" + InteractableState("pickaxe_trigger");
        sceneV3GameState = "" + DoorState("door_13_A") + DoorState("door_13_B") + ChestState("chest_13_A", coin) + ChestState("chest_13_B", null);
        sceneVCGameState = "" + ChestState("chest_14_A", stone_tablet) + ChestState("chest_14_B", tog);
        sceneF3GameState = "" + DoorState("door_15_A") + ChestState("chest_15_A", null);
        sceneFHGameState = "" + DoorState("door_16_A") + ChestState("chest_16_A", null);
        sceneFBGameState = "";
        sceneFBTGameState = "" + InteractableState("key_trigger");
        sceneGSGameState = "" + ChestState("chest_19_A", coin);
        sceneR1GameState = "";
        sceneD1GameState = "" + ChestState("chest_21_A", coin);
        sceneF2GameState = "";
        sceneV2GameState = "" + DoorState("door_23_A") + DoorState("door_23_B") + ChestState("chest_23_A", null) + ChestState("chest_23_B", coin);
        sceneH4GameState = "" + InteractableState("sword_trigger");
        sceneWHGameState = "" + DoorState("door_25_A");
        sceneD2GameState = "" + ChestState("chest_26_A", null);
        sceneD3GameState = "" + InteractableState("cursed_coin_trigger");
        sceneR3GameState = "";
        sceneWHIGameState = "" + ChestState("chest_29_A", coin) + InteractableState("rope_trigger");
        sceneG1GameState = "";
        sceneG2GameState = "";
        sceneG3GameState = "" + InteractableState("tod_trigger");
        sceneF4GameState = "" + ChestState("chest_33_A", null);
        sceneGCGameState = "" + ChestState("chest_34_A", null);
        sceneFoLGameState = "" + ChestState("chest_35_A", tol);
        sceneR2GameState = "";
        sceneGRD1GameState = "" + InteractableState("crowbar_trigger");
        sceneGRD2GameState = "" + InteractableState("bridge_restorer_trigger");
        sceneH5GameState = "" + ChestState("chest_39_A", letter);

        int h6ScrollAmount = ChestItemAmount("chest_40_A", scroll);
        int h6CoinAmount = ChestItemAmount("chest_40_A", coin);
        sceneH6GameState =
            "" + (SceneManager.Instance.IsChestOpen("chest_40_A") ? '1' : '0')
               + (h6ScrollAmount >= 1 ? '0' : '1')
               + (h6CoinAmount >= 1 ? '0' : '1')
               + (h6CoinAmount >= 2 ? '0' : '1');

        sceneH7GameState = "" + ChestState("chest_41_A", null);

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
        Debug.Log("F2: " + sceneF2GameState);
        Debug.Log("V2: " + sceneV2GameState);
        Debug.Log("H4: " + sceneH4GameState);
        Debug.Log("WH: " + sceneWHGameState);
        Debug.Log("D2: " + sceneD2GameState);
        Debug.Log("D3: " + sceneD3GameState);
        Debug.Log("R3: " + sceneR3GameState);
        Debug.Log("WHI: " + sceneWHIGameState);
        Debug.Log("G1: " + sceneG1GameState);
        Debug.Log("G2: " + sceneG2GameState);
        Debug.Log("G3: " + sceneG3GameState);
        Debug.Log("F4: " + sceneF4GameState);
        Debug.Log("GC: " + sceneGCGameState);
        Debug.Log("FoL: " + sceneFoLGameState);
        Debug.Log("R2: " + sceneR2GameState);
        Debug.Log("GRD1: " + sceneGRD1GameState);
        Debug.Log("GRD2: " + sceneGRD2GameState);
        Debug.Log("H5: " + sceneH5GameState);
        Debug.Log("H6: " + sceneH6GameState);
        Debug.Log("H7: " + sceneH7GameState);
    }

    private void ApplyLoadedState()
    {
        //================================================================= I N V E N T O R Y ====================================================================

        foreach (string i in inventoryGameState.Split(','))
        {
            switch (i)
            {
                case "0":
                    inventory.AddItem(new ItemStack
                    {
                        item = coin,
                        amount = 1
                    });
                    break;

                case "1":
                    inventory.AddItem(new ItemStack
                    {
                        item = unlockObject1,
                        amount = 1
                    });
                    break;

                case "2":
                    inventory.AddItem(new ItemStack
                    {
                        item = axe,
                        amount = 1
                    });
                    break;

                case "3":
                    inventory.AddItem(new ItemStack
                    {
                        item = bucket,
                        amount = 1
                    });
                    break;

                case "4":
                    inventory.AddItem(new ItemStack
                    {
                        item = lever,
                        amount = 1
                    });
                    break;

                case "5":
                    inventory.AddItem(new ItemStack
                    {
                        item = key,
                        amount = 1
                    });
                    break;

                case "6":
                    inventory.AddItem(new ItemStack
                    {
                        item = sword,
                        amount = 1
                    });
                    break;

                case "7":
                    inventory.AddItem(new ItemStack
                    {
                        item = cursed_coin,
                        amount = 1
                    });
                    break;

                case "8":
                    inventory.AddItem(new ItemStack
                    {
                        item = rope,
                        amount = 1
                    });
                    break;

                case "9":
                    inventory.AddItem(new ItemStack
                    {
                        item = bridge_restorer,
                        amount = 1
                    });
                    break;

                case "10":
                    inventory.AddItem(new ItemStack
                    {
                        item = crowbar,
                        amount = 1
                    });
                    break;

                case "11":
                    inventory.AddItem(new ItemStack
                    {
                        item = tog,
                        amount = 1
                    });
                    break;

                case "12":
                    inventory.AddItem(new ItemStack
                    {
                        item = tod,
                        amount = 1
                    });
                    break;

                case "13":
                    inventory.AddItem(new ItemStack
                    {
                        item = tol,
                        amount = 1
                    });
                    break;
            }
        }

        //----------------------------------------------------------------- S C E N E   V 1 --------------------------------------------------------------------0

        switch (sceneV1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_0_A", true);
                break;
        }

        switch (sceneV1GameState[1])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_0_B", true);
                break;
        }

        switch (sceneV1GameState[2])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_0_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_0_A", true);
                RemoveItemFromChest("chest_0_A", coin);
                break;
        }

        switch (sceneV1GameState[3])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_0_B", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_0_B", true);
                RemoveItemFromChest("chest_0_B", coin);
                break;
        }

        switch (sceneV1GameState[4])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("well_trigger1", true);
                break;
            case '2':
                SceneManager.Instance.SetInteractableUsed("well_trigger1", true);
                SceneManager.Instance.SetSceneEventState("scene3_special_used", true);
                SceneManager.Instance.SetSceneBackgroundIndex(3, 1);
                break;
        }

        //----------------------------------------------------------------- S C E N E   P 1 --------------------------------------------------------------------1

        switch (sceneP1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_1_A", true);
                break;
        }

        switch (sceneP1GameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_1_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_1_A", true);
                RemoveItemFromChest("chest_1_A", coin);
                break;
        }

        //----------------------------------------------------------------- S C E N E   F 1 --------------------------------------------------------------------2

        //----------------------------------------------------------------- S C E N E   F R --------------------------------------------------------------------3

        //----------------------------------------------------------------- S C E N E   H 1 --------------------------------------------------------------------4

        switch (sceneH1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_4_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_4_A", true);
                RemoveItemFromChest("chest_4_A", coin);
                break;
        }

        //----------------------------------------------------------------- S C E N E   H 2 --------------------------------------------------------------------5

        //----------------------------------------------------------------- S C E N E   H 3 --------------------------------------------------------------------6

        switch (sceneH3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("axe_trigger2", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   C 1 --------------------------------------------------------------------7

        switch (sceneC1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_7_B", true);
                break;
        }

        switch (sceneC1GameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_7_C", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_7_C", true);
                RemoveItemFromChest("chest_7_C", unlockObject1);
                break;
        }

        //----------------------------------------------------------------- S C E N E   R I V --------------------------------------------------------------------8

        switch (sceneRIVGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_8_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_8_A", true);
                RemoveItemFromChest("chest_8_A", coin);
                break;
        }

        //----------------------------------------------------------------- S C E N E   P 2 --------------------------------------------------------------------9

        switch (sceneP2GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_9_A", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   C 3 --------------------------------------------------------------------10

        switch (sceneC3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("lever_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   C 2 --------------------------------------------------------------------11

        //----------------------------------------------------------------- S C E N E   C T --------------------------------------------------------------------12

        switch (sceneCTGameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("pickaxe_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   V 3 --------------------------------------------------------------------13

        switch (sceneV3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_13_A", true);
                break;
        }

        switch (sceneV3GameState[1])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_13_B", true);
                break;
        }

        switch (sceneV3GameState[2])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_13_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_13_A", true);
                RemoveItemFromChest("chest_13_A", coin);
                break;
        }

        switch (sceneV3GameState[3])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_13_B", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   V C --------------------------------------------------------------------14

        switch (sceneVCGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_14_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_14_A", true);
                RemoveItemFromChest("chest_14_A", stone_tablet);
                break;
        }

        switch (sceneVCGameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_14_B", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_14_B", true);
                RemoveItemFromChest("chest_14_B", tog);
                break;
        }

        //----------------------------------------------------------------- S C E N E   F 3 --------------------------------------------------------------------15

        switch (sceneF3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_15_A", true);
                break;
        }

        switch (sceneF3GameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_15_A", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   F H --------------------------------------------------------------------16

        switch (sceneFHGameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_16_A", true);
                break;
        }

        switch (sceneFHGameState[1])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_16_A", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   F B --------------------------------------------------------------------17

        //----------------------------------------------------------------- S C E N E   F B T --------------------------------------------------------------------18

        switch (sceneFBTGameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("key_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   G G --------------------------------------------------------------------19

        switch (sceneGSGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_19_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_19_A", true);
                RemoveItemFromChest("chest_19_A", coin);
                break;
        }

        //----------------------------------------------------------------- S C E N E   R 1 --------------------------------------------------------------------20

        //----------------------------------------------------------------- S C E N E   D 1 --------------------------------------------------------------------21

        switch (sceneD1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_21_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_21_A", true);
                RemoveItemFromChest("chest_21_A", coin);
                break;
        }

        //----------------------------------------------------------------- S C E N E   F 2 --------------------------------------------------------------------22

        //----------------------------------------------------------------- S C E N E   V 2 --------------------------------------------------------------------23

        switch (sceneV2GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_23_A", true);
                break;
        }

        switch (sceneV2GameState[1])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_23_B", true);
                break;
        }

        switch (sceneV2GameState[2])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_23_A", true);
                break;
        }

        switch (sceneV2GameState[3])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_23_B", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_23_B", true);
                RemoveItemFromChest("chest_23_B", coin);
                break;
        }

        //----------------------------------------------------------------- S C E N E   H 4 --------------------------------------------------------------------24

        switch (sceneH4GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("sword_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   W H --------------------------------------------------------------------25

        switch (sceneWHGameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_25_A", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   D 2 --------------------------------------------------------------------26

        switch (sceneD2GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_26_A", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   D 3 --------------------------------------------------------------------27

        switch (sceneD3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("cursed_coin_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   R 3 --------------------------------------------------------------------28

        //----------------------------------------------------------------- S C E N E   W H I --------------------------------------------------------------------29

        switch (sceneWHIGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_29_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_29_A", true);
                RemoveItemFromChest("chest_29_A", coin);
                break;
        }

        switch (sceneWHIGameState[1])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("rope_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   G 1 --------------------------------------------------------------------30

        //----------------------------------------------------------------- S C E N E   G 2 --------------------------------------------------------------------31

        //----------------------------------------------------------------- S C E N E   G 3 --------------------------------------------------------------------32

        switch (sceneG3GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("tod_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   F 4 --------------------------------------------------------------------33

        switch (sceneF4GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_33_A", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   G C --------------------------------------------------------------------34

        switch (sceneGCGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_34_A", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   F o L --------------------------------------------------------------------35

        switch (sceneFoLGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_35_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_35_A", true);
                RemoveItemFromChest("chest_35_A", tol);
                break;
        }

        //----------------------------------------------------------------- S C E N E   R 2 --------------------------------------------------------------------36

        //----------------------------------------------------------------- S C E N E   G R D 1 --------------------------------------------------------------------37

        switch (sceneGRD1GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("crowbar_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   G R D 2 --------------------------------------------------------------------38

        switch (sceneGRD2GameState[0])
        {
            case '1':
                SceneManager.Instance.SetInteractableUsed("bridge_restorer_trigger", true);
                break;
        }

        //----------------------------------------------------------------- S C E N E   H 5 --------------------------------------------------------------------39

        switch (sceneH5GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_39_A", true);
                break;

            case '2':
                SceneManager.Instance.SetChestOpen("chest_39_A", true);
                RemoveItemFromChest("chest_39_A", letter);
                break;
        }

        //----------------------------------------------------------------- S C E N E   H 6 --------------------------------------------------------------------40

        if (sceneH6GameState[0] == '1')
            SceneManager.Instance.SetChestOpen("chest_40_A", true);

        if (sceneH6GameState[1] == '1')
            RemoveOneItemFromChest("chest_40_A", scroll);

        if (sceneH6GameState[2] == '1')
            RemoveOneItemFromChest("chest_40_A", coin);

        if (sceneH6GameState[3] == '1')
            RemoveOneItemFromChest("chest_40_A", coin);

        //----------------------------------------------------------------- S C E N E   H 7 --------------------------------------------------------------------41

        switch (sceneH7GameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_41_A", true);
                break;
        }


        SceneManager.Instance.ReloadCurrentScene();
    }
}

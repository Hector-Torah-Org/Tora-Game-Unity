using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ItemData coin;
    [SerializeField] private ItemData unlockObject1;
    [SerializeField] private ItemData axe;
    [SerializeField] private ItemData bucket;

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

    string sceneV1GameState = "00210";
    string sceneP1GameState = "00";
    string sceneF1GameState = "";
    string sceneFRGameState = "";
    string sceneH1GameState = "0";
    string sceneH2GameState = "";
    string sceneH3GameState = "0";
    string sceneC1GameState = "0000";
    string sceneRIVGameState = "0";
    string sceneP2GameState = "0";


    void Start()
    {
        Debug.Log("in GameStateManager");

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


        //----------------------------------------------------------------- S C E N E   V 1 --------------------------------------------------------------------

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

        //----------------------------------------------------------------- S C E N E   P 1 --------------------------------------------------------------------

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

        //----------------------------------------------------------------- S C E N E   F 1 --------------------------------------------------------------------

        //----------------------------------------------------------------- S C E N E   F R --------------------------------------------------------------------

        //----------------------------------------------------------------- S C E N E   H 1 --------------------------------------------------------------------

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

        //----------------------------------------------------------------- S C E N E   H 2 --------------------------------------------------------------------

        //----------------------------------------------------------------- S C E N E   H 3 --------------------------------------------------------------------

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

        //----------------------------------------------------------------- S C E N E   C 1 --------------------------------------------------------------------

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
         
        //----------------------------------------------------------------- S C E N E   R I V --------------------------------------------------------------------

        switch (sceneRIVGameState[0])
        {
            case '1':
                SceneManager.Instance.SetChestOpen("chest_8_A", true);                                  // Set chest_8_A to open
                break;
        }

        //----------------------------------------------------------------- S C E N E   P 2 --------------------------------------------------------------------

        switch (sceneP2GameState[0])
        {
            case '1':
                SceneManager.Instance.SetDoorOpen("door_9_A", true);                                    // Set door_9_A to open
                break;
        }


        SceneManager.Instance.ReloadCurrentScene();
    }
}
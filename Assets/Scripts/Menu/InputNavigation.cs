using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InputNavigation : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    public Button clickButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        system = EventSystem.current;
        firstInput.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            clickButton.onClick.Invoke();
            Debug.Log("Button pressed");
        }
    }
}

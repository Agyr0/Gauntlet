using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CanvasState
{
    Start,
    Level,
    GameOver,
    Pause
};
public class UIManager : Singleton<UIManager>
{
    public CanvasState state = CanvasState.Start;
    private List<Canvas> curCanvas = new List<Canvas>();

    [Header("Level Canvas")]
    public Canvas levelCanvas;
    [SerializeField] private GameObject title, warrior, valkyrie, wizzard, elf;
    [Space(10)]
    [SerializeField]
    private Text levelText, warriorScore, warriorHealth,
        valkyrieScore, valkyrieHealth, wizzardScore, wizzardHealth, elfScore, elfHealth;
    [SerializeField] private GameObject warriorInventory, valkyrieInventory, wizzardInventory, elfInventory;


    private void Start()
    {
        curCanvas.Add(levelCanvas);
    }



    public void HandleCanvasState()
    {
        switch (state) 
        {
            case CanvasState.Start:
                break;
            case CanvasState.Level:
                break;
            case CanvasState.GameOver:
                break;
            case CanvasState.Pause:
                break;
        
        }
    }




    //Function for turning off all canvases in list other than "selected"
    private void DisableOtherCanvas(Canvas selected)
    {
        for (int i = 0; i < curCanvas.Count; i++)
        {
            //If not selected canvas turn off
            if (curCanvas[i] != selected)
                curCanvas[i].gameObject.SetActive(false);
            //If is selected canvas turn on
            else
                curCanvas[i].gameObject.SetActive(true);
        }
    }

    //Function to turn cursor on or off
    private void CursorState(bool isOn)
    {
        if (isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else if (!isOn)
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}

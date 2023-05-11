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
    [SerializeField]
    private Canvas startCanvas;

    [SerializeField]
    private Canvas levelCanvas;
    [SerializeField] private GameObject title, warrior, valkyrie, wizzard, elf;

    [SerializeField]
    private Canvas pauseCanvas;

    [SerializeField]
    private Text levelText, warriorScore, warriorHealth,
        valkyrieScore, valkyrieHealth, wizzardScore, wizzardHealth, elfScore, elfHealth;
    [SerializeField] private GameObject warriorInventory, valkyrieInventory, wizzardInventory, elfInventory;


    private void OnEnable()
    {
        EventBus.Subscribe(EventType.PLAYER_JOINED, AddPlayerToUI);
        EventBus.Subscribe(EventType.PLAYER_LEFT, RemovePlayerFromUI);
        EventBus.Subscribe(EventType.NEXT_ROUND, HandleRoundNumber);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.PLAYER_JOINED, AddPlayerToUI);
        EventBus.Unsubscribe(EventType.PLAYER_LEFT, RemovePlayerFromUI);
        EventBus.Unsubscribe(EventType.NEXT_ROUND, HandleRoundNumber);
    }


    private void Start()
    {
        CursorState(false);
        SetTimeScale(false);
        curCanvas.Add(startCanvas);
        curCanvas.Add(levelCanvas);
        curCanvas.Add(pauseCanvas);
        Debug.Log(state);
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

    #region Buttons
    public void StartGame()
    {
        state = CanvasState.Start;
        SetTimeScale(true);
        EventBus.Publish(EventType.ENABLE_JOINING);
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
    public void PauseGame()
    {
        state = CanvasState.Pause;
        SetTimeScale(false);
    }
    public void ResumeGame()
    {
        state = CanvasState.Level;
        SetTimeScale(true);
    }
    #endregion


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
    private void SetTimeScale(bool isPlaying)
    {
        Time.timeScale = isPlaying ? 1f : 0f;
    }
    private void HandleRoundNumber()
    {
        levelText.text = "LEVEL\t" + GameManager.Instance.Level;
    }

    public void HandleScoreUI(ClassData _class)
    {
        switch (_class.ClassType)
        {
            case ClassEnum.Warrior:
                warriorScore.text = _class.Score.ToString();
                break;
            case ClassEnum.Valkyrie:
                valkyrieScore.text = _class.Score.ToString();
                break;
            case ClassEnum.Wizard:
                wizzardScore.text = _class.Score.ToString();
                break;
            case ClassEnum.Elf:
                elfScore.text = _class.Score.ToString();
                break;
            default:
                break;
        }
    }
    public void HandleHealthUI(ClassData _class)
    {
        switch (_class.ClassType)
        {
            case ClassEnum.Warrior:
                warriorHealth.text = _class.CurHealth.ToString();
                break;
            case ClassEnum.Valkyrie:
                valkyrieHealth.text = _class.CurHealth.ToString();
                break;
            case ClassEnum.Wizard:
                wizzardHealth.text = _class.CurHealth.ToString();
                break;
            case ClassEnum.Elf:
                elfHealth.text = _class.CurHealth.ToString();
                break;
            default:
                break;
        }
    }
    private void AddPlayerToUI()
    {
        ClassData newPlayer = PlayerManager.Instance.playerConfigs[PlayerManager.Instance.playerConfigs.Count - 1].PlayerParent.GetComponent<PlayerController>().classData;

        switch (newPlayer.ClassType)
        {
            case ClassEnum.Warrior:
                warrior.gameObject.SetActive(true);
                break;
            case ClassEnum.Valkyrie:
                valkyrie.gameObject.SetActive(true);
                break;
            case ClassEnum.Wizard:
                wizzard.gameObject.SetActive(true);
                break;
            case ClassEnum.Elf:
                elf.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void RemovePlayerFromUI()
    {

    }
}

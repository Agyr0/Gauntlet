using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public enum CanvasState
{
    Start,
    Level,
    GameOver,
    Pause,
    Credits
};
public class UIManager : Singleton<UIManager>
{
    public CanvasState state = CanvasState.Start;
    [HideInInspector] public MultiplayerEventSystem _eventSystem;
    private List<Canvas> curCanvas = new List<Canvas>();

    [SerializeField]
    private Canvas startCanvas;
    [SerializeField] private Button startFirstSelectedButton;

    [SerializeField]
    private Canvas levelCanvas;
    [SerializeField] private GameObject title, warrior, valkyrie, wizzard, elf;

    [SerializeField]
    private Canvas pauseCanvas;
    [SerializeField] private Button pauseFirstSelectedButton;
    public bool isPaused = false;

    [SerializeField]
    private Text levelText, warriorScore, warriorHealth,
        valkyrieScore, valkyrieHealth, wizzardScore, wizzardHealth, elfScore, elfHealth;
    [SerializeField] public GameObject warriorInventory, valkyrieInventory, wizzardInventory, elfInventory;

    [SerializeField]
    private Canvas gameOverCanvas;
    [SerializeField] private Button gameOverFirstSelectedButton;

    [SerializeField]
    private Canvas creditsCanvas;
    [SerializeField] private Button creditsFirstSelectedButton;


    private void OnEnable()
    {
        EventBus.Subscribe(EventType.PLAYER_JOINED, AddPlayerToUI);
        EventBus.Subscribe(EventType.PLAYER_LEFT, RefreshPlayerUIActiveState);
        EventBus.Subscribe(EventType.LEVEL_CHANGED, HandleRoundNumber);
        EventBus.Subscribe(EventType.UI_CHANGED, HandleCanvasState);

    }


    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.PLAYER_JOINED, AddPlayerToUI);
        EventBus.Unsubscribe(EventType.PLAYER_LEFT, RefreshPlayerUIActiveState);
        EventBus.Unsubscribe(EventType.LEVEL_CHANGED, HandleRoundNumber);
        EventBus.Unsubscribe(EventType.UI_CHANGED, HandleCanvasState);

    }


    private void Start()
    {
        CursorState(false);
        SetTimeScale(false);
        curCanvas.Add(startCanvas);
        curCanvas.Add(levelCanvas);
        curCanvas.Add(pauseCanvas);
        curCanvas.Add(gameOverCanvas);
        curCanvas.Add(creditsCanvas);
        EventBus.Publish(EventType.UI_CHANGED);

    }



    public void HandleCanvasState()
    {
        switch (state) 
        {
            case CanvasState.Start:
                DisplayStartCanvas();
                break;
            case CanvasState.Level:
                DisplayLevelCanvas();
                break;
            case CanvasState.GameOver:
                DisplayGameOverCanvas();
                break;
            case CanvasState.Pause:
                DisplayPausedCanvas();
                break;
            case CanvasState.Credits:
                DisplayCreditsCanvas();
                break;

        }
    }

    #region Display Canvas

    private void DisplayStartCanvas()
    {
        DisableOtherCanvas(startCanvas);
        
        startFirstSelectedButton.Select();
        for (int i = 0; i < PlayerManager.Instance.playerConfigs.Count; i++)
        {
            PlayerManager.Instance.playerConfigs[i].PlayerParent.GetComponent<PlayerController>().AllowInput(false);
        }
        SetTimeScale(false);
        CursorState(false);
    }
    private void DisplayLevelCanvas()
    {
        DisableOtherCanvas(levelCanvas);
        isPaused = false;
        NaratorManager.Instance.audioSource.UnPause();
        for (int i = 0; i < PlayerManager.Instance.playerConfigs.Count; i++)
        {
            PlayerManager.Instance.playerConfigs[i].PlayerParent.GetComponent<PlayerController>().AllowInput(true);
        }
        SetTimeScale(true);
        CursorState(false);

    }
    private void DisplayGameOverCanvas()
    {
        DisableOtherCanvas(gameOverCanvas);
        SetTimeScale(false);
        CursorState(false);
        gameOverFirstSelectedButton.Select();
    }
    private void DisplayPausedCanvas()
    {
        DisableOtherCanvas(pauseCanvas, levelCanvas);
        for (int i = 0; i < PlayerManager.Instance.playerConfigs.Count; i++)
        {
            PlayerManager.Instance.playerConfigs[i].PlayerParent.GetComponent<PlayerController>().AllowInput(false);
        }
        SetTimeScale(false);
        CursorState(false);
        pauseFirstSelectedButton.Select();
    }
    private void DisplayCreditsCanvas()
    {
        DisableOtherCanvas(creditsCanvas);
        for (int i = 0; i < PlayerManager.Instance.playerConfigs.Count; i++)
        {
            PlayerManager.Instance.playerConfigs[i].PlayerParent.GetComponent<PlayerController>().AllowInput(false);
        }
        SetTimeScale(false);
        CursorState(false);
        creditsFirstSelectedButton.Select();
    }

    #endregion

    #region Buttons
    public void StartGame()
    {
        state = CanvasState.Level;
        EventBus.Publish(EventType.UI_CHANGED);        
        EventBus.Publish(EventType.ENABLE_JOINING);
        EventBus.Publish(EventType.LEVEL_CHANGED);
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
   
    public void ResumeGame()
    {
        state = CanvasState.Level;
        EventBus.Publish(EventType.UI_CHANGED);
    }
    public void Credits()
    {
        state = CanvasState.Credits;
        EventBus.Publish(EventType.UI_CHANGED);
    }
    public void Menu()
    {
        state = CanvasState.Start;
        GameManager.Instance.ResetGame();
        EventBus.Publish(EventType.UI_CHANGED);
    }

    public void PlayerLeave()
    {
        //If there is more than one player than let them leave
        if (PlayerManager.Instance.playerConfigs.Count > 1)
        {
            PlayerController leavingPlayer = null;
            leavingPlayer = _eventSystem.playerRoot.GetComponent<PlayerController>();
            Debug.LogWarning(leavingPlayer);
            //Resets players stats to base and then destorys player
            for (int i = 0; i < PlayerManager.Instance.playerConfigs.Count; i++)
            {
                leavingPlayer.classData.ResetValuesToDefault();
                //Clears player lists
                PlayerManager.Instance.playerConfigs.RemoveAt(i);
                PlayerManager.Instance.usedClasses.RemoveAt(i);
                Destroy(leavingPlayer.gameObject);
            }
            EventBus.Publish(EventType.PLAYER_LEFT);
        }
        //If there is only one player then take back to start and reset game
        else
            Menu();
    }
    #endregion


    /// <summary>
    /// Turns off all canvases in <paramref name="curCanvas"/> other than <paramref name="selected"/>
    /// </summary>
    /// <param name="selected"></param>
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
    /// <summary>
    /// Turns off all canvases in <paramref name="curCanvas"/> other than <paramref name="selected"/> and <paramref name="secondary"/>
    /// </summary>
    /// <param name="selected"></param> <param name="secondary"></param>
    private void DisableOtherCanvas(Canvas selected, Canvas secondary)
    {
        for (int i = 0; i < curCanvas.Count; i++)
        {
            //If not selected canvas turn off
            if (curCanvas[i] != selected && curCanvas[i] != secondary)
                curCanvas[i].gameObject.SetActive(false);
            //If is selected canvas turn on
            else
                curCanvas[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Sets the cursor lockState and visibility to <paramref name="isOn"/>
    /// </summary>
    /// <param name="isOn"></param>
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
    /// <summary>
    /// Sets time scale to 1 if <paramref name="isPlaying"/> is true or to 0 if <paramref name="isPlaying"/> is false
    /// </summary>
    /// <param name="isPlaying"></param>
    private void SetTimeScale(bool isPlaying)
    {
        Time.timeScale = isPlaying ? 1f : 0f;
    }
    private void HandleRoundNumber()
    {
        levelText.text = "LEVEL\t" + GameManager.Instance.Level;
        Debug.Log(GameManager.Instance.Level);
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

    private void RefreshPlayerUIActiveState()
    {
        warrior.gameObject.SetActive(false);
        valkyrie.gameObject.SetActive(false);
        wizzard.gameObject.SetActive(false);
        elf.gameObject.SetActive(false);

        for (int i = 0; i < PlayerManager.Instance.playerConfigs.Count; i++)
        {
            ClassData player = PlayerManager.Instance.playerConfigs[i].PlayerParent.GetComponent<PlayerController>().classData;

            switch (player.ClassType)
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
    }
}

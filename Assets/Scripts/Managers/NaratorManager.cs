using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PromptType
{
    NONE,
    Controls,
    NoFirendlyFireYet,
    DontShootFood,
    PlayerShotFood,
    PlayerNeedsFood,
    NiceJob,
    TryAndGetOut,
    LowHealth,
    AboutToDie
}

[System.Serializable]
public class NarationPrompt
{
    [SerializeField]
    public PromptType Prompt;
    [SerializeField]
    public int PromptID;
    [SerializeField]
    public AudioClip MainClip;
    [SerializeField]
    public bool ToPlayer;
    [SerializeField, HideInInspector]
    public ClassData DirectedToPlayer;


    [SerializeField]
    public ClassEnum DirectedClass { get { return DirectedToPlayer.ClassType; } }
    [SerializeField]
    public AudioClip DirectedClip { get { return DirectedToPlayer.ClassNameClip; } }
    
    
}



[RequireComponent(typeof(AudioSource))]
public class NaratorManager : Singleton<NaratorManager>
{
    #region Variables
    [SerializeField, HideInInspector]
    private AudioSource audioSource;
    [SerializeField]
    private float waitTimeForRandomClip = 20f;
    private IEnumerator RandomClip;

    #region Lists
    public List<ClassData> classList = new List<ClassData>();
    [ListElementTitle("Prompt")]
    public List<NarationPrompt> promptList = new List<NarationPrompt>();
    #endregion

    #region Private Variables
    private PromptType _promptToPlay;
    private int _promtIDToPlay = 0;
    private ClassData _playerDirected;
    #endregion

    #region Properties
    public PromptType PromptToPlay
    {
        get { return _promptToPlay; }
        set { _promptToPlay = value; }
    }
    public int PromptIDToPlay
    {
        get { return _promtIDToPlay; }
        set { _promtIDToPlay = value; }
    }
    public ClassData PlayerDirected
    {
        get { return _playerDirected; }
        set { _playerDirected = value; }
    }
    #endregion
    #endregion
    #region OnEnable/OnDisable
    private void OnEnable()
    {
        EventBus.Subscribe(EventType.NARATION, PlayNaration);

        EventBus.Subscribe(EventType.GAME_START, AssignAndPlayControlsAudio);

        EventBus.Subscribe(EventType.N_NO_FRIENDLY_FIRE, AssignAndPlayNoFriendlyFire);
        EventBus.Subscribe(EventType.N_DONT_SHOOT_FOOD, AssignAndPlayDontShootFood);
        EventBus.Subscribe(EventType.N_NAME_SHOT_FOOD, AssignAndPlayPlayerShotFood);
        EventBus.Subscribe(EventType.N_NAME_NEEDS_FOOD, AssignAndPlayPlayerNeedsFood);
        EventBus.Subscribe(EventType.N_NICE_JOB, AssignAndPlayNiceJob);
        EventBus.Subscribe(EventType.N_TRY_AND_GET_OUT, AssignAndPlayTryAndGetOut);
        EventBus.Subscribe(EventType.N_NAME_LOW_HEALTH, AssignAndPlayLowHealth);
        EventBus.Subscribe(EventType.N_NAME_ABOUT_TO_DIE, AssignAndPlayAboutToDie);

        audioSource = GetComponent<AudioSource>();
        RandomClip = PlayRandomClips();
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.NARATION, PlayNaration);
    }
    #endregion



    #region Set up stuff
   

    #region Assign Narations
    /// <summary>
    /// Must call this function to assign the naration info before the NARATION event is published
    /// </summary>
    /// <param name="_prompt"></param>
    /// <param name="_promptID"></param>
    public void AssignNaration(PromptType _prompt, int _promptID)
    {
        PromptToPlay = _prompt;
        PromptIDToPlay = _promptID;
    }
    /// <summary>
    /// Must call this function to assign the naration info before the NARATION event is published
    /// </summary>
    /// <param name="m_prompt"></param>
    /// <param name="m_promptID"></param>
    /// <param name="m_playerDirected"></param>
    public void AssignNaration(PromptType m_prompt, int m_promptID, ClassData m_playerDirected)
    {
        PromptToPlay = m_prompt;
        PromptIDToPlay = m_promptID;
        PlayerDirected = m_playerDirected;
    }

    /// <summary>
    /// One and done assign and play naration clip
    /// </summary>
    /// <param name="_prompt"></param>
    /// <param name="_promptID"></param>
    public void AssignNarationAndPlay(PromptType _prompt, int _promptID)
    {
        PromptToPlay = _prompt;
        PromptIDToPlay = _promptID;
        EventBus.Publish(EventType.NARATION);
    }
    /// <summary>
    /// One and done assign and play naration clip
    /// <param name="m_prompt"></param>
    /// <param name="m_promptID"></param>
    /// <param name="m_playerDirected"></param>
    public void AssignNarationAndPlay(PromptType m_prompt, int m_promptID, ClassData m_playerDirected)
    {
        PromptToPlay = m_prompt;
        PromptIDToPlay = m_promptID;
        PlayerDirected = m_playerDirected;
        EventBus.Publish(EventType.NARATION);
    }
    #endregion


    //Finds naration clip that is assigned in PromptList and plays it
    private void PlayNaration()
    {
        //Go through Prompt list
        for (int i = 0; i < promptList.Count; i++)
        {
            NarationPrompt prompt = promptList[i];
            //If found the _promptToPlay
            if(prompt.Prompt == _promptToPlay)
            {
                //If the _promptToPlay matches the _promptIDToPlay
                if(prompt.PromptID == _promtIDToPlay)
                {
                    //If the _promptToPlay is directed towards a player
                    if (prompt.ToPlayer)
                    {
                        //Assign the Class that the prompt is directed to
                        prompt.DirectedToPlayer = PlayerDirected;
                        //Play the Class Name Clip
                        audioSource.PlayOneShot( prompt.DirectedClip);
                    }
                    //Play the Prompt Clip
                    audioSource.PlayOneShot(prompt.MainClip);
                }
            }
        }
    }

    #endregion

    #region Assign and Play Clips
    private void AssignAndPlayControlsAudio()
    {
        AssignNarationAndPlay(PromptType.Controls, 0);
        StartCoroutine(RandomClip);
    }
    private void AssignAndPlayNoFriendlyFire()
    {
        AssignNarationAndPlay(PromptType.NoFirendlyFireYet, 1);
    }
    private void AssignAndPlayDontShootFood()
    {
        AssignNarationAndPlay(PromptType.DontShootFood, 2);
    }
    private void AssignAndPlayPlayerShotFood()
    {
        AssignNarationAndPlay(PromptType.PlayerShotFood, 3, PlayerDirected);
    }
    private void AssignAndPlayPlayerNeedsFood()
    {
        AssignNarationAndPlay(PromptType.PlayerNeedsFood, 4, PlayerDirected);
    }
    private void AssignAndPlayNiceJob()
    {
        AssignNarationAndPlay(PromptType.NiceJob, 5);
    }
    private void AssignAndPlayTryAndGetOut()
    {
        AssignNarationAndPlay(PromptType.TryAndGetOut, 6);
    }
    private void AssignAndPlayLowHealth()
    {
        AssignNarationAndPlay(PromptType.LowHealth, 7, PlayerDirected);
    }
    private void AssignAndPlayAboutToDie()
    {
        AssignNarationAndPlay(PromptType.AboutToDie, 8, PlayerDirected);
    }
    #endregion


    #region Handle Narator
    [SerializeField]
    private List<EventType> randomNarationEvents = new List<EventType>();

    private IEnumerator PlayRandomClips()
    {
        int lastIndex = 0;
        while (true)
        {
            int index = Random.Range(0, randomNarationEvents.Count);
            yield return new WaitForSeconds(waitTimeForRandomClip);
            if (!audioSource.isPlaying && index != lastIndex)
            {
                lastIndex = index;
                EventType eventToSend = randomNarationEvents[index];
                EventBus.Publish(eventToSend);
            }
            yield return null;
        }
    }
    #endregion
}

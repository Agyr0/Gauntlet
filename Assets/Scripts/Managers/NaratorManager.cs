using System.Collections;
using System.Collections.Generic;
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
    [SerializeField, HideInInspector]
    private AudioSource audioSource;

    public List<ClassData> classList = new List<ClassData>();
    public List<NarationPrompt> promptList = new List<NarationPrompt>();

    private PromptType _promptToPlay;
    private int _promtIDToPlay = 0;
    private ClassData _playerDirected;

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



    private void OnEnable()
    {
        EventBus.Subscribe(EventType.NARATION, PlayNaration);
        audioSource = GetComponent<AudioSource>();
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.NARATION, PlayNaration);
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Send Naration Event"))
            EventBus.Publish(EventType.NARATION);
        GUILayout.Space(50);
        if (GUILayout.Button("Assign first Controls naration"))
            AssignNaration(PromptType.Controls, 0);
        if (GUILayout.Button("Assign 9th Controls naration"))
            AssignNaration(PromptType.Controls, 9);
        if (GUILayout.Button("Assign last Controls naration towards Valkyrie"))
            AssignNaration(PromptType.Controls, 10, classList[1]);
        if (GUILayout.Button("Assign first LowHealth naration towards Elf"))
            AssignNaration(PromptType.LowHealth, 7, classList[3]);
    }

    /// <summary>
    /// Must Call this function to assign the naration info before the NARATION event is published
    /// </summary>
    /// <param name="_prompt"></param>
    /// <param name="_promptID"></param>
    public void AssignNaration(PromptType _prompt, int _promptID)
    {
        PromptToPlay = _prompt;
        PromptIDToPlay = _promptID;
    }
    /// <summary>
    /// Must Call this function to assign the naration info before the NARATION event is published
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
}

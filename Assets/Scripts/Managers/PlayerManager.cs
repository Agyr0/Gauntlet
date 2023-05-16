using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Info")]
    [HideInInspector]
    public PlayerInputManager playerManager;
    public List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();
    [SerializeField] private GameObject camTarget;

    [Header("Variables")]
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField]
    private int maxPlayers = 4;

    [SerializeField]
    private List<ClassData> possibleClasses = new List<ClassData>();
    public List<int> usedClasses = new List<int>();

    private void OnEnable()
    {
        playerManager = GetComponent<PlayerInputManager>();
        EventBus.Subscribe(EventType.ENABLE_JOINING, EnablePlayerJoining);
        EventBus.Subscribe(EventType.DISABLE_JOINING, DisablePlayerJoining);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(EventType.ENABLE_JOINING, EnablePlayerJoining);
        EventBus.Unsubscribe(EventType.DISABLE_JOINING, DisablePlayerJoining);

    }

    private void Update()
    {
        if(playerConfigs.Count != 0)
            camTarget.transform.position = SetAvgPosition();
        
    }
    private void EnablePlayerJoining()
    {
        playerManager.EnableJoining();
    }
    private void DisablePlayerJoining()
    {
        playerManager.DisableJoining();

    }

    public void HandlePlayerJoin(PlayerInput player)
    {
        //On first Player Joined send out GAME_START event
        if (playerConfigs.Count == 0)
        {
            EventBus.Publish(EventType.GAME_START);
        }
       // Debug.Log(player.gameObject.GetComponent<PlayerController>().classData.PlayerPrefab);
        
        Debug.Log("Player Joined" + player.playerIndex);
        if(!playerConfigs.Any(p => p.PlayerIndex == player.playerIndex))
        {
            player.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(player));
        }
        //Assign prefab to spawn
        //playerManager.playerPrefab = playerConfigs[player.playerIndex].PlayerClass.PlayerPrefab;
        //Give the new player a inventory
        InventoryManager.Instance.LinkInventory(player.gameObject.GetComponent<PlayerController>());
        //Moves player to valid spawn location
        playerConfigs[player.playerIndex].PlayerParent.GetComponent<CharacterController>().enabled = false;
        playerConfigs[player.playerIndex].PlayerParent.position = FindSpawnPos();
        playerConfigs[player.playerIndex].PlayerParent.GetComponent<CharacterController>().enabled = transform;
        //Assign a random class index to player
        int classIndex = Random.Range(0, possibleClasses.Count);
        //If the random classIndex has been used already
        if (usedClasses.Contains(classIndex))
        {
            //Loop till you get a new one thats available
            while (usedClasses.Contains(classIndex))
            {
                classIndex = Random.Range(0, possibleClasses.Count);
            }
        }
        //Assign the new player with the class at classIndex
        playerConfigs[player.playerIndex].PlayerParent.GetComponent<PlayerController>().classData = possibleClasses[classIndex];
        //Add the class index to the usedClasses
        usedClasses.Add(classIndex);
        EventBus.Publish(EventType.PLAYER_JOINED);
    }
    public void HandlePlayerLeft(PlayerInput player)
    {
        EventBus.Publish(EventType.PLAYER_LEFT);
    }


    private Vector3 FindSpawnPos()
    {
        Vector2 newPos = Random.insideUnitCircle * spawnRadius * camTarget.transform.position;
        int maxTrys = 10;
        int curTry = 0;
        while (curTry < maxTrys)
        {
            curTry++;
            Debug.Log("Finding available spawn location");
            newPos = Random.insideUnitCircle * spawnRadius;

            if (!Physics.CheckSphere(newPos, spawnRadius, ~LayerMask.GetMask("Floor")))
            {
                Debug.Log("Spawn location found at " + newPos);
                break;
            }
        }
        if(curTry >= maxTrys)
            Debug.LogError("Couldn't find a good spawn location \nGoing to default spawn location");
        
        Vector3 spawnPos = new Vector3(newPos.x, 0, newPos.y);
        return spawnPos;
    }

    private Vector3 SetAvgPosition()
    {
        float totalX = 0f;
        float totalZ = 0f;
        foreach (PlayerConfiguration player in playerConfigs) 
        {
            totalX += player.Input.transform.position.x;
            totalZ += player.Input.transform.position.z;
        }
        float avgX = totalX / playerConfigs.Count;
        float avgZ = totalZ / playerConfigs.Count;

        return new Vector3(avgX, 0, avgZ);

    }
}


[System.Serializable]
public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput player)
    {
        PlayerClass = player.gameObject.GetComponent<PlayerController>().classData;
        PlayerParent = player.transform;
        PlayerIndex = player.playerIndex;
        Input = player;
        PlayedAboutToDie = false;
        PlayedLowHealth = false;
        PlayedNeedFood = false;
        PlayerDevice = player.devices[0];
    }
    [SerializeField]
    public ClassData PlayerClass { get; set; }
    [SerializeField]
    public Transform PlayerParent { get; set; }
    [SerializeField]
    public PlayerInput Input { get; set; }
    [SerializeField]
    public int PlayerIndex { get; set; }
    [SerializeField]
    public bool PlayedAboutToDie { get; set; }
    [SerializeField]
    public bool PlayedLowHealth { get; set; }
    [SerializeField]
    public bool PlayedNeedFood { get; set; }
    [SerializeField]
    public InputDevice PlayerDevice { get; set; }
    

}
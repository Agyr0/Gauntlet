using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : IPlayer
{
    public AudioClip ClassNameClip { get; }
    public GameObject PlayerPrefab { get; }
    public GameObject ProjectilePrefab { get; }
    public ClassEnum ClassType { get; }
    public float CurHealth { get; set; }
    public float CurSpeed { get; }
    public float ShootTime { get; }
    public int Keys { get; set; }
    public int Potions { get; set; }
    public int NumItems { get; set; }
    public float Melee { get; }
    public float Strength { get; }
    public float Magic { get; }
    public float Score { get; set; }

    private readonly ClassData player;

    public Player(ClassData data)
    {
        player = data;
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Class Data", fileName = "NewClassData", order = 1)]
public class ClassData : ScriptableObject, IPlayer
{
    [SerializeField]
    private AudioClip classNameClip;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private ClassEnum classType;
    [SerializeField]
    private float curHealth;
    [SerializeField]
    private float curSpeed;
    [SerializeField]
    private float shootTime;
    [SerializeField]
    private int keys;
    [SerializeField]
    private int potions;
    [SerializeField]
    private int numItems;
    [SerializeField]
    private float melee;
    [SerializeField]
    private float strength;
    [SerializeField]
    private float magic;
    [SerializeField]
    private float score;

    [SerializeField, HideInInspector]
    private GameObject myUIInventory;

    public AudioClip ClassNameClip
    {
        get { return classNameClip; }
    }
    public GameObject PlayerPrefab
    {
        get { return playerPrefab; }
    }
    public GameObject ProjectilePrefab 
    { 
        get { return projectilePrefab; }
    }
    public ClassEnum ClassType
    {
        get { return classType; }
    }
    public float CurHealth
    {
        get { return curHealth; }
        set { curHealth = value; }
    }
    public float CurSpeed
    {
        get { return curSpeed; }
    }
    public float ShootTime 
    {
        get { return shootTime; } 
    }
    public int Keys
    {
        get { return keys; }
        set { keys = value; }
    }
    public int Potions
    {
        get { return potions; }
        set { potions = value; }
    }
    public int NumItems
    {
        get { return numItems; }
        set { numItems = value; }
    }
    public float Melee
    {
        get { return melee; }
    }
    public float Strength
    {
        get { return strength; }
    }
    public float Magic
    {
        get { return magic; }
    }
    public float Score
    {
        get { return score; }
        set { score = value; }

    }

    public GameObject MyUIInventory
    {
        get { return myUIInventory; }
        set { myUIInventory = value; }
    }

    public void ResetValuesToDefault()
    {
        CurHealth = 700f;
        Score = 0f;
        Keys = 0;
        Potions = 0;
        NumItems = 0;
    }
}

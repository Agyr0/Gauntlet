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
    private ClassData curClass;
    [SerializeField]
    private ClassEnum classType;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float curHealth;
    [SerializeField]
    private float baseSpeed;
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
    public ClassData CurClass
    {
        get { return this; }
    }
    public ClassEnum ClassType
    {
        get { return classType; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
    }
    public float CurHealth
    {
        get { return curHealth; }
    }
    public float BaseSpeed
    {
        get { return baseSpeed; }
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
    }
    public int Potions
    {
        get { return potions; }
    }
    public int NumItems
    {
        get { return numItems; }
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
}

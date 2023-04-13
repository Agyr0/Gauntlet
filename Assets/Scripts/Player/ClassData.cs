using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Class Data", fileName = "NewClassData", order = 1)]
public class ClassData : ScriptableObject, IPlayer
{
    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    public ClassData curClass;
    public ClassEnum classType;
    public float maxHealth;
    public float curHealth;
    public float baseSpeed;
    public float curSpeed;
    public int keys;
    public int potions;
    public int numItems;
    public float melee;
    public float strength;
    public float magic;



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

using UnityEngine;

public enum ClassEnum
{
    Warrior,
    Valkyrie,
    Wizard,
    Elf

}
public interface IPlayer
{
    AudioClip ClassNameClip { get; }
    GameObject PlayerPrefab { get; }
    GameObject ProjectilePrefab { get; }
    ClassEnum ClassType { get; }
    float CurHealth { get; set; }
    float CurSpeed { get; }
    float ShootTime { get; }
    int Keys { get; set; }
    int Potions { get; set; }
    int NumItems { get; set; }
    float Melee { get; }
    float Strength { get; }
    float Magic { get; }
    float Score { get; set; }
}

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
    GameObject PlayerPrefab { get; }
    GameObject ProjectilePrefab { get; }
    ClassData CurClass { get; }
    ClassEnum ClassType { get; }
    float MaxHealth { get; }
    float CurHealth { get; }
    float BaseSpeed { get; }
    float CurSpeed { get; }
    float ShootTime { get; }
    int Keys { get; }
    int Potions { get; }
    int NumItems { get; }
    float Melee { get; }
    float Strength { get; }
    float Magic { get; }
}

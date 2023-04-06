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
    public GameObject PlayerPrefab { get; }
    public ClassData CurClass { get; }
    public ClassEnum ClassType { get; }
    public float MaxHealth { get; }
    public float CurHealth { get; }
    public float BaseSpeed { get; }
    public float CurSpeed { get; }
    public int Keys { get; }
    public int Potions { get; }
    public int NumItems { get; }
    public float Melee { get; }
    public float Strength { get; }
    public float Magic { get; }
}

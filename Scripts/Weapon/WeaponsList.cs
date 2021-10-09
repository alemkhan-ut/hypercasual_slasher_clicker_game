using System.Collections.Generic;
using System;

[Serializable]
public class WeaponsList
{
    public List<Weapons> Weapons = new List<Weapons>();
}

[Serializable]
public class Weapons
{
    public string Sprite;
    public int ID;
    public string Name;
    public int Price;
    public int DesiredLevel;
    public WeaponButton.WeaponState State;
    public WeaponButton.WeaponStatus Status;

}

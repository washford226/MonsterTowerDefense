using UnityEngine;

public class TroopView : MonoBehaviour
{
    public MeleeTroop Data { get; private set; }

    public void Init(MeleeTroop troop)
    {
        Data = troop;
    }
}
using UnityEngine;

public class TroopView : MonoBehaviour
{
    public MeleeTroop Data { get; private set; }

    public Vector3 Position => transform.position;

    public void Init(MeleeTroop troop)
    {
        Data = troop;
    }
}
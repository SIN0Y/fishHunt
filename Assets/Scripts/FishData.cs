// FishData.cs
using UnityEngine;

public enum FishRarity
{
    Common,
    Uncommon,
    Rare
}

public enum FishDifficulty
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "New Fish", menuName = "Fishing/Fish Data")]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite icon;
    public GameObject fishPrefab;

    public FishRarity rarity;
    public FishDifficulty difficulty;

    public int fishScore;
}
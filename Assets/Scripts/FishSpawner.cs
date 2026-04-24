// FishSpawner.cs
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    GameObject currentFish;

    public void SpawnFish(FishData fishData)
    {
        ClearFish();

        if (fishData.fishPrefab != null)
        {
            currentFish = Instantiate(
                fishData.fishPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            currentFish.transform.SetParent(spawnPoint);
            currentFish.transform.localPosition = Vector3.zero;
            currentFish.transform.localRotation = Quaternion.identity;
        }
    }

    public void ClearFish()
    {
        if (currentFish != null)
        {
            Destroy(currentFish);
        }
    }
}
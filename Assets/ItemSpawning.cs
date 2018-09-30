using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawning : MonoBehaviour
{
    private const float spawnTime = 40.0f; // jede 40 Sekunden ein neues Item
    private float timerItemSpawn = spawnTime; // Timer für spawning eines Items
	private float timerFirstItem = 10.0f;
    private const float innerCubeBoundary = 1.5f;
    private bool firstItemSpawned = false;
    public Transform prefab_Item;

	public void ItemEntry()
    {
        timerItemSpawn -= Time.deltaTime;
        timerFirstItem -= Time.deltaTime;

        if (timerItemSpawn <= 0.0f) // wenn Zeit Sekunden abgelaufen, dann ...
        {
            timerItemSpawn = spawnTime;
            spawnItem(); // ... spawne ein Item	
        }

        if (timerFirstItem <= 0.0f)
        {
            if (!firstItemSpawned)
            {
                spawnItem();
                firstItemSpawned = true;
            }
        }

    }

    /// <summary>
    /// Lässt neues Item innerhalb desfinierter Grenzen auf dem Spielfeld erscheinen
    /// </summary>
    void spawnItem()
    {
        // Gültiger Bereich X/Y/Z: -30 bis 30
        // Random.Range inkludiert auch obere Grenze https://docs.unity3d.com/ScriptReference/Random.Range.html
        Vector3 pos = new Vector3(  Random.Range(-innerCubeBoundary, innerCubeBoundary),
                                    Random.Range(-innerCubeBoundary, innerCubeBoundary),
                                    Random.Range(-innerCubeBoundary, innerCubeBoundary));
        //pos = new Vector3(0.0f, 0.0f, 0.0f); // zu Tetszwecken
        Instantiate(prefab_Item, pos, Quaternion.identity);
    }
}

// ProceduralIslandGenerator.cs
using UnityEngine;
using System.Collections.Generic;

// This new helper class is the magic that solves the 1-to-1 problem!
// Now you can group many prefabs under a single set of rules.
[System.Serializable]
public class SpawnableObjectGroup
{
    public string groupName;
    [Tooltip("The set of rules that applies to all prefabs in this group.")]
    public PlacementRule rule;
    [Tooltip("The list of prefabs that will be placed using the above rule.")]
    public List<GameObject> prefabs;
    [Tooltip("How many times the generator should try to place objects from this group.")]
    public int placementAttempts;
    [Tooltip("What physics layer these objects are on. Used for fast density checks.")]
    public LayerMask objectLayer;
}

public class ProceduralIslandGenerator : MonoBehaviour
{
    [Header("Terrain Settings")]
    [Tooltip("The terrain you want to populate.")]
    public Terrain targetTerrain;
    [Tooltip("The global water level height.")]
    public float waterHeight = 50f;

    [Header("Object Placement Settings")]
    [Tooltip("Define all the object groups you want to spawn here.")]
    public List<SpawnableObjectGroup> objectGroups;

    private List<GameObject> generatedObjects = new List<GameObject>();

    // This is now the main entry point called by the editor button.
    public void GenerateIsland()
    {
        if (targetTerrain == null) { Debug.LogError("Target Terrain is not set."); return; }
        if (objectGroups == null || objectGroups.Count == 0) { Debug.LogError("Object Groups list is empty."); return; }

        ClearGeneratedObjects();

        TerrainData terrainData = targetTerrain.terrainData;

        // Loop through each group (e.g., "Trees", "Rocks")
        foreach (var group in objectGroups)
        {
            if (group.prefabs == null || group.prefabs.Count == 0) continue;

            for (int i = 0; i < group.placementAttempts; i++)
            {
                // Pick a random prefab FROM THE CURRENT GROUP
                GameObject prefabToPlace = group.prefabs[Random.Range(0, group.prefabs.Count)];

                // Get a random position on the terrain
                float randomX = Random.Range(0, terrainData.size.x);
                float randomZ = Random.Range(0, terrainData.size.z);
                Vector3 worldPosition = new Vector3(randomX, 0, randomZ) + targetTerrain.transform.position;
                worldPosition.y = targetTerrain.SampleHeight(worldPosition);

                // Check if this position is valid according to THIS GROUP'S RULE
                if (IsPlacementValid(worldPosition, group.rule, group.objectLayer, terrainData))
                {
                    GameObject newObject = Instantiate(prefabToPlace, worldPosition, Quaternion.identity, this.transform);
                    generatedObjects.Add(newObject);
                }
            }
        }
        Debug.Log($"Generation complete. Placed {generatedObjects.Count} objects.");
    }

    // A simpler, safer way to clear previously generated objects.
    public void ClearGeneratedObjects()
    {
        foreach (GameObject obj in generatedObjects)
        {
            // This works both in the Editor and during Play mode.
            DestroyImmediate(obj);
        }
        generatedObjects.Clear();
    }

    private bool IsPlacementValid(Vector3 position, PlacementRule rule, LayerMask layer, TerrainData terrainData)
    {
        // 1. Altitude Check
        if (position.y < rule.minAltitude || position.y > rule.maxAltitude) return false;

        // 2. Water Distance Check
        if (position.y < waterHeight + rule.minWaterDistance) return false;

        // 3. Slope Check
        Vector3 terrainLocalPos = position - targetTerrain.transform.position;
        Vector2 normalizedPos = new Vector2(terrainLocalPos.x / terrainData.size.x, terrainLocalPos.z / terrainData.size.z);
        float slope = terrainData.GetSteepness(normalizedPos.x, normalizedPos.y);
        if (slope > rule.maxSlope) return false;

        // 4. Density Check - THE EFFICIENT WAY!
        // This uses Unity's physics engine to check for nearby objects in a fraction of a millisecond,
        // instead of looping through a massive list. THIS IS THE PERFORMANCE FIX.
        if (Physics.CheckSphere(position, rule.minDistanceBetweenObjects, layer))
        {
            return false;
        }

        return true;
    }
}
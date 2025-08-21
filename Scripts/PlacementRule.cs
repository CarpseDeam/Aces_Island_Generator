// PlacementRule.cs
[System.Serializable]
public class PlacementRule
{
    [Tooltip("The name of this rule set (e.g., 'Tree Rules', 'Rock Rules').")]
    public string ruleName;

    [Header("Placement Criteria")]
    [Tooltip("The lowest altitude this object can spawn at.")]
    public float minAltitude = 0f;
    [Tooltip("The highest altitude this object can spawn at.")]
    public float maxAltitude = 500f;
    [Tooltip("The steepest slope (in degrees) this object can spawn on.")]
    public float maxSlope = 30f;
    [Tooltip("How far away from the water level this object must be.")]
    public float minWaterDistance = 2f;
    [Tooltip("The minimum distance between objects of this type.")]
    public float minDistanceBetweenObjects = 5f;
}
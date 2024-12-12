using UnityEngine;

public static class LineOfSight
{
    /// <summary>
    /// Detect mask will return False if there is Any object between start and end point with mask layer specified
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static bool LoSDetectMask(Vector3 start, Vector3 end, LayerMask mask)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, mask);
    }
}
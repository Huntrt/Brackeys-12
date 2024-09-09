using UnityEngine;

public static class Misc
{
	public static float Percent(float value, float percent)
	{
		return value * (percent/100);
	}

	public static bool CompareLayer(GameObject target, LayerMask layerToCompare)
	{
		return layerToCompare == (layerToCompare | (1 << target.layer));
	}
}

public static class VectorExtensions
{
    public static Vector2 With(this Vector2 point, float? x = null, float? y = null)
    {
        // transform.position = transform.position.Modify(y: 0)
        return new Vector2(x == null ? point.x : x.Value, y == null ? point.y : y.Value);
    }

    public static Vector3 With(this Vector3 point, float? x = null, float? y = null, float? z = null)
    {
        // transform.position = transform.position.Modify(z: 0)
        return new Vector3(x == null ? point.x : x.Value, y == null ? point.y : y.Value, z == null ? point.z : z.Value);
    }

    public static Vector4 With(this Vector4 point, float? x = null, float? y = null, float? z = null, float? w = null)
    {
        // transform.position = transform.position.Modify(w: 0)
        return new Vector4(x == null ? point.x : x.Value, 
            y == null ? point.y : y.Value,
            z == null ? point.z : z.Value, 
            w == null ? point.w : w.Value);
    }
}
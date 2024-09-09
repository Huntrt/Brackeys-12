using UnityEngine;

public class General : MonoBehaviour
{
	#region Set this class to singleton
	static General _i; public static General i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<General>();
			}
			return _i;
		}
	}
	#endregion
    
	public Camera cam;
	public Vector2 mousePos;
	public LayerMask nodeLayer;

	public Vector2 MousePos()
	{
		return cam.ScreenToWorldPoint(Input.mousePosition);
	}
}

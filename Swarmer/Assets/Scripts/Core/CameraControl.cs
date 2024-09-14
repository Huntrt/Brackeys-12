using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region Set this class to singleton
	static CameraControl _i; public static CameraControl i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<CameraControl>();
			}
			return _i;
		}
	}
	#endregion

	public KeyCode up, down, left, right, repositionCam, resetZoom;
	public float maxZoom, minZoom, curZoom, defaultZoom, zoomSpeed;
	public float moveSpeed;
	Vector2 moveDir;
	[SerializeField] Camera cam;
	[SerializeField] TMPro.TextMeshProUGUI zoomInfoTxt;

	void Start()
	{
		curZoom = defaultZoom;
	}

	void Update()
	{
		//Moving
		moveDir = Vector2.zero;
		if(Input.GetKey(up)) {moveDir += Vector2.up;}
		if(Input.GetKey(down)) {moveDir += Vector2.down;}
		if(Input.GetKey(left)) {moveDir += Vector2.left;}
		if(Input.GetKey(right)) {moveDir += Vector2.right;}
		if(Input.GetKeyDown(repositionCam)) {RespostionCamera();}
		//Zoom
		if(Player.i.buildPanel.activeInHierarchy) return;
		curZoom += (-Input.mouseScrollDelta.y) * zoomSpeed;
		curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);
		if(Input.GetKeyDown(resetZoom)) {ResetZoom();}
		cam.orthographicSize = curZoom;
		zoomInfoTxt.text = "Zoom: " + curZoom;
	}

	public void RespostionCamera() {cam.transform.position = Vector2.zero;}
	public void ResetZoom() {curZoom = defaultZoom;}

	void FixedUpdate()
	{
		//Move camera with speed toward the direction
		cam.transform.position = (Vector2)cam.transform.position + (Vector2)(moveDir * (moveSpeed * Time.deltaTime));
		cam.transform.position = new Vector2
		(
			Mathf.Clamp(cam.transform.position.x, Map.i.minMapSize.x, Map.i.maxMapSize.x),
			Mathf.Clamp(cam.transform.position.y, Map.i.minMapSize.y, Map.i.maxMapSize.y)
		);
		//Make sure camera Z axis always below
		cam.transform.position = cam.transform.position.With(z: -10);
	}
}

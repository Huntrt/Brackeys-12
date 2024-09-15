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
		//Disable camera control when build
		if(Player.i.buildPanel.activeInHierarchy) return;
		//Moving
		moveDir = Vector2.zero;
		if(Input.GetKey(SessionOperator.i.config.CamUp)) {moveDir += Vector2.up;}
		if(Input.GetKey(SessionOperator.i.config.CamDown)) {moveDir += Vector2.down;}
		if(Input.GetKey(SessionOperator.i.config.CamLeft)) {moveDir += Vector2.left;}
		if(Input.GetKey(SessionOperator.i.config.CamRight)) {moveDir += Vector2.right;}
		if(Input.GetKeyDown(SessionOperator.i.config.CamToBase)) {RespostionCamera();}
		//Zoom
		if(!Application.isFocused) return;
		curZoom += (-Input.mouseScrollDelta.y) * zoomSpeed;
		curZoom = Mathf.Clamp(curZoom, minZoom, maxZoom);
		if(Input.GetKeyDown(SessionOperator.i.config.ResetZoom)) {ResetZoom();}
		cam.orthographicSize = curZoom;
		zoomInfoTxt.text = "Zoom: " + curZoom;
	}

	public void RespostionCamera() {cam.transform.position = Vector2.zero;}
	public void ResetZoom() {curZoom = defaultZoom;}

	void FixedUpdate()
	{
		//Move camera with speed toward the direction
		cam.transform.position = (Vector2)cam.transform.position + (Vector2)(moveDir * (moveSpeed * Time.unscaledDeltaTime));
		cam.transform.position = new Vector2
		(
			Mathf.Clamp(cam.transform.position.x, Map.i.minMapSize.x, Map.i.maxMapSize.x),
			Mathf.Clamp(cam.transform.position.y, Map.i.minMapSize.y, Map.i.maxMapSize.y)
		);
		//Make sure camera Z axis always below
		cam.transform.position = cam.transform.position.With(z: -10);
	}
}

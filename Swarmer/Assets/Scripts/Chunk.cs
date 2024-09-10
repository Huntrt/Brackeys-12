using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] Vector2Int selectedChunk;
	[SerializeField] List<Vector2Int> generatedChunks;

	void Start()
	{
		GeneratingMap(Vector2Int.zero);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow)) GeneratingMap(selectedChunk + Vector2Int.up);
		if(Input.GetKeyDown(KeyCode.DownArrow)) GeneratingMap(selectedChunk + Vector2Int.down);
		if(Input.GetKeyDown(KeyCode.LeftArrow)) GeneratingMap(selectedChunk + Vector2Int.left);
		if(Input.GetKeyDown(KeyCode.RightArrow)) GeneratingMap(selectedChunk + Vector2Int.right);
	}

	void GeneratingMap(Vector2Int targetChunk)
	{
		//Stop if map of chunk already exist
		if(generatedChunks.Contains(targetChunk))
		{
			Debug.LogWarning("Chunk at " + targetChunk + " already exist");
			return;
		}
		Map.i.CreateMap(targetChunk);
		generatedChunks.Add(targetChunk);
	}
}

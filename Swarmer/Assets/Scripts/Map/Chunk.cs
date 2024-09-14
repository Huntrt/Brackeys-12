using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Chunk : MonoBehaviour
{
	[Serializable] public class ChunkData
	{
		public Vector2Int coord;
		public bool[] neighbors = new bool[4];

		public ChunkData(Vector2Int coord) {this.coord = coord;}

		public List<int> checkAvailableNeighbor()
		{
			List<int> available = new List<int>();
			for (int n = 0; n < 4; n++)
			{
				if(!neighbors[n]) available.Add(n);
			}
			return available;
		}
	}
	[SerializeField] List<ChunkData> generatedChunks;
	[SerializeField] int initalChunk;
	[SerializeField] int chunkEveryLv; [SerializeField] float chunkAppearChance;
	[SerializeField] bool debug;

	void Start()
	{
		CreateChunk(new ChunkData(Vector2Int.zero));
		//Generate multiple chunk on level up
		for (int i = 0; i < initalChunk; i++) {GeneratingChunk();}
	}

	void OnEnable()
	{
		GameLoop.onLevelComplete += ChunkLevelUp;
	}

	void ChunkLevelUp(int lv)
	{
		//When reached the level count to create new chunk
		if(GameLoop.i.level % chunkEveryLv != 0) return;
		//When take the chance to generate new chunk
		if(UnityEngine.Random.Range(0,100) > chunkAppearChance) return;
		GeneratingChunk();
	}

	void GeneratingChunk()
	{
		//Find all the neighbor still have chunk
		List<ChunkData> neighborlessChunk = new List<ChunkData>();
		foreach (ChunkData chunk in generatedChunks)
		{
			if(chunk.checkAvailableNeighbor().ToArray().Length > 0) 
			{
				neighborlessChunk.Add(chunk);
			}
		}
		//Select an random chunk that still have unfill neighbor
		ChunkData selectChunk = neighborlessChunk[UnityEngine.Random.Range(0, neighborlessChunk.Count)];
		int[] availableNeighbor = selectChunk.checkAvailableNeighbor().ToArray();
		//Select an random neighbor to create an chunk there
		int selectNeighbor = availableNeighbor[UnityEngine.Random.Range(0, availableNeighbor.Length)];
		ChunkData neighborChunk = null;
		if(selectNeighbor == 0) {neighborChunk = new ChunkData(selectChunk.coord + Vector2Int.up);}
		if(selectNeighbor == 1) {neighborChunk = new ChunkData(selectChunk.coord + Vector2Int.down);}
		if(selectNeighbor == 2) {neighborChunk = new ChunkData(selectChunk.coord + Vector2Int.left);}
		if(selectNeighbor == 3) {neighborChunk = new ChunkData(selectChunk.coord + Vector2Int.right);}
		//Generate the chunk
		ChunkData generated = CreateChunk(neighborChunk);
		//Upon genrate that chunk will check all direction to apply neighbor status to those chunk and itself
		ChunkData checking = null;
		for (int d = 0; d < 4; d++)
		{
			if(d == 0)
			{
				checking = FindChunk(generated.coord + Vector2Int.up);
				if(checking != null) {generated.neighbors[0] = true; checking.neighbors[1] = true;}
			}
			if(d == 1)
			{
				checking = FindChunk(generated.coord + Vector2Int.down);
				if(checking != null) {generated.neighbors[1] = true; checking.neighbors[0] = true;}
			}
			if(d == 2)
			{
				checking = FindChunk(generated.coord + Vector2Int.left);
				if(checking != null) {generated.neighbors[2] = true; checking.neighbors[3] = true;}
			}
			if(d == 3)
			{
				checking = FindChunk(generated.coord + Vector2Int.right);
				if(checking != null) {generated.neighbors[3] = true; checking.neighbors[2] = true;}
			}
		}
	}

	ChunkData CreateChunk(ChunkData targetChunk)
	{
		//Stop if map of chunk already exist
		for (int c = 0; c < generatedChunks.Count; c++) if(targetChunk.coord == generatedChunks[c].coord)
		{
			Debug.LogWarning("Chunk at " + targetChunk.coord + " already exist");
			return null;
		}
		Map.i.CreateMap(targetChunk.coord);
		generatedChunks.Add(targetChunk);
		return targetChunk;
	}

	ChunkData FindChunk(Vector2Int coord)
	{
		for (int g = 0; g < generatedChunks.Count; g++)
		{
			if(generatedChunks[g].coord == coord) return generatedChunks[g];
		}
		return null;
	}

	void OnDisable()
	{
		GameLoop.onLevelComplete -= ChunkLevelUp;
	}

	void OnDrawGizmos()
	{
		if(!debug) return;
		foreach (ChunkData chunk in generatedChunks)
		{
			Handles.Label((Vector2)chunk.coord * (Map.i.MapSize+1), chunk.coord + "\n" + 
			chunk.neighbors[0] + ", " +
			chunk.neighbors[1] + ", " +
			chunk.neighbors[2] + ", " +
			chunk.neighbors[3] + ", ");
		}
	}
}

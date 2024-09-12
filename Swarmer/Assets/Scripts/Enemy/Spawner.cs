using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject bulldozerPref;
	public GameObject spawnerPrf;
	public GameObject testEnemy; //temp: enemy test
    public int spawnerCount;
	public float spawnerEveryLv;
	public float spawnRate; float spawnRateTimer;
	public List<Node> spawnerBuildeds = new List<Node>();

	void OnEnable()
	{
		GameLoop.onLevelBegin += BeginSpawner;
		GameLoop.onLevelComplete += StopSpawner;
	}

	void Update()
	{
		if(GameLoop.i.raidPhase)
		{
			Spawning();
		}
	}

	void Spawning()
	{
		spawnRateTimer += Time.deltaTime;
		if(spawnRateTimer >= spawnRate)
		{
			foreach (Node spawer in spawnerBuildeds)
			{
				Instantiate(testEnemy, spawer.pos, Quaternion.identity);
			}
			spawnRateTimer -= spawnRateTimer;
		}
	}

	void BeginSpawner(int lv)
	{
		//Scale spawner with level
		spawnerCount = (int)(lv/spawnerEveryLv);
		spawnerCount += 1;
		/// Build spawner on random empty node
		List<Node> vacantNodes = Map.i.GetVacants();
		for (int i = 0; i < spawnerCount; i++)
		{
			Node spawnerNode = vacantNodes[UnityEngine.Random.Range(0, vacantNodes.Count)];
			Instantiate(bulldozerPref, spawnerNode.pos, bulldozerPref.transform.rotation);
			Builder.BuildAtNode(spawnerNode, spawnerPrf);
			spawnerBuildeds.Add(spawnerNode);
		}
	}

	void StopSpawner(int lv)
	{
		foreach (Node spawner in spawnerBuildeds)
		{
			Builder.DemolishAtNode(spawner, 1);
		}
		spawnerBuildeds.Clear();
	}

	void OnDisable()
	{
		GameLoop.onLevelBegin -= BeginSpawner;
		GameLoop.onLevelComplete -= StopSpawner;
	}
}

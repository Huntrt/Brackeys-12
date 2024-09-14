using System.Collections.Generic;
using UnityEngine;

public class EnemyScaling : MonoBehaviour
{
	#region Set this class to singleton
	static EnemyScaling _i; public static EnemyScaling i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<EnemyScaling>();
			}
			return _i;
		}
	}
	#endregion

	[SerializeField] Spawner spawner;
	public float spawnCountEveryLv, spawnCountIncrease;
	public float spawnRateEveryLv, spawnRateIncrease, spawnRateIncreaseChance;
	[System.Serializable] public class Enhancement
	{
		public float speed, health, loot;
		public float chance;

		public Enhancement(float speed, float health, float loot, float chance)
		{
			this.speed = speed;
			this.health = health;
			this.loot = loot;
			this.chance = chance;
		}
	}
	public Enhancement minEnhancement, maxEnhancement;
	[SerializeField] List<Enhancement> enhancements = new List<Enhancement>();
	[SerializeField] int enhancmentsEveryLv;
	
	void OnEnable()
	{
		GameLoop.onLevelBegin += Scaling;
	}

	void OnDisable()
	{
		GameLoop.onLevelBegin -= Scaling;
	}

	void Scaling(int lv)
	{
		///For every X level will increase spawn count by 1
		if(lv % spawnCountEveryLv == 0)
		{
			spawner.spawnerCount += Mathf.RoundToInt(spawnCountIncrease);
		}
		///For every X level will have an chance to increase spawn rate
		if(lv % spawnRateEveryLv == 0)
		{
			if(Random.Range(0,100) < spawnRateIncreaseChance)
			{
				spawner.spawnRate += Mathf.RoundToInt(spawnRateIncrease);
			}
		}///For every X level will have an chance to create an new enchancement for enemy
		if(lv % enhancmentsEveryLv == 0)
		{
			float eSpeed = Random.Range(minEnhancement.speed, maxEnhancement.speed);
			float eHealth = Random.Range(minEnhancement.health, maxEnhancement.health);
			float eLoot = Random.Range(minEnhancement.loot, maxEnhancement.loot);
			float eChance = Random.Range(minEnhancement.chance, maxEnhancement.chance);

			enhancements.Add(new Enhancement(eSpeed, eHealth, eLoot, eChance));
		}
	}

	public Enhancement PickEnhancement()
	{
		//Go through each enhancement and add up the total
		Enhancement picked = new Enhancement(0,0,0,0);
		foreach (Enhancement enhancement in enhancements)
		{
			if(Random.Range(0f,100f) < enhancement.chance)
			{
				picked.speed += enhancement.speed;
				picked.health += enhancement.health;
			}
		}
		return picked;
	}
}
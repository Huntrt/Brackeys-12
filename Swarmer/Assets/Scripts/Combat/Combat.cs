using UnityEngine;

public class Combat : MonoBehaviour
{
    [System.Serializable] public class Stats
	{
		public delegate void OnStatsChange(string stats, float modifier); public OnStatsChange onStatsChange;
		[SerializeField] float firerate; public float FireRate {get => firerate; set {firerate = value; onStatsChange?.Invoke("firerate", value);}} //FRT
		[SerializeField] float range; public float Range {get => range; set {range = value; onStatsChange?.Invoke("range", value);}}//RNG 
		[SerializeField] float damage; public float Damage {get => damage; set {damage = value; onStatsChange?.Invoke("damage", value);}}//DMG

		public void SetStats(Stats statsGiven)
		{
			FireRate = statsGiven.firerate;
			range = statsGiven.range;
			damage = statsGiven.damage;
		}
	}
}

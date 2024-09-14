using UnityEngine;

public class PathBulldozer : MonoBehaviour
{
	[SerializeField] Node resideNode;
	[SerializeField] float stepRate; float stepRateTimer;

	void Start()
	{
		resideNode = Map.i.FindNode(Map.WorldToCoordinates(transform.position));
	}

	void Update()
	{
		stepRateTimer += Time.deltaTime;
		if(stepRateTimer >= stepRate)
		{
			//Go to the next node supposed to flow
			transform.position = resideNode.pos;
			//If node the bulldozer on have occupation
			if(resideNode.HaveOccupation(1))
			{
				//Demolish if the node resign is wall
				if(resideNode.occupations[1].component.HaveCatalog(Structure.Category.wall))
				{
					print("Bulldozed the [" + resideNode.occupations[1].obj.name +"] at " + resideNode.coord);
					BuilderManager.DemolishAtNode(resideNode, 1);
				}
				//Destroy self if have reached heart
				if(resideNode.occupations[1].obj == Player.i.heart.obj)
				{
					Destroy(gameObject);
				}
			}
			resideNode = Map.i.FindNode(resideNode.flows.nextNode);
			stepRateTimer -= stepRateTimer;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player{
	
	Dictionary<string,int> ResourceNum;

	public Player(){
		ResourceNum = new Dictionary<string,int> ();
		foreach (KeyValuePair<int,Resource_Info> info in CommonData.Instance._resList) {
			ResourceNum.Add (info.Value._name, 0);
		}
	}

	public PlayerController InstantiateHero(Vector3 position)
	{
		GameObject hero = Camera.Instantiate (PrefabManager._instance._hero);

		InitHeroObject (hero, position);

		return hero.GetComponent<PlayerController> ();
	}

	public void SetResourceNum(string key,int num)
	{
		ResourceNum.Add (key, num);
	}

	public Dictionary<string,int> GetResourceNum()
	{
		return new Dictionary<string ,int> (ResourceNum);
	}

	void InitHeroObject(GameObject hero, Vector3 position)
	{
		hero.transform.position = position;
	}
}
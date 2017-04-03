using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour {

	public CommonData.ResourceType info;

	const float _maxCollectionValue = 100;//the value you need to collect some resource
	float _collectionValue;//The value the resource has been collected

	[HideInInspector]
	public int _resourceNum;

	// Use this for initialization
	void Start () {
		_collectionValue = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// This resource is got.
	/// </summary>
	void Get(PlayerController picker){
		if (info == CommonData.ResourceType.WaSi) {
			_collectionValue = 0;
		} else {
			picker._isTouchingResource = false;
			picker._nowState = PlayerController.state.stay;
			Destroy (gameObject);
		}
		picker.GetResource (info, _resourceNum);
	}
		
	/// <summary>
	/// Collect this instance.
	/// </summary>
	/// <returns><c>true</c>,this resource has been collected</returns>
	public bool Collect(PlayerController picker, float collectionSpeed)
	{
		_collectionValue += collectionSpeed;
		Debug.Log (_collectionValue);
		if (_collectionValue >= _maxCollectionValue) {
			Debug.Log ("Collection finish");
			Get (picker);
			return true;
		}
		return false;
	}
}

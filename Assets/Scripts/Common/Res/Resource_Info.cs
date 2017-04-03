using UnityEngine;
using System.Collections;

public class Resource_Info
{

	public CommonData.ResourceType _type { get; private set; }
    public string _name { get; private set; }
    public string _description { get; private set; }

	public Resource_Info(string name, string description, CommonData.ResourceType type)
    {
		_type = type;
        _name = name;
        _description = description;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommonData
{
	public enum ResourceType { JinShuCanHai, WaSi, ShiYou, KuangShi, LieYanShi, MingJieHua, HuXiaoShi };

	public Dictionary<int, Resource_Info> _resList= new Dictionary<int, Resource_Info>();
	public Vector3 _normalRepelForce{ get; private set; }

	private static CommonData _instance;
	public static CommonData Instance
	{ get
		{
			if (_instance == null) {
				_instance = new CommonData ();
				_instance.Init ();
			}
			return _instance;
		}
	}

	void Init(){
		_normalRepelForce = new Vector3 (200, 250, 0);
		LoadResourceInfo ();
	}

	/// <summary>
	/// 加载所有资源信息
	/// </summary>
	void LoadResourceInfo()
	{
		Resource_Info jinshucanhai = new Resource_Info("金属残骸", "no", CommonData.ResourceType.JinShuCanHai);
		Resource_Info shiyou = new Resource_Info("石油", "no", ResourceType.ShiYou);
		Resource_Info wasi = new Resource_Info("瓦斯", "no", ResourceType.WaSi);
		Resource_Info kuangshi = new Resource_Info("矿石", "no", ResourceType.KuangShi);
		Resource_Info lieyanshi = new Resource_Info("烈焰石", "no", ResourceType.LieYanShi);
		Resource_Info mingjiehua = new Resource_Info("冥界花", "no", ResourceType.MingJieHua);
		Resource_Info huxiaoshi = new Resource_Info("呼啸石", "no", ResourceType.HuXiaoShi);

		_resList.Add(0, jinshucanhai);
		_resList.Add(1, shiyou);
		_resList.Add(2, wasi);
		_resList.Add(3, kuangshi);
		_resList.Add(4, lieyanshi);
		_resList.Add(5, mingjiehua);
		_resList.Add(6, huxiaoshi);
	}
}
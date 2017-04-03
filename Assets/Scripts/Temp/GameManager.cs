using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public GameObject _itemPrefab;
    public GameObject _buffPrefab;
    public Transform _itemPos;
    public Transform _buffPos;

	public Player _player;

    public List<Buff.BuffType> buffList;

    void Awake()
    {
        _instance = this;
        buffList = new List<Buff.BuffType>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GetBuff(Buff.BuffType.slow, Random.Range(1, 3));
        }


    }

    /// <summary>
    /// 获得物品
    /// </summary>
    /// <param name="id">物品id</param>
    /// <param name="num">物品数量</param>
    public void GetItem(int id, int num)
    {
		string content = CommonData.Instance._resList[id]._name + "+" + num.ToString();

        GameObject go = Instantiate(_itemPrefab, _itemPos.position, Quaternion.identity) as GameObject;
        go.GetComponent<GetItemText>().UpdateText(content);
    }

    public int CheckBuff(Buff.BuffType buffType)
    {
        int length = buffList.Count;
        for(int i = 0;i < length;i++)
        {
            if(buffList[i] == buffType)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 获得buff
    /// </summary>
    /// <param name="buffType"></param>
    /// <param name="existTime"></param>
    public void GetBuff(Buff.BuffType buffType, float existTime)
    {        
        int index = CheckBuff(buffType);
        if (index == -1)
        {
            GameObject go = Instantiate(_buffPrefab, _buffPos.position, Quaternion.identity) as GameObject;
            go.GetComponent<Buff>().UpdateInformation(buffType, existTime);
            buffList.Add(buffType);
        }
        else
        {
            GameObject[] buffs = GameObject.FindGameObjectsWithTag(Tags.buffIcon);
            for(int i = 0; i < buffs.Length; i++)
            {
                if(buffs[i].GetComponent<Buff>()._buffType == buffType)
                {
                    buffs[i].GetComponent<AutoReduce>().DestroyBuffIcon(false);
                    buffList.RemoveAt(index);
                }
            }
            GameObject go = Instantiate(_buffPrefab, _buffPos.position, Quaternion.identity) as GameObject;
            go.GetComponent<Buff>().UpdateInformation(buffType, existTime);
            buffList.Add(buffType);
        }
    }
}
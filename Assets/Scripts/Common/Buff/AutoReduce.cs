using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoReduce : MonoBehaviour {

    public Slider _slider;
    private Buff _buff;

    private GameObject _player;

    void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _buff = GetComponent<Buff>();
        _player = GameObject.FindGameObjectWithTag(Tags.player);
    }

	void Update () {
        _slider.value -= (_slider.maxValue / _buff._existTime) * Time.deltaTime;
        if(_slider.value <= 0)
        {
            DestroyBuffIcon(true);
        }
	}

    public void DestroyBuffIcon(bool isAuto)
    {
        GameObject[] buffs = GameObject.FindGameObjectsWithTag(Tags.buffIcon);
        foreach(GameObject buff in buffs)
        {
            if(buff.transform.position.x > transform.position.x)
            {
                buff.transform.position -= new Vector3(45f, 0);
            }
        }

        switch(_buff._buffType)
        {
            case Buff.BuffType.spiderWeb:
                PlayerData.Instance._moveSpeed = PlayerData.Instance._moveSpeed_origin;
                break;
            case Buff.BuffType.spiderWebStop:
                PlayerData.Instance._moveSpeed = PlayerData.Instance._moveSpeed_origin;
                break;
            case Buff.BuffType.slow:
                PlayerData.Instance._moveSpeed *= 1f / 0.6f;
                PlayerData.Instance._jumpForce *= 1f / 0.6f;
                break;
            case Buff.BuffType.doubleJumpCD:
                _player.GetComponent<DoubleJump>()._isDoubleJumping = false;
                break;
            default:
                break;
        }

        if(isAuto)
            GameManager.Instance.buffList.RemoveAt(GameManager.Instance.CheckBuff(_buff._buffType));

        Destroy(gameObject);
    }
}

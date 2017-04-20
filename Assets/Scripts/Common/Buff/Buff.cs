using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Buff : MonoBehaviour {

    public enum BuffType
    {
        none, slow, poison,
        doubleJumpCD,
        spiderWeb, spiderWebStop
    }

    public BuffType _buffType { get; private set; }
    public float _existTime { get; private set; }

    public Image _backImage;
    public Image _forntImage;

    void Awake()
    {
        _buffType = BuffType.none;
        _existTime = 100f;

        transform.SetParent(GameObject.FindGameObjectWithTag(Tags.ui).transform);
        GameObject[] buffs = GameObject.FindGameObjectsWithTag(Tags.buffIcon);
        transform.position = new Vector3(transform.position.x + GameManager.Instance.buffList.Count * 45f, transform.position.y, transform.position.z);
    }

    public void UpdateInformation(BuffType buffType, float existTime)
    {
        _buffType = buffType;
        _existTime = existTime;

        switch(_buffType)
        {
            case BuffType.spiderWeb:
                PlayerData.Instance._moveSpeed -= PlayerData.Instance._moveSpeed_origin * 0.5f;
                break;
            case BuffType.spiderWebStop:
                PlayerData.Instance._moveSpeed = 0;
                break;
            case BuffType.slow:
                PlayerData.Instance._moveSpeed *= 0.6f;
                PlayerData.Instance._jumpForce *= 0.6f;
                break;
            case BuffType.doubleJumpCD:
                _forntImage.overrideSprite = Resources.Load("Pictures/icon_doublejump", typeof(Sprite)) as Sprite;
                _backImage.overrideSprite = Resources.Load("Pictures/icon_doublejump", typeof(Sprite)) as Sprite;
                break;
            default:
                break;
        }
    }

	void Update () {
	
	}
}

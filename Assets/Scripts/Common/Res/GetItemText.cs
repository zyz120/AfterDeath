using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetItemText : MonoBehaviour {

    private  Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
        Invoke("DelayFadeOut", 2.0f);
        transform.SetParent(GameObject.FindGameObjectWithTag(Tags.ui).transform);
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tags.getItemText);        
        
        foreach(GameObject go in gameObjects)
        {
            go.GetComponent<GetItemText>().MoveDownText();
        }
    }

    /// <summary>
    /// 更新文本
    /// </summary>
    /// <param name="content"></param>
    public void UpdateText(string content)
    {
        _text.text = content;
    }

    /// <summary>
    /// 向下移动文本
    /// </summary>
    public void MoveDownText()
    {
        StartCoroutine(MoveDown());
    }

    /// <summary>
    /// 文本淡出
    /// </summary>
    void DelayFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        for(int i = 0;i < 20;i++)
        {
            _text.color = new Color(255, 255, 255, 1 - 0.1f * i);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);            
    }

    IEnumerator MoveDown()
    {
        for(int i = 0;i < 20; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z);
            yield return new WaitForSeconds(0.02f);
        }
    }

}

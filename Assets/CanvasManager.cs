using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Slider hpSlider;
    public Text hpText;

    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        hpText.text = player.GetComponent<PlayerController>()._health.ToString() + "/" + PlayerData.Instance._maxHealth;
    }

}

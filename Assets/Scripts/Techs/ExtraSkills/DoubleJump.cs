using UnityEngine;
using System.Collections;

public class DoubleJump : MonoBehaviour
{

    Rigidbody2D _rigidbody;
    PlayerController _player;
    public bool _isDoubleJumping;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerController>();
        _rigidbody = _player.GetComponent<Rigidbody2D>();
        _isDoubleJumping = false;
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_player._nowState == PlayerController.state.jump && _isDoubleJumping == false)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
                _rigidbody.AddForce(new Vector2(0, PlayerData.Instance._jumpForce * 0.8f));

                _isDoubleJumping = true;

                GameManager.Instance.GetBuff(Buff.BuffType.doubleJumpCD, 2f);
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Hero controller.
/// </summary>
/// author:Alex

public class PlayerController : MonoBehaviour {

	public enum state{ stay, move, jump, picking }

	/// <summary>
	/// The state of the hero
	/// </summary>
	[HideInInspector]
	public bool _isFacingLeft;//is the hero facing left
	public state _nowState = state.stay;//now state of hero
	public bool _controllable = false;//is the hero controllable
	[HideInInspector]
    public bool _isTouchingResource = false;//is the hero touching some resources
	bool _overHeated = false;//is the weapon heated
    public bool _invicible = false;

    /// <summary>
    /// about UI
    /// </summary>
    public Slider _hpSlider;
    public Slider _overHeatSlider;

    /// <summary>
    /// some data for calculation.
    /// </summary>
    Vector3 _scale;//The inital scale of the hero
	float _firingInterval;//The interval of firing
	float _shotCD;//the rest CD of shotting
    GunController _gun;//The controller of the gun
    ResourceController _nowTouching;//The resource the hero is touching

	/// <summary>
	/// The data of hero
	/// </summary>
	private PlayerData _playerData;
	public float _health{ get; private set; }//The health of hero

    void Awake()
    {
        _playerData = PlayerData.Instance;
		_health = _playerData._maxHealth;
        Debug.Log(_playerData);
    }

	// Use this for initialization
	void Start () {
		InitCalculateDate ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_nowState == state.picking) {
			PickResource ();
		}
		if (_shotCD > 0) {
			_shotCD -= Time.deltaTime;
		}
        ReduceHeat();
		ListenKeyboard ();
		ListenMouse ();
	}

    /// <summary>
    /// Inits the calculate date.
    /// </summary>
    void InitCalculateDate()
    {
        _hpSlider.maxValue = _playerData._maxHealth;
        _overHeatSlider.maxValue = _playerData._overHeatMax;
        _scale = transform.localScale;
        _firingInterval = 1 / _playerData._fireRate;
        _gun = GetComponentInChildren<GunController>();
    }

    /// <summary>
    /// Let the hero turn to a direction
    /// </summary>
    /// <param name="isLeft">If set to <c>true</c> is left.</param>
    public void TurnTo(bool isLeft)
	{
		if (_isFacingLeft == isLeft)
			return;
		else {
			transform.localScale = new Vector3 ((isLeft ? -1 : 1) * _scale.x, _scale.y, _scale.z);
			_isFacingLeft = isLeft;
		}
	}

	/// <summary>
	/// Try to jump.
	/// </summary>
	public bool TryJump()
	{
		if (_nowState == state.jump||_nowState == state.picking||(!_controllable)) {
			return false;
		}
		Jump();
		return true;
	}

	/// <summary>
	/// Try to shot.
	/// </summary>
	/// <returns><c>true</c>, if shot was tryed, <c>false</c> otherwise.</returns>
	public bool TryShot()
	{
		if (_nowState == state.picking ||(!_controllable)) {
			return false;
		}
		if (_shotCD <= 0 && _overHeated == false)
        {
            _overHeatSlider.value += 15f;
			Shot ();
            if(_overHeatSlider.value >= _overHeatSlider.maxValue)
            {
                _overHeated = true;
            }
			return true;
		}
		return false;
	}
		
	/// <summary>
	/// Gets the resource.
	/// </summary>
	/// <param name="resource">Resource.</param>
	public void GetResource(CommonData.ResourceType type,int number)
	{
		GameManager.Instance.GetItem ((int)type, number);
		Debug.Log ("Pick up a resource");
	}

	/// <summary>
	/// Hurt.
	/// </summary>
	/// <param name="damage">Amount of damage.</param>
	/// <param name="isToLeft">If set to <c>true</c> is towards left.</param>
	public void Hurt(int damage,Vector3 attackerPos,Vector3 repelForce, float invicibleTime)
	{
        if (_invicible == true)
            return;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D> ().AddForce (new Vector2 (((attackerPos.x - transform.position.x) > 0 ? -1 : 1) * repelForce.x * _playerData._repulseRate, repelForce.y * _playerData._repulseRate));

        _controllable = false;
        _invicible = true;
		_nowState = state.jump;

        Invoke("SetGetControlToTrue", 0.2f);

        StartCoroutine(Blink(invicibleTime * _playerData._invicibleTimeRate));

		HandleDamage (damage);
	}

	/// <summary>
	/// The hero fall to the ground
	/// </summary>
	public void FallGround()
	{
		_nowState = state.stay;
	}

	/// <summary>
	/// Raises the collision enter2d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag ==Tags.ground)
        {
			FallGround ();
		}
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D(Collider2D coll)
	{
		switch(coll.gameObject.tag)
		{
		case Tags.resource:
			Debug.Log ("Touch resource");
			TouchResource (coll.gameObject.GetComponent<ResourceController> ());
			break;
		}
	}

	/// <summary>
	/// Raises the collision exit2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerExit2D(Collider2D coll)
	{
		switch(coll.gameObject.tag)
		{
		case Tags.resource:
			ExitResource ();
			break;
		}
	}

	/// <summary>
	/// Touchs the resource.
	/// </summary>
	/// <param name="resource">the touching resource.</param>
	void TouchResource(ResourceController resource)
	{
		_isTouchingResource = true;
		_nowTouching = resource;
	}

	/// <summary>
	/// Starts to pick up resource.
	/// </summary>
	void StartPickResource()
	{
		if (_isTouchingResource) {
			_nowState = state.picking;
		}
	}

	/// <summary>
	/// Picking the resource.
	/// </summary>
	void PickResource()
	{
		_nowTouching.Collect (this, _playerData._collectionSpeed);
	}

	/// <summary>
	/// Stops picking resource.
	/// </summary>
	void StopPickResource()
	{
		_nowState = state.stay;
	}

	/// <summary>
	/// Exits the resource.
	/// </summary>
	void ExitResource()
	{
		if (_nowState == state.picking) {
			StopPickResource ();
		}
		_isTouchingResource = false;
		_nowTouching = null;
	}

	void JudgeState(){
	}

	/// <summary>
	/// The hero jumps
	/// </summary>
	void Jump()
	{
		GetComponent<Rigidbody2D> ().AddForce (new Vector3 (0, _playerData._jumpForce, 0));
		_nowState = state.jump;
	}

	/// <summary>
	/// Move undercontrol.
	/// </summary>
	/// <param name="isLeft">If set to <c>true</c> is left.</param>
	void Move(bool isLeft)
	{
		if (_nowState == state.picking||(!_controllable))
        {
            return;
        }
		transform.position += new Vector3 ((isLeft ? -1 : 1) * _playerData._moveSpeed, 0, 0);
	}

	/// <summary>
	/// Shot.
	/// </summary>
	void Shot()
	{
		_gun.Shot ();
		_shotCD = _firingInterval;
	}

	/// <summary>
	/// Handles the damage.
	/// </summary>
	/// <param name="damage">Amount of damage.</param>
	void HandleDamage(int damage)
	{
        _health -= damage * _playerData._defence;
        StartCoroutine(ReduceHp(_health));
        if(_health <= 0)
        {
            StartCoroutine(ReduceHp(0));

            Dead(1);
        }
	}

    public void Dead(int type)
    {
        // TODO 这段代码只是临时使用，不改会爆炸
        GameObject bg = null;
        if (type == 1)
            bg = GameObject.Find("BlackBg(TEMP)");
        else
            bg = GameObject.Find("BlackBg2(TEMP)");

        if (bg != null)            
            bg.GetComponent<GameOverTemp>().isFading = true;

        GameObject canvas = GameObject.Find("Canvas");
        if(canvas!=null)
            canvas.SetActive(false);
    }

    void ReduceHeat()
    {
        if (_overHeated)
        {
            _overHeatSlider.value -= 0.4f;
        }
        else
        {
            _overHeatSlider.value -= 0.8f;
        }
        
        if(_overHeatSlider.value <= 0)
        {
            _overHeated = false;
        }
    }

	/// <summary>
	/// Listen to keyboard and handle it
	/// </summary>
	void ListenKeyboard()
	{
        if (Input.GetKey(KeyCode.A))
        {
            Move(true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryJump();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
			if (_nowState != state.picking)
            {
                StartPickResource();
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
			if (_nowState == state.picking)
            {
                StopPickResource();
            }
        }
	}

	/// <summary>
	/// Listen to mouse and transmit it.
	/// </summary>
	void ListenMouse()
	{
		HandleMousePosition (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        // TODO 这个if招新完删掉
        if (Input.GetMouseButtonDown(0) && _health <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (Input.GetMouseButton (0))
        {
			TryShot ();
		}
	}

	/// <summary>
	/// Handles the mouse position.
	/// </summary>
	void HandleMousePosition(Vector3 mousePosition)
	{
		TurnTo (((mousePosition.x - transform.position.x) < 0) ? true : false);
		_gun.RotateToMouse (mousePosition);
	}

    void SetGetControlToTrue()
    {
		_controllable = true;
    }

    IEnumerator ReduceHp(float nowHp)
    {
        for(int i = 0;i < 10;i++)
        {
            _hpSlider.value -= (_hpSlider.value - nowHp) / (10 - i);
            yield return new WaitForSeconds(0.01f);
        }
        _hpSlider.value = nowHp;
    }

    IEnumerator Blink(float blinkTime)
    {
        int count = (int)(blinkTime / 0.1f);
        SpriteRenderer renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        for (int i = 0; i < count; i++)
        {
            renderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            renderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        _invicible = false;
    }
}
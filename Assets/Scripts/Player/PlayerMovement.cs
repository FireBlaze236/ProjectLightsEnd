using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController _controller;

    [Header("Horizontal Movement")]
    [SerializeField] Vector2 _velocity = new Vector2();
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] bool _sprinting = false;
    [SerializeField] float _sprintMultiplier = 2f;
    [SerializeField] float _sprintCost = 5f;
    [SerializeField] float _sprintRecharge = 4f;
    [SerializeField] TrailRenderer _sprintEffect;
    [SerializeField] PlayerStats _playerStats;

    [SerializeField] bool _moving = false;

    [Header("Gravity/Jump")]
    [SerializeField] int _maxJumps = 1;
    [SerializeField] int _jumpCounter = 0;
    [SerializeField] float _verticalVel = 0f;
    [SerializeField] float _jumpHeight = 5f;

    [Header("GroundCheck")]
    [SerializeField] Transform _groundCheckPoint;
    [SerializeField] bool _isOnGround = false;
    [SerializeField] float _checkRadius = 0.5f;
    [SerializeField] LayerMask _groundLayers;
    const float GRAVITY = -9.8f;

    [Header("Look")]
    [SerializeField] Camera _playerCamera;
    [SerializeField] float _minDistance = 1f;
    [SerializeField] LayerMask _targetableLayers;
    [SerializeField] Transform _gunPoint;
    [SerializeField] Transform _targeter;
    Plane _lookPlane;



    public Action<string, bool> OnPlayerMove;
    public Action<string, float> OnPlayerSpeedChange;
    

    public Action<string> OnJumpStart;
    public Action<string, float> OnJumpBlend;

    public Action<string, bool> OnPlayerFall;


    public Action OnShoot;

    [SerializeField] AudioClip _footStepSound;
    float _stepTime = 0.5f;
    bool _stepBool = false;

    private void Start()
    {
        StartCoroutine(Step(_stepTime));
    }

    private void Update()
    {
        ProcessMovementInput();

        if (_sprinting && _playerStats.hasEnergy)
        {
            if (!_sprintEffect.emitting)
                _sprintEffect.emitting = true;
            if(_moving)
                _playerStats.DrainEnergy(_sprintCost * Time.deltaTime);
            else
                _playerStats.RechargeEnergy(_sprintRecharge * Time.deltaTime);
        }
        else
        {
            _sprintEffect.emitting = false;
            _sprinting = false;
            _playerStats.RechargeEnergy(_sprintRecharge * Time.deltaTime);
        }


    }
    private void FixedUpdate()
    {
        ProcessMovement();
        ProcessLookDirection();

        
    }

    IEnumerator Step(float time)
    {
        _stepBool = true;
        while(_stepBool)
        {
            if(_moving && _isOnGround)
                SFXPlayer.Instance.PlaySoundEffect(_groundCheckPoint.position, _footStepSound);
          
            if(_moving & _sprinting)
                yield return new WaitForSeconds(time / 2f);
            else
                yield return new WaitForSeconds(time);
        }
    }

    private void ProcessMovementInput()
    {
        float z = Input.GetAxisRaw("Vertical");
        float x = Input.GetAxisRaw("Horizontal");
        _velocity = new Vector2(x, z);

        if(Input.GetButtonDown("Fire3") && !_sprinting )
        {
            _sprinting = true;
            
        }
        else if(Input.GetButtonUp("Fire3"))
        {
            _sprinting = false;
            
        }

        if(_velocity.magnitude >= 0.01f)
        {
            _moving = true;
            OnPlayerMove?.Invoke("moving", _moving);

            
        }
        else
        {
            _moving = false;
            OnPlayerMove?.Invoke("moving", _moving);

            _sprintEffect.emitting = false;

            
        }

        



        GroundCheck();

        if (Input.GetButtonDown("Jump") && _jumpCounter != _maxJumps)
        {
            _verticalVel += Mathf.Sqrt(-2 * GRAVITY * _jumpHeight);
            transform.position += Vector3.up * 2f;
            _jumpCounter++;
            _controller.Move(_verticalVel * Time.deltaTime * Vector3.up);

            OnJumpStart?.Invoke("jumpTrigger");

            OnJumpBlend?.Invoke("Jump", 1);
        }
        else if (_isOnGround)
        {
            _verticalVel = -0.01f;
            _jumpCounter = 0;
            OnPlayerFall?.Invoke("falling", false);

            OnJumpBlend?.Invoke("Jump", 0);
        }
        else
        {
            _verticalVel += GRAVITY * Time.deltaTime;
            _controller.Move(_verticalVel * Time.deltaTime * Vector3.up);

            OnPlayerFall?.Invoke("falling", true);
            OnJumpBlend?.Invoke("Jump", 1);
        }

    }

    private void ProcessMovement()
    {
        if (_moving)
        {
            Vector3 vel = new Vector3(_velocity.x, 0, _velocity.y).normalized;
            float speed = 0;
            if (_sprinting)
            {
                speed = _sprintMultiplier * _moveSpeed;
                OnPlayerSpeedChange?.Invoke("runBlend", 1);
            }
            else
            {
                speed = _moveSpeed;
                OnPlayerSpeedChange?.Invoke("runBlend", 0);
            }
            _controller.Move(vel * speed * Time.deltaTime);


            float vert = Vector3.Dot(transform.forward, vel.normalized);
            float hor = Vector3.Dot(transform.right, vel.normalized);

            OnPlayerSpeedChange?.Invoke("verticalVel", vert);
            OnPlayerSpeedChange?.Invoke("horizontalVel", hor);

            
        }
    }

    private void ProcessLookDirection()
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        float dist = (_playerCamera.transform.position - transform.position).magnitude;

        

        if (Physics.Raycast(ray, out RaycastHit hit, dist + 50f, _targetableLayers))
        {
            Vector3 dir = (hit.point - transform.position);
            if (dir.sqrMagnitude <= (_minDistance * _minDistance))
                return;

            _gunPoint.LookAt(hit.point);
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));

            _targeter.transform.position = (hit.point - dir.normalized * 1.0f); ;
            
        }
        else
        {
            _lookPlane = new Plane(Vector3.up, _gunPoint.position);
            _lookPlane.Raycast(ray, out float enter);

            Vector3 point = ray.GetPoint(enter);

            if ((point - transform.position).sqrMagnitude <= (_minDistance * _minDistance))
                return;

            _gunPoint.LookAt(point);
            transform.LookAt( new Vector3(point.x, transform.position.y, point.z));

            _targeter.transform.position = point;
        }
    }

    private void GroundCheck()
    {
        Collider[] cols = Physics.OverlapSphere(_groundCheckPoint.position, _checkRadius, _groundLayers);
        _isOnGround = cols.Length > 0;

        
    }


    
    
}

﻿using UnityEngine;
using System.Collections;

public class Cannon : InteractiveObject {

	public float CannonPower = 10.0f;
	public float RotateSpeed = 5.0f;
	public float Cooldown = 1.5f;
	public int ShootAngle = 90;


	private Transform 	_hinge;
	private Transform 	_spawner;
	private GameObject 	_cannonBall;
	private float 		_angleLimit;
	private float 		_curAngle = 0;
	private bool		_isRightSide;
	private float		_remainCooldown = 0;

	void Start () {
		_hinge 		 = transform.Find("CannonHinge");
		_spawner 	 = _hinge.Find("CannonChamber/BulletSpawner");
		_cannonBall  = Resources.Load<GameObject>("CannonBall");
		_angleLimit  = ShootAngle/2;
		_keyList 	 = new KeyCode[]{ KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
		_isRightSide = _spawner.position.x > _hinge.position.x ? true : false; 
	}

	void FixedUpdate(){
		if(_remainCooldown != 0)
			_remainCooldown = Mathf.Clamp(_remainCooldown - Time.deltaTime, 0, _remainCooldown);

	}
	
	override protected void OnButtonPressed(KeyCode key){
		Debug.Log("cannon press "+key);
		switch(key){
			case KeyCode.A:
				if(!_isRightSide)
					Shoot();
				break;
			case KeyCode.D:
				if(_isRightSide)
					Shoot();
				break;
			default:
				break;
		}
	}

	override protected void OnButtonHold(KeyCode key){
		Debug.Log("cannon hold "+key);
		switch(key){
		case KeyCode.W:
			_curAngle -= RotateSpeed * Time.deltaTime;
			_curAngle = Mathf.Clamp(_curAngle, -_angleLimit, +_angleLimit); // update the object rotation: 
			_hinge.localRotation = Quaternion.Euler(0,0,_curAngle); 
			break;
		case KeyCode.S:
			_curAngle += RotateSpeed * Time.deltaTime;
			_curAngle = Mathf.Clamp(_curAngle, -_angleLimit, +_angleLimit); // update the object rotation: 
			_hinge.localRotation = Quaternion.Euler(0,0,_curAngle); 
			break;
		default:
			break;
		}
	}

	private void Shoot(){
		if(_remainCooldown == 0){
			GameObject bullet = Instantiate(_cannonBall,_spawner.position,_hinge.localRotation) as GameObject;
			Vector3 distance = _spawner.position - _hinge.position;
			Vector3 forceDirection = distance / distance.magnitude;
			bullet.rigidbody.AddForce(CannonPower * forceDirection, ForceMode.Impulse);
			Destroy(bullet,1.0f);
			_remainCooldown = Cooldown;
		}
	}

	override protected void OnButtonRelease(KeyCode key){}

}

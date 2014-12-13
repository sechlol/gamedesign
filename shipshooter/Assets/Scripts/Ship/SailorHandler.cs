﻿using UnityEngine;
using System.Collections;

public class SailorHandler : MonoBehaviour {

	public float sidewaysSpeed = 0.2f;
	public float climbSpeed = 0.1f;
	public float fallSpeed = 0.3f;


	private float handsHeight = 0.02f; // y-position of hands
	private float sailorWidth = 0.09f;  

	bool lastMoveOnLadder = false;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{

		if ( UserInput.Player1Left() && !LeftObstructed() && !FeetInsideFloorOnStairs() )
		{
			Move( new Vector3(-sidewaysSpeed * Time.deltaTime, 0.0f) );
			lastMoveOnLadder = false;
		}

		if ( UserInput.Player1Right() && !RightObstructed() && !FeetInsideFloorOnStairs())
		{
			Move( new Vector3(sidewaysSpeed * Time.deltaTime, 0.0f) );
			lastMoveOnLadder = false;
		}

		if ( UserInput.Player1Up() && ( HandsOnLadder() || FeetOnLadder()) )
		{
			Move( new Vector3(0.0f, climbSpeed * Time.deltaTime ) );
			lastMoveOnLadder = true;
		}

		if ( UserInput.Player1Down() && FeetOnLadder() )
		{
			Move( new Vector3(0.0f, -climbSpeed * Time.deltaTime ) );
			lastMoveOnLadder = true;
		}

		if (!StandingOnFloor () && !FeetOnLadder () && !HandsOnLadder ()) 
		{
			Fall();
			lastMoveOnLadder = false;
		}
	}

	void Move( Vector3 localPosChange )
	{
		transform.localPosition = transform.localPosition + localPosChange;
	}

	void Fall()
	{
		int iterations = 5;
		
		for (int i=0; i< iterations; i++ )
		{
			Vector3 posChange = new Vector3 (0.0f, -fallSpeed/iterations* Time.deltaTime);
			transform.localPosition = transform.localPosition + posChange;
			
			if (StandingOnFloor ())
				break;
		}
	}

	bool StandingOnFloor()
	{
		return (IsColliderAtLocal(transform.localPosition, "Floor"));
	}

	bool FeetInsideFloorOnStairs()
	{
		return FeetOnLadder() && StandingOnFloor() && lastMoveOnLadder;
	}


	bool FeetOnLadder()
	{
		return (IsColliderAtLocal(transform.localPosition, "Ladder"));
	}

	bool HandsOnLadder()
	{
		Vector3 checkPosition = transform.localPosition + new Vector3(0.0f, handsHeight, 0.0f);;
		return (IsColliderAtLocal(checkPosition, "Ladder"));
	}

	bool LeftObstructed()
	{
		Vector3 checkPosition = transform.localPosition+ new Vector3(-sailorWidth/2.0f, 0.0f, 0.0f);
		return (IsColliderAtLocal(checkPosition, "Wall"));
	}

	bool RightObstructed()
	{
		Vector3 checkPosition = transform.localPosition+ new Vector3(sailorWidth/2.0f, 0.0f, 0.0f);
		return (IsColliderAtLocal(checkPosition, "Wall"));
	}

	// Is there a collider with given name at given local position
	bool IsColliderAtLocal(Vector3 localPosition, string name)
	{
		Vector3 position = transform.parent.transform.TransformPoint(localPosition);
		return IsColliderAt(position, name);
	}


	// Is there a collider with given name at given global position
	bool IsColliderAt(Vector3 position, string name)
	{
		Collider2D[] hits = Physics2D.OverlapPointAll (position);

		for (int i=0; i < hits.Length; i++)
			if (hits[i].name == name )
				return true;
		
		return false;
	}

}

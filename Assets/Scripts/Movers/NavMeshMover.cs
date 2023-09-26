using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMover : Mover
{
	private NavMeshAgent agent;

	public bool useDestination = true;
	public override void Move()
	{
		if (agent == null)
		{
			agent = GetComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updateUpAxis = false;
		}
		if(useDestination)
		{
			//AI를 통한 이동 중, Rotation 설정
			Vector3 agentDir = (agent.destination - transform.position);
			direction = agentDir.normalized;
		}
		else
		{
			SetDestination(transform.position+(Vector3)direction);
		}
		
		if(isRotate)
			SetRotationByDirection();
	}
	
	//테스트
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			SetDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	//외부에서 목적지 설정할 때 사용
	public void SetDestination(Vector3 _moveTo)
	{
		if (agent == null)
		{
			agent = GetComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updateUpAxis = false;
		}
		agent.SetDestination((Vector2)_moveTo);
	}

	public void SetDirection(Vector2 _dir)
	{
		direction = _dir;
		useDestination = false;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private float _speed;

    [SerializeField] private List<Node> patrolNodes;
    [SerializeField] private LayerMask _targetLayerMask;
    private Fsm _FSM;

    public Transform GetTarget () => _target;

    private void Awake()
    {
        
        _FSM = new ();
        //_FSM.CreateState("Patrol", new PatrolState(this, patrolNodes));
        //_FSM.CreateState("Chase", new ChaseState(this));
        //_nodeLayerMask = patrolNodes.First().gameObject.layer;
    }

    private void Start()
    {

    }

    private void Update()
    {
        _FSM.Execute();
    }
    public void ChangeState(string state)
    {
        //_FSM.ChangeState(state);
    }
    public bool InFOV()
    {
        var dir = _target.position - transform.position;

        if (dir.magnitude < viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) <= viewAngle * 0.5f)
            {
                var inFOV = LOStoTarget();
                if (inFOV) EnemyManager.Instance.detectTarget.Invoke("Chase");
                return inFOV;
            }
        }

        return false;
    }
    public bool LOStoTarget()
    {
        return GameManager.Instance.LosToWall(transform.position, _target.position);
    }
    public void MoveTo(Vector3 dir)
    {
        transform.position += dir.normalized * _speed * Time.deltaTime;
        transform.forward = dir.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, viewRadius);
        
        Gizmos.color = Color.blue;

        Vector3 lineA = GetVectorFromAngle(viewAngle * 0.5f + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle * 0.5f + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public void ChaseTarget()
    {
        var dir = _target.position - transform.position;
        if(dir.magnitude > 0.2f)MoveTo(dir);
    }
}

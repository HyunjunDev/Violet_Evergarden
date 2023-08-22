using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DaggerPoolable : PoolableObject
{
    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private Vector2 _dir;
    public Vector2 Dir
    {
        get { return _dir; }
        set { _dir = value; }
    }

    private float _speed = 10;
    public float Speed => _speed;

    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask _layerMask = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * -0.05f, transform.right, 0.1f, _layerMask);
        Debug.DrawRay(transform.position, transform.right * 0.1f, Color.red);
        if (hit.collider != null)
        {
            Contact(hit);
        }
        rb.velocity = _dir * _speed;
    }

    public void SetRotation()
    {
        transform.rotation = Utility.GetDashRotation(_dir, 0f);
    }


    public override void PopInit()
    {
    }

    public override void PushInit()
    {

    }

    public override void StartInit()
    {

    }

    private void Contact(RaycastHit2D hit)
    {
        Vector3 direction;
        float distance;
        //bool overlapped = Physics.ComputePenetration(_player.playerCollider, hit.point, _player.transform.rotation, hit.collider, hit.transform.position, hit.transform.rotation, out direction, out distance);
        PoolManager.Instance.Push(this);
    }
}

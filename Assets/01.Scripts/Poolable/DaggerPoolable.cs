using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.AI;

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
        GameObject colliderObject = new GameObject("DynamicCollider");
        BoxCollider2D col = colliderObject.AddComponent<BoxCollider2D>();
        col.size = _player.playerCollider.Col.size;
        col.offset = _player.playerCollider.Col.offset;
        colliderObject.transform.position = new Vector3(hit.point.x, hit.point.y, 0);

        ColliderDistance2D dis = Physics2D.Distance(col, hit.collider);
        //Debug.Log(dis.distance);
        _player.transform.position = (Vector3)hit.point + (transform.right * -dis.distance);  

        PoolManager.Instance.Push(this);

        Destroy(colliderObject);
    }
}

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

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * -0.05f, transform.right, 0.1f, _player.DaggerDataSO.layerMask);
        Debug.DrawRay(transform.position, transform.right * 0.1f, Color.red);
        if (hit.collider != null)
        {
            Contact(hit);
        }
        rb.velocity = _dir * _player.DaggerDataSO.speed;
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
        Vector2 endPosition = hit.point + transform.right * -col.size * 0.5f;
        _player.transform.position = endPosition;

        PoolManager.Instance.Push(this);

        Destroy(colliderObject);
    }
}
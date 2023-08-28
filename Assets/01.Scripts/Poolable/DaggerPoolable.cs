using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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

    private Coroutine _contactCoroutine = null;

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
        if (_contactCoroutine != null)
        {
            StopCoroutine(_contactCoroutine);
        }
    }

    public override void StartInit()
    {

    }

    private void Contact(RaycastHit2D hit)
    {
        if (_contactCoroutine != null)
        {
            StopCoroutine(_contactCoroutine);
        }
        _contactCoroutine = StartCoroutine(ColliderCheck(hit));
    }

    private IEnumerator ColliderCheck(RaycastHit2D hit)
    {
        GameObject colliderObject = new GameObject("DynamicCollider");
        BoxCollider2D col = colliderObject.AddComponent<BoxCollider2D>();
        col.size = _player.playerCollider.Col.size;
        col.offset = _player.playerCollider.Col.offset;
        Vector2 distance = new Vector2(col.size.x * hit.normal.x, col.size.y * hit.normal.y) * 0.5f;
        _player.transform.position = hit.point + distance;
        yield return new WaitForFixedUpdate();
        _player.GetModule<WallGrabModule>(EPlayerModuleType.WallGrab).TryWallGrab();
        Destroy(colliderObject);
        PoolManager.Instance.Push(this);
    }
}
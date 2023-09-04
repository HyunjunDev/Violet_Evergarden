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

    private TrailRenderer _trailRenderer = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.right * -0.25f, transform.right, 0.35f, _player.DaggerDataSO.layerMask);
        Debug.DrawRay(transform.position, transform.right * 0.1f, Color.red);
        if (hit.collider != null)
        {
            Contact(hit);
        }
        rb.velocity = _dir * _player.DaggerDataSO.speed * _player.GetModule<ThrowDaggerModule>(EPlayerModuleType.ThrowDagger).throwSpeedMultiplier;
    }

    public void SetRotation()
    {
        transform.rotation = Utility.GetRotationByVector(_dir, 0f);
    }


    public override void PopInit()
    {
        if (_trailRenderer != null)
        {
            _trailRenderer.Clear();
            _trailRenderer.BakeMesh(new Mesh());
        }
    }

    public override void PushInit()
    {
        if (_contactCoroutine != null)
        {
            StopCoroutine(_contactCoroutine);
        }
        if (_trailRenderer != null)
        {
            _trailRenderer.Clear();
            _trailRenderer.BakeMesh(new Mesh());
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

        Vector2 startPosition = _player.transform.position;
        Vector2 endPosition = Vector3.zero;
        Vector2 distance = new Vector2(col.size.x * hit.normal.x, col.size.y * hit.normal.y) * 0.5f;
        endPosition = hit.point + distance;
        _player.transform.position = endPosition;

        yield return new WaitForFixedUpdate();
        WallGrabModule wallGrabModule = _player.GetModule<WallGrabModule>(EPlayerModuleType.WallGrab);
        if (wallGrabModule != null)
        {
            wallGrabModule.TryWallGrab();
        }
        ThrowDaggerModule daggerModule = _player.GetModule<ThrowDaggerModule>(EPlayerModuleType.ThrowDagger);
        if (daggerModule != null)
        {
            daggerModule.Landed(hit.point, startPosition, endPosition);
        }

        Destroy(colliderObject);
        PoolManager.Instance.Push(this);
    }
}
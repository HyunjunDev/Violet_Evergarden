using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private List<Transform> spawnPosition = null;
    private int spawnCount = 0;
    public PolygonCollider2D cameraAreaCollider = null;
    [Header("Editor Only")]
    [SerializeField]
    private Collider2D _cameraAreaColliderGrid = null;
    [SerializeField]
    private LayerMask _groundMask = 0;

    [SerializeField]
    private float timeDelay = 0.4f;

    public int SpawnCount
    {
        get { return spawnCount; }
        set { spawnCount = value; }
    }

    public void ChangeRoom()
    {
        MapManager.Instance.ChangeRoom(this);
    }

    public void StartTimeDelay()
    {
        if (this.gameObject.activeSelf == true)
            StartCoroutine(TimeDelay(timeDelay));
    }

    public Vector2 GetSpawnPoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(spawnPosition[spawnCount].position, Vector2.down, _groundMask);
        if (hit.collider != null)
        {
            return hit.point;
        }
        return Vector2.zero;
    }

    IEnumerator TimeDelay(float time)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1.0f;
    }

    [ContextMenu("카메라 영역 굽기")]
    public void BakeCameraArea()
    {
        if (cameraAreaCollider == null || _cameraAreaColliderGrid == null)
        {
            return;
        }
        List<Vector2> points = new List<Vector2>();
        Vector2 center = _cameraAreaColliderGrid.bounds.center - transform.position;
        Vector2 expend = _cameraAreaColliderGrid.bounds.size * 0.5f;
        expend.x -= 0.05f;
        expend.y -= 0.05f;
        Vector2 leftTop = center + new Vector2(-expend.x, expend.y);
        Vector2 leftBottom = center + new Vector2(-expend.x, -expend.y);
        Vector2 rightTop = center + new Vector2(expend.x, expend.y);
        Vector2 rightBottom = center + new Vector2(expend.x, -expend.y);
        points.Add(leftTop);
        points.Add(leftBottom);
        points.Add(rightBottom);
        points.Add(rightTop);
        cameraAreaCollider.SetPath(0, points);
        CameraManager.Instance.BakeCurrentConfiner(this);
    }

    private void OnDrawGizmos()
    {
        if (_cameraAreaColliderGrid == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_cameraAreaColliderGrid.bounds.center, _cameraAreaColliderGrid.bounds.size);

        RaycastHit2D hit = Physics2D.Raycast(spawnPosition[spawnCount].position, Vector2.down, _groundMask);
        if(hit.collider != null)
        {
            Gizmos.DrawLine(spawnPosition[spawnCount].position, hit.point);
            Gizmos.DrawWireSphere(hit.point, 0.15f);
            Gizmos.DrawWireCube(hit.point + new Vector2(-0.02f, 0.525f), new Vector2(0.66f, 1.05f));
        }
    }
}

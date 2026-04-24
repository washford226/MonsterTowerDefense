using UnityEngine;

public class ArrowView : MonoBehaviour
{
    private EnemyView target;
    private float speed;
    private bool isInitialized;

    public void Init(EnemyView targetView, float travelTime)
    {
        target = targetView;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        speed = distance / travelTime;

        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized || target == null)
        {
            Destroy(gameObject); // 🔥 clean up dead arrows
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
using UnityEngine;
using UnityEngine.Serialization;

public class FireballProjectile : Projectile
{
    [FormerlySerializedAs("explotionRadius"), SerializeField] private float explosionRadius;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.right, this.speed * Time.deltaTime);

        if (hit)
            Explode(hit.transform.gameObject);

        this.transform.Translate(Vector3.right * (this.speed * Time.deltaTime));
    }

    private void Explode(GameObject hit)
    {
        if (hit.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(this.damage);

        Destroy(this.gameObject);
    }
}
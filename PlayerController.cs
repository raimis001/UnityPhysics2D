#define CIRCLE_OVERLAP_
#define CIRCLE_CAST_
#define CAPSULE_CAST

using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public TMPro.TMP_Text debugText;
  public Vector2 deltaPos = Vector2.zero;
  public Vector2 deltaSize = new Vector2(0.5f, 1f);
  public LayerMask enemyLayer;
  public float distance = 1;

  public float radius = 1;


  void Update()
  {
    debugText.text = "";

#if CIRCLE_OVERLAP
    // Overlap Circle 

    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.TransformDirection(deltaPos), radius, enemyLayer);

    foreach (Collider2D col in colliders)
    {
      debugText.text += col.name + "\n";
    }
#endif
    RaycastHit2D[] hits;
#if CIRCLE_CAST
    // Circle Cast
    hits = Physics2D.CircleCastAll(transform.position + transform.TransformDirection(deltaPos), radius, transform.right, distance, enemyLayer);

#endif

#if CAPSULE_CAST
    // Capsule Cast
    hits = Physics2D.CapsuleCastAll(transform.position + transform.TransformDirection(deltaPos), deltaSize, CapsuleDirection2D.Vertical, transform.eulerAngles.z, transform.right, distance, enemyLayer);
#endif

#if CIRCLE_CAST || CAPSULE_CAST
    foreach (RaycastHit2D hit in hits)
      debugText.text += hit.collider.name + "\n";
#endif

  }
#if UNITY_EDITOR
  void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;

#if CIRCLE_OVERLAP
    //Overlap Circle
    Gizmos.DrawWireSphere(transform.position + transform.TransformDirection(deltaPos), radius );
#endif

#if CIRCLE_CAST
    //Circle Cast
    Gizmos.DrawWireSphere(transform.position + transform.TransformDirection(deltaPos), radius );
    Gizmos.DrawWireSphere(transform.position + transform.TransformDirection(deltaPos) + transform.right * distance, radius );
#endif

#if CAPSULE_CAST
    //Capsule Cast
    DrawCapsule(transform.position + transform.TransformDirection(deltaPos), deltaSize);
    DrawCapsule(transform.position + transform.TransformDirection(deltaPos) + transform.right * distance, deltaSize);
#endif
  }

  void DrawCapsule(Vector3 origin, Vector2 size)
  {
    UnityEditor.Handles.color = Color.yellow;
    Vector3 up = transform.up * (size.y - size.x) / 2f;
    UnityEditor.Handles.DrawWireArc(origin + up, Vector3.forward, transform.right, 180f, size.x / 2f, 2);
    UnityEditor.Handles.DrawWireArc(origin - up, Vector3.forward, -transform.right, 180f, size.x / 2f, 2);
  }
  #endif
}

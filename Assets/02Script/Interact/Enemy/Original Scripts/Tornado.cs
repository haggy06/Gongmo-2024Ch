using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class Tornado : MonoBehaviour
{
    [Space(5)]
    public bool isPush = true;
    public float sinkPower;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * ((isPush ? -1f : 1f) * sinkPower * 30f * Time.fixedDeltaTime));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _))
        {
            PlayerController.Inst.DefaultVelo = (PlayerController.Inst.transform.position - transform.position).normalized * sinkPower * (isPush ? 1f : -1f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _))
        {
            PlayerController.Inst.DefaultVelo = Vector2.zero;
        }
    }
}

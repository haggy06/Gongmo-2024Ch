using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class AttractPlayer : MonoBehaviour
{
    [SerializeField]
    private bool isPush = true;
    [SerializeField]
    private float sinkPower;

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
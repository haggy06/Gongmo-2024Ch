using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DeadParticle : MonoBehaviour
{
    private static ParticleSystem particle;
    private void Awake()
    {
        if (!particle) // ��ƼŬ�� ��� ���� ���
        {
            particle = GetComponent<ParticleSystem>();
        }
    }

    public static void ParticleLoad(Vector2 position, Sprite ownerSprite, float rotation)
    {
        particle.transform.position = position;
        particle.transform.eulerAngles = Vector3.forward * rotation;
        particle.shape.spriteRenderer.sprite = ownerSprite;

        particle.Play();
    }
}

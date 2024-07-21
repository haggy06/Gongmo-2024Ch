using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DeadParticle : MonoBehaviour
{
    [SerializeField]
    private AudioClip monsterPopSound;

    private static DeadParticle deadParticle;
    private ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        if (!deadParticle) // 파티클이 비어 있을 경우
        {
            deadParticle = this;
        }
    }

    public static void ParticleLoad(Vector2 position, Sprite ownerSprite, float rotation)
    {
        AudioManager.Inst.PlaySFX(deadParticle.monsterPopSound);

        deadParticle.particle.transform.position = position;
        deadParticle.particle.transform.eulerAngles = Vector3.forward * rotation;
        deadParticle.particle.shape.spriteRenderer.sprite = ownerSprite;

        deadParticle.particle.Play();
    }
}

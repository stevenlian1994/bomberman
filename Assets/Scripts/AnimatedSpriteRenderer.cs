using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite[] animationSprites;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    public bool loop = true;
    public bool idle = true;
    public float animationTime = 0.25f;

    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnEnable(){
        spriteRenderer.enabled = true;
    }

    private void OnDisable(){
        spriteRenderer.enabled = false;
    }

    private void Start (){
        InvokeRepeating(nameof(NextFrame), animationTime, animationTime);
    }

    private void NextFrame(){
        animationFrame++;

        if(loop && animationFrame >= animationSprites.Length) {
            animationFrame = 0;
        }

        if(idle){
            spriteRenderer.sprite = idleSprite;
        } else if (animationFrame >= 0 && animationFrame < animationSprites.Length) {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }
}

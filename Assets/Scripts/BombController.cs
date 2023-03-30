using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public KeyCode inputKeyForUltimate = KeyCode.V;

    public float bombFuseTime = 3.5f;
    public int bombAmount = 2;
    private int bombsRemaining;
    private bool hasUltimate = true;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    public AnimatedSpriteRenderer spriteRendererUltimate;
    public GameObject tmp;

    private void OnEnable(){
        bombsRemaining = bombAmount;
    }

    private void Update(){
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey)){
            StartCoroutine(PlaceBomb());
        }

        if (hasUltimate && Input.GetKeyDown(inputKeyForUltimate)){
            hasUltimate = !hasUltimate;
            StartCoroutine(CastUltimate());
        }
    }

    private IEnumerator CastUltimate(){
        // tmp.SetActive(true);
        tmp.SetActive(true);
        spriteRendererUltimate.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 0.0001f;
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - start < 0.5f) // 10 < 13
        {
        }
        Time.timeScale = 1;
        tmp.SetActive(false);
        spriteRendererUltimate.enabled = false;
        ChainBomb();
    }

    private void PutBombOneAhead(Vector2 position){
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // //check space ahead
        Vector2 spaceAhead = position + GetComponent<MovementController>().GetDirection();

        if(!Physics2D.OverlapBox(spaceAhead, Vector2.one / 2f, 0f, explosionLayerMask)){
            StartCoroutine(PlaceBomb(spaceAhead));
        }
    }

    private void ChainBomb(){
        ChainBombHelper(this.transform.position, GetComponent<MovementController>().GetDirection(), 5);
    }

    private void ChainBombHelper(Vector2 position, Vector2 direction, int length){
        if(length <= 0) {
            return;
        } else {
            position += direction;
            length--;
            PutBombOneAhead(position);
            ChainBombHelper(position, direction, length);
        }
    }

    private IEnumerator PlaceBomb(){
        Vector2 position = this.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    private IEnumerator PlaceBomb(Vector2 position){
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length){
        if (length <= 0) {
            return;
        }

        position += direction;

        if(Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask)){
            ClearDestructibles(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);
        Explode(position, direction, length - 1);
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }

    private void ClearDestructibles(Vector2 position){
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if(tile != null){
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void Addbomb(){
        bombAmount++;
        bombsRemaining++;
    }

}

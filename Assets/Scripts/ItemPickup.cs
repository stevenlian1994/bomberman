using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        BlastRadius,
        ExtraBomb,
        SpeedIncrease
    }
    public ItemType type;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")) {
            OnItemPickup(other.gameObject);
        }
    }

    private void OnItemPickup(GameObject player){
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().Addbomb();
                break;
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius++;
                break;
            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed++;
                break;
        }
        Destroy(gameObject);
    }
}

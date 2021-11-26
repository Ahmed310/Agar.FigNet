using AgarIOCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public float borderThickness = 1.0f;
    public GameObject tilePrefab;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D upCollider;
    private BoxCollider2D downCollider;
    private BoxCollider2D rightCollider;
    private BoxCollider2D leftCollider;

    private float spriteWidth;
    private float spriteHeight;

    private Vector2 spawnField;

    void Start()
    {
        spawnField.x = AppConstants.MAP_SIZE_X;
        spawnField.y = AppConstants.MAP_SIZE_Y;

        upCollider = gameObject.AddComponent<BoxCollider2D>();
        upCollider.offset = new Vector2(0.0f, spawnField.y);
        upCollider.size = new Vector2(spawnField.x * 2.0f, borderThickness);
        rightCollider = gameObject.AddComponent<BoxCollider2D>();
        rightCollider.offset = new Vector2(spawnField.x, 0.0f);
        rightCollider.size = new Vector2(borderThickness, spawnField.y * 2.0f);
        downCollider = gameObject.AddComponent<BoxCollider2D>();
        downCollider.offset = new Vector2(0.0f, -spawnField.y);
        downCollider.size = new Vector2(spawnField.x * 2.0f, borderThickness);
        leftCollider = gameObject.AddComponent<BoxCollider2D>();
        leftCollider.offset = new Vector2(-spawnField.x, 0.0f);
        leftCollider.size = new Vector2(borderThickness, spawnField.y * 2.0f);
        spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.sprite.bounds.size.x;
        spriteHeight = spriteRenderer.sprite.bounds.size.y;

        PrepareLevel();
    }

    public void PrepareLevel()
    {
        if (spriteWidth > 0.0f && spriteHeight > 0.0f)
        {
            float tilesOnX = (spawnField.x * 2) / spriteWidth;
            float tilesOnY = (spawnField.y * 2) / spriteHeight;


            for (int y = 0; y < tilesOnY + 1; y++)
            {
                for (int x = 0; x < tilesOnX + 1; x++)
                {
                    Vector3 position = new Vector3(-spawnField.x + ((spriteWidth) * x), -spawnField.y + ((spriteHeight) * y), 0.0f);
                    GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                    newTile.transform.parent = gameObject.transform;
                    //tiles.Add(newTile);
                }
            }
        }
        else
        {
            //Print("Tile prefab contains no data!", "error");
        }

    }
}

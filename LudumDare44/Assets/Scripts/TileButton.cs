using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum ButtonType
{
    Play, Exit
}

public class TileButton : MonoBehaviour
{
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] ButtonType type;

    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        spriteRenderer.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = idleSprite;
    }

    private void OnMouseDown()
    {
        spriteRenderer.sprite = downSprite;
        switch (type)
        {
            case ButtonType.Play:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case ButtonType.Exit:
                Application.Quit();
                break;
        }
    }
}

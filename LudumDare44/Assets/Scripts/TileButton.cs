using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public enum ButtonType
{
    Play, Exit, Restart, Back
}

public class TileButton : MonoBehaviour
{
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] ButtonType type;

    [SerializeField] UnityEvent hoverEvent;
    [SerializeField] UnityEvent clickedEvent;

    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        hoverEvent.Invoke();
        spriteRenderer.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = idleSprite;
    }

    private void OnMouseDown()
    {
        spriteRenderer.sprite = downSprite;
        Action();
    }

    private void Update()
    {
        switch (type)
        {
            case ButtonType.Play:
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    Action();
                }
                break;
            case ButtonType.Exit:
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    Action();
                }
                break;
            case ButtonType.Restart:
                if (Input.GetKeyDown(KeyCode.R)) {
                    Action();
                }
                break;
            case ButtonType.Back:
                if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Backspace)) {
                    Action();
                }
                break;
        }
    }

    public void Action()
    {
        clickedEvent.Invoke();
        switch (type)
        {
            case ButtonType.Play:
                // Removed to use screenfading
                break;
            case ButtonType.Exit:
                Application.Quit();
                break;
            case ButtonType.Restart:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case ButtonType.Back:
                SceneManager.LoadScene(0);
                break;
        }
    }
}

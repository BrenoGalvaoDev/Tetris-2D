using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartsMovement : MonoBehaviour
{
    public bool pauseGame;
    public bool canRotate;
    public bool rotate360;

    float fall;

    Vector2 movement;

    private void Update()
    {
        pauseGame = GameManager.Instance.isPauseGame;
        if (Time.time - fall >= (1 / GameManager.Instance.difficulty) && !pauseGame)
        {
            if (ValidPosition())
            {
                GameManager.Instance.UpdateGrid(this);
                transform.position += new Vector3(0, -1, 0);
            }
            else
            {
                FindObjectOfType<PlayerInput>().enabled = false;
                SpawnTetro.instance.Spawn = true;
                enabled = false;
                GameManager.Instance.score += 10;
                GameManager.Instance.scoreDifficulty += 10;
                SpawnTetro.instance.SpawnNewPiece();
                GameManager.Instance.DeleteLine();
                if (GameManager.Instance.AboveGrid(this))
                {
                    GameManager.Instance.isGameOver = true;
                }
            }
            fall = Time.time;
        }
    }

    void RotatePart()
    {
        if (canRotate)
        {
            if (!rotate360)
            {
                if(transform.rotation.z < 0)
                {
                    transform.Rotate(0, 0, 90);
                    if (ValidPosition())
                    {
                        GameManager.Instance.UpdateGrid(this);
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }

                }
                else
                {
                    transform.Rotate(0, 0, -90);
                    if (ValidPosition())
                    {
                        GameManager.Instance.UpdateGrid(this);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
            }
            else
            {
                transform.Rotate(0, 0, -90);
                if (ValidPosition())
                {
                    GameManager.Instance.UpdateGrid(this);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
        }
    }

    public void SetMovementInput(InputAction.CallbackContext value)
    {
        if (value.started && !pauseGame)
        {
            movement = value.ReadValue<Vector2>();
            if (movement != Vector2.zero)
            {
                if (movement.x > 0)
                {
                    transform.position += new Vector3(1, 0, 0);
                    if (ValidPosition())
                    {
                        GameManager.Instance.UpdateGrid(this);
                    }
                    else
                    {
                        transform.position += new Vector3(-1, 0, 0);
                    }
                }
                if (movement.x < 0)
                {
                    transform.position += new Vector3(-1, 0, 0);
                    if (ValidPosition())
                    {
                        GameManager.Instance.UpdateGrid(this);
                    }
                    else
                    {
                        transform.position += new Vector3(1, 0, 0);
                    }
                }
                if (movement.y < 0)
                {
                    if (ValidPosition())
                    {
                        GameManager.Instance.UpdateGrid(this);
                        transform.position += new Vector3(0, -1, 0);
                    }
                    else
                    {
                        enabled = false;
                        SpawnTetro.instance.Spawn = true;
                        GetComponent<PlayerInput>().enabled = false;
                        GameManager.Instance.DeleteLine();
                        if (GameManager.Instance.AboveGrid(this))
                        {
                            GameManager.Instance.isGameOver = true;
                        }
                        SpawnTetro.instance.SpawnNewPiece();
                        GameManager.Instance.score += 10;
                        GameManager.Instance.scoreDifficulty += 10;
                    }
                }
                if (movement.y > 0)
                {
                    RotatePart();
                }
            }
        }

        if (value.canceled)
        {
            movement = new Vector2(0, 0);
        }

    }

    public void SetPauseInput(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            GameManager.Instance.isPauseGame = !GameManager.Instance.isPauseGame;
        }
    }

    bool ValidPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 blockPosition = FindObjectOfType<GameManager>().RoundNumber(child.position);
            if (!FindObjectOfType<GameManager>().InsideTheGrid(blockPosition))
            {
                return false;
            }
            if (GameManager.Instance.PositionTransformGrid(blockPosition) != null && GameManager.Instance.PositionTransformGrid(blockPosition).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}

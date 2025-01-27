using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell2048 : MonoBehaviour
{
    public Cell2048 right;
    public Cell2048 down;
    public Cell2048 left;
    public Cell2048 up;
    public Fill2048 fill;

    private void OnEnable()
    {
        GameController2048.slide += OnSlide;
    }

    private void OnDisable()
    {
        GameController2048.slide -= OnSlide;
    }

    private void OnSlide(string whatWasSent)
    {
        CellCheck();
        if (whatWasSent == "w")
        {
            if (up != null) return;
            Cell2048 currentCell = this;
            SlideUp(currentCell);
        }
        if (whatWasSent == "d")
        {
            if (right != null) return;
            Cell2048 currentCell = this;
            SlideRight(currentCell);
        }
        if (whatWasSent == "s")
        {
            if (down != null) return;
            Cell2048 currentCell = this;
            SlideDown(currentCell);
        }
        if (whatWasSent == "a")
        {
            if (left != null) return;
            Cell2048 currentCell = this;
            SlideLeft(currentCell);
        }
        GameController2048.ticker++;
    }

    void SlideUp(Cell2048 currentCell)
    {
        if (currentCell.down == null) return;

        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.down;
            while (nextCell.down != null && nextCell.fill == null)
            {
                nextCell = nextCell.down;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block merged
                }
                else if (currentCell.down.fill != nextCell.fill)
                {
                    nextCell.fill.transform.parent = currentCell.down.transform;
                    currentCell.down.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block moved
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.down;
            while (nextCell.down != null && nextCell.fill == null)
            {
                nextCell = nextCell.down;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                SlideUp(currentCell);
                GameController2048.instance.blockMoved = true; // Block moved
            }
        }
        if (currentCell.down != null) SlideUp(currentCell.down);
    }

    void SlideRight(Cell2048 currentCell)
    {
        if (currentCell.left == null) return;

        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.left;
            while (nextCell.left != null && nextCell.fill == null)
            {
                nextCell = nextCell.left;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block merged
                }
                else if (currentCell.left.fill != nextCell.fill)
                {
                    nextCell.fill.transform.parent = currentCell.left.transform;
                    currentCell.left.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block moved
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.left;
            while (nextCell.left != null && nextCell.fill == null)
            {
                nextCell = nextCell.left;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                SlideRight(currentCell);
                GameController2048.instance.blockMoved = true; // Block moved
            }
        }
        if (currentCell.left != null) SlideRight(currentCell.left);
    }

    void SlideDown(Cell2048 currentCell)
    {
        if (currentCell.up == null) return;

        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.up;
            while (nextCell.up != null && nextCell.fill == null)
            {
                nextCell = nextCell.up;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block merged
                }
                else if (currentCell.up.fill != nextCell.fill)
                {
                    nextCell.fill.transform.parent = currentCell.up.transform;
                    currentCell.up.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block moved
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.up;
            while (nextCell.up != null && nextCell.fill == null)
            {
                nextCell = nextCell.up;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                SlideDown(currentCell);
                GameController2048.instance.blockMoved = true; // Block moved
            }
        }
        if (currentCell.up != null) SlideDown(currentCell.up);
    }

    void SlideLeft(Cell2048 currentCell)
    {
        if (currentCell.right == null) return;

        if (currentCell.fill != null)
        {
            Cell2048 nextCell = currentCell.right;
            while (nextCell.right != null && nextCell.fill == null)
            {
                nextCell = nextCell.right;
            }
            if (nextCell.fill != null)
            {
                if (currentCell.fill.value == nextCell.fill.value)
                {
                    nextCell.fill.Double();
                    nextCell.fill.transform.parent = currentCell.transform;
                    currentCell.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block merged
                }
                else if (currentCell.right.fill != nextCell.fill)
                {
                    nextCell.fill.transform.parent = currentCell.right.transform;
                    currentCell.right.fill = nextCell.fill;
                    nextCell.fill = null;
                    GameController2048.instance.blockMoved = true; // Block moved
                }
            }
        }
        else
        {
            Cell2048 nextCell = currentCell.right;
            while (nextCell.right != null && nextCell.fill == null)
            {
                nextCell = nextCell.right;
            }
            if (nextCell.fill != null)
            {
                nextCell.fill.transform.parent = currentCell.transform;
                currentCell.fill = nextCell.fill;
                nextCell.fill = null;
                SlideLeft(currentCell);
                GameController2048.instance.blockMoved = true; // Block moved
            }
        }
        if (currentCell.right != null) SlideLeft(currentCell.right);
    }

    void CellCheck()
    {
        if (fill == null) return;

        if (up != null && up.fill != null && up.fill.value == fill.value) return;
        if (right != null && right.fill != null && right.fill.value == fill.value) return;
        if (down != null && down.fill != null && down.fill.value == fill.value) return;
        if (left != null && left.fill != null && left.fill.value == fill.value) return;

        GameController2048.instance.GameOverCheck();
    }
}

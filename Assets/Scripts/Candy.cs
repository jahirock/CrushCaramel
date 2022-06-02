using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public int id;
    
    static Color selectedColor = new Color(0.5F, 0.5F, 0.5F, 1F);
    static Candy previousSelected = null;
    
    bool isSelected = false;

    Vector2[] adjacentDirections = new Vector2[] {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void SelectCandy()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previousSelected = gameObject.GetComponent<Candy>();
    }

    void DeselectCandy()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        previousSelected = null;
    }

    private void OnMouseDown()
    {
        if(spriteRenderer.sprite == null || BoardManager.sharedInstace.isShifting)
        {
            return;
        }

        if(isSelected)
        {
            DeselectCandy();
        }
        else
        {
            if(previousSelected == null)
            {
                SelectCandy();
            }
            else
            {
                if(CanSwipe())
                {
                    SwapSprite(previousSelected);
                    previousSelected.FindAllMatches();
                    previousSelected.DeselectCandy();
                    FindAllMatches();

                    //StopCoroutine(BoardManager.sharedInstace.FindNullCandies());
                    //StartCoroutine(BoardManager.sharedInstace.FindNullCandies());
                }
                else
                {
                    previousSelected.DeselectCandy();
                    SelectCandy();
                }
            }
        }
    }

    public void SwapSprite(Candy newCandy)
    {
        if(spriteRenderer.sprite == newCandy.GetComponent<SpriteRenderer>().sprite)
        {
            return;
        }

        Sprite oldCandy = newCandy.spriteRenderer.sprite;
        int oldId = newCandy.id;

        newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;
        newCandy.id = this.id;

        this.spriteRenderer.sprite = oldCandy;
        this.id = oldId;
    }

    GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        if(hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    List<GameObject> GetAllNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();

        foreach (Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }

        return neighbors;
    }

    private bool CanSwipe()
    {
        return GetAllNeighbors().Contains(previousSelected.gameObject);
    }

    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingCandies = new List<GameObject>();
        //Consulta de los vecinos en la direccion del parametro
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        while(hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite)
        {
            matchingCandies.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }

        return matchingCandies;
    }

    private bool ClearMatch(Vector2[] directions)
    {
        List<GameObject> matchingCandies = new List<GameObject>();
        foreach(Vector2 direction in directions)
        {
            matchingCandies.AddRange(FindMatch(direction));
        }

        if(matchingCandies.Count >= BoardManager.minCandiesMatch)
        {
            foreach (GameObject candy in matchingCandies)
            {
                candy.GetComponent<SpriteRenderer>().sprite = null;
            }

            //StopCoroutine(BoardManager.sharedInstace.FindNullCandies());
            //StartCoroutine(BoardManager.sharedInstace.FindNullCandies());

            return true;
        }
        else
        {
            return false;
        }
    }

    public void FindAllMatches()
    {
        if(spriteRenderer.sprite == null)
        {
            return;
        }

        bool hMatch = ClearMatch(
            new Vector2[2] {
                Vector2.left, Vector2.right
            }
        );

        bool vMatch = ClearMatch(
            new Vector2[2] {
                Vector2.up, Vector2.down
            }
        );

        if(hMatch || vMatch)
        {
            spriteRenderer.sprite = null;
            StopCoroutine(BoardManager.sharedInstace.FindNullCandies());
            StartCoroutine(BoardManager.sharedInstace.FindNullCandies());
        }
    }
}

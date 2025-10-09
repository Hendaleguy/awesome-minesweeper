using UnityEngine;

public class Cell : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 originalSize;
    private Vector3 originalPosition;

    private MineTextures mineTextures;
    private Renderer _renderer;
    
    public Vector2Int PosInBoard { get; set; }
    public bool IsMine { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }
    public int NumMineNeighbors { get; set; }

    /**
     * For when the click is held down, not released yet
     */
    public void Click()
    {
        DepressTile();
    }
    
    /**
     * For when the click goes away, but not explicitly released
     */
    public void UnClick()
    {
        DepressTile(false);
    }

    /**
     * For when the click is released <br/>
     * What should actually happen on click
     */
    public void ClickRelease()
    {
        DepressTile(false);
    }

    public void HandleFlag()
    {
        if (IsRevealed)
        {
            return;
        }

        _renderer.material.mainTexture = IsFlagged ? mineTextures.blank : mineTextures.flag;
        IsFlagged = !IsFlagged;
    }

    public void AddMine()
    {
        IsMine = true;
    }

    private void DepressTile(bool isShrink = true)
    {
        const float depressFactor = 0.5f;

        if (isShrink)
        {
            transform.localScale = new Vector3
                (originalScale.x, originalScale.y, originalScale.z * depressFactor);
            transform.Translate(0, -(originalSize.z * depressFactor) / 2, 0, Space.World);
        }
        else
        {
            transform.localScale = originalScale;
            transform.localPosition = originalPosition;
        }
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        IsMine = false;
        IsRevealed = false;
        IsFlagged = false;
        
        originalScale = transform.localScale;
        originalSize = GetComponent<MeshRenderer>().localBounds.size;
        originalPosition = transform.localPosition;

        mineTextures = GetComponent<MineTextures>();
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}

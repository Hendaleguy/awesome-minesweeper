using UnityEngine;

public class Cell : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 originalSize;
    private Vector3 originalPosition;

    private bool isDepressed;
    
    private MineTextureManager mineTextureManager;
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
     * For when the click goes away
     */
    public void UnClick()
    {
        DepressTile(false);
    }

    public void HandleFlag()
    {
        if (IsRevealed)
        {
            return;
        }

        if (IsFlagged)
        {
            mineTextureManager.SetBlankTexture();
        }
        else
        {
            mineTextureManager.SetFlagTexture();
        }
        
        IsFlagged = !IsFlagged;
    }

    public void Reveal()
    {
        if (IsMine)
        {
            mineTextureManager.SetMineTexture();
        }
        else
        {
            mineTextureManager.SetNumberTexture(NumMineNeighbors);
        }
        
        IsRevealed = true;
    }

    public void AddMine()
    {
        IsMine = true;
    }
    

    private void DepressTile(bool isShrink = true)
    {
        // if (IsFlagged || (isDepressed && !isShrink))
        if (IsFlagged)
        {
            return;
        }
        
        const float depressFactor = 0.5f;

        if (isShrink)
        {
            transform.localScale = new Vector3
                (originalScale.x, originalScale.y, originalScale.z * depressFactor);
            transform.Translate(0, -(originalSize.z * depressFactor) / 2, 0, Space.World);
            // isDepressed = true;
        }
        else
        {
            transform.localScale = originalScale;
            transform.localPosition = originalPosition;
            // isDepressed = false;
        }
        
    }
    
    private void Awake()
    {
        mineTextureManager = GetComponent<MineTextureManager>();
    }

    private void Start()
    {
        originalScale = transform.localScale;
        originalSize = GetComponent<MeshRenderer>().localBounds.size;
        originalPosition = transform.localPosition;
        
        mineTextureManager.SetBlankTexture();
    }
}

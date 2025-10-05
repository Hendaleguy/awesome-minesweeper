using UnityEngine;

public class Cell : MonoBehaviour
{
    private bool _isMine;
    private bool _isRevealed;
    private int _numMineNeighbors;

    private Vector3 _originalScale;
    private Vector3 _originalSize;
    private Vector3 _originalPosition;
    
    public Vector2Int PosInBoard { get; set; }

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
        Debug.Log("Click released!");
    }

    private void DepressTile(bool isShrink = true)
    {
        const float depressFactor = 0.5f;

        if (isShrink)
        {
            transform.localScale = new Vector3
                (_originalScale.x, _originalScale.y, _originalScale.z * depressFactor);
            transform.Translate(0, -(_originalSize.z * depressFactor) / 2, 0, Space.World);
        }
        else
        {
            transform.localScale = _originalScale;
            transform.localPosition = _originalPosition;
        }
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _originalScale = transform.localScale;
        _originalSize = GetComponent<MeshRenderer>().localBounds.size;
        _originalPosition = transform.localPosition;
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class MineTextureManager : MonoBehaviour
{
    private Renderer rend;

    private MineTextures mineTextures;
    
    private static readonly Dictionary<int, int> NumberToTextureIndex = new Dictionary<int, int>()
    {
        {0, 9},
        {1, 0},
        {2, 1},
        {3, 2},
        {4, 3},
        {5, 4},
        {6, 5},
        {7, 6},
        {8, 7}
    };

    public void SetNumberTexture(int num)
    {
        if (num is < 0 or > 8)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        rend.material.mainTexture = mineTextures.textures[NumberToTextureIndex[num]];
    }

    public void SetMineTexture()
    {
        rend.material.mainTexture = mineTextures.textures[11];
    }

    public void SetBlankTexture()
    {
        rend.material.mainTexture = mineTextures.textures[8];
    }

    public void SetFlagTexture()
    {
        rend.material.mainTexture = mineTextures.textures[10];
    }

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mineTextures = GetComponent<MineTextures>();
    }
}

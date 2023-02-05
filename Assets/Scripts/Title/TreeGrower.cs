using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrower : MonoBehaviour
{
    public SpriteRenderer SR;
    public int MaxSize = 0;
    public List<Sprite> Sprites;


    IEnumerator Start()
    {
        int finalIndex = 4 + 2 * MaxSize;

        for (int i = 0; i < finalIndex; i++)
        {
            SR.sprite = Sprites[i];
            yield return new WaitForSeconds(0.3f);
        }
    }

    
    void Update()
    {
        
    }
}

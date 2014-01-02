using UnityEngine;
using System.Collections;

public class AnimateSprite : MonoBehaviour {
    public int lines, rows;
    public int spriteAmount;
    public Vector2[] offsets;
	// Use this for initialization
	void Start () {
        offsets = new Vector2[spriteAmount];
        float lineFactor = 1f / lines;
        float rowFactor = 1f / rows;
        float currentX = 1f;
        float currentY = 0f;

        for (int i = 0; i < spriteAmount; i++)
        {
            currentX -= rowFactor;
            //if (currentX < 0.1f)
              //  currentX = 0f;

            if (currentX < 0f)
            {
                currentX += 1f;
                currentY += lineFactor;
            }
            
            offsets[i] = new Vector2(currentX, currentY);
           // Debug.Log("x: " + offsets[i].x.ToString() + "y: " + offsets[i].y.ToString());
        }
        
            Animate(1f);
	}



   public void Animate(float progress)
    {
        
        int arrayPos = (int) (progress * (spriteAmount - 1));
        renderer.material.SetTextureOffset("_MainTex", offsets[arrayPos]);

        /*
        float newY = renderer.material.GetTextureOffset("_MainTex").y;
        float newX = ((renderer.material.GetTextureOffset("_MainTex").x + 0.2f) % 1);
        if (newX < 0.1f)
        {
            newY = (renderer.material.GetTextureOffset("_MainTex").y - 0.2f) % 1;
        }

        renderer.material.SetTextureOffset("_MainTex", new Vector2(newX, newY));
         */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextPopUp : MonoBehaviour
{
    private TextMeshPro Text;
    public Rigidbody2D rb;
    public float TravelDistance = 5f;
    public float Duration = 2f;
    // Start is called before the first frame update
    void Awake()
    {
        Text = GetComponent<TextMeshPro>();
     
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (5f - 1) * Time.deltaTime;
        }
    }

    public void SpawnText(Vector2 Pos,string text,Color color)
    {
        Text.DOColor(color, 0);
        Text.DOFade(255, 0); // resets alpha
     

        rb.velocity = new Vector2(0, 0);
        //  Text.DOColor();

        float randX = Random.Range(-0.5f, 0.5f);
        float randY = Random.Range(-0.5f, 0.5f);

        //Pos.x += randX;
        //Pos.y += randY;

        Text.SetText(text);
        transform.position = Pos;
        gameObject.SetActive(true);

        transform.DOShakeScale(0.1f);
        


        rb.AddForce(Vector2.up * 7.5f,ForceMode2D.Impulse);
        rb.AddForce(new Vector2(randX,0) * 2.5f, ForceMode2D.Impulse);


    
  
        Text.DOFade(0, 1).OnComplete(DisableText);
        //  transform.DOMoveY(holder, DurationSplit).OnComplete(MoveDown);   



    }

   

    private void DisableText()
    {
        gameObject.SetActive(false);
    }
   
}

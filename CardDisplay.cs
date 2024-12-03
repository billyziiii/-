using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class CardDisplay : MonoBehaviour
{
    //All card elements
    public Card cardData;

    public Image cardImage;
    public TMP_Text cardname;

    private void Update()
    {
        UpdateCardDisplay();
    }
   public void UpdateCardDisplay()
    {
        cardImage.sprite = cardData.artwork;
        cardname.text = cardData.name;
 
    }      
}

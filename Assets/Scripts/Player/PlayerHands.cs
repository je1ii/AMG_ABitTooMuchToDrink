using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public BeerData rightHand;
    public BeerData leftHand;
    
    public bool IsRightEmpty => rightHand == null;
    public bool IsLeftEmpty => leftHand == null;
    
    public void HoldOnLeft(BeerData beer)
    {
        leftHand = beer;
    }
    
    public void HoldOnRight(BeerData beer)
    {
        rightHand = beer;
    }

    public void RemoveOnLeft()
    {
        leftHand = null;
    }

    public BeerData Throwing()
    {
        BeerData temp = rightHand;
        rightHand = null;
        return temp;
    }
}

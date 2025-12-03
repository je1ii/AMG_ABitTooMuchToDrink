using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public BottleData rightHand;
    public BottleData leftHand;
    
    public bool IsRightEmpty => rightHand == null;
    public bool IsLeftEmpty => leftHand == null;
    
    public void HoldOnLeft(BottleData beer)
    {
        leftHand = beer;
    }
    
    public void HoldOnRight(BottleData beer)
    {
        rightHand = beer;
    }

    public BottleData TakeOnRight() => rightHand;

    public void RemoveOnLeft()
    {
        leftHand = null;
    }

    public BottleData Throwing()
    {
        BottleData temp = rightHand;
        rightHand = null;
        return temp;
    }
}

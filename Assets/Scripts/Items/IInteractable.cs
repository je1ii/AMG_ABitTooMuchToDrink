using UnityEngine;

public interface IInteractable
{
    GameObject GetGameObject();
    void EnableOutline();
    void DisableOutline();

    void Interact(GameObject interactor);
}

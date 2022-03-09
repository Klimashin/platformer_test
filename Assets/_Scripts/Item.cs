using UnityEngine;

public class Item : MonoBehaviour, IPlayerCharacterInteraction
{
    public string ItemTag;
    
    public void Interact(PlayerCharacter character)
    {
        character.Inventory.Add(this);
        gameObject.SetActive(false);
    }
}

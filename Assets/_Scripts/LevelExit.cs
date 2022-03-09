using UnityEngine;

public class LevelExit : MonoBehaviour, IPlayerCharacterInteraction
{
    public string RequiredItemTag;
    
    public void Interact(PlayerCharacter character)
    {
        if (!string.IsNullOrEmpty(RequiredItemTag) &&
            !character.Inventory.Exists(item => item.ItemTag == RequiredItemTag))
        {
            return;
        }
        
        character.Win();
    }
}

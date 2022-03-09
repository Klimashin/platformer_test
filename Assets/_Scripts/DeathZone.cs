using UnityEngine;

public class DeathZone : MonoBehaviour, IPlayerCharacterInteraction
{
    public void Interact(PlayerCharacter character)
    {
        character.Die();
    }
}

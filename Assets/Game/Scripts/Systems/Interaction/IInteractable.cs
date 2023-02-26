namespace Game.Scripts.Interaction
{
    public interface IInteractable
    {
        public void Interact();
        
        public void OnInteractionSuccess();
    }
}
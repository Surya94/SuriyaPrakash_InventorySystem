public interface IInteractable
{
    CollectableData CollectableData { get; set; }
    void OnInteract();
}

namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType CursorType { get; }
        bool HandleRaycast(PlayerController controller);
    }
}
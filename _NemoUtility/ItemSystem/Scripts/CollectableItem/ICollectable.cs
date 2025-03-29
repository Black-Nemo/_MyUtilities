public interface ICollectable
{
    public void TouchEnter(Inventory inventory);
    public void TouchExit(Inventory inventory);
    public void Collect(Inventory inventory);
}

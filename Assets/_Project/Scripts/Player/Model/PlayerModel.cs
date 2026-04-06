public class PlayerModel
{
    public int currentTileIndex { get; private set; }

    public PlayerModel(int startIndex = 0)
    {
        currentTileIndex = startIndex;
    }

    public void MoveTo(int tileIndex)
    {
        currentTileIndex = tileIndex;
    }
}

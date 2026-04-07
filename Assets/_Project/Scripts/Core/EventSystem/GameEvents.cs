using System.Collections.Generic;

public struct OnPlayerMoved
{
    public int tileIndex;
}

public struct OnDiceRolled
{
    public int totalValue;
    public int[] diceValues;
}

public struct OnDiceCountChanged
{
    public int count;
}

public struct OnTileRewardCollected
{
    public TileRewardModel reward;
}

public struct OnInventoryChanged { }

public struct OnPlayerMoveRequested
{
    public int fromIndex;
    public int targetIndex;
    public int steps;
    public int totalTileCount;
}

public struct OnPlayerLanded
{
    public int tileIndex;
}

public struct OnGameInputLocked
{
    public bool isLocked;
}

public struct OnBoardBuilt
{
    public IReadOnlyList<TileModel> tiles;
}

public struct OnDiceAnimationsComplete
{
    public int totalValue;
}

public struct OnDiceHidden
{
    public int totalValue;
}
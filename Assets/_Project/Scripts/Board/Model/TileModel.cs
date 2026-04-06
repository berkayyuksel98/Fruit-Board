public class TileModel
{
    public int index { get; }
    public TileRewardModel reward { get; private set; }
    public bool hasReward => reward != null && !reward.isEmpty;

    public TileModel(int index, TileRewardModel reward)
    {
        this.index = index;
        this.reward = reward;
    }
}

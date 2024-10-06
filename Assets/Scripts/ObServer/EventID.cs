namespace Platformer.Observer
{
    public enum EventID
    {
        None = 0,
        Home,
        Replay,
        GameStartUI,
        StartGame,
        IsPlayGame, 
        Victory,
        Lose,
        EndGame,
        BtnSkipLevel,
        EditMode,
        // Shop
        OpenShop,
        SelectSkin,
        PurchaseSkin,
        // Event Test
        OnCarMove,
    }
}
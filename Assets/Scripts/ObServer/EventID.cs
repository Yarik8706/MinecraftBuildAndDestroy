namespace Platformer.Observer
{
    public enum EventID
    {
        None = 0,
        Home,
        Replay,
        GameStartUI,
        Start,
        IsPlayGame, // true chp tur pin false khong cho rut pin
        Victory,
        Loss,
        EndGame,
        BtnSkipLevel,
        NextLevel,
        // Shop
        OpenShop,
        SelectSkin,
        PurchaseSkin,
        // Event Test
        OnCarMove,
    }
}
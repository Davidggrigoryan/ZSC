using ZombieSoccer.GameLayer.Flow;

namespace ZombieSoccer.ZombieSoccer.Scripts.Models.Messages.MapsWidget
{
    public class AddLocationToMapsWidgetMessage : MapsWidgetMessage
    {
        private IGameLocation _gameLocation;
        
        public AddLocationToMapsWidgetMessage(IGameLocation gameLocation)
        {
            _gameLocation = gameLocation;
        }

        public IGameLocation GetGameLocation()
        {
            return _gameLocation;
        }
    }
}
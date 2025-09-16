using EasyH;

namespace SWEng.Data
{
    public class Coord2DManager : Singleton<Coord2DManager>
    {
        public ICoordinateConvertor2D Convertor { get; private set; }

        protected override void OnCreate()
        {
            Convertor = new HexCoordinateConvertor2D();
        }


    }
}
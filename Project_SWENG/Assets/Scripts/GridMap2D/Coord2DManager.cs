namespace BKTools.Gaming.GridMap2D
{
    public class Coord2DManager
    {
        public ICoordinateConvertor2D Convertor { get; private set; }

        private static Coord2DManager _instance;

        public static Coord2DManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Coord2DManager();
                    _instance.OnCreate();
                }
                return _instance;
            }
        }

        protected void OnCreate()
        {
            Convertor = new HexCoordinateConvertor2D();
        }


    }
}
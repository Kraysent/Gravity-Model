namespace Engine.Models
{
    public class MaterialPoint : BaseNotificationClass
    {
        public const double G = 3.35e-11; /* m^3 * kg^-1 * s^-2 */

        private double _mass;
        private Vector _coordinates;

        public double Mass
        {
            get => _mass;
            set
            {
                _mass = value;
                OnPropetyChanged(nameof(Mass));
            }
        }
        public Vector Coordinates
        {
            get => _coordinates;
            set
            {
                _coordinates = value;
                OnPropetyChanged(nameof(Coordinates));
            }
        }
        public Vector Velocity { get; set; }

        public MaterialPoint()
        {
            Velocity = new Vector(0, 0);
            Coordinates = new Vector(0, 0);
            Mass = 0;
        }

        public double DistanceTo(MaterialPoint b2) => (Coordinates - b2.Coordinates).Length;
    }
}

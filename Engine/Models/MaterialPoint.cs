namespace Engine.Models
{
    public class MaterialPoint
    {
        public const double G = 3.35e-11; /* m^3 * kg^-1 * s^-2 */

        public double Mass { get; set; }
        public Vector Coordinates { get; set; }
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

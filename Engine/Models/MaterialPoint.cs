namespace Engine.Models
{
    public class MaterialPoint
    {
        //I don't know why this does not work with G = 6.67e-11. This value is emperic
        public const double G = 3.35e-11; /* m^3 * kg^-1 * s^-2 */ 

        public double Mass { get; set; }
        public Vector Coordinates { get; set; }
        public Vector Velocity { get; set; }

        public MaterialPoint()
        {
            Coordinates = new Vector(0, 0);
            Mass = 0;
            Velocity = new Vector(0, 0);
        }

        public double DistanceTo(MaterialPoint b2) => (Coordinates - b2.Coordinates).Length;

        public override string ToString()
        {
            return $"Mass: {Mass}, Coordinates: {Coordinates}, Velocity: {Velocity}";
        }
    }
}

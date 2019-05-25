namespace Engine.Models
{
    public class PhysicalBody : MaterialPoint
    {
        public double Diameter { get; set; }

        public PhysicalBody(Vector coordinates, double mass, Vector velocity, double diameter) : base(coordinates, mass, velocity)
        {
            Diameter = diameter;
        }
    }
}

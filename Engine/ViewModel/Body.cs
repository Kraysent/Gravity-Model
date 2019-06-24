namespace Engine.ViewModel
{
    public class Body : BaseNotificationClass
    {
        private double _x;
        private double _y;
        private double _width;
        private double _height;
        private string _fill;
        private string _stroke;

        public double X
        {
            get => _x;
            set
            {
                _x = value;
                RaisePropertyChanged(nameof(X));
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                RaisePropertyChanged(nameof(Y));
            }
        }
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                RaisePropertyChanged(nameof(Width));
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }
        public string Fill //Color in HEX format #FF00FF00
        {
            get => _fill;
            set
            {
                _fill = value;
                RaisePropertyChanged(nameof(Fill));
            }
        }
        public string Stroke //Color in HEX format #FF00FF00
        {
            get => _stroke;
            set
            {
                _stroke = value;
                RaisePropertyChanged(nameof(Stroke));
            }
        }
    }
}

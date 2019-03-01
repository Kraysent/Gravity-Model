using System;
using System.Windows;
using System.Windows.Threading;
using Engine.ViewModel;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private readonly Session _session;
        private DispatcherTimer _timer;
        private const double deltaTime = 0.001;

        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, (int)(deltaTime * 1000));

            _session = new Session(
                new Engine.Models.MaterialPoint() { Coordinates = new Engine.Models.Vector(100, 100), Mass = 1e19, Velocity = new Engine.Models.Vector(2000, 0) },
                new Engine.Models.MaterialPoint() { Coordinates = new Engine.Models.Vector(500, 500), Mass = 1e19, Velocity = new Engine.Models.Vector(-2000, 0) });

            DataContext = _session;
            
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _session.UpdateField(deltaTime);
        }
    }
}

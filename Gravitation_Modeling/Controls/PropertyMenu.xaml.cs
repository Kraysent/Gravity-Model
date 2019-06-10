using Engine.Models;
using System.Collections.Generic;
using System.Windows.Controls;

namespace WPFUI.Controls
{
    public partial class PropertyMenu : UserControl
    {
        private string _title = "";
        private List<Property> _properties = new List<Property>();
        private object _currentObject;

        public event TextChangedEventHandler PropertyUpdated;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                TitleLabel.Content = value;
            }
        }

        public PropertyMenu()
        {
            InitializeComponent();
        }
        
        public void AddItem(string propName, double value, int index)
        {
            Property prop = new Property { Name = propName, Value = value, Index = index };

            _properties.Add(prop);
            PropertyListView.Items.Add(prop);
        }

        public void Open(object obj)
        {
            Clear();
            _currentObject = obj;

            if (obj is MaterialPoint)
            {
                MaterialPoint body = obj as MaterialPoint;

                AddItem("Mass", body.Mass, 0);
                AddItem("X coordinate", body.Coordinates.X, 1);
                AddItem("Y coordinate", body.Coordinates.Y, 2);
                AddItem("X velocity", body.Velocity.X, 3);
                AddItem("Y velocity", body.Velocity.Y, 4);
            }
        }

        private void PropertyTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_currentObject is MaterialPoint)
            {
                MaterialPoint body = _currentObject as MaterialPoint;
                Property prop = (sender as TextBox).DataContext as Property;
                double value = 0;

                if (!double.TryParse((sender as TextBox).Text, out value))
                {
                    return;
                }

                switch (prop.Index)
                {
                    case 0:
                        body.Mass = value;
                        break;
                    case 1:
                        body.Coordinates = new Vector(value, body.Coordinates.Y);
                        break;
                    case 2:
                        body.Coordinates = new Vector(body.Coordinates.X, value);
                        break;
                    case 3:
                        body.Velocity = new Vector(value, body.Velocity.Y);
                        break;
                    case 4:
                        body.Velocity = new Vector(body.Velocity.X, value);
                        break;
                }
            }

            PropertyUpdated(sender, e);
        }

        public void Clear()
        {
            _properties.Clear();
            PropertyListView.Items.Clear();
        }
        
        private class Property
        {
            public string Name { get; set; }
            public double Value { get; set; }
            public int Index { get; set; }
        }
    }
}

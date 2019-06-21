using Engine.Models;
using Engine.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI
{
    public partial class AdjustmentWindow : Window
    {
        Universe _currUniverse;

        public AdjustmentWindow(Universe universe)
        {
            InitializeComponent();

            _currUniverse = universe;
            CollisionsTypeComboBox.Items.Add(CollisionType.NoCollisions);
            CollisionsTypeComboBox.Items.Add(CollisionType.InelasticCollisions);
            CollisionsTypeComboBox.Items.Add(CollisionType.ElasticCollisions);
            CollisionsTypeComboBox.SelectedItem = _currUniverse.CollisionsType;
            TracersCheckBox.IsChecked = _currUniverse.EnableTracers;
            SpeedTextBox.Text = _currUniverse.Speed.ToString();
        }
        
        private void TracersCheckBox_Click(object sender, RoutedEventArgs e)
        {
            _currUniverse.EnableTracers = (bool)TracersCheckBox.IsChecked;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SpeedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int numValue;

            if (!int.TryParse(SpeedTextBox.Text, out numValue))
                SpeedTextBox.Text = numValue.ToString();

            _currUniverse.Speed = numValue;
        }

        private void CollisionsTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem.ToString();
            
            if (!String.IsNullOrEmpty(text))
                _currUniverse.CollisionsType = (CollisionType)Enum.Parse(typeof(CollisionType), text);
        }
    }
}

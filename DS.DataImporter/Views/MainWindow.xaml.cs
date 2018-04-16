using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DS.DataImporter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ComboBox));
            if (dpd != null)
            {
                dpd.AddValueChanged(combo, OnSourceUpdated);
            }
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboItems = ((ListBox)combo.Template.FindName("listBox", combo)).SelectedItems;

            foreach (var column in dataGrid.Columns)
            {
                column.Visibility = comboItems.Contains(column.Header) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnSourceUpdated(object sender, EventArgs e)
        {
            ((ListBox)combo.Template.FindName("listBox", combo)).SelectAll();
            combo.UpdateLayout();
            var listBox = ((ListBox)combo.Template.FindName("listBox", combo));
            listBox.SelectionChanged += ListBox_SelectionChanged;

            var comboTitle = ((ItemsControl)combo.Template.FindName("comboBoxTitle", combo));
            var selectedItems = listBox.SelectedItems.Cast<string>();
            var orderedSelectedItems = selectedItems.OrderBy(arg => listBox.Items.IndexOf(arg)).Select(arg => arg + "; ");
            comboTitle.ItemsSource = orderedSelectedItems;
            tooltipColumnsList.Text = string.Join("", orderedSelectedItems);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboTitle = ((ItemsControl)combo.Template.FindName("comboBoxTitle", combo));
            var listBox = (sender as ListBox);
            var selectedItems = listBox.SelectedItems.Cast<string>();
            var orderedSelectedItems = selectedItems.OrderBy(arg => listBox.Items.IndexOf(arg)).Select(arg => arg + "; ");
            comboTitle.ItemsSource = orderedSelectedItems;
            tooltipColumnsList.Text = string.Join("", orderedSelectedItems);
        }
    }
}

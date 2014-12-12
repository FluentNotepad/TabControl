using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Wpf.Controls;

namespace Test
{
    /// <summary>
    /// Interaction logic for WindowUsingItemsSourceProperty.xaml
    /// </summary>
    public partial class WindowUsingItemsSourceProperty : Window
    {
        private IEnumerable<MyObject> _fixedItems, _dynamicItems;
        private int count = 4;

        public WindowUsingItemsSourceProperty()
        {
            InitializeComponent();

            // when adding new items, you MUST implement the NewTabItem event to provide the object for the content of the tabitem, 
            // otherwise an exception will be thrown
            tabControl.NewTabItem +=
                delegate(object sender, NewTabItemEventArgs e)
                {
                    // return a new MyObject to be used as the content of the new tabItem
                    MyObject myObject = new MyObject { Header = "Tab Item " + count, Value = count };
                    e.Content = myObject;
                    count++;
                };
        }

        /// <summary>
        ///     List that the TabControl's ItemsSource property is bound to
        /// </summary>
        public IEnumerable<MyObject> FixedObjects
        {
            get
            {
                if (_fixedItems == null)
                {
                    _fixedItems = new[]
                                {
                                    new MyObject {Value = 1, Header = "Tab Item 1"},
                                    new MyObject {Value = 2, Header = "Tab Item 2"},
                                    new MyObject {Value = 3, Header = "Tab Item 3"},
                                };
                }

                return _fixedItems;
            }
        }
        /// <summary>
        ///     List that the TabControl's ItemsSource property is bound to
        /// </summary>
        public IEnumerable<MyObject> DynamicObjects
        {
            get
            {
                if (_dynamicItems == null)
                {
                    _dynamicItems = new ObservableCollection<MyObject>
                                {
                                    new MyObject {Value = 1, Header = "Tab Item 1"},
                                    new MyObject {Value = 2, Header = "Tab Item 2"},
                                    new MyObject {Value = 3, Header = "Tab Item 3"},
                                };
                }

                return _dynamicItems;
            }
        }

        private void DynamicRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tabControl != null) 
                tabControl.ItemsSource = DynamicObjects;
        }

        private void FixedRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tabControl != null) 
                tabControl.ItemsSource = FixedObjects;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
                tb.SelectAll();
        }
    }

    /// <summary>
    ///      a simple class to use as the ItemsSource for the tabcontrol
    /// </summary>
    public class MyObject : INotifyPropertyChanged
    {
        private string _header;
        private int _value;

        /// <summary>
        ///     Header Property 
        /// </summary>
        public string Header
        {
            get { return _header; }
            set
            {
                if (_header != value)
                {
                    _header = value;
                    OnPropertyChanged("Header");
                }
            }
        }

        /// <summary>
        ///     Value Property 
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null) propertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
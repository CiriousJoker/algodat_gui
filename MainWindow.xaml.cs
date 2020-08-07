using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

using algodat_gui.Library;

namespace algodat_gui
{
    public partial class MainWindow : Window
    {
        IDictionary _dictionary;
        IDictionary dictionary {
            get { return _dictionary; }
            set {
                _dictionary = value;
                StackPanel_ActionBar.Visibility = Visibility.Visible;
            }
        }

        private DictionaryType _typeCurrent;
        public DictionaryType typeCurrent {
            get { return _typeCurrent; }
            set {
                _typeCurrent = value;
                SetType(value);
            }
        }

        public Dictionary<DictionaryType, string> typesWithCaptions { get; } =
            new Dictionary<DictionaryType, string>()
            {
                    {DictionaryType.DemoImplementation, "DemoImplementation"},
            };

        private SnackbarMessageQueue snackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initialize Snackbar
            SnackbarMessage.MessageQueue = snackbarMessageQueue;

            // Hide Actionbar
            StackPanel_ActionBar.Visibility = Visibility.Collapsed;

            // Focus type combobox
            ComboBox_Type.Focus();
        }

        // Helper
        private int GetInput()
        {
            int myNumber = -1;
            Int32.TryParse(TextBox_Input.Text, out myNumber);
            return myNumber;
        }

        // State management
        private void SetType(DictionaryType type)
        {
            ClearOutput();
            ClearInput();
            switch (type)
            {
                // Array
                case DictionaryType.DemoImplementation:
                    dictionary = new DemoImplementation();
                    break;

                default:
                    ShowMessage("Couldn't change type: Not implemented.");
                    return;
            }

            ShowMessage("Changed type.");
            UpdateOutput();
            TextBox_Input.Focus();
        }

        // UI
        private void ClearOutput()
        {
            TextBox_Output.Clear();
        }

        private void ClearInput()
        {
            TextBox_Input.Clear();
            TextBox_Input.Focus();
        }

        private void UpdateOutput()
        {
            printToConsole(delegate ()
            {
                dictionary.print();
            }, "[\u2193] print()");

            TextBox_Output.ScrollToEnd();
        }

        /// <summary>
        /// Redirects standard output for a given function, adding a prefix for clarity
        /// </summary>
        /// <param name="p">The delegate to execute</param>
        /// <param name="prefix">Will appear in the line above before the real output</param>
        private void printToConsole(Action p, string prefix)
        {
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                p.Invoke();
                if (TextBox_Output.Text.Length > 0)
                {
                    TextBox_Output.Text += "\n";
                }
                TextBox_Output.Text += $"{prefix}\n" + writer.ToString().Trim();
            }
        }

        private void ShowMessage(String text)
        {
            snackbarMessageQueue.Enqueue(text);
        }

        // Validations
        private void ValidateOnlyIntegers(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out int x);
        }

        // Event handlers
        private void Rectangle_DragArea_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            int input = GetInput();
            printToConsole(delegate ()
            {
                bool result = dictionary.insert(input);
                ShowMessage((result ? "Added: " : "Failed to add: ") + TextBox_Input.Text);
                ClearInput();
            }, $"[+] insert({input})");
            UpdateOutput();
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            int input = GetInput();

            printToConsole(delegate ()
            {
                bool result = dictionary.search(input);

                ShowMessage($"{input} was" + (result ? "" : " not") + " in the dictionary.");
                ClearInput();
            }, $"[?] search({input})");
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            int input = GetInput();

            printToConsole(delegate ()
            {
                bool result = dictionary.delete(input);
                if (result)
                {
                    ShowMessage("Deleted: " + TextBox_Input.Text);
                }
                else
                {
                    ShowMessage("Failed to delete: " + TextBox_Input.Text);
                }
            }, $"[-] delete({input})");

            ClearInput();
            UpdateOutput();
        }

        private void Button_Demo_Click(object sender, RoutedEventArgs e)
        {
            Random randNum = new Random();
            int[] randomIntegers = Enumerable
                .Repeat(0, 5)
                .Select(i => randNum.Next(0, 100))
                .ToArray();
            printToConsole(delegate ()
            {
                foreach (int i in randomIntegers)
                {
                    dictionary.insert(i);
                }
            }, $"[-] insert({string.Join(", ", randomIntegers)})");

            string sNumbers = string.Join(",", randomIntegers);
            ShowMessage("Added: " + sNumbers);
            ClearInput();
            UpdateOutput();
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            SetType(typeCurrent);
        }
    }

    public enum DictionaryType
    {
        None,

        DemoImplementation
    }
}

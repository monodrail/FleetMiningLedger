using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FleetMiningLedger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog fleetlog;
        private Dictionary<string, Person> members = new Dictionary<string, Person>();
        private static readonly Regex desiredprecenseformat = new Regex(@"((?:[0-9]{2}:?){3}) - (.+) (joined|left)");
        private static readonly Regex desiredmemberformat = new Regex(@"((?:[0-9]{2}:?){3}) - (.+) (joined)");
        private static readonly Regex desiredlootingformat = new Regex(@"((?:[0-9]{2}:?){3}) (.+) (has looted) (.+) x (.+)(\r)");
        //System.IO.StreamReader sr;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Load_File(object sender, RoutedEventArgs e)
        {
            fleetlog = new OpenFileDialog();
            fleetlog.DefaultExt = ".txt"; // Required file extension 
            fleetlog.Filter = "Text documents (.txt)|*.txt"; // Optional file extensions
            fleetlog.ShowDialog();
            file_location.Content = "Location: " + fleetlog.FileName;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            PersonList.Items.Clear();
            PersonList_Enabled.Items.Clear();
            if (fleetlog != null && fleetlog.FileName != "")
            {
                members = Get_members_joined();
                foreach (KeyValuePair<string, Person> member in members)
                {
                    PersonList.Items.Add(new ListBoxItem() { Content = member.Key });
                }
            }

        }

        private Dictionary<string, Person> Get_members_joined()
        {
            string member = File.ReadAllText(fleetlog.FileName);
            string[] lines = member.Split('\n');
            Dictionary<string, Person> temp_members = new Dictionary<string, Person>();
            foreach (string line in lines)
            {
                Match matchesmembers = desiredmemberformat.Match(line);
                if (matchesmembers.Success)
                {
                    if (!temp_members.ContainsKey(matchesmembers.Groups[2].Value))
                    {
                        Person person = new Person()
                        {
                            name = matchesmembers.Groups[2].Value,
                            role = "default",
                            time_joined = DateTime.ParseExact(matchesmembers.Groups[1].Value, "HH:mm:ss", null).TimeOfDay,
                            time_left = null,
                            mining_dictionary = null
                        };
                        temp_members.Add(matchesmembers.Groups[2].Value, person);
                    }
                }
            }
            return temp_members;
        }

        private void MoveRightBtn_Click(object sender, RoutedEventArgs e)
        {
            Stack<object> selectedStack = new Stack<object>();
            foreach (object o in PersonList.SelectedItems)
            {
                selectedStack.Push(o);
            }
            while (selectedStack.TryPop(out object curr))
            {
                if (curr != null) {
                    PersonList.Items.Remove(curr);
                    PersonList_Enabled.Items.Add(curr);
                }
            }
        }

        private void MoveLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            Stack<object> selectedStack = new Stack<object>();
            foreach (object o in PersonList_Enabled.SelectedItems)
            {
                selectedStack.Push(o);
            }
            while (selectedStack.TryPop(out object curr))
            {
                if (curr != null)
                {
                    PersonList_Enabled.Items.Remove(curr);
                    PersonList.Items.Add(curr);
                }
            }
        }
    }
}

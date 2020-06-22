using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        //List<string[]> lootinglog = new List<string[]>();
        //List<string[]> precenselog = new List<string[]>();
        List<string> members = new List<string>();
        bool filled = false;

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
            filled = true;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (filled)
            {
                //sr = new System.IO.StreamReader(fleetlog.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                //sr.Close();
                //Set_Data();
                //MessageBox.Show("Data loaded");
                Get_members_joined();
            }

        }
        private void Set_Data()
        {
            //string test = File.ReadAllText(fleetlog.FileName);
            //string[] lines = test.Split('\n');
            //foreach(string line in lines)
            //{
            //    Match matchesprecence = desiredprecenseformat.Match(line);
            //    Match matcheslooting = desiredlootingformat.Match(line);
            //    if (matchesprecence.Success)
            //    {
            //        string[] temp_list = new string[3];
            //        temp_list[0] = matchesprecence.Groups[1].Value;
            //        temp_list[1] = matchesprecence.Groups[2].Value;
            //        temp_list[2] = matchesprecence.Groups[3].Value;
            //        precenselog.Add(temp_list);
            //    }

            //    if (matcheslooting.Success)
            //    {
            //        string[] temp_list = new string[5];
            //        temp_list[0] = matcheslooting.Groups[1].Value;
            //        temp_list[1] = matcheslooting.Groups[2].Value;
            //        temp_list[2] = matcheslooting.Groups[3].Value;
            //        temp_list[3] = matcheslooting.Groups[4].Value;
            //        temp_list[4] = matcheslooting.Groups[5].Value;
            //        lootinglog.Add(temp_list);
            //    }
            //}
        }

        private void Get_members_joined()
        {            
            string member = File.ReadAllText(fleetlog.FileName);
            string[] lines = member.Split('\n');
            foreach (string line in lines)
            {
                Match matchesmembers = desiredmemberformat.Match(line);
                if (matchesmembers.Success)
                {
                    string member_to_add = matchesmembers.Groups[2].Value;
                    if (members.Count < 1)
                    {
                        members.Add(member_to_add);
                        combobox.Items.Add(member_to_add);


                    }
                    else if (!members.Contains(member_to_add))
                    {
                        members.Add(member_to_add);
                        combobox.Items.Add(member_to_add);
                    }
                }
            }
            combobox.Visibility = Visibility.Visible;
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace CA2_14_15_exam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rgen = new Random(System.Environment.TickCount);
        public string selectBlood;
        public Ward selWard;
        public Patient selPatient;
        public ObservableCollection<Patient> patInWard = new ObservableCollection<Patient>();
        public static int wardCounter = 0; 
        

        public MainWindow()
        {
            InitializeComponent();
            lbxWard.ItemsSource = storeData.Wards;
            lstPatient.ItemsSource = patInWard;
            createWard();
            populatePatient();
            wardCount.Content = "(" + wardCounter + ")";

        }

        public void populatePatient()
        {
            patInWard.Clear();
            if (selWard != null)
            {
                foreach (Patient ptn in storeData.Patients)
                {
                    if (ptn.pWardName == selWard.WardName)
                    {
                        patInWard.Add(ptn);
                    }
                }
            }
            
        }

        private void createWard()
        {
            storeData.Wards.Add(new Ward("St.Peters Ward", 2/*rgen.Next(1, 10)*/));
            storeData.Wards.Add(new Ward("Eastern Ward", rgen.Next(1, 10)));

            storeData.Patients.Add(new Patient("John Browne", "9/8/1983", BloodType.AB, "St.Peters Ward"));
            storeData.Patients.Add(new Patient("Jesus Christ", "25/12/0000", BloodType.A, "St.Peters Ward"));

            storeData.Patients.Add(new Patient("John Lennon", "20/10/1943", BloodType.B, "Eastern Ward"));
            storeData.Patients.Add(new Patient("Matt Bellamy", "25/12/2000", BloodType.O, "Eastern Ward"));

            wardCounter += 2;
        }

        private void btnAddWard(object sender, RoutedEventArgs e)
        {
            string _Ward = tbxWardName.Text;
            int _limit = Convert.ToInt32(sdrLimit.Value);
            IfNotWard(_Ward, _limit);
        }

        private void btnAddPatient(object sender, RoutedEventArgs e)
        {            
            string _pName = tbxPatientName.Text;
            string _dob = dtpDob.ToString();
            string _wName = selWard.WardName;
            if (patInWard.Count < selWard.Limit)
            {
                BloodType selBType;
                if (selectBlood == "A")
                {
                    selBType = BloodType.A;
                    IfNotPatient(_pName, _dob, selBType, _wName);
                }
                if (selectBlood == "B")
                {
                    selBType = BloodType.B;
                    IfNotPatient(_pName, _dob, selBType, _wName);
                }
                if (selectBlood == "AB")
                {
                    selBType = BloodType.AB;
                    IfNotPatient(_pName, _dob, selBType, _wName);
                }
                if (selectBlood == "O")
                {
                    selBType = BloodType.O;
                    IfNotPatient(_pName, _dob, selBType, _wName);
                }
                populatePatient();
            }
            else MessageBox.Show("Ward Limit Reached");
        }

        public void lbxWard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            selWard = lbxWard.SelectedItem as Ward;
            if (selWard != null) populatePatient();
            
            //{ lstPatient.ItemsSource = selWard.PatInWard; }

        }

        private void lstPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selPatient = lstPatient.SelectedItem as Patient;

            updatePatientImg(selPatient);
        }
               
        public void updatePatientImg(Patient selPatient)
        {
            
            Image myimg = new Image();
            BitmapImage img = new BitmapImage();
            
            
            if (selPatient != null)
            {
                img.BeginInit();
                string testFileExists = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName + "/images/" + (selPatient.bType) + ".png";
                //if (!File.Exists(testFileExists)) fname = "actor";
                var path = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).FullName).FullName + "/images/" + selPatient.bType + ".png";

                img.UriSource = new Uri(path);
                imgBlood.Source = img;
                img.EndInit();
            }

        }

        public static void IfNotWard(string _Ward, int _Limit)
        {
            bool foundWard = false;
            foreach (Ward wrd in storeData.Wards)
            {

                if (_Ward == wrd.WardName)
                {
                    MessageBox.Show("Ward Already Exists");
                    foundWard = true;
                    break;
                }
            }
            if (!foundWard)
            {
                storeData.Wards.Add(new Ward(_Ward, _Limit)); ;
                wardCounter ++;
            }

        }

        public static void IfNotPatient(string _pName, string _dob, BloodType _bld , string _wName)
        {
            bool foundPatient = false;
            foreach (Patient ptn in storeData.Patients)
            {
                if (_pName == ptn.Name && Convert.ToDateTime(_dob) == ptn.Dob && _bld == ptn.bType )
                {
                    MessageBox.Show("Patient Already Exists");
                    foundPatient = true;
                    break;
                }
            }
            if (!foundPatient)
            {
                //new Patient(_pName, _dob, _bld, _wName);
                storeData.Patients.Add(new Patient(_pName, _dob, _bld, _wName));
            };
        }

        public void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            if (button.Content == null) return;
            selectBlood = button.Content.ToString();
        }

        /// <summary>
        ///  Saves The same limit for each ward
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter fileWrite = new StreamWriter("SaveWard.txt"))
            {
                foreach (Ward wrd in storeData.Wards)
                {
                    fileWrite.WriteLine(wrd.WardName + "," + wrd.Limit);
                }

            }
            using (StreamWriter fileWrite = new StreamWriter("SavePatient.txt"))
            {
                foreach (Patient ptn in storeData.Patients)
                {
                    fileWrite.WriteLine(ptn.Name + "," + ptn.Dob + "," + ptn.bType + "," + ptn.pWardName);
                }

            }
            MessageBox.Show("file Written");
        }


        /// <summary>
        /// Load Button freezes program when trying to parse limit on ward
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<Ward> fileWard = new ObservableCollection<Ward>();
            using (StreamReader fileRead = new StreamReader("SaveWard.txt"))
            {
                // stuck in while loop sending same ward to duplicate check
                storeData.Wards.Clear();
                string line = "";
                string[] wardItems;
                while (line != null)
                {

                    wardItems = line.Split(',');
                    IfNotWard(wardItems[0], int.Parse(wardItems[1]));
                    fileRead.ReadLine();
                    //fileWard.Add(new Ward(wardItems[0], int.Parse(wardItems[1])));
                }

                if (fileWard.Count != 0)
                {
                    storeData.Wards.Clear();
                    storeData.Wards = fileWard;
                }
            }

            ObservableCollection<Patient> filePatient = new ObservableCollection<Patient>();
            using (StreamReader fileRead = new StreamReader("SavePatient.txt"))
            {
                string line = fileRead.ReadLine();
                string[] patientItems;
                while (line != null)
                {
                    patientItems = line.Split(',');
                    //(Bloodtype)Enum.Parse(typeof(Bloodtype)
                    IfNotPatient(patientItems[0], patientItems[1], (BloodType)Enum.Parse(typeof(BloodType), patientItems[2]),  patientItems[3]);
                    //fileWard.Add(new Ward(wardItems[0], int.Parse(wardItems[1]), null));
                }

                if (filePatient.Count != 0)
                {
                    storeData.Patients.Clear();
                    storeData.Patients = filePatient;
                }
            }

        }
    }

    public class storeData
    {
        private static ObservableCollection<Ward> wards = new ObservableCollection<Ward>();
        public static ObservableCollection<Ward> Wards { get { return wards; } set { wards = value; } }

        private static ObservableCollection<Patient> patients = new ObservableCollection<Patient>();
        public static ObservableCollection<Patient> Patients { get { return patients; } set { patients = value; } }



    }

}

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
using System.IO;
using System.Security.Cryptography;

namespace Implementare_algoritmii_criptare.net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string normalText;
        byte[] encrypted;
        string roundtrip;
        Algorithm encryptionAlgorithm;
        CipherMode cm;
        PaddingMode pm;
        string SizeString;

        public MainWindow()
        {
            InitializeComponent();
            RulesUpdate();
        }

        /// <summary>
        /// Reads text from file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            this.ReadListBox.Items.Clear();
            normalText = "";

            try
            {
                TextReader tr = new StreamReader(@"../../TextFiles/NormalText.txt");
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    normalText += line + "\n";
                    this.ReadListBox.Items.Add(normalText);
                }
            }
            catch (Exception)
            {
                this.ReadListBox.Items.Add("Error in file reading!");
            }
        }

        /// <summary>
        /// Chose encryption algorithm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string algorithmComboBox = (string)((ComboBoxItem)((ComboBox)sender).SelectedValue).Content;
            encryptionAlgorithm = (Algorithm)Enum.Parse(typeof(Algorithm), algorithmComboBox);
        }

        /// <summary>
        /// retruns the key from textbox
        /// </summary>
        /// <returns></returns>
        private byte[] getKey()
        {
            byte[] key;
            string[] bytes = this.KeyTextBox.Text.Split(' ');
            key = new byte[bytes.Length];
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = byte.Parse(bytes[i]);
            }
            return key;
        }

        /// <summary>
        /// returns block from textbox
        /// </summary>
        /// <returns></returns>
        private byte[] getIV()
        {
            byte[] IV;
            string[] bytes = this.IVTextBox.Text.Split(' ');
            IV = new byte[bytes.Length];
            for (int i = 0; i < IV.Length; i++)
            {
                IV[i] = byte.Parse(bytes[i]);
            }
            return IV;
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            this.EncryptedListBox.Items.Clear();
            try
            {
                ImplementEncryptionAlgorithm();
                this.EncryptedListBox.Items.Add(System.Text.Encoding.ASCII.GetString(encrypted));
            }
            catch (Exception) { }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            this.DecryptedListBox.Items.Clear();
            try
            {
                this.DecryptedListBox.Items.Add(roundtrip);
            }
            catch (Exception) { }
        }

        private void ImplementEncryptionAlgorithm()
        {
            try
            {
                byte[] key = getKey();
                byte[] IV = getIV();
                encrypted = EncryptionAlgorithm.EncryptStringToBytes(normalText, key, IV, cm, pm, encryptionAlgorithm,ref SizeString);
                roundtrip = EncryptionAlgorithm.DecryptStringFromBytes(encrypted, key, IV, cm, pm, encryptionAlgorithm);
                RulesUpdate();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }
        
        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ModeComboBox = (string)((ComboBoxItem)((ComboBox)sender).SelectedValue).Content;
            cm = (CipherMode)Enum.Parse(typeof(CipherMode), ModeComboBox);
        }

        private void PaddingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string PaddingComboBox = (string)((ComboBoxItem)((ComboBox)sender).SelectedValue).Content;
            pm = (PaddingMode)Enum.Parse(typeof(PaddingMode), PaddingComboBox);
        }

        /// <summary>
        /// update rules listbox
        /// </summary>
        private void RulesUpdate()
        {
            this.RulesListBox.Items.Clear();
            this.RulesListBox.Items.Add(SizeString);
        }
    }
}

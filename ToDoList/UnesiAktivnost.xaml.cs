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
using System.Windows.Shapes;

namespace ToDoList
{

    /// <summary>
    /// Interaction logic for UnesiAktivnost.xaml
    /// </summary>
    public partial class UnesiAktivnost : Window
    {
        public string Datum;
        public UnesiAktivnost(string datum)
        {
            InitializeComponent();
            Datum = datum;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.FontSize = this.Height / 20;
        }

        private void btnUnesi_Click(object sender, RoutedEventArgs e)
        {
            if(txtMinuti.Text.Length==1)
            {
                txtMinuti.Text = "0" + txtMinuti.Text;
            }
            string vreme = txtSati.Text + ":" + txtMinuti.Text;
            try
            {
                if(!DataLogic.IspravanDatum(Datum))
                {
                    MessageBox.Show("Neispravan format datuma");
                    return;
                }
                if (!DataLogic.IspravnoVreme(vreme))
                {
                    MessageBox.Show("Neispravan format vremena");
                    return;
                }
                DataLogic.Dodaj(txtNaziv.Text, Datum, vreme);
                MessageBox.Show("Sacuvano");
                txtNaziv.Text = "";
                txtSati.Text = "";
                txtMinuti.Text = "";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

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

namespace ToDoList
{
    public partial class MainWindow : Window
    {

        public List<Zadatak> lista = DataLogic.UcitajPodatke();
        public MainWindow()
        {
            InitializeComponent();
            dtDatum.SelectedDate = DateTime.Now;
            NapuniListBox(dtDatum.ToString().Split(" ")[0]);
        }


        public void NapuniListBox(string datum)
        {
            foreach(var i in LBSadrzaj.Items)
            {
                CheckBox c = (CheckBox)i;
                c.Checked -= new RoutedEventHandler(ZavrsenZadatak);
                c.Unchecked -= new RoutedEventHandler(OtkazanZadatak);
            }
          LBSadrzaj.Items.Clear();
          foreach(var i in DataLogic.NaZadatiDatum(lista, datum))
            {
                CheckBox c = new CheckBox
                {
                    Content = i.Naziv + "| Vreme:" + i.Vreme
                };
                if (i.Zavrsen) c.IsChecked = true;
               c.Checked += new RoutedEventHandler(ZavrsenZadatak);
                c.Unchecked+= new RoutedEventHandler(OtkazanZadatak);
                LBSadrzaj.Items.Add(c);
            }
        }

        private void OtkazanZadatak(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            Zadatak z = lista.Where(x => x.Naziv == c.Content.ToString().Split("|")[0]
                                    && x.Vreme == c.Content.ToString().Split("Vreme:")[1]
                                    && DataLogic.ProveriDatum(x.Datum, dtDatum.ToString().Split(" ")[0])).First();
            z.Zavrsen = false;
            DataLogic.SacuvajPromene(lista);
        }

        private void ZavrsenZadatak(object? sender, EventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            // MessageBox.Show(c.Content.ToString().Split("Vreme:")[1]);
            // MessageBox.Show(dtDatum.ToString().Split(" ")[0]);
            Zadatak z = lista.Where(x => x.Naziv == c.Content.ToString().Split("|")[0]
                                     && x.Vreme == c.Content.ToString().Split("Vreme:")[1]
                                     &&DataLogic.ProveriDatum(x.Datum, dtDatum.ToString().Split(" ")[0])).First();
            z.Zavrsen = true;
            DataLogic.SacuvajPromene(lista);
        }



        /// <summary>
        /// Dodaje novu stavku u ListBox i poziva funkciju koja upisuje u bazu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            string s="";
            UnesiAktivnost nova = new UnesiAktivnost(dtDatum.ToString().Split(" ")[0]);
            nova.ShowDialog();
            lista = DataLogic.UcitajPodatke();
            NapuniListBox(dtDatum.ToString().Split(" ")[0]);
            //TODO optimizuj selektovanje
        }

        private void btnUkloni_Click(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)LBSadrzaj.SelectedItem;
            if (c == null) return;
            Zadatak z = new Zadatak
            {
                Naziv = c.Content.ToString().Split("|")[0],
                Datum = dtDatum.ToString().Split(" ")[0],
                Vreme = c.Content.ToString().Split("|")[1].Split("Vreme:")[1]
            };
            DataLogic.UkloniIzListe(lista,z);
            NapuniListBox(z.Datum);
        }
        private void dtDatum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            NapuniListBox(dtDatum.ToString().Split(" ")[0]);
        }
    }
}

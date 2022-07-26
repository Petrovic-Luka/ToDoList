using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToDoList
{
    public static class DataLogic
    {
        public static string fajl = "TestData.json";
        public static List<Zadatak> UcitajPodatke()
        {
           List<Zadatak> list = new List<Zadatak>();
           if(File.Exists(fajl))
            {
                string[] data = File.ReadAllText(fajl).Split("\n");
                foreach (var i in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Zadatak>(i));
                }
            }
            list.RemoveAll(x => x == null);
            return list;

        }

        public static bool ProveriDatum(string S, string Datum)
        {
            string[] arr = S.Split("/");
            if (arr.Length != 3) return false;
            try
            {
                int dan = int.Parse(Datum.Split("/")[0]);
                int mesec = int.Parse(Datum.Split("/")[1]);
                int godina = int.Parse(Datum.Split("/")[2]);

                int danS = int.Parse(arr[0]);
                int mesecS = int.Parse(arr[1]);
                int godinaS = int.Parse(arr[2]);

                if (dan == danS && mesec == mesecS && godina == godinaS) return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IspravanDatum(string S)
        {
            try
            {
                string[] arr = S.Split("/");
                int dan = int.Parse(arr[0]);
                if (dan < 0 || dan > 31) return false;
                int mesec = int.Parse(arr[1]);
                if(mesec< 0 || mesec >12) return false;
                int godina = int.Parse(arr[2]);
                if (godina < 0) return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool IspravnoVreme(string S)
        {
           try
            {
                string[] arr = S.Split(":");
                if (arr[0].Length > 2) return false;
                int sati = int.Parse(arr[0]);
                if (sati < 0 || sati > 24) return false;
                if (arr[1].Length > 2) return false;
                int minuti = int.Parse(arr[1]);
                if (minuti < 0 || minuti > 60) return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        public static List<Zadatak>NaZadatiDatum(List<Zadatak>lista, string S)
        {
            List<Zadatak> output = new List<Zadatak>();
                foreach (var i in lista)
                {
                    if (ProveriDatum(S, i.Datum)) output.Add(i);
                }
            return output;
        }

        public static void UkloniIzListe(List<Zadatak> lista,Zadatak z)
        {
            lista.RemoveAll(i=>ProveriDatum(i.Datum,z.Datum) && i.Naziv==z.Naziv && i.Vreme==z.Vreme);
            List<string> pomonca = new List<string>();
            foreach(var i in lista)
            {
                var jsonData = JsonConvert.SerializeObject(i);
                pomonca.Add(jsonData);
            }
            System.IO.File.WriteAllLines(fajl, pomonca);
        }

        public static void SacuvajPromene(List<Zadatak>lista)
        {
            List<string> pomocna = new List<string>();
            foreach (var i in lista)
            {
                var jsonData = JsonConvert.SerializeObject(i);
                pomocna.Add(jsonData);
            }
            System.IO.File.WriteAllLines(fajl, pomocna);
        }

        public static void Dodaj(string naziv,string datum,string vreme)
        {
            Zadatak z = new Zadatak
            { Naziv=naziv,
              Datum=datum,
              Vreme=vreme
            };
           List<string> pomocni = new();
           pomocni.Add(JsonConvert.SerializeObject(z));
           System.IO.File.AppendAllLines("TestData.json", pomocni);

        }


    }
}

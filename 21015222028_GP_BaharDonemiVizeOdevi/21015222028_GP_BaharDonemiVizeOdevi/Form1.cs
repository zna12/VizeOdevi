using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace _21015222028_GP_BaharDonemiVizeOdevi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (File.Exists(temp))
            {
                String jsondata = File.ReadAllText(temp);
                araclar = JsonSerializer.Deserialize<List<arac>>(jsondata);
            }

            ShowList();
        }

        List<arac> araclar = new List<arac>()
        {
            new arac()
            {
                Plaka = "34 kb 344",
                Marka = "Honda",
                Model = "Civi sedan",
                Yakıt = "1.5L VTEC Turbo ECO",
                Renk = "Beyaz",
                Vites = "Otomatik",
                KasaTipi = "Sedan",
                Acıklama = "ikinci el durumu : 0 araç.",
                PurchaseDate = new DateTime(2023, 1, 12),
            },
            new arac() 
            {   Plaka = "58 sv 588",
                Marka = "Land Rover",
                Model = "Defender",
                Yakıt = "",
                Renk = "Yeşil",
                Vites = "otomatik",
                KasaTipi = "Suv",
                Acıklama = "ikinci el duurmu: 2. el, hasarsız.",
                PurchaseDate = new DateTime(2021, 6,20),
            }
        };

        public object StreamWrite { get; private set; }

        // ShowList() tüm listeyi gösterecek
        public void ShowList() 
        {
            listView1.Items.Clear();
            foreach (arac arac in araclar)
            {
                AddList(arac);
            }
        }

        //AddList listeye bir araç ekleyecek 
        public void AddList(arac arac)
        {
            ListViewItem item = new ListViewItem(new string[]
            {
                arac.Plaka,
                arac.Marka,
                arac.Model,
                arac.Yakıt,
                arac.Renk,
                arac.Vites,
                arac.KasaTipi,
                arac.Acıklama,
                arac.PurchaseDate.ToString(),
            });
            item.Tag = arac;
            listView1.Items.Add(item);
            
        }

        void EditArac(ListViewItem AItem, arac arac)
        {
            AItem.SubItems[0].Text = arac.Plaka;
            AItem.SubItems[1].Text = arac.Marka;
            AItem.SubItems[2].Text = arac.Model;
            AItem.SubItems[3].Text = arac.Yakıt;
            AItem.SubItems[4].Text = arac.Renk;
            AItem.SubItems[5].Text = arac.Vites;
            AItem.SubItems[6].Text = arac.KasaTipi;
            AItem.SubItems[7].Text = arac.Acıklama;
            AItem.SubItems[8].Text = arac.PurchaseDate.ToString();  
            AItem.Tag = arac;
        }
        private void AddCommand(object sender, EventArgs e)
        {
            frmAraba frm = new frmAraba()
            { 
              Text = "Araba Ekle",
              StartPosition = FormStartPosition.CenterParent,
              arac = new arac()
            };  
            if (frm.ShowDialog() == DialogResult.OK)
            {
                araclar.Add(frm.arac);
                AddList(frm.arac);
            }
        }

        private void EditCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            
            ListViewItem AItem = listView1.SelectedItems[0];
            
            arac secili = AItem.Tag as arac;
            
            
            frmAraba frm = new frmAraba()
            {
                Text = "Araba Kaydı Düzenle",
                StartPosition = FormStartPosition.CenterParent,

                arac = Clone (secili),
            };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.arac;
                EditArac(AItem, secili);
            }
        }

        arac Clone (arac arac)
        {
            return new arac()
            {
                id = arac.ID,
                Plaka = arac.Plaka,
                Marka = arac.Marka,
                Model = arac.Model,
                Yakıt = arac.Yakıt,
                Renk = arac.Renk,
                Vites = arac.Vites,
                KasaTipi = arac.KasaTipi,
                Acıklama = arac.Acıklama,
            };
        }

        private void DeleteCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem AItem = listView1.SelectedItems[0];
            arac secili = AItem.Tag as arac;

           var sonuc = MessageBox.Show($" seçili araba silinsin mi? \n\n{secili.Plaka} {secili.Marka} ", "Silmeyi onayla", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
           
            if(sonuc == DialogResult.Yes)
            {
                araclar.Remove(secili);
                listView1.Items.Remove(AItem);

            }
        }

        private void SaveCommand(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formatı|*.Json|Xml Formatı|*.xml"
            };

            if (sf.ShowDialog() == DialogResult.OK)
            {
                if (sf.FileName.EndsWith("json"))
                {
                    String data = JsonSerializer.Serialize(araclar);
                    File.WriteAllText(sf.FileName, data);  
                }
                else if(sf.FileName.EndsWith ("xml"))
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);    
                    XmlSerializer serializer = new XmlSerializer(typeof(List<arac>));
                    serializer.Serialize(sw, araclar);
                    sw.Close();
                }
            }
        }

        private void LoadCommand(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json, Xml Formatı|*.jso;*.xml",
            };
            if(of.ShowDialog() == DialogResult.OK)
            {
                if (of.FileName.ToLower().EndsWith("json"))
                {
                    String jsondata = File.ReadAllText(of.FileName);
                    araclar = JsonSerializer.Deserialize<List<arac>>(jsondata);
                }
                else if (of.FileName.ToLower().EndsWith("xml"))
                {
                   StreamReader sr = new StreamReader(of.FileName);
                   XmlSerializer serializer = new XmlSerializer (typeof(List<arac>));
                   araclar = (List<arac>) serializer.Deserialize(sr);
                   sr.Close();
                }

                ShowList();
            }
        }

        string temp = Path.Combine(Application.CommonAppDataPath, "data");
        protected override void OnClosing(CancelEventArgs e)
        {
            String data = JsonSerializer.Serialize(araclar);
            File.WriteAllText(temp, data);
            
            base.OnClosing(e);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        
    }

    [Serializable]
    public class arac
    {
        public string id;

        [Browsable(false)]
        public string ID
        {
            get
            {
                if (id == null)
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set { id = value; }
        }
        public string Plaka {get; set;}
        public string Marka {get; set;}
        public string Model {get; set;}
        public string Yakıt {get; set;}
        public string Renk {get; set;}
        public string Vites {get; set;}
        public string KasaTipi {get; set;}
        public string Acıklama {get; set;}

        public DateTime PurchaseDate { get; set;}   
    }


}

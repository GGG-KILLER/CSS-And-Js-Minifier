using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Web;

namespace CSSAndJsMinifier
{
    public partial class Form1 : Form
    {
        string[] js, css;
        string folder;
        public Form1()
        {
            InitializeComponent();
            label5.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderSelectDialog fsd = new FolderSelectDialog();
            fsd.ShowDialog();
            folder = fsd.FileName;
            if(!Directory.Exists(folder))
            {
                MessageBox.Show("Directory doesn't exists!", "Error!");
                folder = null;
                return;
            }
            label2.Text = folder;
            SearchCSSAndJS(folder,false);
        }

        private async void MinifyCSS(string filename)
        {
            SetStatus("Minifying " + filename.Replace(folder, "").Replace(".css", ".min.css") + " ...", 50);
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>{{ "input", File.ReadAllText(filename) }};

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://cssminifier.com/raw", content);

                var responseString = await response.Content.ReadAsStringAsync();

                File.WriteAllText(filename.Replace(".css", ".min.css"), responseString);
                
                SetStatus("Minifying and compressing of " + filename.Replace(folder, "").Replace(".css", ".min.css") + " done!", 100);
            }
        }

        private async void MinifyJS(string filename)
        {
            using (var client = new HttpClient())
            {
                SetStatus("Minifying " + filename.Replace(folder, "").Replace(".js", ".min.js") + " ...", 50);
                var values = new Dictionary<string, string> { { "input", File.ReadAllText(filename) } };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://javascript-minifier.com/raw", content);

                var responseString = await response.Content.ReadAsStringAsync();

                File.WriteAllText(filename.Replace(".js", ".min.js"), responseString);
                
                SetStatus("Minifying and compressing of " + filename.Replace(folder, "").Replace(".js", ".min.js") + " done!", 100);
            }
        }

        private void SearchCSSAndJS(string path, bool minify)
        {
            js = Directory.GetFiles(path, "*.js", SearchOption.AllDirectories);
            css = Directory.GetFiles(path, "*.css", SearchOption.AllDirectories);
            foreach (string file in js)
            {
                if (file.IndexOf(".min.js") > -1) continue;

                if (minify)
                {
                    SetStatus("Minifying " + Path.GetFileName(file) + " ...", 0);
                    MinifyJS(file);
                }
                else
                {
                    listBox1.Items.Add(file.Replace(folder, "")); ;
                }
            }

            foreach (string file in css)
            {
                if (file.IndexOf(".min.css") > -1) continue;

                if (minify)
                {
                    SetStatus("Minifying " + Path.GetFileName(file) + " ...", 0);
                    MinifyCSS(file);
                }
                else
                {
                    listBox1.Items.Add(file.Replace(folder,""));;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!(folder is string))
            {
                MessageBox.Show("No folder selected or unexisting folder selected!");
                return;
            }
            SearchCSSAndJS(folder, true);
        }

        private void SetStatus(string stats, int progress)
        {
            label5.Text = stats;
            progressBar1.Value = progress;
        }
    }
}

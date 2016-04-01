using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Yahoo.Yui.Compressor;

namespace CSSAndJsMinifier
{
    public partial class MainForm : Form
    {
        private string[] js, css;
        private string folder;
        private readonly CssCompressor cssCompressor;
        private readonly JavaScriptCompressor jsCompressor;
        private int fileCount = 0, processedFiles = 0;

        public MainForm ( )
        {
            InitializeComponent ( );
            label5.Text = "";

            cssCompressor = new CssCompressor
            {
                RemoveComments = true,
                LineBreakPosition = 8000,
                CompressionType = CompressionType.Standard
            };

            jsCompressor = new JavaScriptCompressor
            {
                CompressionType = CompressionType.Standard,
                Encoding = System.Text.Encoding.UTF8,
                DisableOptimizations = false,
                LineBreakPosition = 8000
            };

            // Styling stuff
            this.FormatStyles ( );
        }

        private async void MinifyCSSAsync ( string filePath )
        {
            string fileName = Path.GetFileName ( filePath );
            string outputFile = filePath.Replace ( ".css", ".min.css" );

            SetStatus ( $"Starting to read \"{fileName}\"...", 0 );

            // Gets the StreamReader to get the file contents
            using ( StreamReader cssReader = File.OpenText ( filePath ) )
            {
                // Save the contents to a string
                string css = await cssReader.ReadToEndAsync ( );

                SetStatus ( $"Read {fileName}, starting to compress...", 33 );

                // Compresses the CSS
                css = cssCompressor.Compress ( css );

                SetStatus ( $"Compressed {fileName}, writing out...", 66 );
                // Creates a StreamWriter to write the contents to the new file
                using ( StreamWriter cssWriter = File.CreateText ( outputFile ) )
                {
                    // Writes the minified CSS to the <filename without extension>.min.css
                    cssWriter.Write ( css );

                    // Flushes the written contents
                    await cssWriter.FlushAsync ( );

                    // Closes the stream
                    cssWriter.Close ( );
                }
                // Using automatically disposes of it.

                // Closes the reader stream
                cssReader.Close ( );
            }
            // Using automatically disposes of it

            SetStatus ( $"Finished compressing {fileName}.", 100 );
            processedFiles++;

            if ( processedFiles >= fileCount )
                SetStatus ( "Finised.", 100 );
        }

        private async void MinifyJSAsync ( string filePath )
        {
            string fileName = Path.GetFileName ( filePath );
            string outputFile = filePath.Replace ( ".js", ".min.js" );
            SetStatus ( $"Starting to read \"{fileName}\"...", 0 );

            // Gets the StreamReader to get the file contents
            using ( StreamReader jsReader = File.OpenText ( filePath ) )
            {
                // Save the contents to a string
                string js = await jsReader.ReadToEndAsync ( );

                SetStatus ( $"Read {fileName}, starting to compress...", 33 );

                // Compresses the Js
                js = jsCompressor.Compress ( js );

                SetStatus ( $"Compressed {fileName}, writing out...", 66 );

                // Creates a StreamWriter to write the contents to the new file
                using ( StreamWriter jsWriter = File.CreateText ( outputFile ) )
                {
                    // Writes the minified CSS to the <filename without extension>.min.js
                    jsWriter.Write ( js );

                    // Flushes the written contents
                    await jsWriter.FlushAsync ( );

                    // Closes the stream
                    jsWriter.Close ( );
                }
                // Using automatically disposes of it.

                // Closes the reader stream
                jsReader.Close ( );
            }
            // Using automatically disposes of it

            SetStatus ( $"Finished compressing {fileName}.", 100 );
            processedFiles++;

            if ( processedFiles >= fileCount )
                SetStatus ( "Finised.", 100 );
        }

        private void SearchCSSAndJS ( string path, bool minify )
        {
            if ( !minify )
            {
                js = Directory.GetFiles ( path, "*.js", SearchOption.AllDirectories );
                css = Directory.GetFiles ( path, "*.css", SearchOption.AllDirectories );
            }

            foreach ( string file in js )
            {
                if ( file.IndexOf ( ".min.js" ) > -1 )
                    continue;

                if ( minify )
                {
                    SetStatus ( "Minifying " + Path.GetFileName ( file ) + " ...", 0 );
                    MinifyJSAsync ( file );
                }
                else
                {
                    listBox1.Items.Add ( file.Replace ( folder, "" ) );
                    fileCount++;
                }
            }

            foreach ( string file in css )
            {
                if ( file.IndexOf ( ".min.css" ) > -1 )
                    continue;

                if ( minify )
                {
                    SetStatus ( "Minifying " + Path.GetFileName ( file ) + " ...", 0 );
                    MinifyCSSAsync ( file );
                }
                else
                {
                    listBox1.Items.Add ( file.Replace ( folder, "" ) );
                    fileCount++;
                }
            }

            if ( minify )
                SetStatus ( "Finished minifying files.", 100 );
        }

        private void button1_Click ( object sender, EventArgs e )
        {
            FolderSelectDialog fsd = new FolderSelectDialog ( );
            fsd.ShowDialog ( );
            folder = fsd.FileName;
            if ( !Directory.Exists ( folder ) )
            {
                MessageBox.Show ( "Directory doesn't exists!", "Error!" );
                folder = null;
                return;
            }

            label2.Text = folder;

            fileCount = 0;
            SearchCSSAndJS ( folder, false );
        }

        private void button2_Click ( object sender, EventArgs e )
        {
            if ( !( folder is string ) )
            {
                progressBar1.ForeColor = System.Drawing.Color.Red;
                SetStatus ( "No folder selected or unexisting folder selected!", 100 );
                MessageBox.Show ( "No folder selected or unexisting folder selected!" );
                return;
            }

            processedFiles = 0;
            SearchCSSAndJS ( folder, true );
        }

        private void SetStatus ( string stats, int progress )
        {
            this.InvokeEx ( form =>
            {
                form.label5.Text = stats + $"({processedFiles}/{fileCount})";
                form.progressBar1.Value = progress;
            } );
        }
    }

    // Class to make invoking easier
    public static class ISynchronizeInvokeExtensions
    {
        public static void InvokeEx<T> ( this T @this, Action<T> action ) where T : ISynchronizeInvoke
        {
            if ( @this.InvokeRequired )
            {
                try
                {
                    @this.Invoke ( action, new object[] { @this } );
                }
                catch ( Exception )
                {
                    throw;
                }
            }
            else
            {
                action?.Invoke ( @this );
            }
        }

        public static void FormatStyles ( this Form @this )
        {
            foreach ( Control c in @this.Controls )
            {
                if ( c is Button || c is TextBox || c is GroupBox || c is Label )
                {
                    c.FormatStyles ( );

                    foreach ( Control subc in c.Controls )
                    {
                        subc.FormatStyles ( );
                    }
                }
            }
        }

        public static void FormatStyles ( this Control c )
        {
            if ( c is Button )
                ( (Button) c ).FlatStyle = FlatStyle.Flat;

            if ( c is TextBox )
                ( (TextBox) c ).BorderStyle = BorderStyle.FixedSingle;

            if ( c is GroupBox )
                ( (GroupBox) c ).FlatStyle = FlatStyle.Flat;

            if ( c is Label )
                ( (Label) c ).FlatStyle = FlatStyle.Flat;
        }
    }
}
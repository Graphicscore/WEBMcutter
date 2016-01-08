using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Pr0tastischer_webM_Cutter
{
    public partial class MainWindow : Window
    {

        #region Variablen
        long lmaxLaufzeitInMilliSek = 0;
        string strInputFile = "";
        string strOutputFile = "";
        string strVorschaubild = "Vorschau.jpg";
        string strVorschauPfad = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\pwc\\" ;
        Zeitpunkt lastZP = new Zeitpunkt();
        string strEinstellungsDatei = System.AppDomain.CurrentDomain.BaseDirectory + "pwc.set";
        Einstellungen E = new Einstellungen();
        List<string> lsFfmpegLog = new List<string>();
        BitmapImage image;
        Vorlage tmpvorlage;
        int tmpcounter = 0;
        int vorgangid = 0;
        double exportStart, exportLaenge;

        readonly List<string> lsRequiredFiles = new List<string> { "ffmpeg.exe", "ffprobe.exe" };
        readonly List<string> lsWichtigeHinweise = new List<string> {
            "Suche Steine...",
            "Stelle Proxy-Verbindung her...",
            "Banne Faggots...",
            "Erstelle 9fag Account...",
            "Lade Pr0n...",
            "MAMA!",
            "Schalte Licht aus...",
            "Lade schlechtes Kommentar...",
            "Prognostiziere Hodenkrebs...",
            "99.999% encoded...",
            "クイーン!",
            "Starte Google-Translater um Beleidigungen zu übersetzen...",
            "โง่!",
            "באַרען איר!"
        };
        #endregion


        public MainWindow()
        {
            InitializeComponent();

            //Den Vorschaubilder-Ordner erstelleun und alte Vorschaubilder löschen
            System.IO.Directory.CreateDirectory(strVorschauPfad);
            try {
                foreach (string file in System.IO.Directory.GetFiles(strVorschauPfad))
                {
                    File.Delete(file);
                }
            }
            catch  { }
            
            //Prüfen ob die benötigten ffmpeg Dateien da sind
            foreach (string s in lsRequiredFiles) {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + s) == false) {
                    MessageBox.Show("Eine benötigte Datei wurde nicht gefunden: " + s + Environment.NewLine + "Bitte lade die aktuelle FFMPEG Version herunter und platziere die fehlende Anwendung in diesem Ordner.", "Fehlende Dateien", MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Environment.Exit(1);
                }
            }

            //Titel laden
            System.Reflection.Assembly me = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvInformation = FileVersionInfo.GetVersionInfo(me.Location);
            this.Title = fvInformation.ProductName + " v." + fvInformation.FileVersion;

            //Prüfen ob es Einstellungen gibt und wenn möglich laden
            try
            {
                if (File.Exists(strEinstellungsDatei)) {
                    XmlSerializer xmls = new XmlSerializer(typeof(Einstellungen));
                    StringReader strr = new StringReader(File.ReadAllText(strEinstellungsDatei));
                    this.E = (Einstellungen)(xmls.Deserialize(strr));
                    strr.Close();
                }

            }
            catch {
                MessageBox.Show("Die Einstellungen konnten nicht geladen werden!", "Ungültige Datei", MessageBoxButton.OK, MessageBoxImage.Error);
                this.E = new Einstellungen();
            }

            LadeVorlagen();
            OeffneDatei();
        }

        #region Funktionen
        public void CreateExport() {
            Zeitpunkt zp = new Zeitpunkt();
            zp.setAll(sstartzeit.Value);
            lastZP = zp;
            if (CheckIfInFrame() == false) {
                MessageBox.Show("Der Ausgewählte Zeitraum ist ungültig.", "Ungültiger Zeitraum", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            zp.setAll(sstartzeit.Value + slaenge.Value);
            lastZP = zp;
            if (CheckIfInFrame() == false)
            {
                MessageBox.Show("Der Ausgewählte Zeitraum ist ungültig.", "Ungültiger Zeitraum", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            exportStart = sstartzeit.Value;
            exportLaenge = slaenge.Value;

            setTempVorlage();

            gridContent.IsEnabled = false;
            gridContentBlurEffekt.Radius = 10;
            gridLoading.Visibility = Visibility.Visible;
            //labLadeInfo
            Random r = new Random();
            labLadeInfo.Content = "[ " + lsWichtigeHinweise.ElementAt(r.Next(lsWichtigeHinweise.Count - 1)) + " ]";


            BackgroundWorker b = new BackgroundWorker();
            b.DoWork += new DoWorkEventHandler(CreateExportWorker);
            b.RunWorkerAsync();
        }

        public void LadeVorlagen() {
            comVorlage.Items.Clear();
            foreach (Vorlage item in this.E.lsVorlagen) {
                comVorlage.Items.Add(item.strName);
            }
        }

        public void OeffneDatei() {
            //Alles auf 0 was so geht
            lastZP = new Zeitpunkt();
            sstartzeit.Value = 0;
            slaenge.Value = 0;
            imgVorschau.Source = null;
            image = null;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Bitte wähle die gewünschte Datei";
            Nullable<bool> r = ofd.ShowDialog();
            if (r == true)
            {
                strInputFile = ofd.FileName;
                BackgroundWorker ba = new BackgroundWorker();
                ba.DoWork += new DoWorkEventHandler(HandlerLoadLength);
                ba.RunWorkerAsync();
                mainGrid.IsEnabled = false;
            }
            else {
                MessageBox.Show("Keine Datei ausgewählt. DAU-Modus aktiviert. Das Programm wird beendet!", "Ungültige Datei", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(1);
            }
        }

        public void GenerateVorschauName() {
            string zp = "Ungültig";
            if (CheckIfInFrame() == true) {
                zp = lastZP.ToString();
            }
            string tmpimg = "";
            try { tmpimg = image.Width.ToString() + "x" + image.Height.ToString() + ", "; } catch { }

            labInfVorschau.Content = zp + ", " + tmpimg + Convert.ToInt32(imgVorschau.ActualWidth) + "x" + Convert.ToInt32(imgVorschau.ActualHeight);
        }

        private bool isTemplateOld() {
            if (tmpvorlage == null) { return false; }

            if (tmpvorlage.iqmin != Convert.ToInt32(sqmin.Value) ||
                tmpvorlage.iqmax != Convert.ToInt32(sqmax.Value) ||
                tmpvorlage.icrf != Convert.ToInt32(scrf.Value) ||
                tmpvorlage.ibreite != Convert.ToInt32(sbreite.Value) ||
                tmpvorlage.ihoehe != Convert.ToInt32(shoehe.Value) ||
                tmpvorlage.icropbreite != Convert.ToInt32(scropbreite.Value) ||
                tmpvorlage.icrophoehe != Convert.ToInt32(scrophoehe.Value) ||
                tmpvorlage.icropx != Convert.ToInt32(scropx.Value) ||
                tmpvorlage.icropy != Convert.ToInt32(scropy.Value)
                ) {
                return true;
            }
            return false;
        }

        private void GenerateVorschau(bool von) {
            if (this.IsInitialized == false) { return; }

            Zeitpunkt zp = new Zeitpunkt();
            if (von)
            {
                zp.setAll(sstartzeit.Value);
            }
            else {
                zp.setAll(sstartzeit.Value + slaenge.Value);
            }

            lastZP = zp;
            GenerateVorschauName();
                        
            if (tmpvorlage == null || isTemplateOld()) {
                setTempVorlage();
            }

            if (CheckIfInFrame() == false) {
                return;
            }

            if (File.Exists(strVorschauPfad + tmpvorlage.strName + lastZP.pathString() + strVorschaubild))
            {
                setImage();
            }
            else {                
                BackgroundWorker ba = new BackgroundWorker();
                ba.DoWork += new DoWorkEventHandler(CreateNewVorschau);
                ba.RunWorkerAsync();
            }            
        }

        private void setTempVorlage() {
            tmpvorlage = new Vorlage("tmp" + tmpcounter, Convert.ToInt32(sqmin.Value), Convert.ToInt32(sqmax.Value), Convert.ToInt32(scrf.Value), Convert.ToInt32(sbreite.Value), Convert.ToInt32(shoehe.Value), Convert.ToInt32(scropbreite.Value), Convert.ToInt32(scrophoehe.Value), Convert.ToInt32(scropx.Value), Convert.ToInt32(scropy.Value), Convert.ToInt32(sMaxFileSize.Value), cbAudioAus.IsChecked.Value);
            tmpcounter += 1;
        }

        private void setImage() {
            if (File.Exists(strVorschauPfad + tmpvorlage.strName + lastZP.pathString() + strVorschaubild))
            {
                try
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(strVorschauPfad + tmpvorlage.strName + lastZP.pathString() + strVorschaubild);
                    image.EndInit();
                    imgVorschau.Source = image;
                }
                catch { }
                
            }
        }

        private bool CheckIfInFrame() {
            return (lastZP.getMilliseconds() <= this.lmaxLaufzeitInMilliSek);
        }

        public string PfadAufraeumen(string Eingabe) {
            foreach (char ungueltigerchar in System.IO.Path.GetInvalidFileNameChars()) {
                Eingabe = Eingabe.Replace(ungueltigerchar+"", "");
            }
            return Eingabe;
        }

        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        private void setmaxLaenge()
        {
            if (sstartzeit.Value + slaenge.Maximum > lmaxLaufzeitInMilliSek)
            {
                slaenge.Maximum = lmaxLaufzeitInMilliSek - sstartzeit.Value;
            }
            else {
                slaenge.Maximum = 120000; //2 Minuten
            }

        }
        private void setQualitätsBereiche() {
            if (this.IsInitialized == false) { return; }
            scrf.Maximum = sqmax.Value;
            scrf.Minimum = sqmin.Value;

            sqmax.Minimum = sqmin.Value;

            labInfQMin.Content = Convert.ToInt32( sqmin.Value);
            labInfQMax.Content = Convert.ToInt32(sqmax.Value);
            labInfCRF.Content = Convert.ToInt32(scrf.Value);            
        }

        private void setSkalierungsBereiche() {
            if (this.IsInitialized == false) { return; }
            if (sbreite.Value == -1 && shoehe.Value == -1) {
                sbreite.Value = 720; //Idiotenshutz. Yeah!
            }

            labInfBreite.Content = Convert.ToInt32(sbreite.Value);
            labInfHoehe.Content = Convert.ToInt32(shoehe.Value);
        }

        private void setCropBereiche() {
            labInfCropBreite.Content = Convert.ToInt32(scropbreite.Value);
            labInfCropHoehe.Content = Convert.ToInt32(scrophoehe.Value);
            labInfCropX.Content = Convert.ToInt32(scropx.Value);
            labInfCropY.Content = Convert.ToInt32(scropy.Value);
        }
        #endregion

        #region BG Handler
        private void HandlerLoadLength(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "ffprobe.exe";
                p.StartInfo.Arguments = "-i \"" + strInputFile + "\" -show_entries format=duration -v quiet -of csv=\"p=0\"";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                p.Start();

                p.WaitForExit();

                string s = p.StandardOutput.ReadToEnd();
                if (IsNumeric(s))
                {
                    this.Dispatcher.BeginInvoke(new Action(() => lmaxLaufzeitInMilliSek = Convert.ToInt64(Convert.ToDecimal(s.Replace(".",","))*1000)));
                    this.Dispatcher.BeginInvoke(new Action(() => sstartzeit.Maximum = lmaxLaufzeitInMilliSek));
                    this.Dispatcher.BeginInvoke(new Action(() => mainGrid.IsEnabled = true));
                }
                else {
                    this.Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("ffprobe gab einen ungültigen Wert zurück.")));
                    this.Dispatcher.BeginInvoke(new Action(() => System.Environment.Exit(1)));
                }
            }
            catch { }
        }

        private void CreateNewVorschau(object sender, EventArgs e) {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "ffmpeg.exe";
                if (tmpvorlage.icropbreite > 0 && tmpvorlage.icrophoehe > 0)
                {
                    p.StartInfo.Arguments = "-ss " + lastZP.ToString() + " -i \"" + strInputFile + "\" -crf " + tmpvorlage.icrf + " -qmin " +
                    tmpvorlage.iqmin + " -qmax " + tmpvorlage.iqmax + "  -vf \"[in]scale=" + tmpvorlage.ibreite + ":" +
                    tmpvorlage.ihoehe + "[middle];[middle]crop=" + tmpvorlage.icropbreite + ":" + tmpvorlage.icrophoehe + ":" +
                    tmpvorlage.icropx + ":" + tmpvorlage.icropy + "[out]\" -vframes 1 -y \"" + strVorschauPfad + tmpvorlage.strName + lastZP.pathString() + strVorschaubild + "\"";
                }
                else {
                    p.StartInfo.Arguments = "-ss " + lastZP.ToString() + " -i \"" + strInputFile + "\" -crf " + tmpvorlage.icrf + " -qmin " +
                    tmpvorlage.iqmin + " -qmax " + tmpvorlage.iqmax  + " -vf \"scale=" + tmpvorlage.ibreite + ":" +
                    tmpvorlage.ihoehe + "\" -vframes 1 -y \"" + strVorschauPfad + tmpvorlage.strName + lastZP.pathString() + strVorschaubild + "\"";
                }                
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                p.Start();

                p.WaitForExit();
                this.Dispatcher.BeginInvoke(new Action(() => setImage()));
            }
            catch (Exception ex){ MessageBox.Show(ex.ToString());  }
        }

        private void CreateExportWorker(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "ffmpeg.exe";

                Zeitpunkt startZP = new Zeitpunkt();
                startZP.setAll(exportStart);
                Zeitpunkt bisZP = new Zeitpunkt();
                bisZP.setAll(exportLaenge);

                strOutputFile = PfadAufraeumen(System.IO.Path.GetFileNameWithoutExtension(strInputFile) + "_" + startZP.pathString() + "_" + bisZP.pathString());
                //-fs 1000000
                p.StartInfo.Arguments = "-ss " + startZP.ToString() + " -i \"" + strInputFile + "\" -sn -c:v libvpx";
                if (tmpvorlage.imaxfilesize > 0) {
                    p.StartInfo.Arguments += " -fs " + (tmpvorlage.imaxfilesize * 1000000);
                }
                if (tmpvorlage.baudioaus == true) {
                    p.StartInfo.Arguments += " -an";
                }
                p.StartInfo.Arguments += " -crf " + tmpvorlage.icrf + " -qmin " + tmpvorlage.iqmin + " -qmax " + tmpvorlage.iqmax;
                if (tmpvorlage.icropbreite > 0 && tmpvorlage.icrophoehe > 0)
                {
                    p.StartInfo.Arguments += " -vf \"[in]scale=" + tmpvorlage.ibreite + ":" +
                    tmpvorlage.ihoehe + "[middle];[middle]crop=" + tmpvorlage.icropbreite + ":" + tmpvorlage.icrophoehe + ":" +
                    tmpvorlage.icropx + ":" + tmpvorlage.icropy + "[out]\"";
                }
                else {
                    p.StartInfo.Arguments += " -vf \"scale=" + tmpvorlage.ibreite + ":" + tmpvorlage.ihoehe + "\"";
                }
                p.StartInfo.Arguments += " -to " + bisZP.ToString() + " -y \"" + strOutputFile + ".webm\"";

                lsFfmpegLog.Add(p.StartInfo.FileName + " " + p.StartInfo.Arguments);

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                p.Start();

                p.WaitForExit();
                this.Dispatcher.BeginInvoke(new Action(() => gridContent.IsEnabled = true));
                this.Dispatcher.BeginInvoke(new Action(() => gridContentBlurEffekt.Radius = 0));
                this.Dispatcher.BeginInvoke(new Action(() => gridLoading.Visibility = Visibility.Collapsed));
            }
            catch { MessageBox.Show("Fehler beim encoden des Videos.", "Vorgang abgebrochen", MessageBoxButton.OK, MessageBoxImage.Error);  }
        }
        #endregion

        #region Qualitäts Regler Handler
        private void sMaxFileSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sMaxFileSize.Value = Convert.ToInt32(sMaxFileSize.Value);
            if (sMaxFileSize.Value == 0)
            {
                labInfMaxFileSize.Content = "aus";
            }
            else {
                labInfMaxFileSize.Content = sMaxFileSize.Value + "MB";
            }
        }

        private void sqmin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setQualitätsBereiche();
        }

        private void sqmax_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setQualitätsBereiche();
        }

        private void scrf_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setQualitätsBereiche();
        }

        private void sbreite_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setSkalierungsBereiche();
        }
        private void shoehe_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setSkalierungsBereiche();
        }

        private void scropbreite_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setCropBereiche();
        }

        private void scrophoehe_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setCropBereiche();
        }

        private void scropx_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setCropBereiche();
        }

        private void scropy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(true);
            setCropBereiche();
        }

        private void labInfQMin_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(1, "Wert für qmin (1 bis 63):", Convert.ToInt32(sqmin.Value).ToString());
        }

        private void labInfQMax_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(2, "Wert für qmax (" + Convert.ToInt32(sqmin.Value) + " bis 63):", Convert.ToInt32(sqmax.Value).ToString());
        }

        private void labInfCRF_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(3, "Wert für CRF (" + Convert.ToInt32(sqmin.Value) + " bis " + Convert.ToInt32(sqmax.Value) + "):", Convert.ToInt32(scrf.Value).ToString());
        }

        private void labInfBreite_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(4, "Wert für die Ausgabe-Breite (-1 = Relativ):", Convert.ToInt32(sbreite.Value).ToString());
        }

        private void labInfHoehe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(5, "Wert für die Ausgabe-Höhe (-1 = Relativ):", Convert.ToInt32(shoehe.Value).ToString());
        }

        private void labInfCropBreite_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(6, "Beschneiden in der Breite um (...px):", Convert.ToInt32(scropbreite.Value).ToString());
        }

        private void labInfCropHoehe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(7, "Beschneiden in der Höhe um (...px):", Convert.ToInt32(scrophoehe.Value).ToString());
        }

        private void labInfCropX_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(8, "Beschneiden in der X-Koordinate um (...px):", Convert.ToInt32(scropx.Value).ToString());
        }

        private void labInfCropY_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            showMagicSingleInput(9, "Beschneiden in der Y-Koordinate um (...px):", Convert.ToInt32(scropy.Value).ToString());
        }

        private void labInfStart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Zeitpunkt z = new Zeitpunkt();
            z.setAll(sstartzeit.Value);
            showTimeMagicInput(1, z.getStunden().ToString("00"), z.getMinuten().ToString("00"), z.getSekunden().ToString("00.000"));
        }

        private void labInfLaenge_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Zeitpunkt z = new Zeitpunkt();
            z.setAll(slaenge.Value);
            showTimeMagicInput(2, z.getStunden().ToString("00"), z.getMinuten().ToString("00"), z.getSekunden().ToString("00.000"));
        }

        private void labInfVorschau_MouseEnter(object sender, MouseEventArgs e)
        {
            VorschauBlurEffekt.Radius = 10;
            gridVorschauBildEffekt.Visibility = Visibility.Visible;
        }

        private void labInfVorschau_MouseLeave(object sender, MouseEventArgs e)
        {
            VorschauBlurEffekt.Radius = 0;
            gridVorschauBildEffekt.Visibility = Visibility.Collapsed;
        }
        #endregion        

        #region MagicSingleInput Handler
        private void showMagicSingleInput(int ivorgangid, string beschreibung, string defaultInput)
        {
            this.vorgangid = ivorgangid;

            tbSingleMagicInput.Text = defaultInput;
            labSingleMagicBeschreibung.Content = beschreibung;

            gridContentBlurEffekt.Radius = 10;
            gridSingleMagicInput.Visibility = Visibility.Visible;

            tbSingleMagicInput.Focus();
        }

        private void btnSingleMagicCancel_Click(object sender, RoutedEventArgs e)
        {
            gridContentBlurEffekt.Radius = 0;
            gridSingleMagicInput.Visibility = Visibility.Collapsed;
        }

        private void tbSingleMagicInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSingleMagicOK_Click(null, null);
            }
        }

        private void btnSingleMagicOK_Click(object sender, RoutedEventArgs e)
        {
            switch (vorgangid)
            {
                case 1: //qmin eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        sqmin.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 2: //qmax eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        sqmax.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 3: //crf eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        scrf.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 4: //breite eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        sbreite.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 5: //höhe eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        shoehe.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 6: //Crop breite eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        scropbreite.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 7: //Crop höhe eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        scrophoehe.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 8: //crop x eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        scropx.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
                case 9: //crop y eingabe
                    if (IsNumeric(tbSingleMagicInput.Text))
                    {
                        scropy.Value = Convert.ToInt32(tbSingleMagicInput.Text);
                    }
                    break;
            }
            gridContentBlurEffekt.Radius = 0;
            gridSingleMagicInput.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region TimeMagic Handler
        private void showTimeMagicInput(int ivorgangid, string Stunden, string Minuten, string Sekunden)
        {
            this.vorgangid = ivorgangid;

            tbTimeMagicStunden.Text = Stunden;
            tbTimeMagicMinuten.Text = Minuten;
            tbTimeMagicSekunden.Text = Sekunden;

            gridContentBlurEffekt.Radius = 10;
            gridTimeMagicInput.Visibility = Visibility.Visible;

            tbTimeMagicStunden.Focus();
        }

        private void btnTimeMagicOk_Click(object sender, RoutedEventArgs e)
        {
            switch (vorgangid)
            {
                case 1: //Startzeitpunkt
                    if (IsNumeric(tbTimeMagicStunden.Text) && IsNumeric(tbTimeMagicMinuten.Text) && IsNumeric(tbTimeMagicSekunden.Text))
                    {
                        int Stunden = Convert.ToInt32(tbTimeMagicStunden.Text);
                        int Minuten = Convert.ToInt32(tbTimeMagicMinuten.Text);
                        double Sekunden = Convert.ToDouble(Convert.ToDecimal(tbTimeMagicSekunden.Text));

                        Zeitpunkt z = new Zeitpunkt();
                        z.setValue(Stunden, Minuten, Sekunden);
                        if (z.getMilliseconds() > lmaxLaufzeitInMilliSek)
                        {
                            sstartzeit.Value = lmaxLaufzeitInMilliSek;
                        }
                        else {
                            sstartzeit.Value = z.getMilliseconds();
                        }
                    }
                    break;
                case 2: //Länge
                    if (IsNumeric(tbTimeMagicStunden.Text) && IsNumeric(tbTimeMagicMinuten.Text) && IsNumeric(tbTimeMagicSekunden.Text))
                    {
                        int Stunden = Convert.ToInt32(tbTimeMagicStunden.Text);
                        int Minuten = Convert.ToInt32(tbTimeMagicMinuten.Text);
                        double Sekunden = Convert.ToDouble(Convert.ToDecimal(tbTimeMagicSekunden.Text));

                        Zeitpunkt z = new Zeitpunkt();
                        z.setValue(Stunden, Minuten, Sekunden);
                        if (sstartzeit.Value + z.getMilliseconds() > lmaxLaufzeitInMilliSek)
                        {
                            slaenge.Value = lmaxLaufzeitInMilliSek - sstartzeit.Value - 1;
                        }
                        else {
                            slaenge.Value = sstartzeit.Value + z.getMilliseconds();
                        }
                    }
                    break;
            }

            gridContentBlurEffekt.Radius = 0;
            gridTimeMagicInput.Visibility = Visibility.Collapsed;
        }

        private void btnTimeMagicCancel_Click(object sender, RoutedEventArgs e)
        {
            gridContentBlurEffekt.Radius = 0;
            gridTimeMagicInput.Visibility = Visibility.Collapsed;
        }

        private void tbTimeMagicStunden_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnTimeMagicOk_Click(null, null);
            }
        }

        private void tbTimeMagicMinuten_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnTimeMagicOk_Click(null, null);
            }
        }

        private void tbTimeMagicSekunden_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnTimeMagicOk_Click(null, null);
            }
        }

        #endregion        

        #region Default Handler
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            setmaxLaenge();
            GenerateVorschau(true);
            labInfStart.Content = lastZP.ToString();            
        }

        private void slider_GotFocus(object sender, RoutedEventArgs e)
        {
            slider_ValueChanged(null, null);
        }
 private void slaenge_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GenerateVorschau(false);
            Zeitpunkt t = new Zeitpunkt();
            t.setAll(slaenge.Value);
            labInfLaenge.Content = t.ToString();
        }

        private void slaenge_GotFocus(object sender, RoutedEventArgs e)
        {
            slaenge_ValueChanged(null, null);
        }

        private void comVorlage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Vorlage item = this.E.lsVorlagen.ElementAt(comVorlage.SelectedIndex);
                sqmin.Value = item.iqmin;
                sqmax.Value = item.iqmax;
                scrf.Value = item.icrf;
                sbreite.Value = item.ibreite;
                shoehe.Value = item.ihoehe;
                scropbreite.Value = item.icropbreite;
                scrophoehe.Value = item.icrophoehe;
                scropx.Value = item.icropx;
                scropy.Value = item.icropy;
                sMaxFileSize.Value = item.imaxfilesize;
                cbAudioAus.IsChecked = item.baudioaus;
            }
            catch { }
        }

        private void btnVorlageDelete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < (this.E.lsVorlagen.Count - 1); i += 1)
            {
                if (this.E.lsVorlagen.ElementAt(i).strName == comVorlage.Text)
                {
                    this.E.lsVorlagen.RemoveAt(i);
                    break;
                }
            }

            comVorlage.Text = "";
            this.E.Speichern();

            LadeVorlagen();
        }

        private void btnVorlageSpeichern_Click(object sender, RoutedEventArgs e)
        {
            string disText = comVorlage.Text;

            if (disText.Length <= 0)
            {
                string tstring = "";
                int tint = this.E.lsVorlagen.Count + 1;
                bool isin = false;

                do
                {
                    tstring = "Vorlage " + tint;
                    isin = false;
                    foreach (Vorlage item in this.E.lsVorlagen)
                    {
                        if (item.strName == tstring)
                        {
                            isin = true;
                            tint += 1;
                            break;
                        }
                    }

                } while (isin == true);
                disText = tstring;
            }

            bool found = false;
            foreach (Vorlage item in this.E.lsVorlagen)
            {
                if (item.strName == disText)
                {
                    item.iqmin = Convert.ToInt32(sqmin.Value);
                    item.iqmax = Convert.ToInt32(sqmax.Value);
                    item.icrf = Convert.ToInt32(scrf.Value);
                    item.ibreite = Convert.ToInt32(sbreite.Value);
                    item.ihoehe = Convert.ToInt32(shoehe.Value);
                    item.icropbreite = Convert.ToInt32(scropbreite.Value);
                    item.icrophoehe = Convert.ToInt32(scrophoehe.Value);
                    item.icropx = Convert.ToInt32(scropx.Value);
                    item.icropy = Convert.ToInt32(scropy.Value);
                    item.imaxfilesize = Convert.ToInt32(sMaxFileSize.Value);
                    item.baudioaus = cbAudioAus.IsChecked.Value;
                    found = true;
                }
            }

            if (found == false)
            {
                Vorlage v = new Vorlage(disText, Convert.ToInt32(sqmin.Value), Convert.ToInt32(sqmax.Value),
                    Convert.ToInt32(scrf.Value), Convert.ToInt32(sbreite.Value), Convert.ToInt32(shoehe.Value),
                    Convert.ToInt32(scropbreite.Value), Convert.ToInt32(scrophoehe.Value), Convert.ToInt32(scropx.Value),
                    Convert.ToInt32(scropy.Value), Convert.ToInt32(sMaxFileSize.Value), cbAudioAus.IsChecked.Value);
                this.E.lsVorlagen.Add(v);
            }

            comVorlage.Text = disText;
            this.E.Speichern();

            LadeVorlagen();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OeffneDatei();
        }

        private void btnCopyright_Click(object sender, RoutedEventArgs e)
        {
            ZeigeRechtliches();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            CreateExport();
        }

        private void labInfCopyrightUser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://pr0gramm.com/user/Teto");
        }

       
        #endregion

        #region Rechliches
        public void ZeigeRechtliches()
        {
            imgVorschau.Visibility = Visibility.Collapsed;
            gridContentBlurEffekt.Radius = 10;
            gridRechtliches.Visibility = Visibility.Visible;
        }
        private void btnCloseRechtliches_Click(object sender, RoutedEventArgs e)
        {
            gridContentBlurEffekt.Radius = 0;
            gridRechtliches.Visibility = Visibility.Hidden;
            imgVorschau.Visibility = Visibility.Visible;
            
        }
        #endregion
    }

    #region Zusatzklassen
    public class Zeitpunkt {
        int iStunden = 0;
        int iMinuten = 0;
        double iSekunden = 0;

        public Zeitpunkt() { }

        public void setAll(long millisec) {
            DateTime D = new DateTime(millisec);

            this.iStunden = Convert.ToInt32(millisec / (1000 * 60 * 60));
            this.iMinuten = Convert.ToInt32((millisec - this.iStunden * 1000 * 60 * 60) / (1000 * 60));
            this.iSekunden = (millisec - this.iStunden * 1000 * 60 * 60 - this.iMinuten * 1000 * 60) / 10000;
        }

        public int getStunden() { return this.iStunden;  }
        public int getMinuten() { return this.iMinuten; }
        public double getSekunden() { return this.iSekunden; }

        public void setAll(double millisec)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(millisec);
            this.iStunden = t.Hours;
            this.iMinuten = t.Minutes;
            this.iSekunden = t.Seconds;
            this.iSekunden += Convert.ToDouble(t.Milliseconds) / 1000;           
        }

        public void setValue(int Stunden, int Minuten, double Sekunden) {
            Minuten += Convert.ToInt32(Sekunden) / 60;
            Sekunden = Sekunden % 60;
            
            Stunden += Convert.ToInt32(Minuten) / 60;
            Minuten = Minuten % 60;

            this.iStunden = Stunden;
            this.iMinuten = Minuten;
            this.iSekunden = Sekunden;
        }

        public long getSeconds() {
            return (this.iStunden * 60 * 60) + (this.iMinuten * 60) + Convert.ToInt32(this.iSekunden);
        }

        public long getMilliseconds() {
            return (this.iStunden * 60 * 60 * 1000) + (this.iMinuten * 60 * 1000) + Convert.ToInt64(this.iSekunden * 1000);
        }

        public override string ToString()
        {
            return this.iStunden.ToString("00") + ":" + this.iMinuten.ToString("00") + ":" + this.iSekunden.ToString("00.000").Replace(",", ".");
        }

        public string pathString() {
            return this.iStunden.ToString("00") + this.iMinuten.ToString("00") + this.iSekunden.ToString("00.000").Replace(",", ".");
        }

    }

    [Serializable()]
    public class Einstellungen {
        public List<Vorlage> lsVorlagen = new List<Vorlage>();
        private string strEinstellungsDatei = System.AppDomain.CurrentDomain.BaseDirectory + "pwc.set";

        public Einstellungen() { }

        public Einstellungen(List<Vorlage> _lsvorlagen) {
            this.lsVorlagen = _lsvorlagen;
        }

        public void Speichern() {
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(Einstellungen));
                StringWriter strw = new StringWriter();
                xmls.Serialize(strw, this);
                string s = strw.ToString();
                strw.Close();

                File.WriteAllText(strEinstellungsDatei, s, Encoding.UTF8);
            }
            catch {
                MessageBox.Show("Die Einstellungen konnten leider nicht abgespeichert werden! Eventuell greift gerade ein anderes Programm auf diese Datei zu.", "Einstellungen nicht gespeichert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    [Serializable()]
    public class Vorlage {
        public string strName;
        public int iqmin, iqmax, icrf, ibreite, ihoehe, icropbreite, icrophoehe, icropx, icropy, imaxfilesize;
        public bool baudioaus;

        public Vorlage() { }

        public Vorlage(string name, int qmin, int qmax, int crf, int breite, int hoehe, int cropbreite, int crophohe, int cropx, int cropy, int maxfilesize, bool audioaus) {
            this.strName = name;
            this.iqmin = qmin;
            this.iqmax = qmax;
            this.icrf = crf;
            this.ibreite = breite;
            this.ihoehe = hoehe;
            this.icropbreite = cropbreite;
            this.icrophoehe = crophohe;
            this.icropx = cropx;
            this.icropy = cropy;
            this.imaxfilesize = maxfilesize;
            this.baudioaus = audioaus;
        }
    }
    #endregion
}

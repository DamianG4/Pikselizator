using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Pikselizator
{
    public partial class Form1 : Form
    {
        // import biblioteki asm
        [DllImport("AsmLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PixelizeProc(IntPtr scan0, int width, int height, int stride, int blockSize);

        private Point _selectedPoint = new Point(0, 0);
        private bool _pointSelected = false;
        private bool _isDragging = false;

        // ukryty przycisk do generowania wynikow
        private Button btnTesty;

        public Form1()
        {
            InitializeComponent();

            // konfiguracja startowa
            this.trackBarThreads.Minimum = 1;
            this.trackBarThreads.Maximum = 64;
            // domyslna liczba watkow z procesora
            this.trackBarThreads.Value = Environment.ProcessorCount;

            // zdarzenia myszki
            this.pictureBox1.MouseDown += (s, e) => pictureBox1_MouseDown(s, e);
            this.pictureBox1.MouseMove += (s, e) => pictureBox1_MouseMove(s, e);
            this.pictureBox1.MouseUp += (s, e) => pictureBox1_MouseUp(s, e);
            this.pictureBox1.Paint += (s, e) => pictureBox1_Paint(s, e);

            this.btnLoad.Click += (s, e) => btnLoad_Click(s, e);

            // odswiezanie gui przy zmianie suwakow
            this.trackBarRadius.Scroll += (s, e) => { UpdateLabels(); pictureBox1.Refresh(); };
            this.trackPowerSize.Scroll += (s, e) => UpdateLabels();
            this.trackBarThreads.Scroll += (s, e) => UpdateLabels();

            // dodanie przycisku testowego po zaladowaniu okna
            this.Load += (s, e) =>
            {
                UpdateLabels();
                DodajPrzyciskTestowy();
            };
        }

        private void DodajPrzyciskTestowy()
        {
            btnTesty = new Button();
            btnTesty.Text = "TESTY (CSV)";
            btnTesty.Size = new Size(120, 30);
            btnTesty.Location = new Point(10, 10);
            btnTesty.BackColor = Color.LightYellow;
            btnTesty.Click += BtnTesty_Click;
            this.Controls.Add(btnTesty);
            btnTesty.BringToFront();
        }

        private void UpdateLabels()
        {
            if (lblRadius != null) lblRadius.Text = $"Promien: {trackBarRadius.Value} (Max: {trackBarRadius.Maximum})";
            if (lblPower != null) lblPower.Text = $"Moc: {trackPowerSize.Value}";
            if (lblThreads != null) lblThreads.Text = $"Watki: {GetThreadCount()}";
        }

        private int GetThreadCount()
        {
            return trackBarThreads.Value;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Obrazy|*.jpg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                WczytajObraz(ofd.FileName);
            }
        }

        private void WczytajObraz(string sciezka)
        {
            Bitmap bmp = new Bitmap(sciezka);
            Bitmap clone = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(clone)) { g.DrawImage(bmp, 0, 0); }
            pictureBox1.Image = clone;

            _selectedPoint = new Point(bmp.Width / 2, bmp.Height / 2);
            _pointSelected = false;

            int maxAllowedRadius = Math.Max(bmp.Width, bmp.Height) / 2;
            trackBarRadius.Maximum = maxAllowedRadius;

            if (trackBarRadius.Value > maxAllowedRadius) trackBarRadius.Value = maxAllowedRadius;

            UpdateLabels();
        }

        // --- AUTOMATYCZNE TESTY (Z PETLA 5x DLA SREDNIEJ) ---

        private void BtnTesty_Click(object sender, EventArgs e)
        {
            string[] pliki = new string[3];
            string[] nazwy = { "MALE", "SREDNIE", "DUZE" };

            MessageBox.Show("Wybierz kolejno 3 zdjecia: Male, Srednie, Duze.");

            for (int i = 0; i < 3; i++)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Wybierz: " + nazwy[i];
                if (ofd.ShowDialog() == DialogResult.OK) pliki[i] = ofd.FileName;
                else return;
            }

            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Zestaw;Rozdzielczosc;Biblioteka;Watki;Sredni czas [us]");

            int[] watkiLista = { 1, 2, 4, 8, 16, 32, 64 };
            int powtorzenia = 5; // TUTAJ ZOSTALO 5 PROB (WYMOG RAPORTU)

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    WczytajObraz(pliki[i]);
                    Bitmap org = (Bitmap)pictureBox1.Image;
                    string opis = nazwy[i];
                    string rozdz = $"{org.Width}x{org.Height}";

                    int maxR = Math.Max(org.Width, org.Height) / 2;
                    int maxP = 100;

                    foreach (bool czyAsm in new[] { false, true })
                    {
                        string libName = czyAsm ? "ASM" : "C#";

                        foreach (int w in watkiLista)
                        {
                            double sumaCzasow = 0;

                            for (int k = 0; k < powtorzenia; k++)
                            {
                                long ticks = UruchomAlgorytmBezGui(org, maxP, maxR, w, czyAsm);
                                double us = (double)ticks / Stopwatch.Frequency * 1_000_000.0;
                                sumaCzasow += us;
                            }

                            double srednia = sumaCzasow / powtorzenia;
                            string linia = $"{opis};{rozdz};{libName};{w};{srednia:F2}";
                            csv.AppendLine(linia);

                            this.Text = $"Test: {linia}";
                            Application.DoEvents();
                        }
                    }
                }

                string plikWynikowy = "wyniki.csv";
                File.WriteAllText(plikWynikowy, csv.ToString());
                MessageBox.Show("Gotowe! Wyniki w pliku wyniki.csv");
                Process.Start(plikWynikowy);
            }
            catch (AggregateException ae)
            {
                // To wyciągnie pierwszy prawdziwy błąd z paczki
                foreach (var en in ae.InnerExceptions)
                {
                    MessageBox.Show("PRAWDZIWY BŁĄD: " + en.Message + "\n" + en.StackTrace);
                    break; // Pokazujemy tylko pierwszy, zeby nie spamowac oknami
                }
            }
            catch (Exception exTest)
            {
                MessageBox.Show("Inny błąd: " + exTest.Message);
            }
            this.Text = "Form1";
        }

        private long UruchomAlgorytmBezGui(Bitmap zrodlo, int moc, int promien, int watki, bool asm)
        {
            using (Bitmap robocza = (Bitmap)zrodlo.Clone())
            {
                Rectangle rect = new Rectangle(0, 0, robocza.Width, robocza.Height);
                BitmapData data = robocza.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                Point srodek = new Point(robocza.Width / 2, robocza.Height / 2);
                int sx = Math.Max(0, srodek.X - promien);
                int sy = Math.Max(0, srodek.Y - promien);
                int ex = Math.Min(robocza.Width, srodek.X + promien);
                int ey = Math.Min(robocza.Height, srodek.Y + promien);
                int w = ex - sx;
                int h = ey - sy;

                ParallelOptions opcje = new ParallelOptions();
                opcje.MaxDegreeOfParallelism = watki;

                Stopwatch stoper = Stopwatch.StartNew();
                IntPtr ptr0 = data.Scan0;
                int stride = data.Stride;

                Parallel.For(0, (h + moc - 1) / moc, opcje, i =>
                {
                    int ly = i * moc;
                    if (ly >= h) return;
                    int ch = (ly + moc > h) ? h - ly : moc;
                    int ay = sy + ly;

                    if (asm)
                    {
                        unsafe
                        {
                            byte* p = (byte*)ptr0;
                            long offset = (long)ay * stride + (long)sx * 4;
                            p += offset;
                            PixelizeProc((IntPtr)p, w, ch, stride, moc);
                        }
                    }
                    else
                    {
                        PrzetworzPasekCSharp(ptr0, stride, moc, sx, ay, w, ch);
                    }
                });

                stoper.Stop();
                robocza.UnlockBits(data);
                return stoper.ElapsedTicks;
            }
        }

        // --- OBSLUGA INTERFEJSU ---

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null) return;
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _selectedPoint = TranslateCoordinates(e.Location, pictureBox1);
                _pointSelected = true;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && pictureBox1.Image != null)
            {
                _selectedPoint = TranslateCoordinates(e.Location, pictureBox1);
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image == null || !_pointSelected) return;

            int rawRadius = trackBarRadius.Value;
            if (rawRadius <= 0) return;

            int maxAllowed = Math.Max(pictureBox1.Image.Width, pictureBox1.Image.Height) / 2;
            int radius = Math.Min(rawRadius, maxAllowed);

            int imgX = _selectedPoint.X - radius;
            int imgY = _selectedPoint.Y - radius;
            int size = radius * 2;
            Rectangle imageRect = new Rectangle(imgX, imgY, size, size);
            Rectangle screenRect = ImageToControlRect(imageRect, pictureBox1);

            using (Pen p = new Pen(Color.Red, 2))
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                e.Graphics.DrawRectangle(p, screenRect);
            }
        }

        // --- GLOWNY PRZYCISK (NORMALNE DZIALANIE - 1 RAZ) ---

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null) { MessageBox.Show("Wczytaj obraz!"); return; }

            int moc = trackPowerSize.Value;
            if (moc < 1) moc = 1;
            int promien = Math.Min(trackBarRadius.Value, Math.Max(pictureBox1.Image.Width, pictureBox1.Image.Height) / 2);

            Bitmap bmp = (Bitmap)pictureBox1.Image;

            // TUTAJ ZMIANA: Robimy tylko raz (bez petli 5x)

            if (!_pointSelected) _selectedPoint = new Point(bmp.Width / 2, bmp.Height / 2);

            int sx = Math.Max(0, _selectedPoint.X - promien);
            int sy = Math.Max(0, _selectedPoint.Y - promien);
            int ex = Math.Min(bmp.Width, _selectedPoint.X + promien);
            int ey = Math.Min(bmp.Height, _selectedPoint.Y + promien);
            int w = ex - sx;
            int h = ey - sy;

            if (w <= 0 || h <= 0) return;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            Stopwatch sw = new Stopwatch();
            ParallelOptions opcje = new ParallelOptions();
            opcje.MaxDegreeOfParallelism = GetThreadCount();

            bool asm = false;
            var ctl = Application.OpenForms[0].Controls.Find("rbAsm", true);
            if (ctl.Length > 0 && ((RadioButton)ctl[0]).Checked) asm = true;

            try
            {
                sw.Start(); // START

                IntPtr ptr0 = data.Scan0;
                int stride = data.Stride;

                Parallel.For(0, (h + moc - 1) / moc, opcje, i =>
                {
                    int ly = i * moc;
                    if (ly >= h) return;
                    int ch = (ly + moc > h) ? h - ly : moc;
                    int ay = sy + ly;

                    if (asm)
                    {
                        unsafe
                        {
                            byte* p = (byte*)ptr0;
                            long offset = (long)ay * stride + (long)sx * 4;
                            p += offset;
                            PixelizeProc((IntPtr)p, w, ch, stride, moc);
                        }
                    }
                    else
                    {
                        PrzetworzPasekCSharp(ptr0, stride, moc, sx, ay, w, ch);
                    }
                });

                sw.Stop(); // STOP
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Blad: " + ex2.Message);
            }
            finally
            {
                bmp.UnlockBits(data);
            }

            pictureBox1.Refresh();

            // Czas pojedynczego wykonania
            double mikroSekundy = (sw.ElapsedTicks / (double)Stopwatch.Frequency) * 1_000_000.0;

            lblTime.Text = $"Czas: {mikroSekundy:F0} us";
        }

        // --- C# IMPL ---

        private unsafe void PrzetworzPasekCSharp(IntPtr scan0, int stride, int blockSize, int startX, int absoluteY, int w, int h)
        {
            byte* ptrBase = (byte*)scan0;
            for (int localX = 0; localX < w; localX += blockSize)
            {
                int absoluteX = startX + localX;
                long rSum = 0, gSum = 0, bSum = 0;
                int count = 0;

                for (int by = 0; by < blockSize; by++)
                {
                    if (by >= h) break;
                    for (int bx = 0; bx < blockSize; bx++)
                    {
                        if (localX + bx >= w) break;
                        byte* pix = ptrBase + ((absoluteY + by) * stride) + ((absoluteX + bx) * 4);
                        bSum += pix[0]; gSum += pix[1]; rSum += pix[2];
                        count++;
                    }
                }
                if (count == 0) continue;
                byte rAvg = (byte)(rSum / count);
                byte gAvg = (byte)(gSum / count);
                byte bAvg = (byte)(bSum / count);

                for (int by = 0; by < blockSize; by++)
                {
                    if (by >= h) break;
                    for (int bx = 0; bx < blockSize; bx++)
                    {
                        if (localX + bx >= w) break;
                        byte* pix = ptrBase + ((absoluteY + by) * stride) + ((absoluteX + bx) * 4);
                        pix[0] = bAvg; pix[1] = gAvg; pix[2] = rAvg;
                    }
                }
            }
        }

        private Rectangle ImageToControlRect(Rectangle imgRect, PictureBox pb)
        {
            if (pb.SizeMode == PictureBoxSizeMode.Normal || pb.SizeMode == PictureBoxSizeMode.AutoSize) return imgRect;
            if (pb.SizeMode == PictureBoxSizeMode.Zoom)
            {
                float imgAspect = (float)pb.Image.Width / pb.Image.Height;
                float ctrlAspect = (float)pb.Width / pb.Height;
                float scale;
                float offsetX = 0, offsetY = 0;

                if (imgAspect > ctrlAspect)
                {
                    scale = (float)pb.Width / pb.Image.Width;
                    offsetY = (pb.Height - (pb.Image.Height * scale)) / 2;
                }
                else
                {
                    scale = (float)pb.Height / pb.Image.Height;
                    offsetX = (pb.Width - (pb.Image.Width * scale)) / 2;
                }
                int screenX = (int)(imgRect.X * scale + offsetX);
                int screenY = (int)(imgRect.Y * scale + offsetY);
                int screenW = (int)(imgRect.Width * scale);
                int screenH = (int)(imgRect.Height * scale);
                return new Rectangle(screenX, screenY, screenW, screenH);
            }
            return imgRect;
        }

        private Point TranslateCoordinates(Point controlCoordinates, PictureBox pb)
        {
            if (pb.Image == null) return new Point(0, 0);
            if (pb.SizeMode == PictureBoxSizeMode.Zoom)
            {
                float imgAspect = (float)pb.Image.Width / pb.Image.Height;
                float ctrlAspect = (float)pb.Width / pb.Height;
                float scale, offX = 0, offY = 0;

                if (imgAspect > ctrlAspect)
                {
                    scale = (float)pb.Width / pb.Image.Width;
                    offY = (pb.Height - pb.Image.Height * scale) / 2;
                }
                else
                {
                    scale = (float)pb.Height / pb.Image.Height;
                    offX = (pb.Width - pb.Image.Width * scale) / 2;
                }
                int x = (int)((controlCoordinates.X - offX) / scale);
                int y = (int)((controlCoordinates.Y - offY) / scale);
                return new Point(Math.Max(0, Math.Min(pb.Image.Width - 1, x)), Math.Max(0, Math.Min(pb.Image.Height - 1, y)));
            }
            return controlCoordinates;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) { }
        private void trackBarRadius_Scroll(object sender, EventArgs e) { }
        private void trackPowerSize_Scroll(object sender, EventArgs e) { }
        private void trackBarThreads_Scroll(object sender, EventArgs e) { }
    }
}   
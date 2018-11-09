using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace projeto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.Owner = new Form();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ajusta bordas.
            this.ControlBox = false;
            this.Text = String.Empty;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            // Especificações.
            int heightSize = 15;

            // Ajusta tamanho.
            System.Drawing.Rectangle rec = Screen.PrimaryScreen.WorkingArea;
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = rec.Height;


            this.MinimumSize = new System.Drawing.Size(width, heightSize);
            this.Size = new Size(width, heightSize);

            // Ajusta posição.
            this.Left = 0;
            this.Top = height - heightSize;

            // Oculta da barra de tarefas.
            this.ShowInTaskbar = false;

            // Form transparente.
            BackColor = Color.Lime;
            TransparencyKey = Color.Lime;
            
            // Cria texto...
            CreateLabel();

            // Tray...
            CreateTray();
        }

        // Remove do tab-dialog.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        // Tray-menu.
        public NotifyIcon trayIcon;
        public ContextMenu trayMenu;

        public void CreateTray ()
        {
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Frases", OnFrases);
            trayMenu.MenuItems.Add("Fechar", OnExit);

            Icon icone = new System.Drawing.Icon(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\icone.ico");
            trayIcon = new NotifyIcon();
            trayIcon.Text = "LogosBar";
            trayIcon.Icon = new Icon(icone, 40, 40);
            
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }
        
        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
        
        private void OnFrases(object sender, EventArgs e)
        {
            Form2 fm = new Form2(this);
            fm.Show();
        }
        
        public void loadFrases ()
        {
            this.CreateLabel();
        }

        // Controle do texto.
        string[] frases = { };
        int index = 0;

        private void CreateLabel ()
        {
            index = 0;
            string filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\frases.txt";

            if (File.Exists(filename) == false)
            {
                MessageBox.Show("Não existem frases cadastradas, acesse o menu \"Frases\" através do ícone no tray e cadastre suas mensagens.");
                return;
            }

            // Carrega frases.
            List<string> list = new List<string>();
            
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(filename);

            while ((line = file.ReadLine()) != null)
            {
                string replaceWith = "";
                string removedBreaks = line.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
                if (removedBreaks != null)
                    if (removedBreaks.Length > 0)
                        list.Add(removedBreaks);
            }
            file.Close();

            frases = list.ToArray();
            
            if (!(frases.Length > 0))
            {
                MessageBox.Show("Você não cadastrou mensagens.");
                return;
            }

            timer.Enabled = true;

            // Inicialização.
            texto.Text = frases[index];
            texto.Top = 1;
            index++;

            texto.Location = new Point(this.Width, texto.Location.Y);
        }
        
        private void timer_Tick(object sender, EventArgs e)
        {
            texto.Location = new Point(texto.Location.X - 1, texto.Location.Y);
            int sz = TextRenderer.MeasureText(frases[index - 1], texto.Font).Width;

            // Percorre para direita.
            if ((texto.Location.X + sz) <= 0)
            {
                if (index >= frases.Length)
                    index = 0;
                texto.Text = frases[index];
                index++;

                texto.Location = new Point(this.Width, texto.Location.Y);
            }
        }
    }
}

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
    public partial class Form2 : Form
    {
        private Form1 parentForm;

        public Form2(Form1 parentForm)
        {
            this.parentForm = parentForm;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string texto = richTextBox1.Text;
            string filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\frases.txt";
            System.IO.File.WriteAllText(filename, texto);
            parentForm.loadFrases();
            MessageBox.Show("Mensagens salvas com sucesso!");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\frases.txt";
            if (File.Exists(filename) == true)
            {
                string texto = System.IO.File.ReadAllText(filename);
                richTextBox1.Text = texto;
            }
        }
    }
}

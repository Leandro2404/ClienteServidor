using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Net.Sockets;
using System.IO;
using Transitions;
using System.Runtime.InteropServices;

namespace Cliente
{    

    public partial class Form1 : Form
    {
        
        Conexion c;

        static private NetworkStream stream;
        static private StreamWriter streamw;
        static private StreamReader streamr;
        static private TcpClient client = new TcpClient();
        static private string nick = "unknown";

        private delegate void DaddItem(String s);




        /*****************************************************************/
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        /*****************************************************************/

        private void AddItem(String s)
        {
            dataGridView1.DataSource = c.QuerySelect();
            FormatoGrid();
            dataGridView1.ClearSelection();
            listBox1.Items.Add(s);
        }
        public Form1()
        {
            InitializeComponent();
        }

        void Listen()
        {
            while(client.Connected)
            {
                try
                {
                    this.Invoke(new DaddItem(AddItem), streamr.ReadLine());
                }
                catch
                {
                    MessageBox.Show("No se ha podido conectar al servidor");
                    Application.Exit();
                }
            }
        }

        bool Conectar()
        {
            try
            {
                client.Connect(textBoxIP.Text, 8000);
                if(client.Connected)
                {
                    if(textBoxIP.Text == "127.0.0.1")
                        c = new Conexion("localhost");
                    else
                        c = new Conexion(textBoxIP.Text);
                    
                    Thread t = new Thread(Listen);
                    t.IsBackground = true;

                    stream = client.GetStream();
                    streamw = new StreamWriter(stream);
                    streamr = new StreamReader(stream);

                    streamw.WriteLine(nick);
                    streamw.Flush();

                    t.Start();
                    return true;
                }
                else
                {
                    MessageBox.Show("Servidor no Disponible");
                    
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Servidor no Disponible");
                
                //Application.Exit();
            }
            return false;
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            

            nick = txtUsuario.Text;
            if (Conectar())
            {

                dataGridView1.DataSource = c.QuerySelect();
                FormatoGrid();
                dataGridView1.ClearSelection();
                //panel3.Visible = false;
                //panel1.Visible = true;
                //panel2.Visible = true;

                Transition t = new Transition(new TransitionType_EaseInEaseOut(400));
                //t.add(lbTitulo1, "Left", 555);
                //t.add(txtUsuario, "Left", 555);
                //t.add(btnConectar, "Left", 555);
                //t.add(listBox1, "Left", 26);
                //t.add(txtMensaje, "Left", 26);
                //t.add(btnEnviar, "Left", 283);
                //t.add(dataGridView1, "Left", 230);


                t.add(panel1, "Left", 12);
                t.add(panel2, "Left", 12);
                t.add(panel3, "Left", 555);
                t.run();

            }




        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string indice = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            c.QueryUpdate(indice);
            Comunicacion(indice);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //btnEnviar.Location = new Point(-329, 250);
            //txtMensaje.Location = new Point(-329, 250);
            //listBox1.Location = new Point(-329, 23);
            //dataGridView1.Location = new Point(-303, 24);

            panel1.Location = new Point(-350, panel1.Location.Y); 
            panel2.Location = new Point(-350, panel2.Location.Y);

            panel1.Visible = true;
            panel2.Visible = true;

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count >= 1)
                btnEnviar.Enabled = true;
            else
                btnEnviar.Enabled = false;
        }

        private void buttonCargar_Click(object sender, EventArgs e)
        {
            c.QueryInsert(textBoxNombre.Text);
            Comunicacion("cliente cargado");
        }

        private void Comunicacion(string mensaje)
        {
            streamw.WriteLine(mensaje);
            streamw.Flush();
            txtMensaje.Clear();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void FormatoGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Columns["Nombre"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}

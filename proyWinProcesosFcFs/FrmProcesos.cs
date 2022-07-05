using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace proyWinProcesosFcFs
{
    public partial class FrmProcesos : Form
    {
        const int NUMFILASMOSTRADAS = 15;
        const int NUMDATOSMAXIMO = 100;
        int numDatosActual = 0;
        int numDatosAnterior = 0;
        int alturaTotal, anchoTabla, altoDeFila;
        //int z1, z2, z3, z4 ;
        bool bolCargandoFormulario;
        public FrmProcesos()
        {
            InitializeComponent();
        }
        
        private void inhabilitarTeclasDireccionSuprimir(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down
            || e.KeyCode == Keys.Up
            || e.KeyCode == Keys.Left
            || e.KeyCode == Keys.Right
            || e.KeyCode == Keys.Delete
            )
                e.Handled = true;
        }
        private void validarTeclaPulsada(object sender, KeyPressEventArgs e)
        {
            TextBox obj = (TextBox)sender;
            bool esDigitoEntero = char.IsDigit(e.KeyChar);
            //bool esPuntoDecimal = ((e.KeyChar == '.')
            // && (obj.Text.IndexOf('.') == -1));
            bool esRetroceso = e.KeyChar == (char)Keys.Back;
            //bool esSignoNegativo = (e.KeyChar == '-') &&
            //(obj.Text.Length == 0);
            if (esDigitoEntero
            || esRetroceso
                //|| esSignoNegativo
                //|| esPuntoDecimal
            )
            {
                e.Handled = false; //Dejar pasar
            }
            else
                e.Handled = true; //Bloquear
        }
        private bool listaCompleta()
        {
            int i = 0;
            bool completa = true;
            while (i < numDatosActual && completa)
            {
                if (dgvDatos[0, i].Value == null)
                    completa = false;
                i++;
            }
            return completa;
        }

        private void inicializarDataGridView()
        {
            toolTip1.IsBalloon = true;
            //inhabilitar ambas barras de desplazamiento
            //dgvDatos.ScrollBars = ScrollBars.None;
            //Inhabilitar estilo visual del DataGridView
            //para aceptar los estilos de color para encabezados
            //dgvDatos.EnableHeadersVisualStyles = false;
            //Establecer el ancho de los encabezados de fila
            //al valor predeterminado + 30 pixeles

            dgvDatos.RowHeadersWidth = dgvDatos.RowHeadersWidth + 30;
            //Alinear por la izquierda

            dgvDatos.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //Crear las 7 columnas

            dgvDatos.ColumnCount = 7;

            //Establecer el ancho de los encabezados de columna
            dgvDatos.Columns[0].Width = 110;
            dgvDatos.Columns[1].Width = 110;
            dgvDatos.Columns[2].Width = 110;
            dgvDatos.Columns[3].Width = 110;
            dgvDatos.Columns[4].Width = 110;
            dgvDatos.Columns[5].Width = 110;

            //Estilos de encabezados de columna
            dgvDatos.ColumnHeadersDefaultCellStyle.Font =
            new Font(
            dgvDatos.DefaultCellStyle.Font.Name,
            12, dgvDatos.DefaultCellStyle.Font.Style);
            dgvDatos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //Si EnableHeadersVisualStyles = true, estos estilos no
            //tienen efecto alguno
            dgvDatos.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;
            dgvDatos.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;

            //Tamaño del contenedor del DataGridView
            anchoTabla =
            dgvDatos.Columns[0].Width +
            dgvDatos.Columns[1].Width +
            dgvDatos.Columns[2].Width +
            dgvDatos.Columns[3].Width +
            dgvDatos.Columns[4].Width +
            dgvDatos.Columns[5].Width +

            dgvDatos.RowHeadersWidth;
            //Alto de fila = alto de encabezado de columna +
            // dos tercios de su valor. (mayor margen de tolerancia)

            alturaTotal = dgvDatos.Columns[0].HeaderCell.Size.Height +
        (2 * dgvDatos.Columns[0].HeaderCell.Size.Height / 3);
            dgvDatos.Size = new Size(anchoTabla, alturaTotal);

            //Establecer rótulos de los encabezados de columna
            dgvDatos.Columns[0].Name = "Proceso";
            dgvDatos.Columns[1].Name = "Tiempo Ll.";
            dgvDatos.Columns[2].Name = "Tiempo F.";
            dgvDatos.Columns[3].Name = "Tiempo S.";
            dgvDatos.Columns[4].Name = "Tiempo E.";
            dgvDatos.Columns[5].Name = "Tiempo R.";
            //dgvDatos.Columns[6].Name = "Tiempo NR.";

            //Ajuste automático del ancho del contenedor
            //con el ancho de la tabla DataGridView
            dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //Centrar contenido de celdas
            dgvDatos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //Inhabilitar ordenación para las 7 columnas
            dgvDatos.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDatos.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDatos.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDatos.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDatos.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvDatos.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            //dgvDatos.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;

            //Inhabilitar segunda columna para edición
            dgvDatos.Columns[0].ReadOnly = true;
            dgvDatos.Columns[1].ReadOnly = true;
            dgvDatos.Columns[2].ReadOnly = true;
            dgvDatos.Columns[3].ReadOnly = true;
            dgvDatos.Columns[4].ReadOnly = true;
            dgvDatos.Columns[5].ReadOnly = true;
            //dgvDatos.Columns[6].ReadOnly = true;

            //Agregar tooltip a las columnas
            dgvDatos.Columns[0].ToolTipText = "Nombre del proceso";
            dgvDatos.Columns[1].ToolTipText = "Tiempo de llegada";
            dgvDatos.Columns[2].ToolTipText = "Tiempo de finalización";
            dgvDatos.Columns[3].ToolTipText = "Tiempo de servicio";
            dgvDatos.Columns[4].ToolTipText = "Tiempo de espera";
            dgvDatos.Columns[5].ToolTipText = "Tiempo de retorno";
            //dgvDatos.Columns[6].ToolTipText = "Tiempo de retorno normalizado";


            //Inhabilitar selección múltiple
            dgvDatos.MultiSelect = false;
            //No mostrar encabezados de filas
            //dgvDatos.RowHeadersVisible = false;
            //Inhabilitar modificación del ancho del encabezado de fila
            dgvDatos.RowHeadersWidthSizeMode =
            DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //Inhabilitar agregar filas por parte del usuario
            dgvDatos.AllowUserToAddRows = false;
            //Inhabilitar redimensionamiento de filas y columnas
            dgvDatos.AllowUserToResizeColumns = false;
            dgvDatos.AllowUserToResizeRows = false;
            //Coloreado de filas alternado
            dgvDatos.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            //Estilo de celdas de las 7 columnas
            dgvDatos.DefaultCellStyle.Font = new Font(
            dgvDatos.DefaultCellStyle.Font.Name,
            12, dgvDatos.DefaultCellStyle.Font.Style);
            //dgvDatos.DefaultCellStyle.ForeColor = Color.Red;
            //Inhabilitar redimensionamiento de encabezado de fila
            //dgvDatos.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }
        private void ajustarControles(ref bool eliminaFilas)
        {
            numDatosActual = (int)nudProcesos.Value;
            if (numDatosActual > numDatosAnterior)
            { //Agregar filas

                eliminaFilas = false;
                for (int i = numDatosAnterior; i < numDatosActual; i++)
                {
                    
                    //dgvDatos[0, 0].Value = "Hola";                 
                    dgvDatos.Rows.Add(); //Agrega una fila
                    //Obtener la altura de la fila 0;
                    altoDeFila = dgvDatos.Rows[0].Height;
                    if (i < NUMFILASMOSTRADAS)
                    {
                        //Actualzar factor de ampliación
                        alturaTotal += altoDeFila;
                        //Ajustar altura del contenedor DataGridView
                        dgvDatos.Size = new Size(anchoTabla, alturaTotal);
                        //Ajustar altura del formulario
                        //this.Height += altoDeFila;
                    }
                    //Establecer titulos de encabezados de fila
                    dgvDatos.Rows[i].HeaderCell.Value = "P" + i;
                    //"Congelar" visualización de las filas (No Scroll)
                    //dgvDatos.Rows[numDatosAnterior].Frozen = true;
                }
            }
            else
            { //Eliminar filas
                eliminaFilas = true;
                for (int i = numDatosAnterior; i > numDatosActual; i--)
                {
                   
                    dgvDatos.Rows.RemoveAt(i - 1);
                    if (i <= NUMFILASMOSTRADAS)
                    {
                        
                        //Actualizar factor de reducción
                        alturaTotal -= altoDeFila;
                        //Ajustar altura del contenedor del DataGridView
                        dgvDatos.Size = new Size(anchoTabla, alturaTotal);
                        //Ajustar altura del formulario
                        //this.Height -= altoDeFila;
                    }
                }
            }
            numDatosAnterior = numDatosActual;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            toolTip1.IsBalloon = true;
            bolCargandoFormulario = true;
            inicializarDataGridView();
            
            bolCargandoFormulario = false;


            btnReset.Enabled = false;
            btnSimular.Enabled = false;

            label5.Text = txt1.Text;
            label6.Text = txt2.Text;
            label7.Text = txt3.Text;
            label8.Text = txt4.Text;

            txt5.KeyPress += new KeyPressEventHandler(validarTeclaPulsada);
            txt6.KeyPress +=new KeyPressEventHandler(validarTeclaPulsada);
            txt7.KeyPress +=new KeyPressEventHandler(validarTeclaPulsada);
            txt8.KeyPress += new KeyPressEventHandler(validarTeclaPulsada);
            txt9.KeyPress += new KeyPressEventHandler(validarTeclaPulsada);
            txt10.KeyPress += new KeyPressEventHandler(validarTeclaPulsada);
            txt11.KeyPress += new KeyPressEventHandler(validarTeclaPulsada);
            txt12.KeyPress += new KeyPressEventHandler(validarTeclaPulsada);
            
        }

        private void nudProcesos_ValueChanged(object sender, EventArgs e)
        {
            bool eliminaFilas = false;

            if (!bolCargandoFormulario)
            {
                ajustarControles(ref eliminaFilas);

            }

            switch (nudProcesos.Value)
            {
                case 1:
                    {
                        if (!eliminaFilas)
                        {

                            txt1.Enabled = true;
                            txt5.Enabled = true;
                            txt9.Enabled = true;
                            txt1.Focus();
                        }
                        else
                        {
                            txt2.Enabled = false;
                            txt6.Enabled = false;
                            txt10.Enabled = false;
                        }
                    }
                    break;


                case 2:
                    {
                        if (!eliminaFilas)
                        {
                            txt2.Enabled = true;
                            txt6.Enabled = true;
                            txt10.Enabled = true;
                            txt2.Focus();
                        }
                        else
                        {
                            txt3.Enabled = false;
                            txt7.Enabled = false;
                            txt11.Enabled = false;
                        }
                    }
                    break;

                case 3:
                    {
                        if (!eliminaFilas)
                        {
                            txt3.Enabled = true;
                            txt7.Enabled = true;
                            txt11.Enabled = true;
                            txt3.Focus();
                        }
                        else
                        {
                            txt4.Enabled = false;
                            txt8.Enabled = false;
                            txt12.Enabled = false;
                        }
                    }
                    break;

                case 4:
                    {
                        if (!eliminaFilas)
                        {
                            txt4.Enabled = true;
                            txt8.Enabled = true;
                            txt12.Enabled = true;
                            txt4.Focus();
                        }
                    }
                    break;

                default:
                    txt1.Enabled = false;
                    txt5.Enabled = false;
                    txt9.Enabled = false;

                    break;
            }
        }

        private void dgvDatos_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvDatos_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Escribir solo numeros en celdas
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == (Char)Keys.Back)
                e.Handled = false;
            else
                e.Handled = true;

        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (this.dgvDatos.Rows.Count == 0)
            {
                this.nudProcesos.Focus();
                MessageBox.Show("Debe ingresar procesos para poder hacer el calculo", "Mensaje", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
            }
            else if ((txt5.Text == "") || (txt6.Text == "") || (txt7.Text == "") || (txt8.Text == "")
                || (txt9.Text == "") || (txt10.Text == "") || (txt11.Text == "") || (txt12.Text == "") || (nudProcesos.Value < 4))
            {
                this.btnSimular.Enabled = false;
                this.btnReset.Enabled = false;


                MessageBox.Show("Debe ingresar los valores en todos los recuadros e ingresar el número de procesos", "Mensaje", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                this.btnSimular.Enabled = true;
                this.btnReset.Enabled = true;
                this.btnReset.Enabled = true;

                //calculo del tiempo de finalizacion
                double ts0 = double.Parse(txt9.Text), ts1 = double.Parse(txt10.Text), ts2 = double.Parse(txt11.Text), ts3 = double.Parse(txt12.Text);
                double tf0, tf1, tf2, tf3 = 0;
                tf0 = double.Parse(txt5.Text) + ts0;
                tf1 = ts1 + tf0;
                tf2 = ts2 + tf1;
                tf3 = ts3 + tf2;

                //calculo del tiempo de retorno
                double tr0, tr1, tr2, tr3 = 0;
                tr0 = tf0 - double.Parse(txt5.Text);
                tr1 = tf1 - double.Parse(txt6.Text);
                tr2 = tf2 - double.Parse(txt7.Text);
                tr3 = tf3 - double.Parse(txt8.Text);

                //Calculo del tiempo de espera
                double te0, te1, te2, te3 = 0;
                te0 = tr0 - ts0;
                te1 = tr1 - ts1;
                te2 = tr2 - ts2;
                te3 = tr3 - ts3;

                //Calculo de tiempo NR
                double tnr0, tnr1, tnr2, tnr3 = 0;
                tnr0 = (tr0 / ts0);
                tnr1 = tr1 / ts1;
                tnr2 = tr2 / ts2;
                tnr3 = tr3 / ts3;

                //asignacion de tiempo de servicio
                dgvDatos[3, 0].Value = txt9.Text;
                dgvDatos[3, 1].Value = txt10.Text;
                dgvDatos[3, 2].Value = txt11.Text;
                dgvDatos[3, 3].Value = txt12.Text;

                //Asignacion de tiempo de llegada
                dgvDatos[1, 0].Value = txt5.Text;
                dgvDatos[1, 1].Value = txt6.Text;
                dgvDatos[1, 2].Value = txt7.Text;
                dgvDatos[1, 3].Value = txt8.Text;

                //asignacion de tiempo de finalizacion
                dgvDatos[2, 0].Value = tf0;
                dgvDatos[2, 1].Value = tf1;
                dgvDatos[2, 2].Value = tf2;
                dgvDatos[2, 3].Value = tf3;

                //Asignacion de tiempo de retorno
                dgvDatos[5, 0].Value = tr0;
                dgvDatos[5, 1].Value = tr1;
                dgvDatos[5, 2].Value = tr2;
                dgvDatos[5, 3].Value = tr3;

                //Asignacion de tiempo de espera
                dgvDatos[4, 0].Value = te0;
                dgvDatos[4, 1].Value = te1;
                dgvDatos[4, 2].Value = te2;
                dgvDatos[4, 3].Value = te3;

                //Asignacion de Nombre proceso
                dgvDatos[0, 0].Value = txt1.Text;
                dgvDatos[0, 1].Value = txt2.Text;
                dgvDatos[0, 2].Value = txt3.Text;
                dgvDatos[0, 3].Value = txt4.Text;

            }
        }

        private void dgvDatos_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //Evento que activa para escrivir solo numeros
            TextBox txtValidar = (TextBox)e.Control;
            txtValidar.KeyDown += new
            KeyEventHandler(inhabilitarTeclasDireccionSuprimir);
            txtValidar.KeyPress += new
            KeyPressEventHandler(validarTeclaPulsada);
        
        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            timer1.Start();
            btnSimular.Enabled = false;
            
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            label5.Text = txt1.Text;
        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {
            label6.Text = txt2.Text;
        }

        private void txt3_TextChanged(object sender, EventArgs e)
        {
            label7.Text = txt3.Text;
        }

        private void txt4_TextChanged(object sender, EventArgs e)
        {
            label8.Text = txt4.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int z1 = int.Parse(txt9.Text);
            int z2 = int.Parse(txt10.Text);
            int z3 = int.Parse(txt11.Text);
            int z4 = int.Parse(txt12.Text);

            timer1.Interval = 1000;
            //proceso 1
            if (progressBar1.Value < 100)
            {
                progressBar1.Increment(1);
                progressBar1.Maximum = z1;
                label9.Text = txt9.Text;

            }

            //para el proceso 2
            if (progressBar2.Value < 100)
            {
                progressBar2.Increment(1);
                progressBar2.Maximum = z2;
                label10.Text = txt10.Text;
            }

            //para proceso 3
            if (progressBar3.Value < 100)
            {
                progressBar3.Increment(1);
                progressBar3.Maximum = z3;
                label11.Text = txt11.Text;
            }

            //para proceso 4
            if (progressBar4.Value < 100)
            {
                progressBar4.Increment(1);
                progressBar4.Maximum = z4;
                label12.Text = txt12.Text;
            }

            //para tiempo de finalizacion progressbar 5
            if (progressBar5.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[2, 0].Value);
                progressBar5.Increment(1);
                progressBar5.Maximum = f1;
                label13.Text = Convert.ToString(dgvDatos[2, 0].Value);
            }

            //para finalizacion 2
            if (progressBar6.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[2, 1].Value);
                progressBar6.Increment(1);
                progressBar6.Maximum = f1;
                label14.Text = Convert.ToString(dgvDatos[2, 1].Value);
            }

            //para finalizacion 3
            if (progressBar7.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[2, 2].Value);
                progressBar7.Increment(1);
                progressBar7.Maximum = f1;
                label15.Text = Convert.ToString(dgvDatos[2, 2].Value);
            }

            //para finalizacion 4
            if (progressBar8.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[2, 3].Value);
                progressBar8.Increment(1);
                progressBar8.Maximum = f1;
                label16.Text = Convert.ToString(dgvDatos[2, 3].Value);
            }

            //para los tiempos de retorno
            if (progressBar9.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[5, 0].Value);
                progressBar9.Increment(1);
                progressBar9.Maximum = f1;
                label17.Text = Convert.ToString(dgvDatos[5, 0].Value);
            }

            //tiempo de reorno 2
            if (progressBar10.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[5, 1].Value);
                progressBar10.Increment(1);
                progressBar10.Maximum = f1;
                label18.Text = Convert.ToString(dgvDatos[5, 1].Value);
            }

            //tiempo de retorno 3
            if (progressBar11.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[5, 2].Value);
                progressBar11.Increment(1);
                progressBar11.Maximum = f1;
                label19.Text = Convert.ToString(dgvDatos[5, 2].Value);
            }

            //tiempo de retorno 4
            if (progressBar12.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[5, 3].Value);
                progressBar12.Increment(1);
                progressBar12.Maximum = f1;
                label20.Text = Convert.ToString(dgvDatos[5, 3].Value);
            }

            //Para tiempo de espera
            if (progressBar13.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[4, 0].Value);
                progressBar13.Increment(1);
                progressBar13.Maximum = f1;
                label24.Text = Convert.ToString(dgvDatos[4, 0].Value);
            }
            //espera 2
            if (progressBar14.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[4, 1].Value);
                progressBar14.Increment(1);
                progressBar14.Maximum = f1;
                label25.Text = Convert.ToString(dgvDatos[4, 1].Value);
            }
            //espera 3
            if (progressBar15.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[4, 2].Value);
                progressBar15.Increment(1);
                progressBar15.Maximum = f1;
                label26.Text = Convert.ToString(dgvDatos[4, 2].Value);
            }
            //espera 4
            if (progressBar16.Value < 100)
            {
                int f1 = Convert.ToInt32(dgvDatos[4, 3].Value);
                progressBar16.Increment(1);
                progressBar16.Maximum = f1;
                label27.Text = Convert.ToString(dgvDatos[4, 3].Value);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;
            progressBar4.Value = 0;
            progressBar5.Value = 0;
            progressBar6.Value = 0;
            progressBar7.Value = 0;
            progressBar8.Value = 0;
            progressBar9.Value = 0;
            progressBar10.Value = 0;
            progressBar11.Value = 0;
            progressBar12.Value = 0;
            progressBar13.Value = 0;
            progressBar14.Value = 0;
            progressBar15.Value = 0;
            progressBar16.Value = 0;
        }

        private void progressBar17_Click(object sender, EventArgs e)
        {

        }

        private void progressBar18_Click(object sender, EventArgs e)
        {

        }

        private void progressBar19_Click(object sender, EventArgs e)
        {

        }

        private void progressBar20_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAcercade_Click(object sender, EventArgs e)
        {
            //AboutBox1 ac = new AboutBox1();
            //ac.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", "http://www.facebook.com/juan.g.banderas"); 
        }

        

        
       

        
    }
}

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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUsuario.Text))
            {
                this.txtUsuario.Focus();
                MessageBox.Show("Ingrese usuario","Mensaje del sistema",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.txtPassword.Focus();
                MessageBox.Show("Ingrese contraseña", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (this.txtUsuario.Text == "admin" && this.txtPassword.Text == "admin")
                {
                    MessageBox.Show("Ingreso correctamente", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                    var frmPrincipal = new FrmProcesos();
                    frmPrincipal.Show();
                }
                else
                {
                    MessageBox.Show("Usuario y/o contraseña incorrecta", "Mensaje del sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}

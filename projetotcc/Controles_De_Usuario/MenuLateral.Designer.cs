namespace projetotcc.Controles_De_Usuario
{
    partial class MenuLateral
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuLateral));
            this.labelCodigoBarras = new System.Windows.Forms.Label();
            this.labelFuncionarios = new System.Windows.Forms.Label();
            this.labelRegistros = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCodigoBarras
            // 
            this.labelCodigoBarras.AutoSize = true;
            this.labelCodigoBarras.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelCodigoBarras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCodigoBarras.Font = new System.Drawing.Font("Arial Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCodigoBarras.ForeColor = System.Drawing.Color.White;
            this.labelCodigoBarras.Location = new System.Drawing.Point(3, 676);
            this.labelCodigoBarras.Name = "labelCodigoBarras";
            this.labelCodigoBarras.Size = new System.Drawing.Size(368, 169);
            this.labelCodigoBarras.TabIndex = 6;
            this.labelCodigoBarras.Text = "CÓDIGOS DE\r\nBARRAS";
            this.labelCodigoBarras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelCodigoBarras.Click += new System.EventHandler(this.irParaCodigoDeBarras);
            // 
            // labelFuncionarios
            // 
            this.labelFuncionarios.AutoSize = true;
            this.labelFuncionarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFuncionarios.Font = new System.Drawing.Font("Arial Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFuncionarios.ForeColor = System.Drawing.Color.White;
            this.labelFuncionarios.Location = new System.Drawing.Point(3, 507);
            this.labelFuncionarios.Name = "labelFuncionarios";
            this.labelFuncionarios.Size = new System.Drawing.Size(368, 169);
            this.labelFuncionarios.TabIndex = 5;
            this.labelFuncionarios.Text = "FUNCIONÁRIOS";
            this.labelFuncionarios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelFuncionarios.Click += new System.EventHandler(this.irParaFuncionarios);
            // 
            // labelRegistros
            // 
            this.labelRegistros.AutoSize = true;
            this.labelRegistros.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelRegistros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRegistros.Font = new System.Drawing.Font("Arial Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRegistros.ForeColor = System.Drawing.Color.White;
            this.labelRegistros.Location = new System.Drawing.Point(3, 338);
            this.labelRegistros.Name = "labelRegistros";
            this.labelRegistros.Size = new System.Drawing.Size(368, 169);
            this.labelRegistros.TabIndex = 4;
            this.labelRegistros.Text = "REGISTROS";
            this.labelRegistros.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelRegistros.Click += new System.EventHandler(this.irParaRegistros);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(21)))), ((int)(((byte)(38)))));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelRegistros, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelFuncionarios, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelCodigoBarras, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.9149F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.95745F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.95744F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.95745F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.15958F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.053192F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(374, 1062);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(368, 332);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBox4, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 848);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(368, 154);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(41, 18);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(288, 117);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.btnVoltar);
            // 
            // MenuLateral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MenuLateral";
            this.Size = new System.Drawing.Size(374, 1062);
            this.SizeChanged += new System.EventHandler(this.MenuLateral_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCodigoBarras;
        private System.Windows.Forms.Label labelFuncionarios;
        private System.Windows.Forms.Label labelRegistros;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox4;
    }
}

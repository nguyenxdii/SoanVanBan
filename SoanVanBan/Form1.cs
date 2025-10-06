using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoanVanBan
{
    public partial class frm_SoanVanBan : Form
    {
        public frm_SoanVanBan()
        {
            InitializeComponent();
        }

        private void loadFont()
        {
            foreach (FontFamily fontFamily in new InstalledFontCollection().Families)
            {
                cmbFont.Items.Add(fontFamily.Name);
            }
            cmbFont.SelectedItem = "Tahoma";
        }

        private void loadSize()
        {
            int[] sizeValues = new int[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbSize.ComboBox.DataSource = sizeValues;
            cmbSize.ComboBox.SelectedItem = 14;
        }

        private void frmSoanVanBan_Load(object sender, EventArgs e)
        {
            loadFont();
            loadSize();

            var f = new Font("Tahoma", 14, FontStyle.Regular);
            rtbContent.Font = f;
            rtbContent.SelectionFont = f;

            cmbFont.SelectedIndexChanged += cmbFont_SelectedIndexChanged;
            cmbSize.SelectedIndexChanged += cmbSize_SelectedIndexChanged;
        }

        private void NewFile()
        {
            rtbContent.Clear();
            cmbFont.SelectedItem = "Tahoma";
            cmbSize.ComboBox.SelectedItem = 14;

            var f = new Font("Tahoma", 14, FontStyle.Regular);
            rtbContent.Font = f;
            rtbContent.SelectionFont = f;
        }

        private void Openfile()
        {
            rtbContent.Clear();
            ofdOpen.CheckFileExists = true;
            ofdOpen.CheckPathExists = true;
            ofdOpen.Filter = "Text files (*.txt)|*.txt|RichText files (*.rtf)|*.rtf";
            ofdOpen.Multiselect = false;

            if (ofdOpen.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = ofdOpen.FileName;
                try
                {
                    if (Path.GetExtension(selectedFileName).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        rtbContent.LoadFile(selectedFileName, RichTextBoxStreamType.PlainText);
                    }
                    else
                    {
                        rtbContent.LoadFile(selectedFileName, RichTextBoxStreamType.RichText);
                    }

                    MessageBox.Show("Tập tin đã được mở thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi trong quá trình mở tập tin: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveFile()
        {
            sfdSave.Filter = "RichText files (*.rtf)|*.rtf|Text files (*.txt)|*.txt";
            sfdSave.AddExtension = true;
            sfdSave.DefaultExt = "rtf";

            if (sfdSave.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = sfdSave.FileName;
                    if (Path.GetExtension(path).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        rtbContent.SaveFile(path, RichTextBoxStreamType.PlainText);
                    }
                    else
                    {
                        rtbContent.SaveFile(path, RichTextBoxStreamType.RichText);
                    }

                    MessageBox.Show("Lưu tập tin thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể lưu: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void mni_System_New_Click(object sender, EventArgs e) => NewFile();

        private void btnNew_Click(object sender, EventArgs e) => NewFile();

        private void mni_System_Open_Click(object sender, EventArgs e) => Openfile();

        private void btnOpen_Click(object sender, EventArgs e) => Openfile();

        private void mni_System_Save_Click(object sender, EventArgs e) => SaveFile();

        private void btnSave_Click(object sender, EventArgs e) => SaveFile();

        private void mni_System_Exit_Click(object sender, EventArgs e) => this.Close();

        private void ToggleFontStyle(FontStyle styleToToggle)
        {
            if (rtbContent.SelectionFont != null)
            {
                FontStyle currentStyle = rtbContent.SelectionFont.Style;

                if (rtbContent.SelectionFont.Style.HasFlag(styleToToggle))
                    currentStyle &= ~styleToToggle;
                else
                    currentStyle |= styleToToggle;

                rtbContent.SelectionFont = new Font(rtbContent.SelectionFont, currentStyle);
            }
        }

        private void btnBold_Click(object sender, EventArgs e) => ToggleFontStyle(FontStyle.Bold);
        private void btnItalic_Click(object sender, EventArgs e) => ToggleFontStyle(FontStyle.Italic);
        private void btnUnderline_Click(object sender, EventArgs e) => ToggleFontStyle(FontStyle.Underline);
        private void mni_Format_Font_Click(object sender, EventArgs e)
        {
            fdlFont.ShowColor = true;
            fdlFont.ShowApply = true;
            fdlFont.ShowEffects = true;
            fdlFont.ShowHelp = true;

            if (fdlFont.ShowDialog() != DialogResult.Cancel)
            {
                rtbContent.SelectionFont = fdlFont.Font;
                rtbContent.SelectionColor = fdlFont.Color;
            }
        }

        private void cmbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFont.SelectedItem == null) return;

            var baseFont = rtbContent.SelectionFont ?? rtbContent.Font;

            string family = cmbFont.SelectedItem.ToString();
            rtbContent.SelectionFont = new Font(family, baseFont.Size, baseFont.Style);
        }

        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSize.SelectedItem == null) return;

            var baseFont = rtbContent.SelectionFont ?? rtbContent.Font;

            float size = Convert.ToSingle(cmbSize.SelectedItem);
            rtbContent.SelectionFont = new Font(baseFont.FontFamily, size, baseFont.Style);
        }
    }
}

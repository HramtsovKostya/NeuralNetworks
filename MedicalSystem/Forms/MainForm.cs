using NeuralNetworks.Model;
using System.Windows.Forms;

namespace MedicalSystem
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        private void AboutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void ImageToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var pictureConverter = new PictureConverter();
                var inputs = pictureConverter.Convert(openFileDialog.FileName);
                var result = Program.Controller.ImageNetwork.Predict(inputs).Output;
            }
        }

        private void EnterToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var enterDataForm = new EnterData();
            var result = enterDataForm.ShowForm();
        }

        private void ExitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
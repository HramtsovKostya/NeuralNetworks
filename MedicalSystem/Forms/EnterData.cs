using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace MedicalSystem
{
    public partial class EnterData : Form
    {
        private readonly List<TextBox> Inputs = new List<TextBox>();

        public EnterData()
        {
            InitializeComponent();
            var properties = typeof(Patient).GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var textBox = CreateTextBox(i, property);
                
                Controls.Add(textBox);
                Inputs.Add(textBox);
            }
        }

        public bool? ShowForm()
        {
            var form = new EnterData();

            if (form.ShowDialog() == DialogResult.OK)
            {
                var patient = new Patient();

                foreach (var textBox in form.Inputs)
                {
                    double.TryParse(textBox.Text, out double result);

                    patient.GetType().InvokeMember(textBox.Tag.ToString(), 
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, 
                        Type.DefaultBinder, patient, new object[] { result });
                }

                //var result = Program.Controller.DataNetwork.Predict().Output;
                return false; //result == 1.0;
            }
            return null;
        }

        private TextBox CreateTextBox(int number, PropertyInfo property)
        {
            var y = number * 25 + 12;

            var textBox = new TextBox
            {
                Anchor = AnchorStyles.Top
                    | AnchorStyles.Left
                    | AnchorStyles.Right,
                Location = new Point(12, y),
                Name = $"textBox{number + 1}",
                Size = new Size(360, 20),
                TabIndex = number,
                Text = property.Name,
                Tag = property.Name,
                Font = new Font("Microsoft Sans Serif",
                    7.8F, FontStyle.Italic, 
                    GraphicsUnit.Point, 204),
                ForeColor = Color.Gray
        };

            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;

            return textBox;
        }       

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
                textBox.Font = new Font("Microsoft Sans Serif",
                    7.8F, FontStyle.Regular, 
                    GraphicsUnit.Point, 204);
                textBox.ForeColor = Color.Black;                
            }
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox.Text == "")
            {
                textBox.Text = textBox.Tag.ToString();
                textBox.Font = new Font("Microsoft Sans Serif",
                    7.8F, FontStyle.Italic, 
                    GraphicsUnit.Point, 204);
                textBox.ForeColor = Color.Gray;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
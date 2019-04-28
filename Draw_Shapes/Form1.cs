using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw_Shapes
{
    public partial class Form1 : Form
    {
        Color color;
        string shape;
        Stack<MyShape> undoHistory = new Stack<MyShape>();
        Stack<MyShape> redoHistory = new Stack<MyShape>();


        public Form1()
        {
            InitializeComponent();
        }

        // Select Color
        private void ComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string colorName = Convert.ToString(comboBox1.SelectedItem);
            color = Color.FromName(colorName);
            
        }

        // Draw Shape
        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            SolidBrush sb = new SolidBrush(color);
            Graphics g = panel1.CreateGraphics();
            Rectangle r = new Rectangle(new Point(e.X - 50, e.Y - 50), new
                Size(100, 100));
            try
            {
                if (shape.Equals("Rectangle"))
                {
                    g.FillRectangle(sb, r);
                }
                else
                {
                    g.FillEllipse(sb, r);
                }
                button1.Enabled = true;
                undoHistory.Push(new MyShape(shape, r, color));

            } catch (NullReferenceException)
            {

            }
            redoHistory = new Stack<MyShape>();
            button2.Enabled = false;
        }

        // Select Shape
        private void ComboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            shape = Convert.ToString(comboBox2.SelectedItem);
        }

        // Undo Button
        private void Button1_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(BackColor);

            redoHistory.Push(undoHistory.Pop());
            Stack<MyShape> copy = new Stack<MyShape>(undoHistory);
            while (copy.Count > 0)
            {
                MyShape sh = copy.Pop();
                SolidBrush sb = new SolidBrush(sh.Col);
                if (sh.Shape.Equals("Rectangle"))
                {
                    g.FillRectangle(sb, sh.Rectangle);
                }
                else
                {
                    g.FillEllipse(sb, sh.Rectangle);
                }
            }
            if (undoHistory.Count == 0)
            {
                button1.Enabled = false;
            }
            button2.Enabled = true;
        }

        // Redo Button
        private void Button2_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();

            MyShape sh = redoHistory.Pop();
            undoHistory.Push(sh);
            SolidBrush sb = new SolidBrush(sh.Col);
            if (sh.Shape.Equals("Rectangle"))
            {
                g.FillRectangle(sb, sh.Rectangle);
            }
            else
            {
                g.FillEllipse(sb, sh.Rectangle);
            }

            if (redoHistory.Count == 0)
            {
                button2.Enabled = false;
            }
        }

        // Reset Button
        private void Button3_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(BackColor);

            undoHistory = new Stack<MyShape>();
            redoHistory = new Stack<MyShape>();

            button1.Enabled = false;
            button2.Enabled = false;
        }
    }

    public class MyShape
    {
        public string Shape { get; }
        public Rectangle Rectangle { get; }
        public Color Col { get; }
        public MyShape(string shape, Rectangle rectangle, Color color)
        {
            this.Shape = shape;
            this.Rectangle = rectangle;
            this.Col = color;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Influence_Maps
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<int, int> nodeList;
        Graphics g;
        const int WIDTH = 50;
        const int HEIGHT = WIDTH;

        private void button1_Click(object sender, EventArgs e)
        {
            g = this.CreateGraphics();
            g.Clear(System.Drawing.Color.White);
            Pen pen = new Pen(System.Drawing.Color.Red);

             nodeList = new Dictionary<int, int>();

            for (int i = 0; i <= WIDTH * 10; i = i + 10)
            {
                g.DrawLine(pen, i, 0, i, WIDTH * 10);
                g.DrawLine(pen, 0, i, HEIGHT * 10, i);
                for (int j = 0; j < HEIGHT * 10; j = j + 10)
                    nodeList[toPoint(i, j)] = 0;
            }

            int xtemp;
            int ytemp;
            int startPoint;
            List<int> goalNodes = new List<int>();
            foreach (DataGridViewRow row in nodeGrid.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0] != null)
                {
                    if (row.Cells[0].Value != null && int.TryParse(row.Cells[0].Value.ToString(), out xtemp) && int.TryParse(row.Cells[1].Value.ToString(), out ytemp) && xtemp < WIDTH && xtemp >= 0 && ytemp < HEIGHT && ytemp >= 0)
                    {
                        startPoint = toPoint(xtemp * 10, ytemp * 10);
                        goalNodes.Add(startPoint);
                    }
                    else
                    {
                        statusWrite("Point (" + row.Cells[0].Value.ToString() + ", " + row.Cells[1].Value.ToString() + ") does not exist");
                    }
                }
            }

            for (int i = 0; i < goalNodes.Count; i++)
            {
                for (int j = i+1; j < goalNodes.Count; j++)
                {
                    findPath(goalNodes[i], goalNodes[j]);
                    //statusWrite("from: " + i + "\tto: " + j);
                }
            }

            for (int i = 0; i < goalNodes.Count; i++)
            {
                nodeList[goalNodes[i]] = -2;
            }

            fillNodes();
            
        }

        private int toPoint(int i, int j)
        {
            return i * 1000 + j;
        }

        private void statusWrite(string str)
        {
            statusBox.Text = statusBox.Text + str + "\n";
        }


        private void findPath(int startPoint, int goalPoint)
        {
            Queue<int> q = new Queue<int>();
            Dictionary<int, int> parent = new Dictionary<int, int>();
            q.Enqueue(startPoint);
            int nextPoint = q.Dequeue();
            parent[nextPoint] = nextPoint;
            while (goalPoint != nextPoint)
            {
                if ((!parent.ContainsKey(nextPoint + toPoint(-10, 0))) && nodeList.ContainsKey(nextPoint + toPoint(-10, 0)))
                {
                    parent[nextPoint + toPoint(-10, 0)] = nextPoint;
                    q.Enqueue(nextPoint + toPoint(-10, 0));
                }
                if ((!parent.ContainsKey(nextPoint + toPoint(0, -10))) && nodeList.ContainsKey(nextPoint + toPoint(0, -10)))
                {
                    parent[nextPoint + toPoint(0, -10)] = nextPoint;
                    q.Enqueue(nextPoint + toPoint(0, -10));
                }
                if ((!parent.ContainsKey(nextPoint + toPoint(0, 10))) && nodeList.ContainsKey(nextPoint + toPoint(0, 10)))
                {
                    parent[nextPoint + toPoint(0, 10)] = nextPoint;
                    q.Enqueue(nextPoint + toPoint(0, 10));
                }
                if ((!parent.ContainsKey(nextPoint + toPoint(10, 0))) && nodeList.ContainsKey(nextPoint + toPoint(10, 0)))
                {
                    parent[nextPoint + toPoint(10, 0)] = nextPoint;
                    q.Enqueue(nextPoint + toPoint(10, 0));
                }
                nextPoint = q.Dequeue();
                //nodeList[nextPoint] = -4;
            }

            nextPoint = parent[goalPoint];
            while (nextPoint != startPoint)
            {
                nodeList[nextPoint] = -5;
                nextPoint = parent[nextPoint];
            }
        }


        private void fillNodes()
        {
            Brush blueBrush = new SolidBrush(System.Drawing.Color.Blue);
            Brush greenBrush = new SolidBrush(System.Drawing.Color.Green);
            Brush yellowBrush = new SolidBrush(System.Drawing.Color.Yellow);
            Brush purpleBrush = new SolidBrush(System.Drawing.Color.Purple);
            Brush orangeBrush = new SolidBrush(System.Drawing.Color.Orange);

            for (int x = 0; x < WIDTH * 10; x = x + 10)
            {
                for (int y = 0; y < HEIGHT * 10; y = y + 10)
                {
                    Rectangle toFill = new Rectangle(x + 1, y + 1, 9, 9);
                    switch (nodeList[toPoint(x, y)])
                    {
                        
                        case -1:
                            g.FillRectangle(blueBrush, toFill);
                            break;
                        case -2:
                            g.FillRectangle(greenBrush, toFill);
                            break;
                        case -3:
                            //g.FillRectangle(yellowBrush, toFill);
                            break;
                        case -4:
                            //g.FillRectangle(purpleBrush, toFill);
                            break;
                        case -5:
                            g.FillRectangle(orangeBrush, toFill);
                            break;
                    }

                }
            }
        }


    }

    
}

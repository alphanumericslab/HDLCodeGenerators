using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;



namespace t01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        //button1_Click is Main call
        private void button1_Click(object sender, EventArgs e)
        {
          
            int number_of_input = int.Parse(textBox2.Text);
            int number_of_output = int.Parse(textBox5.Text);
            int number_of_node = int.Parse(textBox12.Text);
            int number_of_hidden_Layer = int.Parse(textBox1.Text);
            int clk_max = int.Parse(textBox7.Text);
            int clk_data=int.Parse(textBox6.Text);
            int number_of_mult = (int) Math.Ceiling((number_of_input * number_of_node*1.00) / (clk_max / clk_data));
            int length_i_m=int.Parse(textBox3.Text);
            int length_i_n=int.Parse(textBox9.Text);
            int length_i=length_i_m+length_i_n;//(s+m)+n
            int length_w_m=int.Parse(textBox11.Text);
            int length_w_n=int.Parse(textBox10.Text);
            int length_w=length_w_m+length_w_n;//(s+m)+n
            int length_o = length_i;
            int Depth_Sample = int.Parse(textBox13.Text);
            string path = label5.Text;
            string path_WeightIn = label9.Text;

            //Create Weight
            ConvertFile2Binary_NotPoint(path_WeightIn,path, "\\Weight_Out.txt", length_w_m, length_w_n);
            string path_WeightOut = path + "\\Weight_Out.txt";

            Create_TopModule(number_of_input, number_of_hidden_Layer, number_of_node, number_of_output, length_i, length_w, clk_max, clk_data, path, path_WeightOut);
            Create_Layer(number_of_node, number_of_node, number_of_hidden_Layer, number_of_mult, length_i, length_w, length_o, path);
            Create_Layer1(number_of_input, number_of_node, number_of_hidden_Layer, number_of_mult, length_i, length_w, length_o, path);
            Create_Layer_End(number_of_node, number_of_output, number_of_hidden_Layer, number_of_mult, length_i, length_w, length_o, path);
            //Create_Layer_End(number_of_input, number_of_output, number_of_hidden_Layer, number_of_mult, length_i, length_w, length_o, path);
            Create_AF(number_of_node, length_i_m + length_w_m, length_i_n + length_w_n, length_i_m, length_i_n, length_w, path);
            Create_AF_End(number_of_output, length_i_m + length_w_m, length_i_n + length_w_n, length_i_m, length_i_n,length_w, path);
            Create_mult(length_i, length_w, path);
            //Create_Function_Interpolation(length_i + length_w, length_i, Depth_Sample, path);

            if (comboBox1.SelectedItem.ToString() == "Hard-Limit (0,1)")
                Create_Function_HardLimit01(length_i + length_w, length_i, Depth_Sample, path);
            if (comboBox1.SelectedItem.ToString() == "Hard-Limit (-1,1)")
                Create_Function_HardLimit11(length_i + length_w, length_i, Depth_Sample, path);
            if (comboBox1.SelectedItem.ToString() == "Linear ( / )")
                Create_Function_Linear(length_i + length_w, length_i, Depth_Sample, path);
            if (comboBox1.SelectedItem.ToString() == "Log-sigmoeid (1/1+e^-n)")
                Create_Function_Interpolation(length_i + length_w, length_i, Depth_Sample, path);

            //create test bench
            Create_Test_TopModule(number_of_input, number_of_output, length_i, path);
            Create_Tcl(path);
            Create_bat_file(path);
            Create_Samples_file(path);
            //////////////string add1 = label5.Text + "\\B1.bat";
            //////////////System.Diagnostics.Process.Start(@add1);
            
       
        }


        public  string convert(int input)
        {
            string s;
            if (input < 10)
                s = "00" + input.ToString();
            else
                if (input < 100)
                s = "0" + input.ToString();
            else
                    s = input.ToString();
            return s;      
        }
        public string  sum_bit()
        {
            int z,y,o;
            z=int.Parse(textBox3.Text)*2+1;//lengh*2+1
            y=int.Parse(textBox2.Text);//input

            if (y >= 0 && y <= 2)
                o = z + 1;
            else if (y >= 3 && y <= 4)
                o = z + 2;
            else if (y >= 5 && y <= 8)
                o = z + 3;
            else if (y >= 9 && y <= 16)
                o = z + 4;
            else if (y >= 17 && y <= 32)
                o = z + 5;
            else if (y >= 33 && y <= 64)
                o = z + 6;
            else if (y >= 65 && y <= 128)
                o = z + 7;
            else if (y >= 129 && y <= 256)
                o = z + 8;
            else if (y >= 257 && y <= 512)
                o = z + 9;
            else if (y >= 513 && y <= 1024)
                o = z + 10;
            else if (y >= 1025 && y <= 2048)
                o = z + 11;
            else
                o = z + 12;
            string o2;
            o2=o.ToString ();
            return o2;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {

                System.Windows.Forms.Application.Exit();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() ==DialogResult.OK)

                       label5.Text = folderBrowserDialog1.SelectedPath;
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text Files|*.txt";
            openFileDialog1.FileName = String.Empty;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
             {
                 label9.Text = openFileDialog1.FileName;
                 label7.Text = openFileDialog1.SafeFileName;
                 label34.Text = label9.Text.Substring(0, label9.Text.Length - label7.Text.Length);
             }
         }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult respons;
            if (label9.Text == "Select file")
                respons = MessageBox.Show("Select a file for weight...", "Err", System.Windows.Forms.MessageBoxButtons.OK);
            else if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox8.Text == "")
                respons = MessageBox.Show("TextBox is empty... Enter a number in: \n\n   Hidden Layer\n   Number of inputs\n   length of bits\n   Number of outputs\n   clk speed\n   Parameter pipeline", "Err", System.Windows.Forms.MessageBoxButtons.OK);
            else
            {
                button1_Click(sender, e);
                string add1 = label5.Text ;
                System.Diagnostics.Process.Start(@add1);
            }
                
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        public void Create_Top()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

           
            string add2 = label9.Text;
            StreamReader f0 = new StreamReader(@add2);

            string add = label5.Text + "\\Top.v";
            TextWriter f1 = new StreamWriter(@add);

            try
            {
                int i;
                int j;
                //////////////////////        Top.v     /////////////////////////
                f1.WriteLine("`timescale 1ns / 1ps");
                f1.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f1.WriteLine("// Engineer:          Pezhman Torabi");
                f1.WriteLine("// Design Name:       Artificial neural network (Perceptron)");
                f1.WriteLine("// Module Name:       " + add);
                f1.WriteLine("// Project Name: ");
                f1.WriteLine("// Target Devices: ");
                f1.WriteLine("// Tool versions: ");
                f1.WriteLine("// Description:        Hidden Layer:       " + textBox1.Text);
                f1.WriteLine("//                     Number of inputs:   " + textBox2.Text);
                f1.WriteLine("//                     length of bits:     " + textBox3.Text);
                f1.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f1.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f1.WriteLine("// Dependencies: ");
                f1.WriteLine("// Create Date:       " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f1.WriteLine("// Version:           1.01");
                f1.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f1.WriteLine("");
                f1.WriteLine("");

                //input------------------------
                f1.Write("module Top(");
                i = int.Parse(textBox1.Text) + 1;
                i = 1;
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f1.Write("o" + convert(i - 1) + convert(k) + ",");

                for (int k = 1; k <= int.Parse(textBox5.Text); k++)
                    f1.Write("Out" + convert(k) + ",");

                f1.WriteLine("clk,en,res);");
                f1.WriteLine("");

                i = int.Parse(textBox1.Text) + 1;
                i = 1;
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f1.WriteLine("      input signed [" + textBox3.Text + ":0] o" + convert(i - 1) + convert(k) + ";");
                for (int k = 1; k <= int.Parse(textBox5.Text); k++)
                    f1.WriteLine("      output Out" + convert(k) + ";");

                f1.WriteLine("      input clk,en,res;");
                f1.WriteLine("");

                //reg w------------------------
                f1.WriteLine("");
                for (i = 1; i <= int.Parse(textBox1.Text); i++)
                {
                    for (j = 1; j <= int.Parse(textBox2.Text); j++)
                    {
                        //Weight
                        for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                            f1.Write("      reg [" + textBox3.Text + ":0] w" + convert(i) + convert(j) + convert(k) + ";");
                        f1.WriteLine("");
                    }
                    f1.WriteLine("");
                }

                i = int.Parse(textBox1.Text) + 1;
                j = 1;
                //Weight
                for (j = 1; j <= int.Parse(textBox5.Text); j++)
                {
                    //Weight
                    for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                        f1.Write("      reg [" + textBox3.Text + ":0] w" + convert(i) + convert(j) + convert(k) + ";");
                    f1.WriteLine("");
                }
                /*for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f1.Write("      reg [" + textBox3.Text + ":0] w" + convert(i) + convert(j) + convert(k) + ";");
                
                 */
                f1.WriteLine("");
                f1.WriteLine("");


                //wire o ------------------------
                f1.WriteLine("//------------------wire o ---------------------");
                f1.WriteLine("");
                for (i = 1; i <= int.Parse(textBox1.Text); i++)
                {
                    for (j = 1; j <= int.Parse(textBox2.Text); j++)
                    {
                        //wire Output
                        f1.Write("      wire [" + textBox3.Text + ":0] o" + convert(i) + convert(j) + ";");
                    }
                    f1.WriteLine("");
                }

                //Initial------------------------
                string StrText;
                //int m = 1;
                f1.WriteLine("");
                f1.WriteLine("//                  |---------------------------------------------------|");
                f1.WriteLine("//                  |                  initial Weight                   |");
                f1.WriteLine("//                  |---------------------------------------------------|");

                f1.WriteLine("initial begin");

                for (i = 1; i <= int.Parse(textBox1.Text); i++)
                {
                    f1.WriteLine("//Weight Node Layer(" + i.ToString() + ")");
                    for (j = 1; j <= int.Parse(textBox2.Text); j++)
                    {
                        //Weight
                        for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                        {
                            StrText = f0.ReadLine().ToString();
                            f1.Write("       w" + convert(i) + convert(j) + convert(k) + "=" + StrText + ";");
                        }
                        f1.WriteLine("");
                    }
                    f1.WriteLine("");
                }

                i = int.Parse(textBox1.Text) + 1;
                //j = 1;
                //Weight
                f1.WriteLine("//Weight Node End Layer");
                for (j = 1; j <= int.Parse(textBox5.Text); j++)
                {
                    //Weight
                    for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    {
                        StrText = f0.ReadLine().ToString();
                        f1.Write("       w" + convert(i) + convert(j) + convert(k) + "=" + StrText + ";");
                    }
                    f1.WriteLine("");
                }

                f1.WriteLine("");
                f1.WriteLine("end");
                f1.WriteLine("");

                //node----------------------------------------------------------------------------------
                //node----------------------------------------------------------------------------------
                f1.WriteLine("//                  |---------------------------------------------------|");
                f1.WriteLine("//                  |                  instance node                    |");
                f1.WriteLine("//                  |---------------------------------------------------|");
                f1.WriteLine("");
                for (i = 1; i <= int.Parse(textBox1.Text); i++)
                {
                    f1.WriteLine("//-------------Hidden Layer(" + i.ToString() + ")---------------------");
                    for (j = 1; j <= int.Parse(textBox2.Text); j++)
                    {

                        f1.Write("      nod nod_" + convert(i) + convert(j) + "(");
                        //Input 
                        for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                            f1.Write("o" + convert(i - 1) + convert(k) + ",");
                        //Weight
                        for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                            f1.Write("w" + convert(i) + convert(j) + convert(k) + ",");
                        //Output
                        f1.Write("o" + convert(i) + convert(j));
                        f1.WriteLine(",clk,en,res);");
                    }
                    f1.WriteLine("");

                }


                //Output nod------------------------
                f1.WriteLine("//------------------End Layer---------------------");

                for (j = 1; j <= int.Parse(textBox5.Text); j++)
                {

                    f1.Write("      nod nod_" + convert(i) + convert(j) + "(");
                    //Input 
                    for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                        f1.Write("o" + convert(i - 1) + convert(k) + ",");
                    //Weight
                    for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                        f1.Write("w" + convert(i) + convert(j) + convert(k) + ",");
                    //Output
                    f1.Write("Out" + convert(j));
                    f1.WriteLine(",clk,en,res);");
                }
                f1.WriteLine("");
                f1.WriteLine("endmodule");
            }
            finally
            {
                f1.Flush();
                f1.Close();
                f0.Close();
            }

            //System.Diagnostics.Process.Start(@add);
        }
        public void Create_nod()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            //////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////nod///////////////////////////////////////////////////


            string add1 = label5.Text + "\\nod.v";
            TextWriter f2 = new StreamWriter(@add1);

            try
            {
                f2.WriteLine("`timescale 1ns / 1ps");
                f2.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f2.WriteLine("// Engineer:           Pezhman Torabi");
                f2.WriteLine("// Design Name:        Artificial neural network (Perceptron)");
                f2.WriteLine("// Module Name:        " + add1);
                f2.WriteLine("// Project Name: ");
                f2.WriteLine("// Target Devices: ");
                f2.WriteLine("// Tool versions: ");
                f2.WriteLine("// Description:        Hidden Layer:       " + textBox1.Text);
                f2.WriteLine("//                     Number of inputs:   " + textBox2.Text);
                f2.WriteLine("//                     length of bits:     " + textBox3.Text);
                f2.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f2.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f2.WriteLine("// Dependencies: ");
                f2.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f2.WriteLine("// Version:            1.01");
                f2.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f2.WriteLine("");
                f2.WriteLine("");

                //input------------------------
                f2.Write("module nod(");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f2.Write("i" + k.ToString() + ",");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f2.Write("w" + k.ToString() + ",");
                f2.WriteLine("Out,clk,en,res);");
                f2.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f2.WriteLine("      input signed [" + textBox3.Text + ":0] i" + k.ToString() + ";");

                f2.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f2.WriteLine("      input signed [" + textBox3.Text + ":0] w" + k.ToString() + ";");

                f2.WriteLine("");
                f2.WriteLine("      output signed [" + textBox3.Text + ":0] Out;");
                f2.WriteLine("      input clk,en,res;");

                f2.WriteLine("");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f2.WriteLine("      reg [" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + ";");

                //////////////////////////////////reg [n:0] temp011
                f2.WriteLine("");
                int m = 0;
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                {
                    f2.WriteLine("      reg [" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + "1;");
                    for (m = 2; m <= int.Parse(textBox8.Text); m++)
                    {
                        f2.WriteLine("      reg [" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + m.ToString() + ";");
                    }
                    f2.WriteLine("");
                }

                /////////////////////////////////

                f2.WriteLine("");
                f2.WriteLine("      reg [" + sum_bit().ToString() + ":0] temp;");
                f2.WriteLine("      reg [" + textBox3.Text + ":0] Out;");
                //reg [3:0] Out;
                f2.WriteLine("");
                f2.WriteLine("      initial Out=0;");
                f2.WriteLine("");
                f2.WriteLine("//----------------");
                f2.WriteLine("");
                f2.WriteLine("");

                //always----------------------
                f2.WriteLine("       always @(posedge clk)");
                f2.WriteLine("       begin");
                f2.WriteLine("             if(en)");
                f2.WriteLine("             begin");
                f2.WriteLine("                   if (res)");
                f2.WriteLine("                          Out <=0;");
                f2.WriteLine("                   else");
                f2.WriteLine("                   begin");

                //f2.Write("              temp<=");
                //Pipelining-------------------------
                f2.WriteLine("");
                m = 0;
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                {
                    f2.WriteLine("                         temp" + convert(k) + "1<=i" + k.ToString() + "*w" + k.ToString() + ";");
                    for (m = 2; m <= int.Parse(textBox8.Text); m++)
                    {
                        f2.WriteLine("                         temp" + convert(k) + m.ToString() + "<=" + "temp" + convert(k) + (m - 1).ToString() + ";");
                    }
                    f2.WriteLine("                         temp" + convert(k) + "<=" + "temp" + convert(k) + (m - 1).ToString() + ";");
                    f2.WriteLine("");
                }
                //end pipelining--------------

                //for (int k = 1; k <= int.Parse(textBox2.Text) ; k++)
                // f2.WriteLine("                         temp" + convert(k) + "<=i" + k.ToString() + "*w" + k.ToString() + ";");
                f2.WriteLine("");
                f2.Write("                         temp<=");
                for (int k = 1; k <= int.Parse(textBox2.Text) - 1; k++)
                    f2.Write("temp" + convert(k) + "+");

                int i = int.Parse(textBox2.Text);
                f2.Write("temp" + convert(i) + ";");

                f2.WriteLine("");
                //label14.Text = comboBox1.SelectedIndex.ToString();
                //if (comboBox1.SelectedIndex == 0)
                f2.WriteLine("                         Out<=~temp[" + sum_bit().ToString() + "];");
                //else
                //    f2.WriteLine("              Out=temp;");
                f2.WriteLine("                    end");
                f2.WriteLine("              end");
                f2.WriteLine("       end");
                f2.WriteLine("");
                f2.WriteLine("");
                f2.WriteLine("endmodule");
                f2.WriteLine("");

            }
            finally
            {
                f2.Flush();
                f2.Close();
            }
            //System.Diagnostics.Process.Start(@add1);

        }
        public void Create_Test_Bench_Top()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
            //////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////////////////////Test_Bench/////////////////////////////////////////////////////

            string add4 = "D:\\Test_Bench_Top111.v";
            TextWriter f3 = new StreamWriter(@add4);

            try
            {
                int i;
                f3.WriteLine("`timescale 1ns / 1ps");
                f3.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f3.WriteLine("// Engineer:           Pezhman Torabi");
                f3.WriteLine("// Design Name:        Artificial neural network (Perceptron)");
                f3.WriteLine("// Module Name:        " + add4);
                f3.WriteLine("// Project Name: ");
                f3.WriteLine("// Target Devices: ");
                f3.WriteLine("// Tool versions: ");
                f3.WriteLine("// Description:        Hidden Layer:       " + textBox1.Text);
                f3.WriteLine("//                     Number of inputs:   " + textBox2.Text);
                f3.WriteLine("//                     length of bits:     " + textBox3.Text);
                f3.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f3.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f3.WriteLine("// Dependencies: ");
                f3.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f3.WriteLine("// Version:            1.01");
                f3.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f3.WriteLine("");
                f3.WriteLine("");

                //input------------------------
                f3.WriteLine("module Test_Bench_Top;");

                f3.WriteLine("");

                i = int.Parse(textBox1.Text) + 1;
                i = 1;
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f3.WriteLine("      reg [" + textBox3.Text + ":0] o" + convert(i - 1) + convert(k) + ";");
                f3.WriteLine("      reg clk;");
                f3.WriteLine("");
                //f3.WriteLine("      wire Out;");
                for (int k = 1; k <= int.Parse(textBox5.Text); k++)
                    f3.WriteLine("      wire Out" + convert(k) + ";");

                f3.WriteLine("");
                f3.WriteLine("      Top uut (");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f3.WriteLine("         .o" + convert(i - 1) + convert(k) + "(o" + convert(i - 1) + convert(k) + "),");
                for (int k = 1; k <= int.Parse(textBox5.Text); k++)
                    f3.WriteLine("         .Out" + convert(k) + "(Out" + convert(k) + "),");
                // f3.WriteLine("         .Out(Out),");
                f3.WriteLine("         .clk(clk)");
                f3.WriteLine("       );");
                f3.WriteLine("");
                f3.WriteLine("");
                f3.WriteLine("    integer f1;");
                f3.WriteLine("");
                f3.WriteLine("    initial begin");
                f3.WriteLine("        f1=$fopen(" + '"' + "Result.txt" + '"' + "," + '"' + "w" + '"' + ");");
                f3.WriteLine("    end");
                f3.WriteLine("");
                f3.WriteLine("");

                f3.WriteLine("    initial begin");
                f3.WriteLine("       clk=0;");
                f3.WriteLine("       forever #" + textBox4.Text + " clk=~clk;");
                f3.WriteLine("    end");
                f3.WriteLine("");
                f3.WriteLine("    initial begin");
                f3.WriteLine("");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f3.WriteLine("         o" + convert(i - 1) + convert(k) + "=0;");
                f3.WriteLine("");
                f3.WriteLine("         #100;");
                f3.WriteLine("");
                f3.WriteLine("    end");
                f3.WriteLine("");

                //Result write in file    --------------------------------
                f3.WriteLine("    always @(posedge clk)");
                f3.Write("        $fwrite (f1," + '"' + "Output01:%d");
                for (int k = 2; k <= int.Parse(textBox5.Text); k++)
                    f3.Write(" | Output" + convert(k) + ":%d");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f3.Write(" | input" + convert(k) + ":%d");

                f3.Write("\\n" + '"');//+ ",Out"
                for (int k = 1; k <= int.Parse(textBox5.Text); k++)
                    f3.Write(",Out" + convert(k));
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f3.Write(",o" + convert(i - 1) + convert(k));

                f3.Write(");");
                f3.WriteLine("");
                f3.WriteLine("");
                f3.WriteLine("endmodule");
            }
            finally
            {
                f3.Flush();
                f3.Close();
            }
            //System.Diagnostics.Process.Start(@add4);
        }
     //////////////////////////////////////////////////////////////////////////////////////////////
        public void Create_nod2_Hard_Limit_01()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add5 = label5.Text + "\\nod1.v";
            TextWriter f4 = new StreamWriter(@add5);

            try
            {
                f4.WriteLine("`timescale 1ns / 1ps");
                f4.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f4.WriteLine("// Engineer:           Pezhman Torabi");
                f4.WriteLine("// Design Name:        Artificial neural network (Perceptron)");
                f4.WriteLine("//                     nod1_Hard_Limit_01");
                f4.WriteLine("// Module Name:        " + add5);
                f4.WriteLine("// Project Name: ");
                f4.WriteLine("// Target Devices: ");
                f4.WriteLine("// Tool versions: ");
                f4.WriteLine("// Description:        Hidden Layer:       " + textBox1.Text);
                f4.WriteLine("//                     Number of inputs:   " + textBox2.Text);
                f4.WriteLine("//                     length of bits:     " + textBox3.Text);
                f4.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f4.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f4.WriteLine("// Dependencies: ");
                f4.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f4.WriteLine("// Version:            1.01");
                f4.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f4.WriteLine("");
                f4.WriteLine("");

                //input------------------------
                f4.Write("module nod1(");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.Write("i" + k.ToString() + ",");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.Write("w" + k.ToString() + ",");
                f4.WriteLine("Out,clk,en,res);");
                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      input signed [" + textBox3.Text + ":0] i" + k.ToString() + ";");

                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      input signed [" + textBox3.Text + ":0] w" + k.ToString() + ";");

                f4.WriteLine("");
                f4.WriteLine("      output signed [" + textBox3.Text + ":0] Out;");
                f4.WriteLine("      input clk,en,res;");

                f4.WriteLine("");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      wire signed [" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] z" + convert(k) + ";");
                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      mult m" + k.ToString() + "(i" + k.ToString() + ",w" + k.ToString() + ",z" + convert(k) + ");");

                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      reg signed[" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + ";");

                //////////////////////////////////
                f4.WriteLine("      reg [" + sum_bit().ToString() + ":0] temp;");
                /////////////////////////////////

                f4.WriteLine("      reg Out;");
                f4.WriteLine("      initial Out=0;");
                f4.WriteLine("");
                f4.WriteLine("//----------------");
                f4.WriteLine("");
                f4.WriteLine("");

                //always----------------------
                f4.WriteLine("       always @(posedge clk)");
                f4.WriteLine("       begin");
                f4.WriteLine("             if(en)");
                f4.WriteLine("             begin");
                f4.WriteLine("                   if (res)");
                f4.WriteLine("                          Out <=0;");
                f4.WriteLine("                   else");
                f4.WriteLine("                   begin");

                //f2.Write("              temp<=");
                //Pipelining-------------------------
                f4.WriteLine("");
                //m = 0;
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                {
                    f4.WriteLine("                         temp" + convert(k) + "<=" + "z" + convert(k) + ";");
                    //f4.WriteLine("");
                }
                //end pipelining--------------

                f4.WriteLine("");
                f4.Write("                         temp<=");
                for (int k = 1; k <= int.Parse(textBox2.Text) - 1; k++)
                    f4.Write("temp" + convert(k) + "+");
                int i = int.Parse(textBox2.Text);
                f4.Write("temp" + convert(i) + ";");
                f4.WriteLine("");

                //label14.Text = comboBox1.SelectedIndex.ToString();
                //if (comboBox1.SelectedIndex == 0)
                f4.WriteLine("                         Out<=~temp[" + sum_bit().ToString() + "];");
                //else
                //    f2.WriteLine("              Out=temp;");
                f4.WriteLine("                    end");
                f4.WriteLine("              end");
                f4.WriteLine("       end");
                f4.WriteLine("");
                f4.WriteLine("");
                f4.WriteLine("endmodule");
                f4.WriteLine("");

            }
            finally
            {
                f4.Flush();
                f4.Close();
            }
            //System.Diagnostics.Process.Start(@add5);
        }
        public void Create_nod1_Hard_Limit_01()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add5 = label5.Text + "\\nod1.v";
            TextWriter f4 = new StreamWriter(@add5);

            try
            {
                f4.WriteLine("`timescale 1ns / 1ps");
                f4.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f4.WriteLine("// Engineer:           Pezhman Torabi");
                f4.WriteLine("// Design Name:        Artificial neural network (Perceptron)");
                f4.WriteLine("//                     nod1_Hard_Limit_01");
                f4.WriteLine("// Module Name:        " + add5);
                f4.WriteLine("// Project Name: ");
                f4.WriteLine("// Target Devices: ");
                f4.WriteLine("// Tool versions: ");
                f4.WriteLine("// Description:        Hidden Layer:       " + textBox1.Text);
                f4.WriteLine("//                     Number of inputs:   " + textBox2.Text);
                f4.WriteLine("//                     length of bits:     " + textBox3.Text);
                f4.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f4.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f4.WriteLine("// Dependencies: ");
                f4.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f4.WriteLine("// Version:            1.01");
                f4.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f4.WriteLine("");
                f4.WriteLine("");

                //input------------------------
                f4.Write("module nod1(");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.Write("i" + k.ToString() + ",");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.Write("w" + k.ToString() + ",");
                f4.WriteLine("Out,clk,en,res);");
                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      input signed [" + textBox3.Text + ":0] i" + k.ToString() + ";");

                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      input signed [" + textBox3.Text + ":0] w" + k.ToString() + ";");

                f4.WriteLine("");
                f4.WriteLine("      output signed [" + textBox3.Text + ":0] Out;");
                f4.WriteLine("      input clk,en,res;");

                f4.WriteLine("");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      wire signed [" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] z" + convert(k) + ";");
                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      mult m" + k.ToString() + "(i" + k.ToString() + ",w" + k.ToString() + ",z" + convert(k) + ");");

                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f4.WriteLine("      reg signed[" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + ";");

                //////////////////////////////////
                f4.WriteLine("      reg [" + sum_bit().ToString() + ":0] temp;");
                /////////////////////////////////

                f4.WriteLine("");
                f4.WriteLine("      initial Out=0;");
                f4.WriteLine("");
                f4.WriteLine("//----------------");
                f4.WriteLine("");
                f4.WriteLine("");

                //always----------------------
                f4.WriteLine("       always @(posedge clk)");
                f4.WriteLine("       begin");
                f4.WriteLine("             if(en)");
                f4.WriteLine("             begin");
                f4.WriteLine("                   if (res)");
                f4.WriteLine("                          Out <=0;");
                f4.WriteLine("                   else");
                f4.WriteLine("                   begin");

                //f2.Write("              temp<=");
                //Pipelining-------------------------
                f4.WriteLine("");
                //m = 0;
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                {
                    f4.WriteLine("                         temp" + convert(k) + "<=" + "z" + convert(k) + ";");
                    //f4.WriteLine("");
                }
                //end pipelining--------------

                f4.WriteLine("");
                f4.Write("                         temp<=");
                for (int k = 1; k <= int.Parse(textBox2.Text) - 1; k++)
                    f4.Write("temp" + convert(k) + "+");
                int i = int.Parse(textBox2.Text);
                f4.Write("temp" + convert(i) + ";");
                f4.WriteLine("");

                //label14.Text = comboBox1.SelectedIndex.ToString();
                //if (comboBox1.SelectedIndex == 0)
                f4.WriteLine("                         Out<=~temp[" + sum_bit().ToString() + "];");
                //else
                //    f2.WriteLine("              Out=temp;");
                f4.WriteLine("                    end");
                f4.WriteLine("              end");
                f4.WriteLine("       end");
                f4.WriteLine("");
                f4.WriteLine("");
                f4.WriteLine("endmodule");


                f4.WriteLine("");



            }
            finally
            {
                f4.Flush();
                f4.Close();
            }
            //System.Diagnostics.Process.Start(@add5);
        }
        public void Create_mul()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();


          //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            string add6 = label5.Text + "\\mult.v";
            TextWriter f5 = new StreamWriter(@add6);

            try
            {
                f5.WriteLine("`timescale 1ns / 1ps");
                f5.WriteLine("");
                //------------------------
                f5.WriteLine("module mult(a,b,z);");
                f5.WriteLine("");
                f5.WriteLine("   (* use_dsp48 =" + '"' + "yes" + '"' + "*)");
                f5.WriteLine("   input [" + textBox3.Text + ":0] a,b;");
                f5.WriteLine("   reg [" + textBox3.Text + ":0] t1,t2;");
                f5.WriteLine("   output [" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] z;");
                f5.WriteLine("");
                //f5.WriteLine("   assign z=a*b;");
                f5.WriteLine("   always @(posedge clk)");
		        f5.WriteLine("     begin");
		        f5.WriteLine("       t1<=a*b;");
		        f5.WriteLine("       t2<=t1;");
                f5.WriteLine("        z<=t2;");
                f5.WriteLine("     end");
                f5.WriteLine("endmodule");
                f5.WriteLine("");

            }
            finally
            {
                f5.Flush();
                f5.Close();
            }
            //System.Diagnostics.Process.Start(@add6);


        }
        public void Create_nod_Hardware_Reuse()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add7 = label5.Text + "\\nod_Hardware_Reuse.v";
            TextWriter f6 = new StreamWriter(@add7);

            try
            {
                f6.WriteLine("`timescale 1ns / 1ps");
                f6.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f6.WriteLine("// Engineer:           Pezhman Torabi");
                f6.WriteLine("// Design Name:        Artificial neural network (Perceptron)");
                f6.WriteLine("// Module Name:        " + add7);
                f6.WriteLine("// Project Name: ");
                f6.WriteLine("// Target Devices: ");
                f6.WriteLine("// Tool versions: ");
                f6.WriteLine("// Description:        Hidden Layer:       " + textBox1.Text);
                f6.WriteLine("//                     Number of inputs:   " + textBox2.Text);
                f6.WriteLine("//                     length of bits:     " + textBox3.Text);
                f6.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f6.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f6.WriteLine("// Dependencies: ");
                f6.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f6.WriteLine("// Version:            1.01");
                f6.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f6.WriteLine("");
                f6.WriteLine("");

                //input------------------------
                f6.Write("module nod1(");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f6.Write("i" + k.ToString() + ",");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f6.Write("w" + k.ToString() + ",");
                f6.WriteLine("");
                f6.WriteLine("Out,clk,en,res);");
                f6.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f6.WriteLine("      input signed [" + textBox3.Text + ":0] i" + k.ToString() + ";");

                f6.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                    f6.WriteLine("      input signed [" + textBox3.Text + ":0] w" + k.ToString() + ";");

                f6.WriteLine("");
                f6.WriteLine("      output signed [" + textBox3.Text + ":0] Out;");
                f6.WriteLine("      reg [" + textBox3.Text + ":0] Out;");
                f6.WriteLine("      input clk,en,res;");
                f6.WriteLine("");
                f6.WriteLine("      reg signed [" + textBox3.Text + ":0] a,b;");
                f6.WriteLine("      wire signed [" + (int.Parse(textBox3.Text) * 2 + 1).ToString() + ":0] z;");
                f6.WriteLine("");
                f6.WriteLine("      mult m1(a,b,z);");
                f6.WriteLine("      reg [" + sum_bit().ToString() + ":0] sum;");
                f6.WriteLine("      reg [5:0] state;");

                f6.WriteLine("");
                f6.WriteLine("      initial Out=0;");
                f6.WriteLine("");
                f6.WriteLine("      initial state=0;");
                f6.WriteLine("//----------------");
                f6.WriteLine("");
                f6.WriteLine("");
                f6.WriteLine("       always @(*)");
                f6.WriteLine("       begin");
                f6.WriteLine("            case (state)");
                for (int k = 1; k <= int.Parse(textBox2.Text); k++)
                {
                    f6.WriteLine("                " + (k - 1).ToString() + ":begin");
                    f6.WriteLine("                       a=i" + k.ToString() + ";");
                    f6.WriteLine("                       b=w" + k.ToString() + ";");
                    f6.WriteLine("                  end");
                }
                f6.WriteLine("            endcase");
                f6.WriteLine("       end");
                f6.WriteLine("");
                f6.WriteLine("");
                //always----------------------
                f6.WriteLine("       always @(posedge clk)");
                f6.WriteLine("       begin");
                f6.WriteLine("");
                f6.WriteLine("             if(state==" + textBox2.Text + ")");
                f6.WriteLine("                    state <= 0;");
                f6.WriteLine("             if(en)");
                f6.WriteLine("             begin");
                f6.WriteLine("                   if (res)");
                f6.WriteLine("                   begin");
                f6.WriteLine("                          Out <= 0;");
                f6.WriteLine("                          state <= 0;");
                f6.WriteLine("                   end");
                f6.WriteLine("                   else");
                f6.WriteLine("                   begin");
                f6.WriteLine("                        state <= state + 1;");
                f6.WriteLine("                        case (state)");
                f6.WriteLine("                             0:sum<=z;");
                for (int k = 1; k <= int.Parse(textBox2.Text) - 1; k++)
                {
                    f6.WriteLine("                              " + k.ToString() + ":sum <= sum + z;");
                }
                f6.WriteLine("                             " + textBox2.Text + ":Out<= sum + z;");
                f6.WriteLine("                         endcase");
                f6.WriteLine("                   end");
                f6.WriteLine("");
                f6.WriteLine("             end");
                f6.WriteLine("");
                f6.WriteLine("        end");
                f6.WriteLine("endmodule");
                f6.WriteLine("");



            }
            finally
            {
                f6.Flush();
                f6.Close();
            }
            //System.Diagnostics.Process.Start(@add7);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

        private void textBox002_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
        private void textBox003_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void textBox002_KeyPress_1(object sender, KeyPressEventArgs e)
        {
                    if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                        e.Handled = false;
                    else
                        e.Handled = true;
        }
        private void textBox001_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0' || e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void label004_Click(object sender, EventArgs e)
        {

        }
        private void button001_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text Files|*.txt";
            openFileDialog1.FileName = String.Empty;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                label014.Text = openFileDialog1.FileName;
            }
        }
//////////////////////////////////////////      Conv     //////////////////////////////////////////////
        private void button6_Click(object sender, EventArgs e)
        {
            Create_Top_Mat_conv();
            Create_Conv();
            Create_Top_nod_conv();
            Create_nod_conv_Hard_Limit();
            Create_mul_conv();
           
        }
        public void Create_Conv()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add001 = label5.Text + "\\Conv.v";
            TextWriter f001 = new StreamWriter(@add001);

            try
            {
                int i;
                int j;
                //////////////////////        Conv.v     /////////////////////////
                f001.WriteLine("`timescale 1ns / 1ps");
                f001.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f001.WriteLine("// Engineer:          Pezhman Torabi");
                f001.WriteLine("// Design Name:       Convolutional neural networks");
                f001.WriteLine("// Module Name:       " + add001);
                f001.WriteLine("// Project Name: ");
                f001.WriteLine("// Target Devices: ");
                f001.WriteLine("// Tool versions: ");
                f001.WriteLine("// Description:        Number of inputs(k):" + textBox001.Text);
                f001.WriteLine("//                     Size(i*j):          " + textBox002.Text+" * "+ textBox003.Text);
                f001.WriteLine("//                     Number of w(n):     " + textBox004.Text);
                f001.WriteLine("//                     Size(x*y):          " + textBox005.Text + " * " + textBox006.Text);
                f001.WriteLine("//                     length of bits:     " + textBox009.Text);
                f001.WriteLine("// Dependencies: ");
                f001.WriteLine("// Create Date:       " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f001.WriteLine("// Version:           1.02");
                f001.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f001.WriteLine("");
                f001.WriteLine("");

                //input------------------------
                f001.Write("module Conv(");

                //Matrix Input
                for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                    {
                        for ( i = 1; i <= int.Parse(textBox002.Text); i++)
                            for (j = 1; j <= int.Parse(textBox003.Text); j++)
                                f001.Write("Mat_in" + convert(k) + "_" + convert(i) + "_" + convert(j) + ", ");
              
                        f001.WriteLine("");
                        f001.Write("            ");
                    }
                f001.WriteLine("");
                f001.Write("            ");

                //a*b ?
                int a;
                int b;
                //number of nuron= (W-F+2P)/s+1
                a = (int.Parse(textBox002.Text) - int.Parse(textBox005.Text) + 2 * int.Parse(textBox008.Text)) / int.Parse(textBox007.Text) + 1;
                b = (int.Parse(textBox003.Text) - int.Parse(textBox006.Text) + 2 * int.Parse(textBox008.Text)) / int.Parse(textBox007.Text) + 1;
                label020.Text = "Output Matrix size (a*b):  a=" + a.ToString() + "    b=" + b.ToString();

                //Matrix Output
                for (int n = 1; n <= int.Parse(textBox004.Text); n++)
                {
                    for (i = 1; i <= a; i++)
                        for (j = 1; j <= b; j++)
                            f001.Write("Mat_out" + convert(n) + "_" + convert(i) + "_" + convert(j) + ", ");

                    f001.WriteLine("");
                    f001.Write("            ");
                }

                f001.WriteLine("");
                f001.Write("            ");
                f001.WriteLine("clk,en,res);");
                f001.WriteLine("");

                //input signed Mat_in
                for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                {   
                    f001.Write("      input signed [" + textBox009.Text + ":0] " );
                    for (i = 1; i <= int.Parse(textBox002.Text); i++)
                        for (j = 1; j <= int.Parse(textBox003.Text); j++)
                        {
                            f001.Write("Mat_in" + convert(k) + "_" + convert(i) + "_" + convert(j) );
                            if ((i == int.Parse(textBox002.Text) ) && (j == int.Parse(textBox003.Text)))
                                f001.Write(";");
                            else
                                f001.Write(",   ");
                        }

                    f001.WriteLine("");
                }

                f001.WriteLine("");
                f001.WriteLine("");

                //output signed Mat_out
                for (int n = 1; n <= int.Parse(textBox004.Text); n++)
                {
                    f001.Write("      output signed [" + textBox009.Text + ":0] ");
                    for (i = 1; i <= a; i++)
                        for (j = 1; j <= b; j++)
                        {
                            f001.Write("Mat_out" + convert(n) + "_" + convert(i) + "_" + convert(j) );
                            if ((i == a) && (j == b))
                                f001.Write(";");
                            else
                                f001.Write(",   ");
                        }
                    f001.WriteLine("");
                }

                f001.WriteLine("");
                f001.WriteLine("      input clk,en,res;");
                f001.WriteLine("");
                f001.WriteLine("");

                //Weigth Input
                for (int n = 1; n <= int.Parse(textBox004.Text); n++)
                    for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                    {
                        f001.Write("      reg [" + textBox3.Text + ":0] ");
                        for (int x = 1; x <= int.Parse(textBox005.Text); x++)
                            for (int y = 1; y <= int.Parse(textBox006.Text); y++)
                            {
                                f001.Write("W" + convert(n) + convert(k) + "_" + convert(x) + "_" + convert(y));
                                if((x==int.Parse(textBox005.Text))&&(y==int.Parse(textBox006.Text)))
                                    f001.Write(";");
                                else
                                    f001.Write(",   ");
                            }

                        f001.WriteLine("");
                    }
                f001.WriteLine("");
               
                //int m = 1;
                f001.WriteLine("");
                f001.WriteLine("//                  |---------------------------------------------------|");
                f001.WriteLine("//                  |                  initial Weight                   |");
                f001.WriteLine("//                  |---------------------------------------------------|");
                f001.WriteLine("initial begin");

                string StrText;
                string add000 = label014.Text;
                StreamReader f000 = new StreamReader(@add000);

                for (int n = 1; n <= int.Parse(textBox004.Text); n++)
                    for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                    {
                        f001.Write("    ");
                        for (int x = 1; x <= int.Parse(textBox005.Text); x++)
                            for (int y = 1; y <= int.Parse(textBox006.Text); y++)
                            {
                                StrText = f000.ReadLine().ToString();
                                f001.Write("  W" + convert(n) + convert(k) + "_" + convert(x) + "_" + convert(y) + "=" + StrText + ";");
                            }
                        f001.WriteLine("");
                    }
                f001.WriteLine("");
                f001.WriteLine("");
                f001.WriteLine("end");
                f001.WriteLine("");
                f001.WriteLine("//                  |---------------------------------------------------|");
                f001.WriteLine("//                  |                  instance Top node                |");
                f001.WriteLine("//                  |---------------------------------------------------|");
                f001.WriteLine("");

                for (int n = 1; n <= int.Parse(textBox004.Text); n++)
                {
                    f001.WriteLine("");
                    f001.Write("Top_Mat_conv TMC_" + n.ToString() + "(");
                    for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                    {
                        for (i = 1; i <= int.Parse(textBox002.Text); i++)
                            for (j = 1; j <= int.Parse(textBox003.Text); j++)
                                f001.Write("Mat_in" + convert(k) + "_" + convert(i) + "_" + convert(j) + ",   ");
                         f001.WriteLine("");
                         f001.Write("                   ");
                    }

                    f001.WriteLine("");
                    f001.Write("                   ");                   
                    for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                    {
                        for (int x = 1;x <= int.Parse(textBox005.Text); x++)
                            for (int y = 1; y <= int.Parse(textBox006.Text); y++)
                                f001.Write("W" + convert(n) + convert(k) + "_" + convert(x) + "_" + convert(y) + ",   ");
                        f001.WriteLine("");
                        f001.Write("                   ");
                    }

                    f001.WriteLine("");
                    for (i = 1; i <= a; i++)
                    {
                        f001.Write("                   ");
                        for (j = 1; j <= b; j++)
                            f001.Write("Mat_out" + convert(n) + "_" + convert(i) + "_" + convert(j) + ", ");
                    }
                    f001.WriteLine("");
                    f001.WriteLine("");
                    f001.WriteLine("                   clk,en,res);");
                }

                f001.WriteLine("");
                f001.WriteLine("endmodule");
                f001.WriteLine("");

            }
            finally
            {
                f001.Flush();
                f001.Close();
                //f000.Close();
            }

            //System.Diagnostics.Process.Start(@add001);
        }
        public void Create_nod_conv_Hard_Limit()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add002 = label016.Text + "\\nod_conv.v";
            TextWriter f002 = new StreamWriter(@add002);

            try
            {
                f002.WriteLine("`timescale 1ns / 1ps");
                f002.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f002.WriteLine("// Engineer:           Pezhman Torabi");
                f002.WriteLine("// Design Name:        CNN nod hard Limit)");
                f002.WriteLine("//                     nod1_Hard_Limit_01");
                f002.WriteLine("// Module Name:        " + add002);
                f002.WriteLine("// Project Name: ");
                f002.WriteLine("// Target Devices: ");
                f002.WriteLine("// Tool versions: ");
                f002.WriteLine("// Description:        Number of inputs(k):" + textBox001.Text);
                f002.WriteLine("//                     Size(i*j):          " + textBox002.Text + " * " + textBox003.Text);
                f002.WriteLine("//                     Number of w(n):     " + textBox004.Text);
                f002.WriteLine("//                     Size(x*y):          " + textBox005.Text + " * " + textBox006.Text);
                f002.WriteLine("//                     length of bits:     " + textBox009.Text);
                f002.WriteLine("// Dependencies: ");
                f002.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f002.WriteLine("// Version:            1.01");
                f002.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f002.WriteLine("");
                f002.WriteLine("");

                //input------------------------
                int X = int.Parse(textBox005.Text);
                int Y = int.Parse(textBox006.Text);
                f002.Write("module nod_conv(");
                for (int k = 1; k <= (X*Y); k++)
                    f002.Write("i" + k.ToString() + ",");
                for (int k = 1; k <= (X * Y); k++)
                    f002.Write("w" + k.ToString() + ",");
                f002.WriteLine("Out,clk,en,res);");
                f002.WriteLine("");

                for (int k = 1; k <= (X * Y); k++)
                    f002.WriteLine("      input signed [" + textBox009.Text + ":0] i" + k.ToString() + ";");

                f002.WriteLine("");

                for (int k = 1; k <= (X * Y); k++)
                    f002.WriteLine("      input signed [" + textBox009.Text + ":0] w" + k.ToString() + ";");

                f002.WriteLine("");
                f002.WriteLine("      output signed [" + textBox009.Text + ":0] Out;");
                f002.WriteLine("      input clk,en,res;");

                f002.WriteLine("");
                for (int k = 1; k <= (X * Y); k++)
                    f002.WriteLine("      wire signed [" + (int.Parse(textBox009.Text) * 2 + 1).ToString() + ":0] z" + convert(k) + ";");
                f002.WriteLine("");

                for (int k = 1; k <= (X * Y); k++)
                    f002.WriteLine("      mult_conv m" + k.ToString() + "(i" + k.ToString() + ",w" + k.ToString() + ",z" + convert(k) + ");");

                f002.WriteLine("");

                for (int k = 1; k <= (X * Y); k++)
                    f002.WriteLine("      reg signed[" + (int.Parse(textBox009.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + ";");

                //////////////////////////////////
                f002.WriteLine("      reg [" + sum_bit().ToString() + ":0] temp;");
                /////////////////////////////////
                f002.WriteLine("");
                f002.WriteLine("      reg [" + textBox009.Text + ":0] Out;");
                f002.WriteLine("");
                f002.WriteLine("      initial Out=0;");
                f002.WriteLine("");
                f002.WriteLine("//----------------");
                f002.WriteLine("");
                f002.WriteLine("");

                //always----------------------
                f002.WriteLine("       always @(posedge clk)");
                f002.WriteLine("       begin");
                f002.WriteLine("             if(en)");
                f002.WriteLine("             begin");
                f002.WriteLine("                   if (res)");
                f002.WriteLine("                          Out <=0;");
                f002.WriteLine("                   else");
                f002.WriteLine("                   begin");

                //Pipelining-------------------------
                f002.WriteLine("");
                for (int k = 1; k <= (X * Y); k++)
                {
                    f002.WriteLine("                         temp" + convert(k) + "<=" + "z" + convert(k) + ";");
                }
                //end pipelining--------------

                f002.WriteLine("");
                f002.Write("                         temp<=");
                for (int k = 1; k <= (X * Y) - 1; k++)
                    f002.Write("temp" + convert(k) + "+");
                int i = (X * Y);
                f002.Write("temp" + convert(i) + ";");
                f002.WriteLine("");

                f002.WriteLine("                         Out<=~temp[" + sum_bit().ToString() + "];");
                f002.WriteLine("                    end");
                f002.WriteLine("              end");
                f002.WriteLine("       end");
                f002.WriteLine("");
                f002.WriteLine("");
                f002.WriteLine("endmodule");
                f002.WriteLine("");

            }
            finally
            {
                f002.Flush();
                f002.Close();
            }
            //System.Diagnostics.Process.Start(@add002);
        }
        public void Create_Top_Mat_conv()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add003 = label016.Text + "\\Top_Mat_conv.v";
            TextWriter f003 = new StreamWriter(@add003);

            try
            {
                f003.WriteLine("`timescale 1ns / 1ps");
                f003.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f003.WriteLine("// Engineer:           Pezhman Torabi");
                f003.WriteLine("// Design Name:        CNN Top Matrix ");
                f003.WriteLine("//                      ");
                f003.WriteLine("// Module Name:        " + add003);
                f003.WriteLine("// Project Name: ");
                f003.WriteLine("// Target Devices: ");
                f003.WriteLine("// Tool versions: ");
                f003.WriteLine("// Description:        Number of inputs(k):" + textBox001.Text);
                f003.WriteLine("//                     Size(i*j):          " + textBox002.Text + " * " + textBox003.Text);
                f003.WriteLine("//                     Number of w(n):     " + textBox004.Text);
                f003.WriteLine("//                     Size(x*y):          " + textBox005.Text + " * " + textBox006.Text);
                f003.WriteLine("//                     length of bits:     " + textBox009.Text);
                f003.WriteLine("// Dependencies: ");
                f003.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f003.WriteLine("// Version:            1.01");
                f003.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f003.WriteLine("");
                f003.WriteLine("");

                //input------------------------

                //a*b ?
                int a;
                int b;
                //number of nuron= (W-F+2P)/s+1
                
                f003.Write("module Top_Mat_conv(");

                for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                {
                    for (int i = 1; i <= int.Parse (textBox002.Text); i++)
                        for (int j = 1; j <= int.Parse (textBox003.Text); j++)
                            f003.Write("Mat_in_" + convert(k) + "_" + convert(i) + "_" + convert(j) + ", ");
                    f003.WriteLine("");
                    f003.Write("                    ");
                }

                f003.WriteLine("");
                f003.Write("                    ");
                int X = int.Parse(textBox005.Text);
                int Y = int.Parse(textBox006.Text);
                for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                {
                    for (int x = 1; x <= X; x++)
                        for (int y = 1; y <= Y; y++)
                            f003.Write("W_" + convert(k) + "_" + convert(x) + "_" + convert(y) + ", ");
                    f003.WriteLine("");
                    f003.Write("                    ");
                }

                f003.WriteLine("");

                a = (int.Parse(textBox002.Text) - int.Parse(textBox005.Text) + 2 * int.Parse(textBox008.Text)) / int.Parse(textBox007.Text) + 1;
                b = (int.Parse(textBox003.Text) - int.Parse(textBox006.Text) + 2 * int.Parse(textBox008.Text)) / int.Parse(textBox007.Text) + 1;
                label020.Text = "Output Matrix size (a*b):  a=" + a.ToString() + "    b=" + b.ToString();

                f003.WriteLine("");
                f003.Write("                    ");
                for( int i=1;i<=a;i++)
                    for(int j=1;j<=b;j++)
                        f003.Write("Mat_out_" + convert(i) + "_" + convert(j) + ", ");

                f003.WriteLine("");
                f003.WriteLine("");
                f003.WriteLine("                    clk,en,res);");
                f003.WriteLine("");
                f003.WriteLine("");
                f003.WriteLine("");
                
                for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                {
                    f003.Write("input signed [" + textBox009.Text + ":0] ");
                    for (int i = 1; i <= int.Parse(textBox002.Text); i++)
                        for (int j = 1; j <= int.Parse(textBox003.Text); j++)
                        {
                            f003.Write("Mat_in_" + convert(k) + "_" + convert(i) + "_" + convert(j) );
                            if ((i == int.Parse(textBox002.Text)) && (j == int.Parse(textBox003.Text)))
                                f003.Write(";");
                            else
                                f003.Write(", ");
                        }
                    f003.WriteLine("");
                    //f003.Write("                    ");
                }

                f003.WriteLine("");
                for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                {
                    f003.Write("input signed [" + textBox009.Text + ":0] ");
                    for (int x = 1; x <= X; x++)
                        for (int y = 1; y <= Y; y++)
                        {
                            f003.Write("W_" + convert(k) + "_" + convert(x) + "_" + convert(y));
                            if ((x == X) && (y == Y))
                                f003.Write(";");
                            else
                                f003.Write(", ");
                        }
                    f003.WriteLine("");
                }

                f003.WriteLine("");
                f003.WriteLine("input clk,en,res;");

                f003.WriteLine("");
                for (int i = 1; i <= a; i++)
                    for (int j = 1; j <= b; j++)
                        f003.WriteLine("output [" + textBox009.Text + ":0] "+ "Mat_out_" + convert(i) + "_" + convert(j) + ";");
               
                f003.WriteLine("");
                f003.WriteLine("");
                int s = int.Parse(textBox007.Text);
                for (int i = 1; i <= a; i++)
                    for (int j = 1; j <= b; j++)
                    {
                        f003.Write("Top_nod_conv Tnc_" + convert(i) + "_" + convert(j) + "(");

                        for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                        {
                            
                            for (int x = 1; x <= X; x++)
                                for (int y = 1; y <= Y; y++)
                                    f003.Write("Mat_in_" + convert(k) + "_" + convert(x + (j - 1) * s) + "_" + convert(y + (i - 1) * s) + ", ");
                            f003.WriteLine("");
                            f003.Write("                       ");
                        }

                        f003.WriteLine("");
                        f003.Write("                       ");

                        for (int k = 1; k <= int.Parse(textBox001.Text); k++)
                        {
                            for (int x = 1; x <= X; x++)
                                for (int y = 1; y <= Y; y++)
                                    f003.Write("W_" + convert(k) + "_" + convert(x) + "_" + convert(y) + ", ");
                            f003.WriteLine("");
                            f003.Write("                       ");
                        }

                        f003.WriteLine("");
                        f003.Write("                       Mat_out_" + convert(i) + "_" + convert(j) + ",clk,en,res);");
                        f003.WriteLine("");
                        f003.WriteLine("");
                    }

                f003.WriteLine("");
                f003.WriteLine("endmodule");
                f003.WriteLine("");

            }
            finally
            {
                f003.Flush();
                f003.Close();
            }
            //System.Diagnostics.Process.Start(@add003);
        }
        public void Create_Top_nod_conv()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add003 = label016.Text + "\\Top_nod_conv.v";
            TextWriter f003 = new StreamWriter(@add003);

            try
            {
                f003.WriteLine("`timescale 1ns / 1ps");
                f003.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f003.WriteLine("// Engineer:           Pezhman Torabi");
                f003.WriteLine("// Design Name:        CNN Top nod ");
                f003.WriteLine("//                      ");
                f003.WriteLine("// Module Name:        " + add003);
                f003.WriteLine("// Project Name: ");
                f003.WriteLine("// Target Devices: ");
                f003.WriteLine("// Tool versions: ");
                f003.WriteLine("// Description:        Number of inputs(k):" + textBox001.Text);
                f003.WriteLine("//                     Size(i*j):          " + textBox002.Text + " * " + textBox003.Text);
                f003.WriteLine("//                     Number of w(n):     " + textBox004.Text);
                f003.WriteLine("//                     Size(x*y):          " + textBox005.Text + " * " + textBox006.Text);
                f003.WriteLine("//                     length of bits:     " + textBox009.Text);
                f003.WriteLine("// Dependencies: ");
                f003.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f003.WriteLine("// Version:            1.01");
                f003.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f003.WriteLine("");
                f003.WriteLine("");

                //input------------------------
                int X = int.Parse(textBox005.Text);
                int Y = int.Parse(textBox006.Text);
                f003.Write("module Top_nod_conv(");
                for (int o = 1; o <= ( X * Y); o++)
                    for(int h=1;h<=int.Parse(textBox001.Text);h++)
                        f003.Write("m_in_" + o.ToString() + "_" + h.ToString() + ", ");
                f003.WriteLine("");
                f003.Write("                    ");
                for (int o = 1; o <= (X * Y); o++)
                    for (int h = 1; h <= int.Parse(textBox001.Text); h++)
                        f003.Write("w_in_" + o.ToString() + "_" + h.ToString() + ", ");
                f003.WriteLine("");
                f003.Write("                    ");
                f003.WriteLine("Out,clk,en,res);");
                f003.WriteLine("");
                f003.WriteLine("");
                f003.WriteLine("");
                f003.Write("      input signed [" + textBox009.Text + ":0] ");
                for (int o = 1; o <= (X * Y); o++)
                    for (int h = 1; h <= int.Parse(textBox001.Text); h++)
                    {
                        f003.Write("m_in_" + o.ToString() + "_" + h.ToString());
                        if ((o == (X * Y)) && (h == int.Parse(textBox001.Text)))
                            f003.Write(";");
                        else
                            f003.Write(", ");
                    }

                f003.WriteLine("");
                f003.Write("      input signed [" + textBox009.Text + ":0] ");
                for (int o = 1; o <= (X * Y); o++)
                    for (int h = 1; h <= int.Parse(textBox001.Text); h++)
                    {
                        f003.Write("w_in_" + o.ToString() + "_" + h.ToString());
                        if ((o == (X * Y)) && (h == int.Parse(textBox001.Text)))
                            f003.Write(";");
                        else
                            f003.Write(", ");
                    }

                f003.WriteLine("");
                f003.WriteLine("      output signed [" + textBox009.Text + ":0] Out;");
                f003.WriteLine("      input clk,en,res;");

                f003.WriteLine("");
                for (int k = 1; k <= X ; k++)
                    f003.WriteLine("      wire signed [" + (int.Parse(textBox009.Text) * 2 + 1).ToString() + ":0] z" + convert(k) + ";");
                f003.WriteLine("");

                int t = 0;
                int r = 0;
                for(int m=1;m<=X;m++)
                {                 
                    f003.Write("      nod_conv nc_"+m.ToString ()+"(");
                    for (int o = 1; o <= Y; o++)
                    {
                        t = t + 1;
                        for (int h = 1; h <= int.Parse(textBox001.Text); h++)
                            f003.Write("m_in_" + t.ToString() + "_" + h.ToString() + ", ");
                    }

                    for (int o = 1; o <= Y; o++)
                    {
                        r = r + 1;
                        for (int h = 1; h <= int.Parse(textBox001.Text); h++)
                        {
                            f003.Write("w_in_" + r.ToString() + "_" + h.ToString());
                            if ((o == Y) && (h == int.Parse(textBox001.Text)))
                                f003.Write("");
                            else
                                f003.Write(", ");
                        }
                    }
                    f003.Write(",z"+convert(m));
                    f003.WriteLine(",clk,en,res);");
                }

                f003.WriteLine("");
                for (int k = 1; k <= X; k++)
                    f003.WriteLine("      reg signed[" + (int.Parse(textBox009.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + ";");

                //////////////////////////////////
                f003.WriteLine("      reg [" + sum_bit().ToString() + ":0] temp;");
                f003.WriteLine("      reg [" + textBox009.Text + ":0] Out;");
                /////////////////////////////////

                f003.WriteLine("");
                f003.WriteLine("      initial Out=0;");
                f003.WriteLine("");
                f003.WriteLine("//----------------");
                f003.WriteLine("");
                f003.WriteLine("");

                //always----------------------
                f003.WriteLine("       always @(posedge clk)");
                f003.WriteLine("       begin");
                f003.WriteLine("             if(en)");
                f003.WriteLine("             begin");
                f003.WriteLine("                   if (res)");
                f003.WriteLine("                          Out <=0;");
                f003.WriteLine("                   else");
                f003.WriteLine("                   begin");

                //Pipelining-------------------------
                f003.WriteLine("");
                for (int k = 1; k <= X; k++)
                {
                    f003.WriteLine("                         temp" + convert(k) + "<=" + "z" + convert(k) + ";");
                }
                //end pipelining--------------
                f003.WriteLine("");
                f003.Write("                         temp<=");
                for (int k = 1; k <= X - 1; k++)
                    f003.Write("temp" + convert(k) + "+");
                int i = X;
                f003.Write("temp" + convert(i) + ";");
                f003.WriteLine("");
                f003.WriteLine("                         Out<=~temp[" + sum_bit().ToString() + "];");
                f003.WriteLine("                    end");
                f003.WriteLine("              end");
                f003.WriteLine("       end");
                f003.WriteLine("");
                f003.WriteLine("");
                f003.WriteLine("endmodule");
                f003.WriteLine("");

            }
            finally
            {
                f003.Flush();
                f003.Close();
            }
            //System.Diagnostics.Process.Start(@add003);
        }
        public void Create_mul_conv()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string add6 = label5.Text + "\\mult_conv.v";
            TextWriter f5 = new StreamWriter(@add6);
            try
            {
                f5.WriteLine("`timescale 1ns / 1ps");
                f5.WriteLine("");
                //------------------------
                f5.WriteLine("module mult_conv(a,b,z);");
                f5.WriteLine("");
                f5.WriteLine("   (* use_dsp48 =" + '"' + "yes" + '"' + "*)");
                f5.WriteLine("   input [" + textBox103.Text + ":0] a,b;");
                f5.WriteLine("   output [" + (int.Parse(textBox103.Text) * 2 + 1).ToString() + ":0] z;");
                f5.WriteLine("");
                f5.WriteLine("   assign z=a*b;");
                f5.WriteLine("");
                f5.WriteLine("endmodule");
                f5.WriteLine("");
            }
            finally
            {
                f5.Flush();
                f5.Close();
            }
            //System.Diagnostics.Process.Start(@add6);

        }

        // button7_Click  folderBrowserDialog1 for Path in CNN
        private void button7_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)

                label016.Text = folderBrowserDialog1.SelectedPath;
        }
        private void textBox001_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox009_TextChanged(object sender, EventArgs e)
        {

        }
        private void button12_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text Files|*.txt";
            openFileDialog1.FileName = String.Empty;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                label109.Text = openFileDialog1.FileName;
            }
        }
        private void button101_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                label110.Text = folderBrowserDialog1.SelectedPath;
        }
        private void button104_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void label3_Click(object sender, EventArgs e)
        {}
//////////////////////////////////////////    Auto Encoder  ///////////////////////////////////////////
        private void button103_Click(object sender, EventArgs e)
        {
           
            Create_mul_AE();

            string t2 = textBox102.Text;

            int Temp = int.Parse(textBox102.Text);
            for (int i = 1; i <= int.Parse(textBox101.Text)  ; i++)
            {
                //textBox101.Text = t1;
                if (Temp - (i - 1) * int.Parse(textBox105.Text) > 1)
                {
                    textBox102.Text = (Temp - (i - 1) * int.Parse(textBox105.Text)).ToString();
                    l1.Text = convert(i);
                    Create_nod_AE_Hard_Limit();
                }

               
            }

            textBox102.Text = t2;




            Create_Top_AE();
        }
        public void Create_mul_AE()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string add6 = label107.Text + "\\mult_AE.v";
            TextWriter f5 = new StreamWriter(@add6);
            try
            {
                f5.WriteLine("`timescale 1ns / 1ps");
                f5.WriteLine("");
                //------------------------
                f5.WriteLine("module mult_AE(a,b,z);");
                f5.WriteLine("");
                f5.WriteLine("   (* use_dsp48 =" + '"' + "yes" + '"' + "*)");
                f5.WriteLine("   input [" + textBox103.Text + ":0] a,b;");
                f5.WriteLine("   output [" + (int.Parse(textBox103.Text) * 2 + 1).ToString() + ":0] z;");
                f5.WriteLine("");
                f5.WriteLine("   assign z=a*b;");
                f5.WriteLine("");
                f5.WriteLine("endmodule");
                f5.WriteLine("");
            }
            finally
            {
                f5.Flush();
                f5.Close();
            }
            //System.Diagnostics.Process.Start(@add6);

        }
        public void Create_nod_AE_Hard_Limit()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add5 = label5.Text + "\\nod_AE"+l1.Text+".v";
            TextWriter f4 = new StreamWriter(@add5);

            try
            {
                f4.WriteLine("`timescale 1ns / 1ps");
                f4.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f4.WriteLine("// Engineer:           Pezhman Torabi");
                f4.WriteLine("// Design Name:        AE node " + l1.Text + " hard limit");
                f4.WriteLine("//                     ");
                f4.WriteLine("// Module Name:        " + add5);
                f4.WriteLine("// Project Name: ");
                f4.WriteLine("// Target Devices: ");
                f4.WriteLine("// Tool versions: ");
                f4.WriteLine("// Description:        Hidden Layer:       " + textBox101.Text);
                f4.WriteLine("//                     Number of inputs:   " + textBox102.Text);
                f4.WriteLine("//                     length of bits:     " + textBox103.Text);
                f4.WriteLine("//                     Number of outputs:  " + textBox104.Text);
                f4.WriteLine("//                     ");
                f4.WriteLine("// Dependencies: ");
                f4.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f4.WriteLine("// Version:            1.01");
                f4.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f4.WriteLine("");
                f4.WriteLine("");

                //input------------------------
                f4.Write("module nod_AE" + l1.Text + "(");
                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f4.Write("i" + k.ToString() + ",");
                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f4.Write("w" + k.ToString() + ",");
                f4.WriteLine("Out,clk,en,res);");
                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f4.WriteLine("      input signed [" + textBox3.Text + ":0] i" + k.ToString() + ";");

                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f4.WriteLine("      input signed [" + textBox103.Text + ":0] w" + k.ToString() + ";");

                f4.WriteLine("");
                f4.WriteLine("      output signed [" + textBox103.Text + ":0] Out;");
                f4.WriteLine("      input clk,en,res;");

                f4.WriteLine("");
                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f4.WriteLine("      wire signed [" + (int.Parse(textBox103.Text) * 2 + 1).ToString() + ":0] z" + convert(k) + ";");
                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f4.WriteLine("      mult_AE m" + k.ToString() + "(i" + k.ToString() + ",w" + k.ToString() + ",z" + convert(k) + ");");

                f4.WriteLine("");

                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f4.WriteLine("      reg signed[" + (int.Parse(textBox103.Text) * 2 + 1).ToString() + ":0] temp" + convert(k) + ";");

                //////////////////////////////////
                f4.WriteLine("      reg [" + sum_bit().ToString() + ":0] temp;");
                /////////////////////////////////

                f4.WriteLine("      reg Out;");
                f4.WriteLine("      initial Out=0;");
                f4.WriteLine("");
                f4.WriteLine("//----------------");
                f4.WriteLine("");
                f4.WriteLine("");

                //always----------------------
                f4.WriteLine("       always @(posedge clk)");
                f4.WriteLine("       begin");
                f4.WriteLine("             if(en)");
                f4.WriteLine("             begin");
                f4.WriteLine("                   if (res)");
                f4.WriteLine("                          Out <=0;");
                f4.WriteLine("                   else");
                f4.WriteLine("                   begin");

                //Pipelining-------------------------
                f4.WriteLine("");
                //m = 0;
                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                {
                    f4.WriteLine("                         temp" + convert(k) + "<=" + "z" + convert(k) + ";");
                }
                //end pipelining--------------

                f4.WriteLine("");
                f4.Write("                         temp<=");
                for (int k = 1; k <= int.Parse(textBox2.Text) - 1; k++)
                    f4.Write("temp" + convert(k) + "+");
                int i = int.Parse(textBox2.Text);
                f4.Write("temp" + convert(i) + ";");
                f4.WriteLine("");
                f4.WriteLine("                         Out<=~temp[" + sum_bit().ToString() + "];");
                f4.WriteLine("                    end");
                f4.WriteLine("              end");
                f4.WriteLine("       end");
                f4.WriteLine("");
                f4.WriteLine("");
                f4.WriteLine("endmodule");
                f4.WriteLine("");

            }
            finally
            {
                f4.Flush();
                f4.Close();
            }
            //System.Diagnostics.Process.Start(@add5);
        }
        public void Create_Top_AE()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            //if(label9 .Text=="Select file") 
            // label9.Text="C:\\Users\\Pezhman\\Desktop\\test001\\Book1.txt";
            string add2 = label109.Text;
            StreamReader f0 = new StreamReader(@add2);

            string add = label107.Text + "\\Top_AE.v";
            TextWriter f1 = new StreamWriter(@add);
            
            int t001=int.Parse(textBox101.Text );
            while (t001*int.Parse(textBox105.Text)>int.Parse(textBox102.Text))
            {
                t001--;
            }
            textBox101.Text = t001.ToString();
            int Temp_out = int.Parse(textBox102.Text) - (t001 * int.Parse(textBox105.Text));
            
            textBox104.Text = Temp_out.ToString();

            try
            {
                int i;
                int j;
                //////////////////////        Top_AE.v     /////////////////////////
                f1.WriteLine("`timescale 1ns / 1ps");
                f1.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f1.WriteLine("// Engineer:          Pezhman Torabi");
                f1.WriteLine("// Design Name:       Top_AE");
                f1.WriteLine("// Module Name:       " + add);
                f1.WriteLine("// Project Name: ");
                f1.WriteLine("// Target Devices: ");
                f1.WriteLine("// Tool versions: ");
                f1.WriteLine("// Description:        Hidden Layer:       " + textBox101.Text);
                f1.WriteLine("//                     Number of inputs:   " + textBox102.Text);
                f1.WriteLine("//                     length of bits:     " + textBox3.Text);
                f1.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f1.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f1.WriteLine("// Dependencies: ");
                f1.WriteLine("// Create Date:       " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f1.WriteLine("// Version:           1.01");
                f1.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f1.WriteLine("");
                f1.WriteLine("");

                //input------------------------
                f1.Write("module Top_AE(");
                i = int.Parse(textBox101.Text) + 1;
                i = 1;
                for (int k = 1; k <= int.Parse(textBox102.Text) ; k++)
                    f1.Write("i"  + convert(k) + ",");
                f1.WriteLine("");
                f1.Write("               ");
                for (int k = 1; k <= Temp_out; k++)
                    f1.Write("Out" + convert(k) + ",");
                f1.WriteLine("");
                f1.Write("               ");
                f1.WriteLine("clk,en,res);");
                f1.WriteLine("");

                i = int.Parse(textBox101.Text) + 1;
                i = 1;
                for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                    f1.WriteLine("      input signed [" + textBox103.Text + ":0] i" + convert(i - 1) + convert(k) + ";");
                for (int k = 1; k <= Temp_out; k++)
                    f1.WriteLine("      output signed [" + textBox103.Text + ":0] Out" + convert(k) + ";");

                f1.WriteLine("      input clk,en,res;");
                f1.WriteLine("");

                //reg w------------------------
                f1.WriteLine("");
                i = 0;
                int m = 1;
                for (j = 1; j <= int.Parse(textBox102.Text) - i; j++)
                {
                    //Weight
                    for (int k = 1; k <= int.Parse(textBox102.Text) - i; k++)
                        f1.Write("      reg [" + textBox103.Text + ":0] w" + convert(m) + convert(j) + convert(k) + ";");
                    f1.WriteLine("");
                }
                f1.WriteLine("");

                m = 1;
                for (i = 0; i <= (int.Parse(textBox101.Text)+int.Parse(textBox105.Text)); i = i + int.Parse(textBox105.Text))
                {
                    m = m + 1;
                    for (j = 1; j <=int.Parse(textBox102.Text)-i; j++)
                    {
                        //Weight
                        for (int k = 1; k <= int.Parse(textBox102.Text) - i ; k++)
                            f1.Write("      reg [" + textBox103.Text + ":0] w" + convert(m) + convert(j) + convert(k) + ";");
                        f1.WriteLine("");
                    }
                    f1.WriteLine("");
                   
                }

                f1.WriteLine("");
                f1.WriteLine("");


                //wire o ------------------------
                f1.WriteLine("//------------------wire o ---------------------");
                f1.WriteLine("");
                m = 0;
                for (i = 0; i <= (int.Parse(textBox101.Text) + int.Parse(textBox105.Text)); i = i + int.Parse(textBox105.Text))
                {
                    m = m + 1;
                    for (j = 1; j <= int.Parse(textBox102.Text) - i; j++)
                    {
                        //Weight
                        f1.Write("      wire [" + textBox103.Text + ":0] o" + convert(m) + convert(j) + ";");
                        //f1.WriteLine("");
                    }
                    f1.WriteLine("");

                }
                f1.WriteLine("");
                f1.WriteLine("");
                //Initial------------------------
                string StrText;
                //int m = 1;
                f1.WriteLine("");
                f1.WriteLine("//                  |---------------------------------------------------|");
                f1.WriteLine("//                  |                  initial Weight                   |");
                f1.WriteLine("//                  |---------------------------------------------------|");

                f1.WriteLine("initial begin");

                f1.WriteLine("");
                i = 0;
                m = 1;
                for (j = 1; j <= int.Parse(textBox102.Text) - i; j++)
                {
                    //Weight
                    for (int k = 1; k <= int.Parse(textBox102.Text) - i; k++)
                    {
                        StrText = f0.ReadLine().ToString();
                        f1.Write("       w" + convert(m) + convert(j) + convert(k) +"="+StrText + ";");
                    }
                    f1.WriteLine("");
                }
                f1.WriteLine("");

                m = 1;
                for (i = 0; i <= (int.Parse(textBox101.Text) + int.Parse(textBox105.Text)); i = i + int.Parse(textBox105.Text))
                {
                    m = m + 1;
                    for (j = 1; j <= int.Parse(textBox102.Text) - i; j++)
                    {
                        //Weight
                        for (int k = 1; k <= int.Parse(textBox102.Text) - i; k++)
                        {
                            StrText = f0.ReadLine().ToString();
                            f1.Write("       w" + convert(m) + convert(j) + convert(k) + "=" + StrText + ";");
                        }
                        f1.WriteLine("");
                    }
                    f1.WriteLine("");
                }
                f1.WriteLine("");
                f1.WriteLine("");
                f1.WriteLine("");
                f1.WriteLine("end");
                f1.WriteLine("");

                //node----------------------------------------------------------------------------------
                //node----------------------------------------------------------------------------------
                f1.WriteLine("//                  |---------------------------------------------------|");
                f1.WriteLine("//                  |                  instance node                    |");
                f1.WriteLine("//                  |---------------------------------------------------|");
                f1.WriteLine("");




                f1.WriteLine("");
                i = 1;
                m = 1;
                for (j = 1; j <= int.Parse(textBox102.Text); j++)
                {
                    f1.Write("      nod_AE nod_" + convert(i) + convert(j) + "(");
                   // for (int k = 1; k <= int.Parse(textBox102.Text) - i; k++)
                    //    f1.Write("i" + convert(m) + convert(j) + convert(k) + ",  ");


                    for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                        f1.Write("i" + convert(i - 1) + convert(k) + ", ");

                    f1.Write("  ");


                    for (int k = 1; k <= int.Parse(textBox102.Text) ; k++)
                        f1.Write("w" + convert(m) + convert(j) + convert(k)  + ", ");
                    f1.Write(" o" + convert(i) + convert(j)); 
                    f1.WriteLine(", clk,en,res);");
                }
                f1.WriteLine("");






                m = 2;
                for (i = int.Parse(textBox105.Text); i <= (int.Parse(textBox101.Text) + int.Parse(textBox105.Text) + int.Parse(textBox105.Text)); i = i + int.Parse(textBox105.Text))
                {
                    m = m + 1;
                    for (j = 1; j <= int.Parse(textBox102.Text) - i; j++)
                    {
                        f1.Write("      nod_AE nod_" + convert(m-1) + convert(j) + "(");
                        for (int k = 1; k <= int.Parse(textBox102.Text) + int.Parse(textBox105.Text) - i; k++)
                            f1.Write("o" + convert(m-2) + convert(k) + ",  ");

                        for (int k = 1; k <= int.Parse(textBox102.Text) + int.Parse(textBox105.Text) - i; k++)
                           f1.Write("w" + convert(m-1) + convert(j) + convert(k) +  ",  ");
                        if (i-1 <= (int.Parse(textBox101.Text) + int.Parse(textBox105.Text)))
                            f1.Write(" o" + convert(m - 1) + convert(j));
                        else
                            f1.Write("Out" + convert(j));
                            
                        f1.WriteLine(",clk,en,res);");
                    }
                    f1.WriteLine("");
                }



/*
                for (int k = 1; k <= Temp_out; k++)
                    f1.Write("      nod_AE nod_");
                
                f1.Write("Out" + convert(k) + ";");

*/


                //Output nod------------------------
                f1.WriteLine("//------------------End Layer---------------------");

                for (j = 1; j <= Temp_out; j++)
                {

                    f1.Write("      nod_AE nod_" + convert(i) + convert(j) + "(");
                    //Input 
                    for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                        f1.Write("o" + convert(i - 1) + convert(k) + ",");
                    //Weight
                    for (int k = 1; k <= int.Parse(textBox102.Text); k++)
                        f1.Write("w" + convert(i) + convert(j) + convert(k) + ",");
                    //Output
                    f1.Write("Out" + convert(j));
                    f1.WriteLine(",clk,en,res);");
                }
                f1.WriteLine("");
                f1.WriteLine("endmodule");
            }
            finally
            {
                f1.Flush();
                f1.Close();
                f0.Close();
            }

            //System.Diagnostics.Process.Start(@add);
        }
        public void Create_Layer_Hardware_Reuse_only_one_mult ()
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string add7 = label5.Text + "\\Layer_Hardware_Reuse_only_one_mult.v";
            TextWriter f6 = new StreamWriter(@add7);

            try
            {
                f6.WriteLine("`timescale 1ns / 1ps");
                f6.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f6.WriteLine("// Engineer:           Pezhman Torabi");
                f6.WriteLine("// Design Name:        Artificial neural network (Perceptron)");
                f6.WriteLine("// Module Name:        " + add7);
                f6.WriteLine("// Project Name: ");
                f6.WriteLine("// Target Devices: ");
                f6.WriteLine("// Tool versions: ");
                f6.WriteLine("// Description:        Hidden Layer:       " + textBox1.Text);
                f6.WriteLine("//                     Number of inputs:   " + textBox2.Text);
                f6.WriteLine("//                     length of bits:     " + textBox3.Text);
                f6.WriteLine("//                     Number of outputs:  " + textBox5.Text);
                f6.WriteLine("//                     Parameter pipeline: " + textBox8.Text);
                f6.WriteLine("// Dependencies: ");
                f6.WriteLine("// Create Date:        " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f6.WriteLine("// Version:            1.01");
                f6.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f6.WriteLine("");
                f6.WriteLine("");


                int Number_of_input = int.Parse(textBox2.Text);
                int length_of_bits =1+ int.Parse(textBox3.Text) + int.Parse(textBox9.Text);  //length_of_bits =  1 bit sign + Q (.) + M
                int Number_of_output = int.Parse(textBox5.Text);
                int Number_of_nod = int.Parse(textBox5.Text);  // Number of nod in Layer (= Number of inputs)
                int length_of_bits_W = 1 + int.Parse(textBox11.Text) + int.Parse(textBox10.Text); //length_of_bits_W =  1 bit sign + Q (.) + M

                //input------------------------
                f6.Write("module Layer_Hardware_Reuse_only_one_mult(");
                for (int k = 1; k <= Number_of_input; k++)
                    f6.Write("i" + k.ToString() + ",");
                for (int i = 1; i <= Number_of_input; i++)
                    for (int k = 1; k <= Number_of_nod; k++)
                        f6.Write("w" + i.ToString()+k.ToString() + ",");
                for (int k = 1; k <= Number_of_output; k++)
                    f6.Write("Out" + k.ToString() + ",");
                f6.WriteLine("clk,en,res);");
                f6.WriteLine("");
                f6.WriteLine("");
                for (int k = 1; k <= Number_of_input; k++)
                    f6.WriteLine("      input [" + (length_of_bits - 1).ToString() + ":0] i" + k.ToString() + ";");
                f6.WriteLine("");
                f6.WriteLine("");
                for (int i = 1; i <= Number_of_input; i++)
                    {
                        for (int k = 1; k <= Number_of_nod; k++)
                            f6.Write("      input [" + (length_of_bits_W - 1).ToString() + ":0] w" + i.ToString() + k.ToString() + ";");
                        f6.WriteLine("");
                     }
                f6.WriteLine("");
                for (int k = 1; k <= Number_of_output; k++)
                    f6.WriteLine("      output reg [" + sum_bit().ToString() + ":0] Out" + k.ToString() + ";");
                f6.WriteLine("");
                f6.WriteLine("");
                f6.WriteLine("      input clk,en,res;");
                f6.WriteLine("");
                f6.WriteLine("");
                f6.WriteLine("      reg signed [" + length_of_bits.ToString() + ":0] a_n,b_n;");
                f6.WriteLine("      wire signed [" +( length_of_bits * 2 + 1).ToString() + ":0] z_n;");
                f6.WriteLine("");
                f6.WriteLine("      mult m1(a_n,b_n,z_n,clk);");
                f6.WriteLine("      reg [" + sum_bit().ToString() + ":0] sum_n;");
                f6.WriteLine("      reg [5:0] state_n;");

                f6.WriteLine("");
                f6.WriteLine("      initial ");
                f6.WriteLine("          begin");
                for (int k = 1; k <= Number_of_output; k++)
                    f6.WriteLine("              Out" + k.ToString() + "=0;");
                f6.WriteLine("              state_n=0;");
                f6.WriteLine("          end");
                f6.WriteLine("//----------------");
                f6.WriteLine("");
                f6.WriteLine("");
                f6.WriteLine("    always @(*)");
                f6.WriteLine("       begin");
                f6.WriteLine("            case (state_n)");
                int c = 0;
                for (int i = 1; i <= Number_of_nod; i++)
                {
                    for (int k = 1; k <= Number_of_input; k++)
                    {
                        c++;
                        f6.Write("                " + (c-1).ToString() + ":begin");
                        f6.Write("   a_n=i" + k.ToString() + ";");
                        f6.Write("   b_n=w" + i.ToString() + k.ToString() + ";");
                        f6.WriteLine("  end");
                    }
                    //c++;
                    f6.WriteLine("");
                }
                f6.WriteLine("            endcase");
                f6.WriteLine("       end");
                f6.WriteLine("");
                f6.WriteLine("//c="+c.ToString());
                //always----------------------
                f6.WriteLine("   always @(posedge clk)");
                f6.WriteLine("       begin");
                f6.WriteLine("");
                f6.WriteLine("             if(state_n==" + c + ")");
                f6.WriteLine("                    state_n <= 0;");
                f6.WriteLine("             if(en)");
                f6.WriteLine("             begin");
                f6.WriteLine("                   if (res)");
                f6.WriteLine("                   begin");
                f6.Write("                         ");
                for (int k = 1; k <= Number_of_output; k++)
                    f6.Write(" Out" + k.ToString() + " <= 0;");
                f6.WriteLine("");
                f6.WriteLine("                          state_n <= 0;");
                f6.WriteLine("                   end");
                f6.WriteLine("                   else");
                f6.WriteLine("                   begin");
                f6.WriteLine("                        state_n <= state_n + 1;");
                f6.WriteLine("                        case (state_n)");

                c = 1;
                for (int i = 1; i <= Number_of_nod; i++)
                {
                    f6.WriteLine("                             " + (c - 1).ToString() + ":sum_n <= z_n ;");
                    for (int k = 1; k <=Number_of_input-2 ; k++)
                    {
                        c++;
                        f6.WriteLine("                             " + (c - 1).ToString() + ":sum_n <= sum_n + z_n;");
                    }
                    c++;
                    f6.WriteLine("                             " + (c - 1).ToString() + ":Out" + i + "<= sum_n + z_n;");
                    c++;
                    f6.WriteLine("");
                }
                f6.WriteLine("                         endcase");
                f6.WriteLine("                   end");
                f6.WriteLine("");
                f6.WriteLine("             end");
                f6.WriteLine("");
                f6.WriteLine("        end");
                f6.WriteLine("endmodule");
                f6.WriteLine("");
            }
            finally
            {
                f6.Flush();
                f6.Close();
            }
            //System.Diagnostics.Process.Start(@add7);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

//////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Create_Layer(int number_of_input, int number_of_node, int number_Layer, int number_of_mult, int length_i, int length_w, int length_o, string path)
        {
             // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\Layer.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("// Create Date:     " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f.WriteLine("// number_of_input: " + number_of_input);
                f.WriteLine("// number_of_node:  " + number_of_node);
                f.WriteLine("// number_of_mult:  " + number_of_mult);
                f.WriteLine("// path:            " + address);
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.Write("module Layer ( ");
                f.WriteLine("");
                f.Write("              ");
                for (int k = 1; k <= number_of_input; k++)
                    f.Write("i" + k + ",");
                f.WriteLine("");
                f.Write("              ");
                for (int i = 1; i <= number_of_node; i++)
                {
                    for (int k = 1; k <= number_of_input; k++)
                        f.Write("w" + convert(i) + convert(k) + ",");
                    f.Write("B" + convert(i) + ", ");
                }
                f.WriteLine("");
                f.Write("              ");
                for (int k = 1; k <= number_of_node; k++)
                    f.Write("Out" + k + ",");
                f.WriteLine("");
                f.WriteLine("              clk,en,res);");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                for (int k = 1; k <= number_of_input; k++)
                    f.WriteLine("      input [" + (length_i - 1) + ":0] i" + k + ";");
                f.WriteLine("");
                f.WriteLine("");
                for (int i = 1; i <= number_of_node ; i++)
                {
                    for (int k = 1; k <= number_of_input; k++)
                        f.Write("      input [" + (length_w - 1) + ":0] w" + convert(i) + convert(k) + ";");
                    f.Write("      input [" + (length_w - 1) + ":0] B" + convert(i) + ";");
                    f.WriteLine("");
                }
                f.WriteLine("");
                f.WriteLine("");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      output reg [" + (length_i+length_w -1) + ":0] Out" + k + ";");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("      input clk,en,res;");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("      //reg for set all output in 1 clock");
                int temp1 = (int)((Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input)) - number_of_input + 1);
                int tempout = 0;
                for (int i = 1; i <= number_of_node; i++)
                {
                    
                    for (int j = 1; j <= (number_of_mult); j++)
                    {
                        if (tempout >= number_of_node)
                            break;
                        tempout++;
                        f.Write("      ");
                        for (int k = 1; k <= (temp1); k++)
                            f.Write("  reg [" + (length_i + length_w - 1) + ":0] t" + k + "o" + tempout + ";");
                        f.WriteLine("");
                    }
                    temp1 = temp1 - number_of_input;
                    
                }
                f.WriteLine("");
                f.WriteLine("");

               
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      reg [" + (length_i - 1) + ":0] a" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      reg [" + (length_w - 1) + ":0] b" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      wire [" + (length_i + length_w - 1) + ":0] z" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      reg [" + (length_i + length_w - 1) + ":0] sum" + k + "; ");
                f.WriteLine(" ");
                //int temp3 = Math.Max(number_of_node, number_of_input);
                int temp3 = (int)(Math.Log(((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input),2));
                //f.WriteLine("      reg [" + (number_of_node - 1) + ":0] state;");
                f.WriteLine("      reg [" + (temp3 + 1) + ":0] state;");
                f.WriteLine(" ");
                
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      initial Out" + k + " = 0;");
                f.WriteLine(" ");
                f.WriteLine("      initial state = 0;");
                f.WriteLine(" ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      mult m" + k + " (a" + k + ",b" + k + ",z" + k + ",clk); ");
                f.WriteLine(" ");
                f.WriteLine(" ");


                int number_need_mult;
                number_need_mult = number_of_input * number_of_node;
                f.WriteLine("// number_need_mult: " + number_need_mult);
                f.WriteLine("// number_of_mult:   " + number_of_mult);
                f.WriteLine("");
                double temp = (number_need_mult * 1.00) / (number_of_mult * 1.00);
                f.WriteLine("// number_need_mult/number_of_mult:   " + temp + "    " + Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00) * number_of_input));
                f.WriteLine(" ");
                double c = Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input);
                f.WriteLine("//Math.Ceiling((Math.Ceiling((number_of_input * 1.00) / (number_of_mult * 1.00))) * number_of_node)     c = " + c);


                f.WriteLine(" ");
                f.WriteLine("always @(*)");
                f.WriteLine("       begin");
                for (int i = 1; i <= number_of_mult; i++)
                {
                    if (i > number_of_input)
                        break;

                    f.WriteLine("             case (state)");

                    int k = 1;
                    int m = 0;
                    int n = i;
                    for (int j = 1; j <= c; j++)
                    {
                        f.WriteLine("                  " + m + ":begin  a" + i + "=i" + k + ";   b" + i + "=w" + convert(n) + convert(k)+";   end");
                        m++;
                        //n++;
                        if (k == number_of_input)
                        {
                            f.WriteLine(" ");
                            k = 1;
                            n = n + number_of_mult;
                        }
                        else
                            k++;
                        if (n > number_of_node)
                            break;
 
                    }
                    f.WriteLine("             endcase");

                    f.WriteLine(" ");
                }
                f.WriteLine("       end");
                f.WriteLine(" ");



                f.WriteLine("always @(posedge clk)");
                f.WriteLine("  begin");
                f.WriteLine("");

                f.WriteLine("             if(en)");
                f.WriteLine("             begin");
                f.WriteLine("                   if (res)");
                f.WriteLine("                   begin");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("                          Out" + k.ToString() + " <= 0;");
                f.WriteLine("");
                f.WriteLine("                           state <= 0;");
                f.WriteLine("                   end");
                f.WriteLine("                   else");
                f.WriteLine("");
                f.WriteLine("                   begin");
                f.WriteLine("                         if(state==" + (c + 1) + ")");
                f.WriteLine("                                state <= 0;");
                f.WriteLine("                         else");
                f.WriteLine("                                state <= state + 1;");
                
                

                for (int i = 1; i <= number_of_node; i++)//mult
                {
                    if (i > number_of_mult)//input
                        break;
                    f.WriteLine("                        case (state)");
                    int k = 1;
                    int m = 0;
                    int n = i;
                    for (int j = 1; j <= c ; j++)
                    {
                        
                        //f.WriteLine(((m + 1) % number_of_input));
                        if (number_of_input == 1)
                        {
                            f.WriteLine("                              " + (m + 1) + ":t1o" + n + "<= z" + i + "+B" + convert(n) + ";");
                            f.WriteLine("");
                        }
                        else if ((m % number_of_input) == 0 )
                            f.WriteLine("                              " + (m + 1) + ":sum" + n + "<= z" + i + "+B" + convert(n) + ";");
                        else if (((m + 1) % number_of_input) == 0)
                        {

                            f.WriteLine("                              " + (m + 1) + ":t1o" + n + "<=" + "sum" + n + "+z" + i + ";");
                            f.WriteLine("");
                        }
                        else
                            f.WriteLine("                              " + (m + 1) + ":sum" + n + "<=" + "sum" + n + "+z" + i + ";");
                        m++;
                        //n++;
                        if (k == number_of_input)
                        {
                            //f.WriteLine(" ");
                            k = 1;
                            n = n + number_of_mult;
                        }
                        else
                            k++;
                        if (n > number_of_node)//input
                            break;
                    }
                    f.WriteLine("                        endcase");
                    f.WriteLine(" ");
                    }


                    f.WriteLine("                        // set all output in 1 clock");
                    f.WriteLine("");
                    temp1 = (int)((Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input))-number_of_input+1);
                    tempout = 0;
                    for (int i = 1; i <= number_of_node; i++)
                    {

                        for (int j = 1; j <= (number_of_mult); j++)
                        {
                            if (tempout >= number_of_node)
                                break;
                            tempout++;
                            //f.Write("      ");
                            for (int k = 2; k <= (temp1); k++)
                                f.WriteLine("                        t" + k + "o" + tempout + " <= t" + (k-1) + "o" + tempout + ";");
                            f.WriteLine("                        Out" + tempout + " <= t" + temp1 + "o" + tempout + ";");
                            f.WriteLine("");
                        }
                        temp1 = temp1 - number_of_input;

                    }






                    f.WriteLine("                     end");
                    f.WriteLine("");
                    f.WriteLine("            end");
                    f.WriteLine("  end");
                    f.WriteLine("endmodule");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

       

        }
        public void Create_Layer1(int number_of_input, int number_of_node, int number_Layer, int number_of_mult, int length_i, int length_w, int length_o, string path)
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\Layer1.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                //number_of_input=number_of_node;
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("// Create Date:     " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f.WriteLine("// number_of_input: " + number_of_input);
                f.WriteLine("// number_of_node:  " + number_of_node);
                f.WriteLine("// number_of_mult:  " + number_of_mult);
                f.WriteLine("// path:            " + address);
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.Write("module Layer1 ( ");
                f.WriteLine("");
                f.Write("              ");
                for (int k = 1; k <= number_of_input; k++)
                    f.Write("i" + k + ",");
                f.WriteLine("");
                f.Write("              ");
                for (int i = 1; i <= number_of_node; i++)
                {
                    for (int k = 1; k <= number_of_input; k++)
                        f.Write("w" + convert(i) + convert(k) + ",");
                    f.Write(" B" + convert(i) + ",  ");
                }
                f.WriteLine("");
                f.Write("              ");
                for (int k = 1; k <= number_of_node; k++)
                    f.Write("Out" + k + ",");
                f.WriteLine("");
                f.WriteLine("              clk,en,res);");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                for (int k = 1; k <= number_of_input; k++)
                    f.WriteLine("      input [" + (length_i - 1) + ":0] i" + k + ";");
                f.WriteLine("");
                f.WriteLine("");
                for (int i = 1; i <= number_of_node; i++)
                {
                    for (int k = 1; k <= number_of_input; k++)
                        f.Write("      input [" + (length_w - 1) + ":0] w" + convert(i) + convert(k) + ";");
                    f.Write("      input [" + (length_w - 1) + ":0] B" + convert(i) + ";");
                    f.WriteLine("");
                }
                f.WriteLine("");
                f.WriteLine("");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      output reg [" + (length_i + length_w - 1) + ":0] Out" + k + ";");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("      input clk,en,res;");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("      //reg for set all output in 1 clock");
                int temp1 = (int)((Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input)) - number_of_input + 1);
                int tempout = 0;
                for (int i = 1; i <= number_of_node; i++)
                {

                    for (int j = 1; j <= (number_of_mult); j++)
                    {
                        if (tempout >= number_of_node)
                            break;
                        tempout++;
                        f.Write("      ");
                        for (int k = 1; k <= (temp1); k++)
                            f.Write("  reg [" + (length_i + length_w - 1) + ":0] t" + k + "o" + tempout + ";");
                        f.WriteLine("");
                    }
                    temp1 = temp1 - number_of_input;

                }
                f.WriteLine("");
                f.WriteLine("");


                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      reg [" + (length_i - 1) + ":0] a" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      reg [" + (length_w - 1) + ":0] b" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      wire [" + (length_i + length_w - 1) + ":0] z" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      reg [" + (length_i + length_w - 1) + ":0] sum" + k + "; ");
                f.WriteLine(" ");
                ////////////////////////////////////////////////////////////////////////////err
                //int temp3 = Math.Max(number_of_node, number_of_input);
                int temp3 = (int)(Math.Log(((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input), 2));
                //f.WriteLine("      reg [" + (number_of_node - 1) + ":0] state;");
                f.WriteLine("      reg [" + (temp3 + 1) + ":0] state;");

                //f.WriteLine("      reg [" + (number_of_node - 1) + ":0] state;");
                f.WriteLine(" ");

                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      initial Out" + k + " = 0;");
                f.WriteLine(" ");
                f.WriteLine("      initial state = 0;");
                f.WriteLine(" ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      mult m" + k + " (a" + k + ",b" + k + ",z" + k + ",clk); ");
                f.WriteLine(" ");
                f.WriteLine(" ");


                int number_need_mult;
                number_need_mult = number_of_input * number_of_node;
                f.WriteLine("// number_need_mult: " + number_need_mult);
                f.WriteLine("// number_of_mult:   " + number_of_mult);
                f.WriteLine("");
                double temp = (number_need_mult * 1.00) / (number_of_mult * 1.00);
                f.WriteLine("// number_need_mult/number_of_mult:   " + temp + "    " + Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00) * number_of_input));
                f.WriteLine(" ");
                double c = Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input);
                f.WriteLine("//Math.Ceiling((Math.Ceiling((number_of_input * 1.00) / (number_of_mult * 1.00))) * number_of_node)     c = " + c);


                f.WriteLine(" ");
                f.WriteLine("always @(*)");
                f.WriteLine("       begin");
                for (int i = 1; i <= number_of_mult; i++)
                {
                    //if(number_of_input<number_of_mult
                    if (i > number_of_input)
                        break;

                    f.WriteLine("             case (state)");

                    int k = 1;
                    int m = 0;
                    int n = i;
                    for (int j = 1; j <= c; j++)
                    {
                        f.WriteLine("                  " + m + ":begin  a" + i + "=i" + k + ";   b" + i + "=w" + convert(n) + convert(k) + ";   end");
                        m++;
                        //n++;
                        if (k == number_of_input)
                        {
                            f.WriteLine(" ");
                            k = 1;
                            n = n + number_of_mult;
                        }
                        else
                            k++;
                        if (n > number_of_node)
                            break;

                    }
                    f.WriteLine("             endcase");

                    f.WriteLine(" ");
                }
                f.WriteLine("       end");
                f.WriteLine(" ");



                f.WriteLine("always @(posedge clk)");
                f.WriteLine("  begin");
                f.WriteLine("");
                //f.WriteLine("             if(state==" + (c + 1) + ")");
                //f.WriteLine("                    state <= 0;");
                f.WriteLine("             if(en)");
                f.WriteLine("             begin");
                f.WriteLine("                   if (res)");
                f.WriteLine("                   begin");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("                          Out" + k.ToString() + " <= 0;");
                f.WriteLine("");
                f.WriteLine("                           state <= 0;");
                f.WriteLine("                   end");
                f.WriteLine("                   else");
                f.WriteLine("                   begin");
                f.WriteLine("");
                f.WriteLine("                         if(state==" + (c + 1) + ")");
                f.WriteLine("                                state <= 0;");
                f.WriteLine("                         else");
                f.WriteLine("                                state <= state + 1;");
                f.WriteLine("");

                for (int i = 1; i <= number_of_node; i++)//mult
                {
                    if (i > number_of_mult)//input
                        break;
                    f.WriteLine("                        case (state)");
                    int k = 1;
                    int m = 0;
                    int n = i;
                    for (int j = 1; j <= c; j++)
                    {

                        //f.WriteLine(((m + 1) % number_of_input));
                        if (number_of_input == 1)
                        {
                            f.WriteLine("                              " + (m + 1) + ":t1o" + n + "<= z" + i + "+B" + convert(n) + ";");
                            f.WriteLine("");
                        }
                        else if ((m % number_of_input) == 0)
                            f.WriteLine("                              " + (m + 1) + ":sum" + n + "<= z" + i + "+B" + convert(n) + ";");
                        else if (((m + 1) % number_of_input) == 0)
                        {

                            f.WriteLine("                              " + (m + 1) + ":t1o" + n + "<=" + "sum" + n + "+z" + i + ";");
                            f.WriteLine("");
                        }
                        else
                            f.WriteLine("                              " + (m + 1) + ":sum" + n + "<=" + "sum" + n + "+z" + i + ";");
                        m++;
                        //n++;
                        if (k == number_of_input)
                        {
                            //f.WriteLine(" ");
                            k = 1;
                            n = n + number_of_mult;
                        }
                        else
                            k++;
                        if (n > number_of_node)//input
                            break;
                    }
                    f.WriteLine("                        endcase");
                    f.WriteLine(" ");
                }


                f.WriteLine("                        // set all output in 1 clock");
                f.WriteLine("");
                temp1 = (int)((Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input)) - number_of_input + 1);
                tempout = 0;
                for (int i = 1; i <= number_of_node; i++)
                {

                    for (int j = 1; j <= (number_of_mult); j++)
                    {
                        if (tempout >= number_of_node)
                            break;
                        tempout++;
                        //f.Write("      ");
                        for (int k = 2; k <= (temp1); k++)
                            f.WriteLine("                        t" + k + "o" + tempout + " <= t" + (k - 1) + "o" + tempout + ";");
                        f.WriteLine("                        Out" + tempout + " <= t" + temp1 + "o" + tempout + ";");
                        f.WriteLine("");
                    }
                    temp1 = temp1 - number_of_input;

                }






                f.WriteLine("                     end");
                f.WriteLine("");
                f.WriteLine("            end");
                f.WriteLine("  end");
                f.WriteLine("endmodule");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        }
        public void Create_Layer_End(int number_of_input, int number_of_node, int number_Layer, int number_of_mult, int length_i, int length_w, int length_o, string path)
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\Layer_End.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("// Create Date:     " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f.WriteLine("// number_of_input: " + number_of_input);
                f.WriteLine("// number_of_node:  " + number_of_node);
                f.WriteLine("// number_of_mult:  " + number_of_mult);
                f.WriteLine("// path:            " + address);
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.Write("module Layer_End ( ");
                f.WriteLine("");
                f.Write("              ");
                for (int k = 1; k <= number_of_input; k++)
                    f.Write("i" + k + ",");
                f.WriteLine("");
                f.Write("              ");
                for (int i = 1; i <= number_of_node; i++)
                {
                    for (int k = 1; k <= number_of_input; k++)
                        f.Write("w" + convert(i) + convert(k) + ",");
                    f.Write(" B" + convert(i) + ",  ");
                }
                f.WriteLine("");
                f.Write("              ");
                for (int k = 1; k <= number_of_node; k++)
                    f.Write("Out" + k + ",");
                f.WriteLine("");
                f.WriteLine("              clk,en,res);");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                for (int k = 1; k <= number_of_input; k++)
                    f.WriteLine("      input [" + (length_i - 1) + ":0] i" + k + ";");
                f.WriteLine("");
                f.WriteLine("");
                for (int i = 1; i <= number_of_node; i++)
                {
                    for (int k = 1; k <= number_of_input; k++)
                        f.Write("      input [" + (length_w - 1) + ":0] w" + convert(i) + convert(k) + ";");
                    f.Write("      input [" + (length_w - 1) + ":0] B" + convert(i) + ";");
                    f.WriteLine("");
                }
                f.WriteLine("");
                f.WriteLine("");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      output reg [" + (length_i + length_w - 1) + ":0] Out" + k + ";");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("      input clk,en,res;");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("      //reg for set all output in 1 clock");
                int temp1 = (int)((Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input)) - number_of_input + 1);
                int tempout = 0;
                for (int i = 1; i <= number_of_node; i++)
                {

                    for (int j = 1; j <= (number_of_mult); j++)
                    {
                        if (tempout >= number_of_node)
                            break;
                        tempout++;
                        f.Write("      ");
                        for (int k = 1; k <= (temp1); k++)
                            f.Write("  reg [" + (length_i + length_w - 1) + ":0] t" + k + "o" + tempout + ";");
                        f.WriteLine("");
                    }
                    temp1 = temp1 - number_of_input;

                }
                f.WriteLine("");
                f.WriteLine("");


                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      reg [" + (length_i - 1) + ":0] a" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      reg [" + (length_w - 1) + ":0] b" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      wire [" + (length_i + length_w - 1) + ":0] z" + k + "; ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      reg [" + (length_i + length_w - 1) + ":0] sum" + k + "; ");
                f.WriteLine(" ");
                //f.WriteLine("      reg [" + (number_of_node - 1) + ":0] state;");
                int temp3 = Math.Max(number_of_node, number_of_input);
                //f.WriteLine("      reg [" + (number_of_node - 1) + ":0] state;");
                f.WriteLine("      reg [" + (temp3 - 1) + ":0] state;");

                f.WriteLine(" ");

                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("      initial Out" + k + " = 0;");
                f.WriteLine(" ");
                f.WriteLine("      initial state = 0;");
                f.WriteLine(" ");
                f.WriteLine(" ");
                for (int k = 1; k <= number_of_mult; k++)
                    f.WriteLine("      mult m" + k + " (a" + k + ",b" + k + ",z" + k + ",clk); ");
                f.WriteLine(" ");
                f.WriteLine(" ");


                int number_need_mult;
                number_need_mult = number_of_input * number_of_node;
                f.WriteLine("// number_need_mult: " + number_need_mult);
                f.WriteLine("// number_of_mult:   " + number_of_mult);
                f.WriteLine("");
                double temp = (number_need_mult * 1.00) / (number_of_mult * 1.00);
                f.WriteLine("// number_need_mult/number_of_mult:   " + temp + "    " + Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00) * number_of_input));
                f.WriteLine(" ");
                double c = Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input);
                f.WriteLine("//Math.Ceiling((Math.Ceiling((number_of_input * 1.00) / (number_of_mult * 1.00))) * number_of_node)     c = " + c);


                f.WriteLine(" ");
                f.WriteLine("always @(*)");
                f.WriteLine("       begin");
                for (int i = 1; i <= number_of_mult; i++)
                {
                    if (i > number_of_node)
                        break;

                    f.WriteLine("             case (state)");

                    int k = 1;
                    int m = 0;
                    int n = i;
                    for (int j = 1; j <= c; j++)
                    {
                        f.WriteLine("                  " + m + ":begin  a" + i + "=i" + k + ";   b" + i + "=w" + convert(n) + convert(k) + ";   end");
                        m++;
                        //n++;
                        if (k == number_of_input)
                        {
                            f.WriteLine(" ");
                            k = 1;
                            n = n + number_of_mult;
                        }
                        else
                            k++;
                        if (n > number_of_node)
                            break;

                    }
                    f.WriteLine("             endcase");

                    f.WriteLine(" ");
                }
                f.WriteLine("       end");
                f.WriteLine(" ");



                f.WriteLine("always @(posedge clk)");
                f.WriteLine("  begin");
                f.WriteLine("");
                //f.WriteLine("             if(state==" + (c + 1) + ")");
                //f.WriteLine("                    state <= 0;");
                f.WriteLine("             if(en)");
                f.WriteLine("             begin");
                f.WriteLine("                   if (res)");
                f.WriteLine("                   begin");
                for (int k = 1; k <= number_of_node; k++)
                    f.WriteLine("                          Out" + k.ToString() + " <= 0;");
                f.WriteLine("");
                f.WriteLine("                           state <= 0;");
                f.WriteLine("                   end");
                f.WriteLine("                   else");
                f.WriteLine("                   begin");
                
                f.WriteLine("");
                f.WriteLine("                         if(state==" + (c + 1) + ")");
                f.WriteLine("                                state <= 0;");
                f.WriteLine("                         else");
                f.WriteLine("                                state <= state + 1;");
                f.WriteLine("");


                for (int i = 1; i <= number_of_node; i++)//mult
                {
                    if (i > number_of_mult)//input
                        break;
                    f.WriteLine("                        case (state)");
                    int k = 1;
                    int m = 0;
                    int n = i;
                    for (int j = 1; j <= c; j++)
                    {

                        //f.WriteLine(((m + 1) % number_of_input));
                        if (number_of_input == 1)
                        {
                            f.WriteLine("                              " + (m + 1) + ":t1o" + n + "<= z" + i + "+B" + convert(n) + ";");
                            f.WriteLine("");
                        }
                        else if ((m % number_of_input) == 0)
                            f.WriteLine("                              " + (m + 1) + ":sum" + n + "<= z" + i + "+B"+ convert(n) + ";");
                        else if (((m + 1) % number_of_input) == 0)
                        {

                            f.WriteLine("                              " + (m + 1) + ":t1o" + n + "<=" + "sum" + n + "+z" + i + ";");
                            f.WriteLine("");
                        }
                        else
                            f.WriteLine("                              " + (m + 1) + ":sum" + n + "<=" + "sum" + n + "+z" + i + ";");
                        m++;
                        //n++;
                        if (k == number_of_input)
                        {
                            //f.WriteLine(" ");
                            k = 1;
                            n = n + number_of_mult;
                        }
                        else
                            k++;
                        if (n > number_of_node)//input
                            break;
                    }
                    f.WriteLine("                        endcase");
                    f.WriteLine(" ");
                }


                f.WriteLine("                        // set all output in 1 clock");
                f.WriteLine("");
                temp1 = (int)((Math.Ceiling((Math.Ceiling((number_of_node * 1.00) / (number_of_mult * 1.00))) * number_of_input)) - number_of_input + 1);
                tempout = 0;
                for (int i = 1; i <= number_of_node; i++)
                {

                    for (int j = 1; j <= (number_of_mult); j++)
                    {
                        if (tempout >= number_of_node)
                            break;
                        tempout++;
                        //f.Write("      ");
                        for (int k = 2; k <= (temp1); k++)
                            f.WriteLine("                        t" + k + "o" + tempout + " <= t" + (k - 1) + "o" + tempout + ";");
                        f.WriteLine("                        Out" + tempout + " <= t" + temp1 + "o" + tempout + ";");
                        f.WriteLine("");
                    }
                    temp1 = temp1 - number_of_input;

                }






                f.WriteLine("                     end");
                f.WriteLine("");
                f.WriteLine("            end");
                f.WriteLine("  end");
                f.WriteLine("endmodule");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        }
        public void Create_TopModule(int Input, int Layer,int Node, int Output,int length_I,int length_W, int clk_Max, int clk_data, string Path, string path_WI)

        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = Path + "\\Top.v";
            TextWriter f = new StreamWriter(@address);
            
            string add = path_WI;
            StreamReader f0 = new StreamReader(@add);
            string StrText;

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("// Create Date: " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f.WriteLine("// Input:       " + Input);
                f.WriteLine("// Layer:       " + Layer);
                f.WriteLine("// Output:      " + Output);
                f.WriteLine("// clk_Max:     " + clk_Max);
                f.WriteLine("// clk_data:    " + clk_data);
                f.WriteLine("// path:        " + address);
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.Write("module TOP ( ");
                for (int i = 1; i <= Input; i++)
                {
                    f.Write(" In" + i + ", ");
                }
                f.WriteLine("");
                f.Write("             ");
                for (int i = 1; i <= Output ; i++)
                {
                    f.Write(" Out" + i + ",");
                }
                f.WriteLine("");
                f.WriteLine("              clk,en,res);");
                f.WriteLine("");
                f.WriteLine("");

                for (int i = 1; i <= Input; i++)
                {
                    f.WriteLine("    input ["+(length_I-1)+":0] In" + i + ";");
                }
                f.WriteLine("");
                for (int i = 1; i <= Output; i++)
                {
                    f.WriteLine("    output ["+(length_I-1)+":0] Out" + i + ";");
                }
                f.WriteLine("");
                f.WriteLine("    input clk,en,res;");
                f.WriteLine("");
                // wire
                for (int i = 1; i <= Layer+1; i++)
                {
                    for (int j = 1; j <= Node; j++)
                        f.Write("    wire [" + (length_I + length_W - 1) + ":0] OL" + convert(i) +convert(j)+ ";");
                    f.WriteLine("");
                }
                f.WriteLine("");
                //for (int i = 1; i <= Layer+1; i++)
                for (int i = 1; i <= Layer ; i++)
                {
                    for (int j = 1; j <= Node; j++)
                        f.Write("    wire [" + (length_I - 1) + ":0] OF" + convert(i) + convert(j) + ";");
                    f.WriteLine("");
                }
                f.WriteLine("");
                f.WriteLine("");

                
                f.WriteLine("// W  Layer 1");
                for (int i = 1; i <= Node; i++)
                {
                    f.Write("    reg [" + (length_W - 1) + ":0]");
                    for (int k = 1; k <= Input; k++)
                    {
                        StrText = f0.ReadLine().ToString();
                        f.Write(" W001" +convert( i) + convert(k) + "=16'b" + StrText +",");
                    }
                    StrText = f0.ReadLine().ToString();
                    f.Write("  B001" + convert(i) + "=16'b" + StrText + ";");
                    f.WriteLine("");
                }
                for (int i = 2; i <= Layer; i++)
                {
                    f.WriteLine("// W  Layer "+i);
                    for (int j = 1; j <= Node; j++)
                    {
                        f.Write("    reg [" + (length_W - 1) + ":0]");
                        for (int k = 1; k <= Node; k++)
                        {
                            StrText = f0.ReadLine().ToString();
                            f.Write(" W" + convert(i) + convert(j) + convert(k) + "=16'b" + StrText + ",");
                        }
                        StrText = f0.ReadLine().ToString();
                        f.Write("  B" + convert(i) + convert(j) + "=16'b" + StrText + ";");
                        f.WriteLine("");
                    }
                }
                f.WriteLine("// W  Layer End");
                for (int i = 1; i <= Output; i++)
                {
                    f.Write("    reg [" + (length_W - 1) + ":0]");
                    for (int k = 1; k <= Node; k++)
                    {
                        StrText = f0.ReadLine().ToString();
                        f.Write(" W" + convert(Layer + 1) + convert(i) + convert(k) + "=16'b" + StrText + ",");
                    }
                    StrText = f0.ReadLine().ToString();
                    f.Write("  B" + convert(Layer + 1) + convert(i) + "=16'b" + StrText + ";");
                    f.WriteLine("");
                }
                /*
                for (int i = 1; i <= Layer+1; i++)
                {
                    for (int j = 1; j <= Input; j++)
                        for (int k = 1; k <= Node; k++)
                            f.Write("    reg ["+ (length_W-1) +":0] W" + i +j+ k + "=1;");
                    f.WriteLine("");
                }    */
                //instance Layer & Act.Func
                f.WriteLine("//instance Layer & Act.Func");
                f.WriteLine("");
                f.Write("   Layer1   L1  (");
                for (int i = 1; i <= Input; i++)
                {
                    f.Write("In" + i+",");
                }
                f.Write("  ");
                for (int i = 1; i <= Node ; i++)
                {
                    for (int k = 1; k <= Input; k++)
                    {
                        f.Write("W001" + convert(i) + convert(k) + ",");
                    }
                    f.Write(" B001" + convert(i) + ",  ");
                }
                f.Write("  ");
                for (int i = 1; i <= Node; i++)
                {
                    f.Write("OL001" +convert( i) + ",");
                }
                f.Write("  ");
                f.WriteLine("clk,en,res);");
                f.Write("   ActFunc AF1 (");
                for (int i = 1; i <= Node; i++)
                {
                    f.Write("OL001" + convert(i) + ",");
                }
                f.Write("  ");
                for (int i = 1; i <= Node; i++)
                {
                    f.Write("OF001" +convert( i) + ",");
                }
                f.Write("  ");
                f.WriteLine("clk,en,res);");
                f.WriteLine("");

                for (int i = 2; i <= Layer; i++)
                {
                    f.Write("   Layer   L" + i + "  (");
                    for (int j = 1; j <= Node; j++)
                    {
                        f.Write("OF" + convert(i - 1) + convert(j) + ",");
                    }
                    f.Write("  ");
                    for (int j = 1; j <= Node; j++)
                    {
                        for (int k = 1; k <= Node; k++)
                        {
                            f.Write("W" + convert(i) + convert(j) + convert(k) + ",");
                        }
                        f.Write(" B" + convert(i) + convert(j) + ",  ");
                    }
                    f.Write("  ");
                    for (int j = 1; j <= Node; j++)
                    {
                        f.Write("OL" + convert(i) + convert(j) + ",");
                    }
                    f.Write("  ");
                    f.WriteLine("clk,en,res);");
                    
                    f.Write("   ActFunc AF" + i + " (");
                    for (int j = 1; j <= Node; j++)
                    {
                        f.Write("OL" + convert(i) + convert(j) +",");
                    }
                    f.Write("  ");
                    for (int j = 1; j <= Node; j++)
                    {
                        f.Write("OF" + convert(i) + convert(j) + ",");
                    }
                    f.Write("  ");
                    f.WriteLine("clk,en,res);");
                    f.WriteLine("");
                    
                }
                f.Write("   Layer_End   L_End  (");
                for (int i = 1; i <= Node; i++)
                {
                    f.Write("OF"+ convert(Layer) + convert(i) + ",");
                }
                f.Write("  ");
                for (int i = 1; i <=Output ; i++)
                {
                    for (int k = 1; k <= Node; k++)
                    {
                        f.Write("W"+convert(Layer+1) + convert(i) + convert(k) + ",");
                    }
                    f.Write(" B" + convert(Layer + 1) + convert(i) + ",  ");
                }
                f.Write("  ");
                for (int i = 1; i <= Output; i++)
                {
                    f.Write("OL" + convert(Layer + 1) + convert(i) + ",");
                }
                f.Write("  ");
                f.WriteLine("clk,en,res);");
                f.Write("   ActFunc_End AF_End (");
                for (int i = 1; i <= Output; i++)
                {
                    f.Write("OL" + convert(Layer + 1) + convert(i) + ",");
                }
                f.Write("  ");
                for (int i = 1; i <= Output; i++)
                {
                    f.Write("Out" + i + ",");
                }
                f.Write("  ");
                f.WriteLine("clk,en,res);");
                f.WriteLine("");

                f.WriteLine("");
                /*
                f.WriteLine("always @(posedge clk)");
                f.WriteLine("    begin");
                for (int i = 1; i <= Output; i++)
                {
                    f.WriteLine("         Out"+i+" <= OF" + (Layer + 1) + i + ";");
                }
                f.WriteLine("    end");
                f.WriteLine("");
                */
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("endmodule");

            }
            finally
            {
                f.Flush();
                f.Close();
                f0.Close();
            }
            //System.Diagnostics.Process.Start(@address);
        
        }
        public void Create_AF(int number_of_input,int length_in_m, int length_in_n, int length_out_x, int length_out_y,int length_W, string path)
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\ActFunc.v";
            TextWriter f = new StreamWriter(@address);


            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("// Create Date:            " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f.WriteLine("// Input:       " + number_of_input);
                f.WriteLine("// WordLenght Input(m+n):  " + (length_in_m + length_in_n) + "   (" + length_in_m + " + " + length_in_n + ")");
                f.WriteLine("// WordLenght Output(x+y): " + (length_out_x + length_out_y) + "   (" + length_out_x + " + " + length_out_y + ")");
                f.WriteLine("// path:        " + address);
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.Write("module ActFunc ( ");
                for (int i = 1; i <= number_of_input; i++)
                {
                    f.Write(" In_AF" + i + ", ");
                }
                f.WriteLine("");
                f.Write("             ");
                for (int i = 1; i <= number_of_input; i++)
                    f.Write(" Out_AF" + i + ",");
                f.WriteLine("");
                f.WriteLine("              clk,en,res);");
                f.WriteLine("");
                f.WriteLine("");

                for (int i = 1; i <= number_of_input; i++)
                    f.WriteLine("    input [" + (length_in_m + length_in_n-1) + ":0] In_AF" + i + ";");
                f.WriteLine("");
                for (int i = 1; i <= number_of_input; i++)
                    f.WriteLine("    output reg [" + (length_out_x + length_out_y-1) + ":0] Out_AF" + i + ";");
                f.WriteLine("");
                f.WriteLine("    input clk,en,res;");
                f.WriteLine("");

                f.WriteLine("    reg signed [" + (length_in_m + length_in_n-1) + ":0] a;");
                f.WriteLine("    wire signed [" + (length_out_x + length_out_y-1) + ":0] z;");
                f.WriteLine("");
                f.WriteLine("    Function_Interpolation FI1 (a,z,clk);");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("    reg [5:0] state;");
                f.WriteLine("");
                f.WriteLine("    initial state=0;");
                f.WriteLine("");
                f.WriteLine("always @(*)");
                f.WriteLine("    begin");
                f.WriteLine("         case (state)");
                for (int i = 0; i <= number_of_input-1; i++)
                    f.WriteLine("              " + i + ":a<=In_AF" + (i + 1) + ";");

                f.WriteLine("         endcase");
                f.WriteLine("    end");
                f.WriteLine("");
                f.WriteLine("always @(posedge clk)");
                f.WriteLine("   begin");

                f.WriteLine("        if(en)");
                f.WriteLine("           begin");
                f.WriteLine("                 if (res)");
                f.WriteLine("                    begin");
                f.WriteLine("                        state <= 0;");
                for (int i = 0; i <= number_of_input - 1; i++)
                    f.WriteLine("                        Out_AF" + (i+1) + "<=0;");



                f.WriteLine("                    end");
                f.WriteLine("                  else");
                f.WriteLine("                    begin");
                f.WriteLine("                          if(state==" + number_of_input + ")");
                f.WriteLine("                                 state <= 0;");
                f.WriteLine("                          else");
                f.WriteLine("                                 state <= state + 1;");
                f.WriteLine("");
                f.WriteLine("                          case (state)");
                for (int i = 0; i <= number_of_input - 1; i++)
                    f.WriteLine("                              " + i + ":Out_AF" + (i + 1) + "<=z;");
                f.WriteLine("                          endcase");


                f.WriteLine("                    end");
                f.WriteLine("             end");
                f.WriteLine("   end");
                f.WriteLine("");

                /* f.WriteLine("always @(posedge clk)");
                 f.WriteLine("    begin");
                 for (int i = 1; i <= Output; i++)
                 {
                     f.WriteLine("         Out" + i + " <= OF" + (Layer + 1) + i + ";");
                 }
                 f.WriteLine("    end");
                 */
                f.WriteLine("");

                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("endmodule");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);
        
        }
        public void Create_AF_End(int number_of_input, int length_in_m, int length_in_n, int length_out_x, int length_out_y, int length_W, string path)
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\ActFunc_End.v";
            TextWriter f = new StreamWriter(@address);


            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("// Create Date:            " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f.WriteLine("// Input:       " + number_of_input);
                f.WriteLine("// WordLenght Input(m+n):  " + (length_in_m + length_in_n) + "   (" + length_in_m + " + " + length_in_n + ")");
                f.WriteLine("// WordLenght Output(x+y): " + (length_out_x + length_out_y) + "   (" + length_out_x + " + " + length_out_y + ")");
                f.WriteLine("// path:        " + address);
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.Write("module ActFunc_End ( ");
                for (int i = 1; i <= number_of_input; i++)
                {
                    f.Write(" In_AF" + i + ", ");
                }
                f.WriteLine("");
                f.Write("             ");
                for (int i = 1; i <= number_of_input; i++)
                    f.Write(" Out_AF" + i + ",");
                f.WriteLine("");
                f.WriteLine("              clk,en,res);");
                f.WriteLine("");
                f.WriteLine("");

                for (int i = 1; i <= number_of_input; i++)
                    f.WriteLine("    input [" + (length_in_m + length_in_n-1 ) + ":0] In_AF" + i + ";");
                f.WriteLine("");
                for (int i = 1; i <= number_of_input; i++)
                    f.WriteLine("    output reg [" + (length_out_x + length_out_y-1) + ":0] Out_AF" + i + ";");
                f.WriteLine("");
                f.WriteLine("    input clk,en,res;");
                f.WriteLine("");

                f.WriteLine("    reg signed [" + (length_in_m + length_in_n-1) + ":0] a;");
                f.WriteLine("    wire signed [" + (length_out_x + length_out_y-1) + ":0] z;");
                f.WriteLine("");
                f.WriteLine("    Function_Interpolation FI1 (a,z,clk);");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("    reg [5:0] state;");
                f.WriteLine("");
                f.WriteLine("    initial state=0;");
                f.WriteLine("");
                f.WriteLine("always @(*)");
                f.WriteLine("    begin");
                f.WriteLine("         case (state)");
                for (int i = 0; i <= number_of_input - 1; i++)
                    f.WriteLine("              " + i + ":a<=In_AF" + (i + 1) + ";");

                f.WriteLine("         endcase");
                f.WriteLine("    end");
                f.WriteLine("");
                f.WriteLine("always @(posedge clk)");
                f.WriteLine("   begin");
                //f.WriteLine("        if(state==" + number_of_input + ")");
                //f.WriteLine("                state <= 0;");
                f.WriteLine("        if(en)");
                f.WriteLine("           begin");
                f.WriteLine("                 if (res)");
                f.WriteLine("                    begin");
                f.WriteLine("                         state <= 0;");
                for (int i = 0; i <= number_of_input - 1; i++)
                    f.WriteLine("                        Out_AF" + (i+1) + "<=0;");



                f.WriteLine("                    end");
                f.WriteLine("                  else");
                f.WriteLine("                    begin");

                f.WriteLine("                          if(state==" + number_of_input + ")");
                f.WriteLine("                                 state <= 0;");
                f.WriteLine("                          else");
                f.WriteLine("                                 state <= state + 1;");
                f.WriteLine("");
                //f.WriteLine("                          state <= state + 1;");
                f.WriteLine("                          case (state)");
                for (int i = 0; i <= number_of_input - 1; i++)
                    f.WriteLine("                              " + i + ":Out_AF" + (i + 1) + "<=z;");
                f.WriteLine("                          endcase");


                f.WriteLine("                    end");
                f.WriteLine("             end");
                f.WriteLine("   end");
                f.WriteLine("");
                /*f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                 f.WriteLine("always @(posedge clk)");
                 f.WriteLine("    begin");
                 for (int i = 1; i <= Output; i++)
                 {
                     f.WriteLine("         Out" + i + " <= OF" + (Layer + 1) + i + ";");
                 }
                 f.WriteLine("    end");
                 */
                f.WriteLine("");

                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("endmodule");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);

        }
        public void Create_mult(int length_I,int length_W,string path)
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\mult.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("");
                //------------------------
                f.WriteLine("module mult(a,b,z,clk);");
                f.WriteLine("");
                //f.WriteLine("   (* use_dsp48 =" + '"' + "yes" + '"' + "*)");
                f.WriteLine("   input [" + (length_I -1) + ":0] a;");
                f.WriteLine("   input [" + (length_W -1) + ":0] b;");
                f.WriteLine("   input clk;");
                //f.WriteLine("   reg [" + (length_I + length_W - 1) + ":0] t1,t2;");
                f.WriteLine("   output reg [" + (length_I + length_W-1) + ":0] z;");
                f.WriteLine("");
                f.WriteLine("   always @(posedge clk)");
                f.WriteLine("     begin");
                f.WriteLine("       z<=a*b;");
                //f.WriteLine("       t2<=t1;");
                //f.WriteLine("        z<=t2;");
                f.WriteLine("     end");
                f.WriteLine("endmodule");
                f.WriteLine("");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);


        }
        public void Create_Function_Interpolation(int length_I, int length_O,int depth, string path)
        {
            // length_I is s+m+n
            // length_O is s+x+y
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\Function_Interpolation.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("");
                //------------------------
                f.WriteLine("module Function_Interpolation(inputVal,outputVal,clk);");
                f.WriteLine("");
                f.WriteLine("   input [" + (length_I - 1) + ":0] inputVal; //m+n bit (m contains s), in range [-4,4): m=3, n=7");
                f.WriteLine("   output reg [" + (length_O - 1) + ":0] outputVal;  //x+y bit (x contains s), in range (-1,1): x=1, y=15");
                f.WriteLine("   input clk;");
                f.WriteLine("   reg signed [" + (length_O - 1) + ":0] samples [0:" + (depth - 1) + "]; //width:x+y, depth:128");
                f.WriteLine("   initial $readmemb (" + '"' + "Samples.txt" + '"' + ", samples, 0, " + (depth-1 )+ ");");
                int depth2 = (int) (Math.Log(Convert.ToDouble(depth), 2));
                f.WriteLine("// log2(" + depth+")="+depth2);
                //f.WriteLine("   wire [" + (length_O - depth2) + ":0] address = {~inputVal[" + (length_O - depth2) + "],inputVal[" + (length_O - depth2 - 1) + ":0]}; //2's comp to SOB(unsigned)");
                f.WriteLine("   wire [" + (length_I - 1) + ":0] address = {~inputVal[" + (length_I - 1) + "],inputVal[" + (length_I - 2) + ":0]}; //2's comp to SOB(unsigned)  , length_Input="+ length_I );
                //f.WriteLine("   wire [" + (depth2 - 1) + ":0]x1=address[" + (length_O - depth2) + ":3]; //log2(128)=7, first sample address");
                f.WriteLine("   wire [" + (depth2 - 1) + ":0]x1=address[" + (length_I - 1) + ":"+ (length_I -depth2) +"]; //log2(128)=7, first sample address");
                f.WriteLine("   wire [" + (depth2 - 1) + ":0]x2=x1+1; //second sample address");
                f.WriteLine("   wire ["+ (length_I -depth2-1) +":0]xmx1=address["+ (length_I -depth2-1) +":0];");
                f.WriteLine("   wire signed ["+ (length_I -depth2) +":0] xmx1s = {1'b0,xmx1};");//change
                f.WriteLine("   reg signed [" + (length_O - 1) + ":0] SampleX1, SampleX2, SampleX1L1, SampleX1L2;");
                f.WriteLine("   reg signed ["+ (length_I -depth2) +":0] xmx1sL1, xmx1sL2;");
                f.WriteLine("   reg signed [" + (length_O - 1) + ":0] diff;");
                f.WriteLine("   reg signed [" + (length_O - 1) + ":0] InterpolateValue;");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("   always @(posedge clk)");
                f.WriteLine("     begin");
                f.WriteLine("           //pipeline stage 1:");
                f.WriteLine("           SampleX1 <= samples[x1];");
                f.WriteLine("           SampleX2 <= samples[x2];");
                f.WriteLine("           xmx1sL1 <= xmx1s;");
                f.WriteLine("           ");
                f.WriteLine("           //pipeline stage 2:");
                f.WriteLine("           diff <= SampleX2-SampleX1;");
                f.WriteLine("           SampleX1L1 <= SampleX1;");
                f.WriteLine("           xmx1sL2 <= xmx1sL1;");
                f.WriteLine("           ");
                f.WriteLine("           //pipeline stage 3:");
                f.WriteLine("           InterpolateValue <= (diff*xmx1sL2)>>>"+ (length_I -depth2) +";");
                f.WriteLine("           SampleX1L2 <= SampleX1L1;");
                f.WriteLine("           ");
                f.WriteLine("           //pipeline stage 4:");
                f.WriteLine("           outputVal <= SampleX1L2+InterpolateValue;");
                f.WriteLine("");
                f.WriteLine("     end");
                f.WriteLine("endmodule");
                f.WriteLine("");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);


        }

        //New    creat AF Linear & HardLimit    ...not tested...
        public void Create_Function_Linear(int length_I, int length_O, int depth, string path)
        {
            // length_I is s+m+n
            // length_O is s+x+y
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\Function_Interpolation.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("");
                //------------------------
                f.WriteLine("module Function_Interpolation(inputVal,outputVal,clk);");
                f.WriteLine("");
                f.WriteLine("   input [" + (length_I - 1) + ":0] inputVal; //m+n bit (m contains s), in range [-4,4): m=3, n=7");
                f.WriteLine("   output reg [" + (length_O - 1) + ":0] outputVal;  //x+y bit (x contains s), in range (-1,1): x=1, y=15");
                f.WriteLine("   input clk;");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("   always @(posedge clk)");
                f.WriteLine("     begin");
                f.WriteLine("           outputVal<=inputVal;");
                f.WriteLine("");
                f.WriteLine("           if (inputVal > "+(length_O^2)+")");
                f.Write("               outputVal<="+length_O+"'b0");
                for (int i = 2; i < length_O; i++)
                    f.Write("1");
                f.WriteLine(";");
                f.WriteLine("           else");
                f.WriteLine("                if (inputVal < -)"+(length_O^2)+")");
                f.WriteLine("                    outputVal<=16'b1");
                for (int i = 2; i < length_O; i++)
                    f.Write("0");
                f.WriteLine(";");
                f.WriteLine("");
                f.WriteLine("     end");
                f.WriteLine("endmodule");
                f.WriteLine("");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);


        }
        public void Create_Function_HardLimit01(int length_I, int length_O, int depth, string path)
        {
            // length_I is s+m+n
            // length_O is s+x+y
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\Function_Interpolation.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("");
                //------------------------
                f.WriteLine("module Function_Interpolation(inputVal,outputVal,clk);");
                f.WriteLine("");
                f.WriteLine("   input [" + (length_I - 1) + ":0] inputVal; //m+n bit (m contains s), in range [-4,4): m=3, n=7");
                f.WriteLine("   output reg [" + (length_O - 1) + ":0] outputVal;  //x+y bit (x contains s), in range (-1,1): x=1, y=15");
                f.WriteLine("   input clk;");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("   always @(posedge clk)");
                f.WriteLine("     begin");
                f.WriteLine("           outputVal<=inputVal;");
                f.WriteLine("");
                f.WriteLine("           if (inputVal >= 0) ");
                f.WriteLine("               outputVal<=1;");
                f.WriteLine("           else");
                f.WriteLine("                if (inputVal < 0)");
                f.WriteLine("                    outputVal<=0;");
                f.WriteLine("");
                f.WriteLine("     end");
                f.WriteLine("endmodule");
                f.WriteLine("");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);


        }
        public void Create_Function_HardLimit11(int length_I, int length_O, int depth, string path)
        {
            // length_I is s+m+n
            // length_O is s+x+y
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = path + "\\Function_Interpolation.v";
            TextWriter f = new StreamWriter(@address);

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("");
                //------------------------
                f.WriteLine("module Function_Interpolation(inputVal,outputVal,clk);");
                f.WriteLine("");
                f.WriteLine("   input [" + (length_I - 1) + ":0] inputVal; //m+n bit (m contains s), in range [-4,4): m=3, n=7");
                f.WriteLine("   output reg [" + (length_O - 1) + ":0] outputVal;  //x+y bit (x contains s), in range (-1,1): x=1, y=15");
                f.WriteLine("   input clk;");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("   always @(posedge clk)");
                f.WriteLine("     begin");
                f.WriteLine("           outputVal<=inputVal;");
                f.WriteLine("");
                f.WriteLine("           if (inputVal >= 0) ");
                f.WriteLine("               outputVal<=1;");
                f.WriteLine("           else");
                f.WriteLine("                if (inputVal < 0)");
                f.WriteLine("                    outputVal<=-1;");
                f.WriteLine("");
                f.WriteLine("     end");
                f.WriteLine("endmodule");
                f.WriteLine("");

            }
            finally
            {
                f.Flush();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@address);


        }
        
        // create Test bench.....
        public void Create_Test_TopModule(int Input, int Output, int length_I, string Path)//, int clk_Max, int clk_data, string Path, string path_Input)
        {
            // p is Date & Time
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            string address = Path + "\\Test_Top.v";
            TextWriter f = new StreamWriter(@address);
            /*
            string add = path_Input;
            StreamReader f0 = new StreamReader(@add);
            string StrText;
             */

            try
            {
                f.WriteLine("`timescale 1ns / 1ps");
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("// Create Date: " + p.GetHour(DateTime.Now) + ":" + p.GetMinute(DateTime.Now) + ":" + p.GetSecond(DateTime.Now) + "    " + p.GetYear(DateTime.Now) + "/" + p.GetMonth(DateTime.Now) + "/" + p.GetDayOfMonth(DateTime.Now));
                f.WriteLine("// Input:       " + Input);
                f.WriteLine("// Output:      " + Output);
                //f.WriteLine("// clk_Max:     " + clk_Max);
                //f.WriteLine("// clk_data:    " + clk_data);
                f.WriteLine("// path:        " + address);
                f.WriteLine("//////////////////////////////////////////////////////////////////////////////////");
                f.WriteLine("module Test_Top ;");
                // Inputs
                f.WriteLine("    //Inputs");
                for (int i = 1; i <= Input; i++)
                {
                    f.WriteLine("    reg [" + (length_I - 1) + ":0] In" + i + ";");
                }
                f.WriteLine("    reg clk;");
                f.WriteLine("    reg en;");
                f.WriteLine("    reg res;");
                // Outputs
                f.WriteLine("    // Outputs");
                for (int i = 1; i <= Output; i++)
                {
                    f.WriteLine("    wire [" + (length_I - 1) + ":0] Out" + i + ";");
                }
                // Instantiate the Unit Under Test (UUT)
                f.WriteLine("    // Instantiate the Unit Under Test (UUT)");
                
                f.WriteLine("    TOP uut (");
                for (int i = 1; i <= Input; i++)
                {
                    f.WriteLine("    .In" + i + "(In" + i + "),");
                }
                for (int i = 1; i <= Output; i++)
                {
                    f.WriteLine("    .Out" + i + "(Out" + i + "),");
                }
                f.WriteLine("    .clk(clk),");
                f.WriteLine("    .en(en),");
                f.WriteLine("    .res(res)");
                f.WriteLine("    );");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("    integer f1;");
                f.WriteLine("");
                f.WriteLine("    initial begin");
                //f1=$fopen("Result.txt","w");
                f.WriteLine("        f1=$fopen(" + '"' + "Result.txt" + '"' + "," + '"' + "w" + '"' + ");");
                f.WriteLine("    end");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("  initial begin");
                for (int i = 1; i <= Input; i++)
                {
                    f.WriteLine("    In" + i + "=0;");
                }
                f.WriteLine("    clk = 0;");
                f.WriteLine("    en = 1;");
                f.WriteLine("    res = 0;");
                f.WriteLine("    ");
                /*
                 for (int i = 1; i <= Number_of_TestVector; i++)
                 {                 
                     for (int k = 1; k <= Input; k++)
                        {
                            StrText = f0.ReadLine().ToString();
                            f.WriteLine(" In =" + StrText +";");
                            
                        }
                     f.WriteLine("// Wait 100 ns for global reset to finish");
                     f.WriteLine("#2000;");
                 }
                    		
        In1 = 4;
		clk = 0;
		en = 1;
		res = 0;

		// Wait 100 ns for global reset to finish
		#2000;
      In1 = 2; 
                 */




                f.WriteLine("  end");
                f.WriteLine("    ");
                f.WriteLine("  always #5 clk = ~clk; ");
                f.WriteLine("    ");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("  always @(*)");
                //$fwrite (f1,"1:%d | 2:%d \n",In1,Out1);
//                f.Write("        $fwrite (f1," + '"' + "Out1:%d");
                f.Write("        $fwrite (f1," + '"');

                for (int i = 1; i <= Input; i++)
                {
                    f.Write(" In" + i + ": %b |");
                }
                for (int i = 1; i <= Output; i++)
                {
                    f.Write(" Out" + i + ":%b |");
                }

                f.Write('"' + ",");
                for (int i = 1; i <= Input; i++)
                {
                    f.Write(" In" + i + ",");
                }
                for (int i = 1; i <= Output; i++)
                {
                    f.Write(" Out" + i + ",");
                }



                
                f.Write('"'+" \\n" + '"');
                f.WriteLine(");");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine(" endmodule  ");
                f.WriteLine("    ");
                f.WriteLine("    ");

                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");
                f.WriteLine("");

            }
            finally
            {
                f.Flush();
                f.Close();
                //f0.Close();
            }
            //System.Diagnostics.Process.Start(@address);

        }

        public void Create_Tcl( string Path)//, int clk_Max, int clk_data, string Path, string path_Input)
        {

            string address = Path + "\\Tcl1.tcl";
            TextWriter f = new StreamWriter(@address);
           
            string Path1 = Path;
            Path1 = Path1.Replace("\\", "/");
                try
                {
                    f.WriteLine("# where all output will be created");
                    f.WriteLine("set compile_directory " + Path1);
                    f.WriteLine("# the top level of our HDL source");
                    f.WriteLine("set top_name Top");
                    f.WriteLine("#input source files");
                    f.WriteLine("set hdl_files {");
                    f.WriteLine("     " + Path1 + "/Top.v");
                    f.WriteLine("     " + Path1 + "/Layer1.v");
                    f.WriteLine("     " + Path1 + "/Layer.v");
                    f.WriteLine("     " + Path1 + "/Layer_End.v");
                    f.WriteLine("     " + Path1 + "/ActFunc.v");
                    f.WriteLine("     " + Path1 + "/ActFunc_End.v");
                    f.WriteLine("     " + Path1 + "/Function_Interpolation.v");
                    f.WriteLine("     " + Path1 + "/mult.v");
                    f.WriteLine("     " + Path1 + "/Samples.txt");
                    f.WriteLine("}");
                    f.WriteLine("#create compile dirctory if it has not been created before");
                    f.WriteLine("if {![file isdirectory $compile_directory]} {");
                    f.WriteLine("	file mkdir $compile_directory");
                    f.WriteLine("}");
                    f.WriteLine("cd $compile_directory");
                    f.WriteLine("# make project in compile directory if there is none");
                    f.WriteLine("project new project_top");
                    f.WriteLine("project set family " + '"' + comboBox3.SelectedItem.ToString() + '"');
                    //f.WriteLine("project set family " + '"' + "Virtex4" + '"');
                    if (comboBox3.SelectedItem.ToString() == "Virtex7")
                    {
                        f.WriteLine("project set device " + '"' + "xc7vx330t" + '"');
                        f.WriteLine("project set package " + '"' + "FFG1157" + '"');
                    }
                    if (comboBox3.SelectedItem.ToString() == "Virtex4")
                    {
                        f.WriteLine("project set device " + '"' + "xc4vfx12" + '"');
                        f.WriteLine("project set package " + '"' + "sf363" + '"');
                    }
                    if (comboBox3.SelectedItem.ToString() == "Spartan6")
                    {
                        f.WriteLine("project set device " + '"' + "xc6slx150" + '"');
                        f.WriteLine("project set package " + '"' + "fgg484" + '"');
                    }

                    f.WriteLine("# add specified files to project");
                    f.WriteLine("foreach filename $hdl_files {");
                    f.WriteLine("	xfile add $filename");
                    f.WriteLine("   puts " + '"' + "Adding file $filename to the project." + '"');
                    f.WriteLine("}");
                    f.WriteLine("process run  " + '"' + "Synthesize" + '"');
                    f.WriteLine("project save");
                    f.WriteLine("project close");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");
                    f.WriteLine("");

                }
                finally
                {
                    f.Flush();
                    f.Close();
                    //f0.Close();
                }
            //System.Diagnostics.Process.Start(@address);

        }
        public void Create_bat_file(string Path)//, int clk_Max, int clk_data, string Path, string path_Input)
        {

            string address = Path + "\\B1.bat";
            TextWriter f = new StreamWriter(@address);
            try
            {
                f.WriteLine("xtclsh "+Path +"\\Tcl1.tcl");
                f.WriteLine( Path + "\\Top.syr");


            }
            finally
            {
                f.Flush();
                f.Close();
                //f0.Close();
            }
            //System.Diagnostics.Process.Start(@address);

        }
        public void Create_Samples_file(string Path)//, int clk_Max, int clk_data, string Path, string path_Input)
        {

            string address = Path + "\\Samples.txt";
            TextWriter f = new StreamWriter(@address);
            try
            {
                f.WriteLine("1000000000010110");
                f.WriteLine("1000000000011000");
                f.WriteLine("1000000000011100");
                f.WriteLine("1000000000100000");
                f.WriteLine("1000000000100100");
                f.WriteLine("1000000000101000");
                f.WriteLine("1000000000101110");
                f.WriteLine("1000000000110100");
                f.WriteLine("1000000000111100");
                f.WriteLine("1000000001000100");
                f.WriteLine("1000000001001100");
                f.WriteLine("1000000001010110");
                f.WriteLine("1000000001100010");
                f.WriteLine("1000000001101110");
                f.WriteLine("1000000001111110");
                f.WriteLine("1000000010001110");
                f.WriteLine("1000000010100010");
                f.WriteLine("1000000010111000");
                f.WriteLine("1000000011010000");
                f.WriteLine("1000000011101100");
                f.WriteLine("1000000100001010");
                f.WriteLine("1000000100101110");
                f.WriteLine("1000000101010110");
                f.WriteLine("1000000110000010");
                f.WriteLine("1000000110110110");
                f.WriteLine("1000000111110000");
                f.WriteLine("1000001000110010");
                f.WriteLine("1000001001111100");
                f.WriteLine("1000001011010000");
                f.WriteLine("1000001100101110");
                f.WriteLine("1000001110011010");
                f.WriteLine("1000010000010010");
                f.WriteLine("1000010010011010");
                f.WriteLine("1000010100110100");
                f.WriteLine("1000010111100010");
                f.WriteLine("1000011010100100");
                f.WriteLine("1000011110000000");
                f.WriteLine("1000100001111000");
                f.WriteLine("1000100110001110");
                f.WriteLine("1000101011000110");
                f.WriteLine("1000110000100100");
                f.WriteLine("1000110110101100");
                f.WriteLine("1000111101100010");
                f.WriteLine("1001000101001010");
                f.WriteLine("1001001101101010");
                f.WriteLine("1001010111001000");
                f.WriteLine("1001100001101000");
                f.WriteLine("1001101101010000");
                f.WriteLine("1001111010000100");
                f.WriteLine("1010001000001010");
                f.WriteLine("1010010111100110");
                f.WriteLine("1010101000011110");
                f.WriteLine("1010111010110010");
                f.WriteLine("1011001110101010");
                f.WriteLine("1011100100000010");
                f.WriteLine("1011111010111110");
                f.WriteLine("1100010011011000");
                f.WriteLine("1100101101010010");
                f.WriteLine("1101001000100000");
                f.WriteLine("1101100101000000");
                f.WriteLine("1110000010100110");
                f.WriteLine("1110100001000110");
                f.WriteLine("1111000000010100");
                f.WriteLine("1111100000000010");
                f.WriteLine("0000000000000000");
                f.WriteLine("0000011111111101");
                f.WriteLine("0000111111101011");
                f.WriteLine("0001011110111001");
                f.WriteLine("0001111101011001");
                f.WriteLine("0010011010111111");
                f.WriteLine("0010110111011111");
                f.WriteLine("0011010010101110");
                f.WriteLine("0011101100100111");
                f.WriteLine("0100000101000010");
                f.WriteLine("0100011011111101");
                f.WriteLine("0100110001010110");
                f.WriteLine("0101000101001101");
                f.WriteLine("0101010111100010");
                f.WriteLine("0101101000011010");
                f.WriteLine("0101110111110110");
                f.WriteLine("0110000101111100");
                f.WriteLine("0110010010110000");
                f.WriteLine("0110011110010111");
                f.WriteLine("0110101000110111");
                f.WriteLine("0110110010010101");
                f.WriteLine("0110111010110101");
                f.WriteLine("0111000010011110");
                f.WriteLine("0111001001010100");
                f.WriteLine("0111001111011100");
                f.WriteLine("0111010100111010");
                f.WriteLine("0111011001110010");
                f.WriteLine("0111011110001000");
                f.WriteLine("0111100001111111");
                f.WriteLine("0111100101011011");
                f.WriteLine("0111101000011110");
                f.WriteLine("0111101011001011");
                f.WriteLine("0111101101100101");
                f.WriteLine("0111101111101110");
                f.WriteLine("0111110001100110");
                f.WriteLine("0111110011010001");
                f.WriteLine("0111110100110000");
                f.WriteLine("0111110110000100");
                f.WriteLine("0111110111001110");
                f.WriteLine("0111111000001111");
                f.WriteLine("0111111001001001");
                f.WriteLine("0111111001111101");
                f.WriteLine("0111111010101010");
                f.WriteLine("0111111011010010");
                f.WriteLine("0111111011110101");
                f.WriteLine("0111111100010100");
                f.WriteLine("0111111100110000");
                f.WriteLine("0111111101001000");
                f.WriteLine("0111111101011110");
                f.WriteLine("0111111101110001");
                f.WriteLine("0111111110000010");
                f.WriteLine("0111111110010001");
                f.WriteLine("0111111110011110");
                f.WriteLine("0111111110101001");
                f.WriteLine("0111111110110011");
                f.WriteLine("0111111110111100");
                f.WriteLine("0111111111000100");
                f.WriteLine("0111111111001011");
                f.WriteLine("0111111111010001");
                f.WriteLine("0111111111010111");
                f.WriteLine("0111111111011100");
                f.WriteLine("0111111111100000");
                f.WriteLine("0111111111100100");
                f.WriteLine("0111111111100111");



            }
            finally
            {
                f.Flush();
                f.Close();
                //f0.Close();
            }
            //System.Diagnostics.Process.Start(@address);

        }
        

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

           // textBox14 .Text=(BinaryStringToSingle(textBox14.Text )).ToString() ;
        }
        public static float BinaryStringToSingle(string s)
        {
            int i = Convert.ToInt32(s, 2);
            byte[] b = BitConverter.GetBytes(i);
            return BitConverter.ToSingle(b, 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string add00 = label5.Text +"\\Top.syr";
            StreamReader f0 = new StreamReader(@add00);
            //string StrText;
            string StrText = f0.ReadToEnd().ToString();
           
            int i = StrText.IndexOf("Maximum Frequency: ");
            //label28.Text = i.ToString();
            
            label28.Text=StrText.Substring(i+19,7);
           // int j = int.Parse(label28.Text);
            if (int.Parse(StrText.Substring(i + 19, 3)) > 200)
                label28.ForeColor = Color.Red ;


          
        
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private void button9_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("xtclsh C:\\Users\\Pezhman\\Desktop\\Newfolder\\new1.tcl","cmd");
            //Create_Test_Bench_Top();
            string add = label5.Text + "\\B1.bat";
            System.Diagnostics.Process.Start(@add);
  
            
        }

        public static string Convert2Binary (string Input,int m,int n)
    {
//این تابع یک رشته محتوی یک عدد اعشاری و همچنین تعداد بیت های قسمت صحیح و قسمت اعشار را دریافت می کند و معادل باینری عدد را برمیگرداند.
// عدد باینری خروجی دقیقا بر اساس تعداد بیت های مشخص شده است.

            string Input_Text_A = Input.Substring(0, Input.IndexOf("."));//قسمت صحیح
            string Input_Text_B = Input.Substring((Input.IndexOf(".") + 1), ((Input.Length) - Input.IndexOf(".") - 1));//قسمت اعشار عدد


            string Out_Text_A = "";//نتیجه قسمت صحیح در این متغیر ذخیره می شود


            if (Input_Text_A == "")
                Input_Text_A = "0";
            int Input_A = int.Parse(Input_Text_A);
            int B;
            int K;
            while ((Input_A / 2) != 0)
            {
                K = Input_A / 2;
                B = Input_A % 2;
                Input_A = K;

                Out_Text_A = (B).ToString() + Out_Text_A;
            }

            Out_Text_A = (Input_A).ToString() + Out_Text_A;
 
///////////////////////////////////////////////////////////////
            
            string Out_Text_B = "";//نتیجه قسمت اعشار در این متغیر ذخیره می شود

            Input_Text_B = "0." + Input_Text_B;
            double Input_B = double.Parse(Input_Text_B);
           
            int L = (Input_B.ToString()).IndexOf(".");

            double Temp = Input_B;
            string Int_Part;
            string Float_Part;

            for (int i = 1; i <= n; i++)
            {
                Temp = Temp * 2;
                if (Temp == 0)
                {
                    Temp = 0.0;
                    Out_Text_B = Out_Text_B + "0";
                    continue;
                }
                Int_Part = (Temp.ToString()).Substring(L - 1, 1);//قسمت صحیح عدد
                if (Temp - int.Parse(Int_Part) == 0)
                {
                    Temp = 0.0;
                    Out_Text_B = Out_Text_B + Int_Part;
                    continue;
                }
                Float_Part = (Temp.ToString()).Substring(L + 1, ((Temp.ToString().Length) - 2));//قسمت اعشار عدد
                Out_Text_B = Out_Text_B + Int_Part;
                Temp = double.Parse ("0." + Float_Part);

            }
            L=Out_Text_A.Length;
            if (L < m)
                for (int i = 1; i <= m -L ; i++)
                    Out_Text_A = "0" + Out_Text_A;

            if (Out_Text_A.Length + Out_Text_B.Length <= m + n)
                return (Out_Text_A + "." + Out_Text_B);
            else
                return ("err: length is large");

    }
        public static void ConvertFile2Binary(string Path,string File_Name_In,string File_Name_Out,int m, int n)
        {
            //m تعداد بیت های خروجی بخش صحیح عدد
            //n تعداد بیت های خروجی بخش اعشار عدد 
            string addressIn = Path + File_Name_In;
            StreamReader f = new StreamReader(@addressIn);

            string addressOut = Path + File_Name_Out;
            TextWriter f0 = new StreamWriter(@addressOut);

            string StrText;
            StrText = f.ReadLine().ToString();


            
            try
            {
                while (StrText != "")
                {
                    f0.WriteLine(Convert2Binary(StrText, m, n));
                    StrText = f.ReadLine().ToString();
                }

                f0.WriteLine("");
                f0.WriteLine("");
            }
            finally
            {
                f0.Flush();
                f0.Close();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@addressOut);
        



        }
        public static string Convert2Binary_NotPoint(string Input, int m, int n)
        {
            //این تابع یک رشته محتوی یک عدد اعشاری و همچنین تعداد بیت های قسمت صحیح و قسمت اعشار را دریافت می کند و معادل باینری عدد را برمیگرداند.
            // عدد باینری خروجی دقیقا بر اساس تعداد بیت های مشخص شده است.

            string Input_Text_A = Input.Substring(0, Input.IndexOf("."));//قسمت صحیح
            string Input_Text_B = Input.Substring((Input.IndexOf(".") + 1), ((Input.Length) - Input.IndexOf(".") - 1));//قسمت اعشار عدد


            string Out_Text_A = "";//نتیجه قسمت صحیح در این متغیر ذخیره می شود


            if (Input_Text_A == "")
                Input_Text_A = "0";
            int Input_A = int.Parse(Input_Text_A);
            int B;
            int K;
            while ((Input_A / 2) != 0)
            {
                K = Input_A / 2;
                B = Input_A % 2;
                Input_A = K;

                Out_Text_A = (B).ToString() + Out_Text_A;
            }

            Out_Text_A = (Input_A).ToString() + Out_Text_A;

            ///////////////////////////////////////////////////////////////

            string Out_Text_B = "";//نتیجه قسمت اعشار در این متغیر ذخیره می شود

            Input_Text_B = "0." + Input_Text_B;
            double Input_B = double.Parse(Input_Text_B);

            int L = (Input_B.ToString()).IndexOf(".");

            double Temp = Input_B;
            string Int_Part;
            string Float_Part;

            for (int i = 1; i <= n; i++)
            {
                Temp = Temp * 2;
                if (Temp == 0)
                {
                    Temp = 0.0;
                    Out_Text_B = Out_Text_B + "0";
                    continue;
                }
                Int_Part = (Temp.ToString()).Substring(L - 1, 1);//قسمت صحیح عدد
                if (Temp - int.Parse(Int_Part) == 0)
                {
                    Temp = 0.0;
                    Out_Text_B = Out_Text_B + Int_Part;
                    continue;
                }
                Float_Part = (Temp.ToString()).Substring(L + 1, ((Temp.ToString().Length) - 2));//قسمت اعشار عدد
                Out_Text_B = Out_Text_B + Int_Part;
                Temp = double.Parse("0." + Float_Part);

            }
            L = Out_Text_A.Length;
            if (L < m)
                for (int i = 1; i <= m - L; i++)
                    Out_Text_A = "0" + Out_Text_A;

            if (Out_Text_A.Length + Out_Text_B.Length <= m + n)
                return (Out_Text_A + Out_Text_B);
            else
                return ("err: length is large");

        }
        public static void ConvertFile2Binary_NotPoint(string Path_WIn_NameFile, string Path_WOut, string File_Name_Out, int m, int n)
        {
            //m تعداد بیت های خروجی بخش صحیح عدد
            //n تعداد بیت های خروجی بخش اعشار عدد 
            string addressIn = Path_WIn_NameFile;// +File_Name_In;
            StreamReader f = new StreamReader(@addressIn);

            string addressOut = Path_WOut + File_Name_Out;
            TextWriter f0 = new StreamWriter(@addressOut);

            string StrText;
            StrText = f.ReadLine().ToString();



            try
            {
                while (StrText != "")
                {
                    f0.WriteLine(Convert2Binary_NotPoint(StrText, m, n));
                    StrText = f.ReadLine().ToString();
                }

                f0.WriteLine("");
                f0.WriteLine("");
            }
            finally
            {
                f0.Flush();
                f0.Close();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@addressOut);




        }
    
        private void button10_Click(object sender, EventArgs e)
        {

            label29.Text = Convert2Binary((textBox14.Text),5,11).ToString();

        }
        public static double Convert_Bin2Dec(string StrInput)
        {

            //string StrInput = textBox14.Text;
            string Input_Text_A = StrInput.Substring(0, StrInput.IndexOf("."));//قسمت صحیح
            string Input_Text_B = StrInput.Substring((StrInput.IndexOf(".") + 1), ((StrInput.Length) - StrInput.IndexOf(".") - 1));//قسمت اعشار عدد

            int Temp = 0;
            int Pow = Input_Text_A.Length;

            for (int i = 0; i < Input_Text_A.Length; i++)
            {
                Pow--;
                Temp = Temp + (int.Parse(Input_Text_A.Substring(i, 1))) * ((int)Math.Pow(2, Pow));

            }
            double Temp1 = 0;
            Pow = 0;
            for (int i = 0; i < Input_Text_B.Length; i++)
            {
                Pow--;
                Temp1 = Temp1 + (int.Parse(Input_Text_B.Substring(i, 1))) * (1 / ((double)Math.Pow(2, i + 1)));

            }
                return (Temp +Temp1);


        }
        public static void Cmp_File_Binary(string Path, string FileName1, string FileName2)
        {
            string add1 = Path + FileName1;
            StreamReader f1 = new StreamReader(@add1);
            string StrText1;

            string add2 = Path + FileName2;
            StreamReader f2 = new StreamReader(@add2);
            string StrText2;

            string addressOut = Path + "CMP.txt";
            TextWriter f0 = new StreamWriter(@addressOut);


            StrText1 = f1.ReadLine().ToString();
            StrText2 = f2.ReadLine().ToString();

            try
            {
                while (StrText1 != "")
                {
                    f0.WriteLine((Convert_Bin2Dec(StrText1) - Convert_Bin2Dec(StrText2)).ToString());
                    StrText1 = f1.ReadLine().ToString();
                    StrText2 = f2.ReadLine().ToString();
                }

            }
            finally
            {
                f0.Flush();
                f0.Close();
                f1.Close();
                f2.Close();
            }
            //System.Diagnostics.Process.Start(@addressOut);

        }
        public static void Cmp_File_Org_With_Binary(string Path, string FileNameOrg, string FileName1, string FileName2, string FileNameOut)
        {
            string addOrg = Path + FileNameOrg;
            StreamReader fOrg = new StreamReader(@addOrg);
            string StrTextOrg;

            string add1 = Path + FileName1;
            StreamReader f1 = new StreamReader(@add1);
            string StrText1;

            string add2 = Path + FileName2;
            StreamReader f2 = new StreamReader(@add2);
            string StrText2;

            string addressOut = Path + FileNameOut;
            TextWriter fOut = new StreamWriter(@addressOut);


            StrTextOrg = fOrg.ReadLine().ToString();
            StrText1 = f1.ReadLine().ToString();
            StrText2 = f2.ReadLine().ToString();

            //int T1;

            try
            {
                //fOut.WriteLine("Ogr-W1    /   Org-W2        /W1-W2");
                //fOut.WriteLine("Ogr        /   W1        /           W2");
                while (StrTextOrg != "")
                {
                    //T1 = int.Parse(StrTextOrg);
                    fOut.WriteLine(StrTextOrg + "      " + Convert_Bin2Dec(StrText1).ToString() + "      " + Convert_Bin2Dec(StrText2).ToString() );
                    //fOut.WriteLine(((double.Parse(StrTextOrg) - Convert_Bin2Dec(StrText1)).ToString()) + "      " + ((double.Parse(StrTextOrg) - Convert_Bin2Dec(StrText2)).ToString()) + "     " + (((Convert_Bin2Dec(StrText1) - Convert_Bin2Dec(StrText2))).ToString()));
                    StrTextOrg = fOrg.ReadLine().ToString();
                    StrText1 = f1.ReadLine().ToString();
                    StrText2 = f2.ReadLine().ToString();
                }

            }
            finally
            {
                fOut.Flush();
                fOut.Close();
                f1.Close();
                f2.Close();
                fOrg.Close();
            }
            //System.Diagnostics.Process.Start(@addressOut);

        }
        //  button11_Click  Convert & view Chart.....
        private void button11_Click(object sender, EventArgs e)
        {
            string path = label34.Text;
            string f_name = label7.Text;
            if (label9.Text.Length>12)
                if (label9.Text.Substring(label9.Text.Length - 12, 12) == "Export_W.txt")
                {
                    path = label9.Text.Substring(0, label9.Text.Length - 12);
                    f_name = "Export_W.txt";
                }

            ConvertFile2Binary(path, f_name, "WO1.txt", int.Parse(textBox11.Text), int.Parse(textBox10.Text));
            ConvertFile2Binary(path, f_name, "WO2.txt", int.Parse(textBox17.Text), int.Parse(textBox16.Text));
            //Cmp_File_Binary("D:\\", "WO1.txt", "WO2.txt");
            Cmp_File_Org_With_Binary(path, f_name, "WO1.txt", "WO2.txt", "CMP4.txt");

            if (System.IO.File.Exists(Application.StartupPath + "\\CMP4.txt"))
                File.Delete(Application.StartupPath + "\\CMP4.txt");
            System.IO.File.Copy((path + "CMP4.txt"), (Application.StartupPath + "\\CMP4.txt"));
            
            System.Diagnostics.Process.Start(Application.StartupPath+"\\ReadFile.exe");
            
        }
        //  button13_Click   Input Arch. from file.....
        private void button13_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Text Files|*.txt";
            openFileDialog2.FileName = String.Empty;
            DialogResult result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                label33.Text = openFileDialog2.FileName;

                string add1 = openFileDialog2.FileName;
                StreamReader f1 = new StreamReader(@add1);
                string StrText1="               ";
                int c = 0;
                //(i <= 10) || 
                //for (int i = 0; (StrText1 != "Weight:") || (f1.EndOfStream); i++)
                    for (int i = 0;(!f1.EndOfStream); i++)
                {
                    StrText1 = (f1.ReadLine().ToString());
                    //nt i = StrText.IndexOf("Maximum Frequency: ");
                    //label28.Text = i.ToString();
                  
                    if (StrText1.Length < 7)
                        continue;

                    if (StrText1.Substring(0, 7) == "Weight:")
                        break;
                    

                    if (StrText1.Substring(0,2) =="//")
                        continue;
                    if (StrText1.Length < 12)
                        continue;
                    if (StrText1.Substring(0, 12) == "Path output:")
                    {
                        label5.Text = StrText1.Substring(12, (StrText1.Length - 12));
                        c++;
                        continue;
                    }
                    if (StrText1.Length < 13)
                        continue;
                    if (StrText1.Substring(0, 13) == "Hidden layer:")
                    {
                        textBox1.Text = StrText1.Substring(13, (StrText1.Length - 13));
                        c++;
                        continue;
                    }
                    if (StrText1.Substring(0, 13) == "Depth Sample:")
                    {
                        textBox13.Text = StrText1.Substring(13, (StrText1.Length - 13));
                        c++;
                        continue;
                    }
                    if (StrText1.Length < 15)
                        continue;
                    if (StrText1.Substring(0, 15) == "Number of node:")
                    {
                        textBox12.Text = StrText1.Substring(15, (StrText1.Length - 15));
                        c++;
                        continue;
                    }
                    if (StrText1.Length < 16)
                        continue;
                    if (StrText1.Substring(0, 16) == "Number of input:")
                    {
                        textBox2.Text = StrText1.Substring(16, (StrText1.Length - 16));
                        c++;
                        continue;
                    }
                   
                    if (StrText1.Substring(0, 16) == "Data Clock(MHz):")
                    {
                        textBox6.Text = StrText1.Substring(16, (StrText1.Length - 16));
                        c++;
                        continue;
                    }
                    if (StrText1.Length < 17)
                        continue;
                    if (StrText1.Substring(0, 17) == "Number of output:")
                    {
                        textBox5.Text = StrText1.Substring(17, (StrText1.Length - 17));
                        c++;
                        continue;
                    }

                    if (StrText1.Length < 20)
                        continue;
                    if (StrText1.Substring(0, 20) == "Activation function:")
                    {
                        comboBox1.Text = StrText1.Substring(20, (StrText1.Length - 20));
                        c++;
                        continue;
                    }
                    if (StrText1.Length < 21)
                        continue;
                    if (StrText1.Substring(0, 21) == "Length of bits input:")
                    {
                        textBox3.Text = StrText1.Substring(21, StrText1.IndexOf(".")-21 );
                        textBox9.Text = StrText1.Substring(StrText1.IndexOf(".")+1, (StrText1.Length - StrText1.IndexOf(".")-1));
                        c++;
                        continue;
                    }
                    if (StrText1.Length < 22)
                        continue;
                    if (StrText1.Substring(0, 22) == "Length of bits Weight:")
                    {
                        textBox11.Text = StrText1.Substring(22, StrText1.IndexOf(".") - 22);
                        textBox10.Text = StrText1.Substring(StrText1.IndexOf(".") + 1, (StrText1.Length - StrText1.IndexOf(".")-1));
                        c++;
                        continue;
                    }
                    
                   

                    
                }

                


                // W Export in new File

                string addressW = "D:\\Export_W.txt";
                TextWriter f = new StreamWriter(@addressW);

                int c1 = 0;
                try
                {
                    
                    if (!f1.EndOfStream)
                        StrText1 = f1.ReadLine().ToString();

                    while ((StrText1 != "End")||(!f1.EndOfStream))
                    {
                        double  number;
                        if(double.TryParse(StrText1,out number))
                        {
                            f.WriteLine(StrText1);
                            c1++;
                        }
                        //StrText1 = f1.ReadLine().ToString();
                        if (!f1.EndOfStream)
                            StrText1 = f1.ReadLine().ToString();
                        else
                            break;
                        
                        
                    }
                    f.WriteLine("");
                    
                }
                finally
                {
                    f.Flush();
                    f.Close();
                    f1.Close();
                }
                
                //if (c < 10)

                if ((c == 10) && (c1 != 0))
                {
                    MessageBox.Show("Complete Import File", "successful...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    label9.Text = addressW;
                    //System.Diagnostics.Process.Start(@addressW);



                    DialogResult respons;
                    if (label9.Text == "Select file")
                        respons = MessageBox.Show("Select a file for weight...", "Err", System.Windows.Forms.MessageBoxButtons.OK);
                    else if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox8.Text == "")
                        respons = MessageBox.Show("TextBox is empty... Enter a number in: \n\n   Hidden Layer\n   Number of inputs\n   length of bits\n   Number of outputs\n   clk speed\n   Parameter pipeline", "Err", System.Windows.Forms.MessageBoxButtons.OK);
                  //  else
                    //    button1_Click(sender, e);





                }
                else
                    MessageBox.Show("Information is not enough...   Err code is " + c.ToString() + " " + c1.ToString(), "Err", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            DialogResult respons;
            if (label9.Text == "Select file")
                respons = MessageBox.Show("Select a file for weight...", "Err", System.Windows.Forms.MessageBoxButtons.OK);
            else if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox8.Text == "")
                respons = MessageBox.Show("TextBox is empty... Enter a number in: \n\n   Hidden Layer\n   Number of inputs\n   length of bits\n   Number of outputs\n   clk speed\n   Parameter pipeline", "Err", System.Windows.Forms.MessageBoxButtons.OK);
            else
            {
                button1_Click(sender, e);
                string add1 = label5.Text + "\\B1.bat";
                System.Diagnostics.Process.Start(@add1);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
          


           string addressIn = "C:\\Users\\Pezhman\\Desktop\\02\\Input_Vector.txt";
            StreamReader f = new StreamReader(@addressIn);

            string addressOut = "C:\\Users\\Pezhman\\Desktop\\02\\Output_Vector_ANN.txt";
            TextWriter f0 = new StreamWriter(@addressOut);

            string StrText;
            StrText = f.ReadLine().ToString();

            string s1, s2,s3,s4,s5,s6,s7,s8,s9,s10,s11,s12,s13,s14,s15,s16,s17,s18,s19,s20;

            try
            {
                while (StrText != "")
                {
                    s1 = Convert_Bin2Dec(Convert2Binary(StrText, 1, 15).ToString()).ToString();
                    s2 = Convert_Bin2Dec(Convert2Binary(s1, 1, 15).ToString()).ToString();
                    s3 = Convert_Bin2Dec(Convert2Binary(s2, 1, 15).ToString()).ToString();
                    s4 = Convert_Bin2Dec(Convert2Binary(s3, 1, 15).ToString()).ToString();
                    s5 = Convert_Bin2Dec(Convert2Binary(s4, 1, 15).ToString()).ToString();
                    s6 = Convert_Bin2Dec(Convert2Binary(s5, 1, 15).ToString()).ToString();
                    s7 = Convert_Bin2Dec(Convert2Binary(s6, 1, 15).ToString()).ToString();
                    s8 = Convert_Bin2Dec(Convert2Binary(s7, 1, 15).ToString()).ToString();
                    s9 = Convert_Bin2Dec(Convert2Binary(s8, 1, 15).ToString()).ToString();
                    s10 = Convert_Bin2Dec(Convert2Binary(s9, 1, 15).ToString()).ToString();
                    s11 = Convert_Bin2Dec(Convert2Binary(s10, 1, 15).ToString()).ToString();
                    s12 = Convert_Bin2Dec(Convert2Binary(s11, 1, 15).ToString()).ToString();
                    s13 = Convert_Bin2Dec(Convert2Binary(s12, 1, 15).ToString()).ToString();
                    s14 = Convert_Bin2Dec(Convert2Binary(s13, 1, 15).ToString()).ToString();
                    s15 = Convert_Bin2Dec(Convert2Binary(s14, 1, 15).ToString()).ToString();
                    s16 = Convert_Bin2Dec(Convert2Binary(s15, 1, 15).ToString()).ToString();
                    s17 = Convert_Bin2Dec(Convert2Binary(s16, 1, 15).ToString()).ToString();
                    s18 = Convert_Bin2Dec(Convert2Binary(s17, 1, 15).ToString()).ToString();
                    s19 = Convert_Bin2Dec(Convert2Binary(s18, 1, 15).ToString()).ToString();
                    s20 = Convert_Bin2Dec(Convert2Binary(s19, 1, 15).ToString()).ToString();
                    
                    f0.WriteLine(StrText+ "*" + s10);
                    StrText = f.ReadLine().ToString();
                }

                f0.WriteLine("");
                f0.WriteLine("");
            }
            finally
            {
                f0.Flush();
                f0.Close();
                f.Close();
            }
            //System.Diagnostics.Process.Start(@addressOut);
        





        }
    }
    }


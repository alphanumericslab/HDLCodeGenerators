#include "../Header/Header.h"

void Graph::print(char prefix, int n, int l, string file_name, string path, vector<string>& vfiles) {


	ofstream vfile;
	vfile.open(path + file_name + ".v", fstream::out);
	if (vfile.is_open())
	{

		vfile << "//c = " << vertices;
		vfile << "\nmodule " << file_name;
		if (prefix == 'C' && !latch_in && !latch_out)
		{
			vfile << "(";
			if (layer_has_clk)
				vfile << "clk, ";
			if (layer_has_rst)
				vfile << "rst, ";
			if (layer_has_ce)
				vfile << "ce, ";
		}
		else
		{
			//vfile << "(clk, rst, ce";
			vfile << "(clk, rst";
			if (layer_has_ce)
				vfile << ", ce";
			if (num_pi > 0 || num_po > 0)
				vfile << ", ";
		}
		if (num_pi > 0)
		{
			vfile << "ins";
			if (num_po > 0)
				vfile << ", ";
		}
		if (num_po > 0)
			vfile << "outs";
		vfile << ");\n";
		if (prefix == 'C' && !latch_in && !latch_out)
		{
			if (layer_has_clk)
				vfile << "input clk;\n";
			if (layer_has_rst)
				vfile << "input rst;\n";
			if (layer_has_ce)
				vfile << "input ce;\n";
		}
		else //(prefix !='C' || (prefix == 'C' && (latch_in || latch_out)))
		{
			vfile << "input clk;\n";
			vfile << "input rst;\n";
			if (layer_has_ce)
				vfile << "input ce;\n";
		}
		//vfile << "input clk, rst;\n";
		if (num_pi > 1)
			vfile << "input [" << num_pi - 1 << " : " << 0 << "] ins;\n";
		else if (num_pi == 1)
			vfile << "input ins;\n";

		if (num_po > 1)
		{
			vfile << "output [" << num_po - 1 << " : " << 0 << "] outs;\n";
			/*
			if (latch_out)
				vfile << "reg [" << po - 1 << " : " << 0 << "] outs;\n";
				*/
		}

		else if (num_po == 1)
		{
			vfile << "output outs;\n";
			/*
			if (latch_out)
				vfile << "reg outs;\n";
				*/
		}

		//	vfile << "\nparameter GND = 1'b0;\nparameter VCC = 1'b1;\n\n";
		vfile.close();

		latching_start(file_name, path, vfiles);

		vfile.open(path + file_name + ".v", fstream::app);
		for (int i = 0; i < vertices; i++)
		{
			ifstream instant;
			instant.open(insts_path + vfiles[elements[i].get_ID()] + "_" + "instant.txt");

			if (instant.is_open())
			{

				string line;
				getline(instant, line);
				//check if this module is primitive
				if (elements[i].get_prim())
				{
					print_primitive(vfile, i, line);
				}
				else
				{

					vfile << "\n" << line;
					if (prefix == 'C')
						vfile << " C_" << n << "_" << l << "_" << i << " (";
					else if (prefix == 'L')
						vfile << " L_" << l << "_" << i << " (";
					else if (prefix == 'N')
						vfile << " D_" << i << " ("; //disjoint area

					print_other(instant, vfile, i, latch_in);
				}

				instant.close();
			}
			else
			{
				cerr << "Cannot open file " << vfiles[elements[i].get_ID()] + "_instant.txt in \"print\" function.\n";
			}
		}

		latching_finish(file_name, path, vfiles);

		vfile << "\nendmodule";
		vfile.close();
		//return true;
	}
	else
	{
		cerr << "Cannot open Verilog file " << file_name << " specified for write in \"print\" function.\n";
		//return false;
	}


}

void Graph::print_primitive(ofstream &vfile, int i, string name) {
	
	int num_ins = elements[i].get_num_ins();
	int num_outs = elements[i].get_num_outs();
	
	if (num_ins > 0)
	{
		if (name.compare("nand") == 0 ||name.compare("nor") == 0 || name.compare("xnor") == 0 )
			vfile << "assign w_out_" << i << "_0 = ~(";
		else
			vfile << "assign w_out_" << i << "_0 = ";
		//now print input ports
		if (name.compare("not") == 0)
			vfile << "~ w_in_" << i << "_" << "0;\n";
		else
		{
			int newline = 1;
			for (int j = 0; j < num_ins; j++)
			{
				
				newline++;
				if (newline % 7 == 0)
					vfile << endl;

				if (latch_in)
					vfile << "r_in_" << i << "_" << j;
				else
					vfile << "w_in_" << i << "_" << j;

				if (j != elements[i].get_num_ins() - 1)//to avoid extra , at end
				{
					if (name.compare("and") == 0 || name.compare("nand") == 0)
						vfile << " & ";
					else if (name.compare("or") == 0 || name.compare("nor") == 0)
						vfile << " | ";
					else if (name.compare("xor") == 0 || name.compare("xnor") == 0)
						vfile << " ^ ";
				}
			}
			if (name.compare("nand") == 0 ||name.compare("nor") == 0 || name.compare("xnor") == 0 )
				vfile << ")";
			vfile << ";\n" << endl;
		}
	}
}

void Graph::print_other(ifstream &instant, ofstream &vfile, int i, bool latch_in) {

	string line;
	
	getline(instant, line);
	int num_ins = atoi(line.c_str());
	int nprim = -1;
	int newline = 0;
	
	string pn;
	for (int n = 0; n < num_ins; n++)
	{

		newline++;
		if (newline % 7 == 0)
			vfile << "\n\t\t";

		getline(instant, line, '\t');
		
		getline(instant, line);
		pn = line;
		vfile << "." << pn << "(";
		if (pn == "clk")
			vfile << "clk";
		else if (pn == "rst")
			vfile << "rst";
		else if (pn == "ce")
			vfile << "ce";
		else
		{
			nprim++;//rst and clk and ce are not part of inputs adj matrix
			if (latch_in)
			{
				vfile << "r_in_" << i << "_" << nprim;
			}
			else
			{
				vfile << "w_in_" << i << "_" << nprim;
			}
		}

		if (n<num_ins - 1)// to avoid extra ',' at end
			vfile << "), ";
		else
			vfile << ")";
	}



	/* PRINT OUTPUTS */
	newline = 1;
	getline(instant, line);
	int num_outs = atoi(line.c_str());
	if (num_ins >0 && num_outs>0)// to avoid extra ',' at end
		vfile << ",\n\t\t";
	int k = 0;
	for (int n = 0; n < num_outs; n++)
	{
		getline(instant, line, '\t');

		getline(instant, line);
		pn = line;
		newline++;
		if (newline % 7 == 0)
			vfile << "\n\t\t";
		vfile << "." << pn << "(";

		{
			vfile << "w_out_" << i << "_" << n;
		}

		if (n<num_outs - 1) // to avoid extra ',' at end
			vfile << "), ";
		else
			vfile << ")";
	}


	vfile << ");" << endl;
}
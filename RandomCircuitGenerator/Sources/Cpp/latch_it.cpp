#include "../Header/Header.h"

vector<vector<couple>> outputs_classified;
void Graph::latching_start(string file_name, string path, vector<string>& vfiles) {
	
	
	outputs_classified.clear();// because the variable is global, it needs to be cleared before re-use
	outputs_classified.resize(vertices);

	ofstream vfile;
	vfile.open(path + file_name + ".v", fstream::app);

	for (int i = 0; i<vertices; i++)
	{
		//vfile << "\n// Declaring ports of module " << i << "\n";
		ifstream instant;
		vfile << endl;
		instant.open(insts_path + vfiles[elements[i].get_ID()] + "_" + "instant.txt");
		string line;
		getline(instant, line); //name of file, don't need it
		
		getline(instant, line); //number of inputs
		int ins = atoi(line.c_str());
		int jprim = 0;
		for (int j = 0; j<ins; j++)
		{
			getline(instant, line, '\t');//size of this input
			int size = atoi(line.c_str());
			getline(instant, line);
			if (line != "rst" && line != "clk" && line != "ce")
			{
				if (latch_in)
				{
					vfile << "reg ";
				}
				else
				{
					vfile << "wire ";
				}

				if (size>1)
				{
					vfile << "[" << size - 1 << " : 0] ";
				}
				if (latch_in)
				{
					vfile << "r_in_" << i << "_" << jprim << ";\n";
				}
				else
				{
					vfile << "w_in_" << i << "_" << jprim << ";\n";
				}
				///// LATCH_IN CHANGES 
				/*
				if (latch_in && g.get_element(i).get_prim()) //is primtive and needs to bw wire as well
				{
					vfile << "wire ";
					if (size > 1)
						vfile << "[" << size - 1 << " : 0] ";
					vfile << "w_in_" << i << "_" << jprim << ";\n";
				}
				*/
				jprim++;//rst and clk and ce are not part of inputs adj matrix
			}

		}

		/******* PRINT OUTPUTS OF THIS MODULE *********/

		getline(instant, line); //number of outputs

		int outs = atoi(line.c_str());

		for (int j = 0; j<outs; j++)
		{
			getline(instant, line, '\t');//size of this output

			int size = atoi(line.c_str());
			for (int k = 0; k < size; k++)
			{
				couple cpl(j, k);

				outputs_classified[i].push_back(cpl);
			}

			getline(instant, line);

			if (size > 1)
				vfile << "wire [" << size - 1 << " : 0] w_out_" << i << "_" << j << ";\n";
			else if (size == 1)
				vfile << "wire w_out_" << i << "_" << j << ";\n";

			if (latch_out)//new a reg for it
			{
				if (size > 1)
					vfile << "reg [" << size - 1 << " : 0] r_out_" << i << "_" << j << ";\n";
				else if (size == 1)
					vfile << "reg r_out_" << i << "_" << j << ";\n";
			}



		}


		/*****************************/
		instant.close();
	}


	vfile << endl;
	vfile.close();

}

void Graph::latching_finish(string file_name, string path, vector<string>& vfiles) {

	ofstream vfile;
	vec_int prims;

	vfile.open(path + file_name + ".v", fstream::app);


	if (latch_in)
	{
		vfile << "\nalways @ (posedge clk)\nbegin\n\tif(~rst)\n\tbegin\n";
	}

	for (int i = 0; i < vertices; i++)
	{
		ifstream instant;
		instant.open(insts_path + vfiles[elements[i].get_ID()] + "_" + "instant.txt");
		string line;
		getline(instant, line); //name of file
		getline(instant, line); //number of inputs
		int ins = atoi(line.c_str());
		int jprim = 0;
		int sizeprim = 0;
		for (int j = 0; j < ins; j++)
		{
			getline(instant, line, '\t');//size of this input
			int size = atoi(line.c_str());
			getline(instant, line);
			if (line == "clk" || line == "rst" || line == "ce")
			{
				continue;
			}
			if (latch_in)
			{
				vfile << "\t\tr_in_" << i << "_" << jprim << " <= 0;\n";
				jprim++;
			}
		}
		instant.close();
	}
	if (latch_in)
	{
		//vfile << "\tend\n\telse\n\t\tif(ce)\n\t\tbegin\n";
		vfile << "\tend\n\telse\n\t\tbegin\n";
	}

	/*
	instant.clear();
	instant.seekg(0, ios::beg);
	*/
	/***********************/

	for (int i = 0; i < vertices; i++)
	{
		if (elements[i].get_prim())
		{
			prims.push_back(i);	//inputs of prims are wire
		}
		ifstream instant;
		instant.open(insts_path + vfiles[elements[i].get_ID()] + "_" + "instant.txt");
		string line;
		getline(instant, line); //name of file
		getline(instant, line); //number of inputs
		int ins = atoi(line.c_str());
		int jprim = 0;
		int sizeprim = 0;

		vfile << endl;
		for (int j = 0; j < ins; j++)
		{
			getline(instant, line, '\t');//size of this input
			int size = atoi(line.c_str());
			getline(instant, line);
			if (line == "clk" || line == "rst" || line == "ce")
			{
				continue;
			}

			if (latch_in)
			{
				vfile << "\t\t\tr_in_" << i << "_" << jprim << " <= ";
			}
			else
			{
				vfile << "assign w_in_" << i << "_" << jprim << " = ";
			}
			if (size > 1)
				vfile << "{";

			for (int k = 0; k < size; k++)
			{
				int to_mdl = ins_adj_mdl[i][sizeprim];// g.get_ins_adj_mdl(i, sizeprim);
				int to_port = ins_adj_port[i][sizeprim]; //g.get_ins_adj_port(i, sizeprim);


				if (to_mdl == toPI)
				{
					if (num_pi > 1)
						vfile << "ins[" << ins_adj_port[i][sizeprim] << "]";// g.get_ins_adj_port(i, sizeprim) << "]";
					else
						vfile << "ins";
				}
				else if (to_mdl == toVCC)
					vfile << "1'b1";
				else if (to_mdl == toGND)
					vfile << "1'b0";
				else
				{
					int this_mdl = outputs_classified[to_mdl][to_port].get_module();
					int prev_mdl = to_port > 0 ? outputs_classified[to_mdl][to_port - 1].get_module() : outOfBound;
					int next_mdl = to_port < outputs_classified[to_mdl].size() - 1 ? outputs_classified[to_mdl][to_port + 1].get_module() : outOfBound;
					if (latch_out)
					{
						if (prev_mdl == this_mdl || next_mdl == this_mdl)
							vfile << "r_out_" << to_mdl << "_" << this_mdl << "[" << outputs_classified[to_mdl][to_port].get_port() << "]";
						else
							vfile << "r_out_" << to_mdl << "_" << this_mdl;
					}
					else
					{
						if (prev_mdl == this_mdl || next_mdl == this_mdl)
							vfile << "w_out_" << to_mdl << "_" << this_mdl << "[" << outputs_classified[to_mdl][to_port].get_port() << "]";
						else
							vfile << "w_out_" << to_mdl << "_" << this_mdl;
					}

				}
				if (k < size - 1)
				{
					vfile << ",";
				}
				sizeprim++;
			}
			if (size > 1)
				vfile << "}";
			vfile << ";\n";
			jprim++;

		}

		instant.close();
	}

	if (latch_in)
	{
		vfile << "\t\tend\nend\n";
	}
	vfile << "\n";

	/********** LATCH OUTPUT PORTS OF EACH MODULE*********/
	///// LATCH_IN CHANGES 
	/*
	if (latch_in)
	{
		for (int i = 0; i< prims.size(); i++)
		{
			for (int j = 0; j< g.get_element(prims[i]).get_num_ins(); j++)
			{
				vfile << "assign w_in_" << prims[i] << "_" << j << " = " << "r_in_" << prims[i] << "_" << j << ";" << endl;
			}
		}
	}
	*/
	/********** CHECK IF THERE ARE OUTPUTS TO BE LATCHED *****************/
	int has_outs = 1;
	for (int m_o = 0; m_o < vertices; m_o++)
	{
		if (outputs_classified[m_o].size() > 0)
			has_outs++;
	}
	//though it is weird to have module with no outputs but I have considered it!
	if (has_outs>0 && latch_out)
	{
		vfile << "\nalways @ (posedge clk)\nbegin\n\tif (~rst)\n\tbegin\n\t\t\t";
		for (int m_o = 0; m_o < vertices; m_o++)
		{
			if (outputs_classified[m_o].size() > 0)//there is any output ports
				for (int p_o = 0; p_o <= outputs_classified[m_o][outputs_classified[m_o].size() - 1].get_module(); p_o++)
				{
					vfile << "r_out_" << m_o << "_" << p_o << " <= 0;\n\t\t\t";
				}
		}
		//vfile << "\n\tend\n\telse\n\t\tif(ce)\n\t\tbegin\n\t\t\t";
		vfile << "\n\tend\n\telse\n\t\tbegin\n\t\t\t";
		for (int m_o = 0; m_o < vertices; m_o++)
		{
			if (outputs_classified[m_o].size() > 0)//there is any output ports
				for (int p_o = 0; p_o <= outputs_classified[m_o][outputs_classified[m_o].size() - 1].get_module(); p_o++)
				{
					vfile << "r_out_" << m_o << "_" << p_o << " <= " <<
						"w_out_" << m_o << "_" << p_o << ";\n\t\t\t";
				}
		}
		vfile << "\n\t\tend\nend";
	}
	vfile << endl;
	if (num_po > 0)
	{
		/*
		if (latch_out)
		{
			vfile << "\nalways @ (posedge clk)\nbegin\n\t";
			//vfile << "if (~rst)\n\t\t outs <= 0;\n\telse\n\t\tif (ce)\n\t\touts <= {";
			vfile << "if (~rst)\n\t\t outs <= 0;\n\telse\n\t\touts <= {";
		}
		else*/
		{
			vfile << "\nassign outs = {";
		}

		int PO_port, PO_mdl;
		for (int m = 0; m < get_PO_size(); m++)
		{
			PO_mdl = get_PO_mdl(m);
			PO_port = get_PO_port(m);
			int this_mdl;
			int prev_mdl;
			int next_mdl;
			if (PO_mdl == unconnected)
			{
				//vfile << "****";
				if (m < get_PO_size() - 1)
					vfile << ",";
				continue;
			}
			else
			{
				this_mdl = outputs_classified[PO_mdl][PO_port].get_module();
				prev_mdl = PO_port > 0 ? outputs_classified[PO_mdl][PO_port - 1].get_module() : outOfBound;
				next_mdl = PO_port < outputs_classified[PO_mdl].size() - 1 ? outputs_classified[PO_mdl][PO_port + 1].get_module() : outOfBound;
			}
			if (latch_out)
			{
				if (prev_mdl == this_mdl || next_mdl == this_mdl)
					vfile << "r_out_" << PO_mdl << "_" << this_mdl << "[" << outputs_classified[PO_mdl][PO_port].get_port() << "]";
				else
					vfile << "r_out_" << PO_mdl << "_" << this_mdl;//<<"["<< outputs_classified[PO_mdl][PO_port].get_port()<<"]";
			}
			else
			{
				if (prev_mdl == this_mdl || next_mdl == this_mdl)
					vfile << "w_out_" << PO_mdl << "_" << this_mdl << "[" << outputs_classified[PO_mdl][PO_port].get_port() << "]";
				else
					vfile << "w_out_" << PO_mdl << "_" << this_mdl;
			}
			if (m < get_PO_size() - 1)
				vfile << ", ";
			if (m > 0 && m % 6 == 0)
			{
				vfile << "\n\t\t\t";

			}
		}

		vfile << "};\n";
		/*
		if (latch_out)
		{
			vfile << "\nend";
		}*/
	}
	/*******************/
	vfile.close();
}
#include "../Header/Header.h"

double make_top(int n, int l, int c, string library_path, string print_path, string log_path){

	
	ofstream logFile;
	logFile.open(log_path, ios::app);
	logFile << "\nTop Module\n";

	vector<string> top_files;
	vector<string> top_paths;
	int num_files = list_it(top_files, top_paths, library_path.c_str(), library_path.size()-1 );
	if( num_files > 0)
	{
		 vector<Node> tops_lib;
		 init_lib(top_files, top_paths, tops_lib);
		 int lib_size = top_files.size();

		/*
		lib_size is supposed to be n here. But if occasionally they are not equal
		-which I have no example of it!- minimum of these two is used in the for loop
		to avoid an out of range exception
		
		*/
		
		 int min = n;
		if (n > lib_size)
		{
			logFile<< "\t*WARNING: Number of Verilog files founded for building top module is "<<lib_size<<" instead of "<< n<< "!\n";
			min = lib_size;
		}
		

		int ppi = 0;
		int ppo = 0;
		sum_ins = 0;
		sum_outs = 0;
		sum_inouts = 0;

		for (int i = 0; i < min; i++)
		{
			int in = tops_lib[i].get_num_ins();
			ppi += in;
		
			int out = tops_lib[i].get_num_outs();
			ppo += out;
		
			sum_ins += in;
			sum_outs += out;
		}

		logFile << "\tFinal number of primary input ports: " << ppi << endl;
		logFile << "\tFinal number of primary output ports: " << ppo << endl;

		Graph top_g(n, ppi, ppo, false, false, false, false);
		
		for (int i = 0; i < min; i++)
		{
			//top_g.get_element(i) = tops_lib[i];
			top_g.set_element(i, tops_lib[i]);
			int in = tops_lib[i].get_num_ins();
			
			int inout = tops_lib[i].get_num_inouts();
			
			sum_inouts += inout;

			top_g.set_size(i, in, inout);
		}

		int po_index = 0;
		int pi_index = 0;
		rand_init(0, min - 1);
		max_net_degree = 1;
		for (int i = 0; i < min; i++)
		{
			int in = top_g.get_element(i).get_num_ins();
			int out = top_g.get_element(i).get_num_outs();
			for (int j = 0; j < in; j++)
			{

				if (top_g.add_PI(pi_index, i, j))
					pi_index++;
			}

			for (int j = 0; j < out; j++)
			{

				if (top_g.add_PO(po_index, i, j))
					po_index++;
				
			}

		}
		

		
		//logFile << "\n make top elapsedTime:  " << elapsedTime << endl;
		
		string name = "TOP_"+to_string(n)+"_"+to_string(l)
						+"_"+to_string(c);
		
		//name = "top";
		top_g.print('N', n, l, name, print_path, top_files);
		return 0;
	}
	else
	{
		logFile <<"\n\t*WARNING: No Verilog module founded.\n";
		logFile.close();
		return -1;
	}
}

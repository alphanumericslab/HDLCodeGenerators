#include "../Header/Header.h"

double Graph::make_layer(int n, int l, int c, bool latch_in, bool latch_out, string library_path, string print_path, string log_path, int m) {


	ofstream logFile;
	logFile.open(log_path, ios::app);
	logFile << "\n\nN = " << l << endl;

	vector<string> top_files;
	vector<string> top_paths;

	int num_files = list_it(top_files, top_paths, library_path.c_str(), library_path.size() - 1);
	if (num_files > 0)
	{
		vector<Node> tops_lib;
		init_lib(top_files, top_paths, tops_lib);
		int lib_size = top_files.size();

		sum_ins = 0;
		sum_outs = 0;
		sum_inouts = 0;

		/*
		lib_size is supposed to be c here. But if occasionally they are not equal
		-which I have no example of it!- minimum of these two is used in the for loop
		to avoid an out of randge exception

		*/

		int min = c;
		if (c > lib_size)
		{
			logFile << "\n\t*WARNING: Number of top files founded is " << lib_size << " instead of " << c << "!";
			min = lib_size;
		}
		logFile.close();

		Graph top_g(min, num_pi, num_po, latch_in, latch_out, comb_loop, seq_loop);

		for (int i = 0; i < min; i++)
		{
			//top_g.get_element(i) = tops_lib[i];
			top_g.set_element(i, tops_lib[i]);
			int in = tops_lib[i].get_num_ins();
			int inout = tops_lib[i].get_num_inouts();
			top_g.set_size(i, in, inout);
			int out = tops_lib[i].get_num_outs();
			sum_ins += in;
			sum_outs += out;
			sum_inouts += inout;
		}

		rand_init(0, min - 1);
		if (m == -1)
		{
			if (top_g.setIO(num_pi, num_po, log_path) == 0)
				top_g.connect_inputs(log_path);
		}
		else
		{
			if (top_g.setIO(num_pi, num_po, "") == 0)
				top_g.connect_inputs("");
		}

		int new_po = top_g.dangle(num_po, true, true, log_path);

		logFile.open(log_path, ios::app);
		top_g.print('L', n, l, "Disjoint_" + to_string(l), print_path, top_files);

		return 0;

	}
	else
	{
		logFile << "\n\t*WARNING: No Verilog module founded.\n";
		logFile.close();
		return -1;
	}
}

#include "../Header/Header.h"

#include<cstdlib>
#include<ctime>
#include <time.h>
#include <map>


/* Dont chnage these global values */
int max_net_degree = 1;
int sum_ins = 0;
int sum_of_ins = 0;
int sum_outs = 0;
int sum_inouts = 0;

bool layer_has_clk = false;
bool layer_has_rst = false;
bool layer_has_ce = false;

string output_path = "/../outputFiles/";
string insts_path = "../outputFiles/tmp/";
string Verilog_lib_path = "/../Sources/VerilogFiles";

int main(int argc, char* argv[]) {

	if (argc < 12)
	{
		cout << "Not enough arguments!\n";
		return -1;
	}
	else if (argc > 12)
	{
		cout << "Too many arguments!\n";
		return -1;
	}
	else
	{

		int n = atoi(argv[1]);
		int l = atoi(argv[2]);
		int c = atoi(argv[3]);
		int pi = atoi(argv[4]);
		int po = atoi(argv[5]);

		int num_mutants = atoi(argv[6]);
		int num_mutations = atoi(argv[7]);

		string latch_in = argv[8];
		string latch_out = argv[9];

		string comb_loop = argv[10];
		string seq_loop = argv[11];

		bool latch_in_C = false;
		bool latch_out_C = false;

		if (latch_in.compare("true") == 0)
		{
			latch_in_C = true;
		}
		else if (latch_in.compare("false") == 0)
		{
			latch_in_C = false;
		}

		if (latch_out.compare("true") == 0)
		{
			latch_out_C = true;
		}
		else if (latch_out.compare("false") == 0)
		{
			latch_out_C = false;
		}

		bool have_comb_loop = false;
		bool have_seq_loop = false;

		if (comb_loop.compare("true") == 0 || comb_loop.compare("True") == 0 || comb_loop.compare("TRUE") == 0 || comb_loop.compare("1") == 0)
		{
			have_comb_loop = true;
		}
		else if (comb_loop.compare("false") == 0 || comb_loop.compare("Flase") == 0 || comb_loop.compare("FALSE") == 0 || comb_loop.compare("0") == 0)
		{
			have_comb_loop = false;
		}

		if (seq_loop.compare("true") == 0 || seq_loop.compare("True") == 0 || seq_loop.compare("TRUE") == 0 || seq_loop.compare("1") == 0)
		{
			have_seq_loop = true;
		}
		else if (seq_loop.compare("false") == 0 || seq_loop.compare("Flase") == 0 || seq_loop.compare("FALSE") == 0 || seq_loop.compare("0") == 0)
		{
			have_seq_loop = false;
		}


		bool latch_in_L = false;
		bool latch_out_L = true;

		if (latch_out_C || latch_in_C)
		{
			//l = 1;//L has no meaning and will be overwritten -> DONE IN Batch File
			latch_out_L = false;
			latch_in_L = false;
		}
		else
		{
			latch_out_L = true;
			latch_in_L = false;
		}



	
		cout << "#Disjoint areas: " << n << endl;
		cout << "#Layers: " << l << endl;
		cout << "#Elements in a layer: " << c << endl << endl;
		return circ_gen(n, l, c, pi, po, num_mutants, num_mutations,
			latch_in_C, latch_out_C, have_comb_loop, have_seq_loop,
			latch_in_L, latch_out_L);
	}
}


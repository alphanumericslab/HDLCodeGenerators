#include "../Header/Header.h"
#include <list>

//selects c elements of library randomly
void Graph::select(vector<Node>& lib) {

	
	sum_ins = 0;
	sum_outs = 0;
	sum_inouts = 0;
	
	int lib_size = lib.size();

	for (int i = 0; i < vertices; i++)
	{
		int t = rand_mod(0, lib_size);
		int in = lib[t].get_num_ins();
		int inout = lib[t].get_num_inouts();

		elements[i] = lib[t];

		if (lib[t].get_clk())
		{
			layer_has_clk = true;
		}

		if (lib[t].get_rst())
		{
			layer_has_rst = true;
		}

		if (lib[t].get_ce())
		{
			layer_has_ce = true;
		}

		set_size(i, in, inout);

		sum_ins += in;
		sum_outs += lib[t].get_num_outs();
		sum_inouts += inout;
		
	}
	sum_of_ins = sum_ins;
	
}


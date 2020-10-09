#include "../Header/Header.h"
#include <list>

int Graph::connect_inputs(string log_path) {//complete connections of circuit


	int limit = 4 * vertices;
	int done = 0;
	
	std::list<couple> ls;
	list<couple>::iterator it;
	ofstream logFile;
	
	rand_init(0, vertices - 1);

	max_net_degree = 1;

	while (max_net_degree <= maximum_net_degree)
	{
		// give every port a chance to get connected
		
		for (int i=0; i<vertices; i++)
		{
			for (int j = 0; j < elements[i].get_num_ins(); j++)
			{
				couple cc(i, j);
				ls.push_back(cc);
			}
		}
		
		while (ls.size() > 0)
		{
			
			int r = rand_mod(0, ls.size());
			it = ls.begin();
			advance(it, r);
			int in_mdl = (*it).get_module();
			int in_port = (*it).get_port();
			

			if (ins_adj_mdl[in_mdl][in_port] == unconnected)// check if this input is free
			{
				int iter = 0;
				bool found = false;
				while (!found  && iter < limit)
				{
					iter++;
					
					int out_mdl = rand_c();
					if (out_mdl >= 0)
					{
						int out_port = rand_mod(0, elements[out_mdl].get_num_outs());
						if (out_port >= 0)
						{
							if (seq_loop & !comb_loop)
							{
								if (latch_in || latch_out)
								{
									found = check_degree(out_mdl, out_port, in_mdl, in_port);
								}
								else
								{
									found = check_DFS(out_mdl, out_port, in_mdl, in_port);
								}
							}
							else if (!seq_loop & !comb_loop)
							{
								found = check_DFS(out_mdl, out_port, in_mdl, in_port);
							}
							else if (seq_loop & comb_loop)
							{
								found = check_degree(out_mdl, out_port, in_mdl, in_port);
							}

							if(found)
							{
								done++;
							}
							
						}
					}

				}
			}
			
			ls.erase(it);
			
		}
		
		max_net_degree++;
		//cout << endl <<  done << " ports connected!\n";
		
	}

	logFile.open(log_path, ios::app);
	logFile <<"\t"<< done << " unconnected input port(s) connected to output of other modules.\n";
	logFile.close();

	return done;
}


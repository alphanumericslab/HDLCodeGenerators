#include "../Header/Header.h"

int Graph::dangle(int po, bool dangle_ins, bool dangle_outs, string log_path) {

	ofstream logFile;

	int new_po = po;
	int dngl_in = 0;

	if (dangle_ins) //keep them, connect to VCC/GND
	{
		for (int i = 0; i < vertices; i++)
		{
			for (int j = 0; j < elements[i].get_num_ins(); j++)
			{
				if (ins_adj_mdl[i][j] == unconnected)
				{
					dngl_in++;
					if (rand_mod(0, 2) == 1)
					{
						add_edge(toVCC, 0, i, j);
					}
					else
					{
						add_edge(toGND, 0, i, j);
					}

				}
			}
		}
	}

	logFile.open(log_path, ios::app);
	if (dngl_in > 0)
	{
		logFile << "\t" << dngl_in << " input port(s) connected to constant to avoid dangling inputs.\n";
	}

	/***** HNADLE DANGLING OUTPUTS *******/
	if (dangle_outs)
	{
		for (int i = 0; i < vertices; i++)
		{
			for (int j = 0; j < elements[i].get_num_outs(); j++)
			{
				if (elements[i].get_degree(j) == 0) //connected to nowhere
				{
					new_po++;
					extra_PO(new_po, i, j);

				}
			}
		}
	}


	if (new_po > po)
	{
		logFile << "\t" << new_po - po << " extra primary output(s) added to avoid dangling outputs.\n";
	}
	logFile.close();
	return new_po;
}

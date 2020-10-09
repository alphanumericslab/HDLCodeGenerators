#include "../Header/Header.h"

int Graph::mutate_add(int mut, string log_path) {


	ofstream logFile;
	logFile.open(log_path, ios::app);
	logFile << "\tNumber of ADD mutations: " << mut << "\n";

	rand_init(0, vertices - 1);

	int done = 0;
	int iter = 0;
	int T = mut * vertices;
	max_net_degree = maximum_net_degree;

	while (done < mut && iter < T )
	{
		int q = rand_c();
		int b = rand_mod(0, elements[q].get_num_ins());
		// search (p,a) -> (q,b) : p = out, q= in

		if ( b>= 0)
		{
			int current_out_mdl = ins_adj_mdl[q][b]; 
			int current_out_port = ins_adj_port[q][b]; 

			if (current_out_mdl == unconnected)
			{
				int p = rand_c();
				int a = rand_mod(0, elements[p].get_num_outs());
	
				if (a >= 0 )
				{
					bool ok = false;
					if(latch_in || latch_out)
					{
						ok = check_degree( p, a, q, b);
					}
					else
					{
						ok = check_DFS(p, a, q, b);
					}

					if(ok)
					{
						done++;
						logFile << "\t\tNew connection (" << p << "," << a << ") -> (" << q << ", " << b << ") added.\n";
					}
				}
			}
		}

		iter++;
	}

	
	logFile << "\t\tSuccessful Add mutations: " << done << "\n";
	logFile.close();
	return done;
}

int Graph::mutate_replace(int mut, string log_path) {

	ofstream logFile;
	logFile.open(log_path, ios::app);
	logFile << "\tNumber of REPLACE mutations: " << mut << "\n";

	rand_init(0, vertices - 1);

	int iter = 0;
	int T = mut * vertices;
	int done = 0;
	max_net_degree = maximum_net_degree;

	while (done < mut && iter < T )
	{

		int p = rand_c();
		int a = rand_mod(0, elements[p].get_num_outs());

		int q = rand_c();
		int b = rand_mod(0, elements[q].get_num_ins());
		//(p, a) -> (q, b)

		if (a>=0 && b>=0)
		{
			int current_out_mdl = ins_adj_mdl[q][b];
			int current_out_port = ins_adj_port[q][b];

			//connected to somewhere else
			if (current_out_mdl == unconnected || current_out_mdl == toPI
				|| current_out_mdl == toVCC || current_out_mdl == toGND)// actually no such thing
			{
				//nothing for now!
			}
			else
			{
				del_edge(current_out_mdl, current_out_port, q, b);
				bool ok = false;
				if(latch_in || latch_out)
				{
					ok = check_degree(p, a, q, b);
				}
				else
				{
					ok = check_DFS(p, a, q, b);
				}

				if(ok)
				{
					done++;
					logFile << "\t\tConnection pair (" << current_out_mdl << ", " << current_out_port << ") -> (" << q << ", " << b <<
						") replaced with new connection (" << p << ", " << a << ") -> (" << q << ", " << b << ").\n";
				}
				else
				{
					add_edge(current_out_mdl, current_out_port, q, b);//new connection makes loop, undo it
				}
				
			}

		}
		iter++;
	}
	
	
	logFile << "\t\tSuccessful Replace mutations: " << done << "\n";
	logFile.close();
	return done;
	
}


int Graph::mutate_swap(int mut, string log_path) {
	
	ofstream logFile;
	logFile.open(log_path, ios::app);
	logFile << "\tNumber of SWAP mutations: " << mut << "\n";

	rand_init(0, vertices - 1);

	int iter = 0;
	int T = mut * vertices;
	int done = 0;
	max_net_degree = maximum_net_degree;

	while (done < mut && iter < T )
	{

		int q = rand_c();
		int b = rand_mod(0, elements[q].get_num_ins());

		int z = rand_c();
		int d = rand_mod(0, elements[z].get_num_ins());

		if (b >= 0 && d >= 0)
		{
			//(p, a) -> (q, b)
			//(y, e) -> (z, d)

			int p = ins_adj_mdl[q][b];
			int a = ins_adj_port[q][b];

			int y = ins_adj_mdl[z][d];
			int e = ins_adj_port[z][d];


			if ((p == unconnected || p == toPI || p == toVCC || p == toGND)
				|| (y == unconnected || y == toPI || y == toVCC || y == toGND))
			{
				//nothing for now!
			}
			else
			{
				del_edge(p, a, q, b);
				del_edge(y, e, z, d);
				bool ok = false;
				if(latch_in || latch_out)
				{
					ok = check_degree(p, a, z, d) && check_degree(y, e, q, b);
				}
				else
				{
					ok = check_DFS(p, a, z, d) && check_DFS(y, e, q, b);
				}

				if(ok)
				{
					done++;
					logFile << "\t\tConnections (" << p << ", " << a << ") -> (" << q << ", " << b << ") and (" <<
						y << ", " << e << ") -> (" << z << ", " << d << ") swapped.\n";
				}
				else //new connection makes loop, undo it
				{
					add_edge(p, a, q, b);
					add_edge(y, e, z, d);
				}
			}
		}


		iter++;
	}
	
	
	logFile << "\t\tSuccessful Swap mutations: " << done << "\n";
	logFile.close();
	return done;
}
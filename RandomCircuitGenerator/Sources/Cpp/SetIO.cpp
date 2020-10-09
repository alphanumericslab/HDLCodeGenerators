#include "../Header/Header.h"

int Graph::setIO(int& pi, int& po, string log_path) {
	
	ofstream logFile;
	logFile.open(log_path, ios::app);
	logFile << "\tNumber of all input ports: "<< sum_ins <<endl;

	if (pi > sum_ins)
	{
		logFile << "\tSpecified PI ("<<pi<<") is more than total input ports (" << sum_ins << ")!\n";
		//logFile << "\t" << pi - sum_ins << " primary input port(s) cannot be connected!\n";
		//logFile << "\t" << sum_ins << " input ports connected to PI.";
		logFile << "\tNumber of PI decreased to "<< sum_ins << endl;
		pi = sum_ins;
	}
	else
	{
		logFile << "\t" << pi << " input port(s) connected to PI." << endl;
	}
	

	/* Connect primary inputs */

	vector<bool> succeeded(pi, false);
	int limit = 2 * vertices;
	max_net_degree = 1;

	for (int i = 0; i < pi; i++)
	{
		int T = 0;
			
		while (!succeeded[i] && T<limit)
		{
			int mdl = rand_c();
			if (mdl >= 0)
			{
				int port = rand_mod(0, elements[mdl].get_num_ins());
				if (port >= 0 && ins_adj_mdl[mdl][port] == unconnected)// its free and can be PI
				{
					if (add_PI(i, mdl, port))
					{
						succeeded[i] = true;
					}

				}
			}
			T++;
		}
		
	}
		
		
	
/* Check if there is any unconnected input ports that could be connetcted */
	
	if (pi <= sum_ins ) //all PIs can definitely get connected, so keep searching
	{
		for (int i=0; i<pi; i++)
		{
			if(!succeeded[i])//not connected yet
			{
				for(int j=0; j<vertices; j++)
				{
					for(int k=0; k< elements[j].get_num_ins(); k++)
					{
						if(ins_adj_mdl[j][k] == unconnected)
						{
							if (add_PI(i, j, k))//must definitly return 1 !
							{
								succeeded[i] = true;
								break;
							}
						}
					}
					if (succeeded[i])
					{
						break;
					}
				}
				
			}
		}
	}
	
	/************* CONNECT PRIMARY OUTPUTS *************/
	
	
	if (po > sum_outs * maximum_net_degree)
	{
		logFile << "\tSpecified PO (" << po << ") is more than total output ports (" <<
			sum_outs << ") * maximum_net_degree ("<<maximum_net_degree<<") !\n";
		//logFile << "\tThere is no way to connect all the primary output ports!\n";
		//logFile << "\t" << po - sum_outs * maximum_net_degree << " port(s) remain unconnected!\n";
		logFile << "\tNumber of PO decreased to " << sum_outs * maximum_net_degree << "!\n";
		po = sum_outs * maximum_net_degree;
	}
	else if (po > sum_outs)
	{
		logFile << "\tSpecified PO (" << po << ") is more than total output ports (" << sum_outs << ")!\n";
		logFile << "\tMultiple primary output ports may be connected to an output port.\n";
	}
	
	succeeded.resize(po);
	for (int j = 0; j < po; j++)
	{
		succeeded[j] = false;
	}

	max_net_degree = 0;

	while (max_net_degree < maximum_net_degree)
	{
		max_net_degree++;
		for (int j = 0; j < po; j++)
		{
			int T = 0;
			while (!succeeded[j] && T<limit)
			{
				int mdl = rand_c();
				if (mdl >= 0)
				{
					int port = rand_mod(0, elements[mdl].get_num_outs());
					if (port >= 0 )//no connection. ,means no connection to another PO
					{
						if (add_PO(j, mdl, port))//check fanout limit
						{
							succeeded[j] = true;
						}
						
					}
				}

				T++;
			}
		}
	}

	/**** CHECK IF THERE IS ANY UNCONNECTED OUTPUT PORTS THAT COULD BE CONNECTED *******/
	max_net_degree = maximum_net_degree;
	
	if(po <= sum_outs * maximum_net_degree) 
	{
		for (int i=0; i<po; i++)
		{
			if(!succeeded[i])//not connected yet
			{
				for(int j=0; j<vertices; j++)
				{
					for(int k=0; k< elements[j].get_num_outs(); k++)
					{
						
						if (add_PO(i, j, k))//must definitly return 1 !
						{
							succeeded[i] = true;
							break;
						}
						
					}
					if (succeeded[i])
					{
						break;
					}
				}
			}
		}
	}
	
	
	if(pi >= sum_ins) // There is no unconnected input port, terminate process
	{
		logFile << "\tNo unconnected input ports for connecting to output of other modules!\n";
		logFile.close();
		return -1;
	}

	if (po >= sum_outs * maximum_net_degree)
	{
		logFile << "\tNo available output port for connecting to input ports of other modules!\n";
		logFile.close();
		return -1;
	}

	logFile.close();
	return 0;

}

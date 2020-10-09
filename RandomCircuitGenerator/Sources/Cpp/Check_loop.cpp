#include "../Header/Header.h"


bool Graph::check_DFS(int out_mdl, int out_port, int in_mdl, int in_port) {

	bool found = false;
	
	if (out_mdl != in_mdl) //avoid self-loop
	{
		if (add_edge(out_mdl, out_port, in_mdl, in_port))// fanout< max
		{
			if (!DFS(in_mdl, comb_loop, seq_loop)) //no loop
			{
				found = true;
			}
			else //makes loop, undo it!
			{
				del_edge(out_mdl, out_port, in_mdl, in_port);
			}

		}
	}


	return found;

}

/*************************/
bool Graph::check_degree(int out_mdl, int out_port, int in_mdl, int in_port){

	if (add_edge(out_mdl, out_port, in_mdl, in_port))//check maximum degree limits
	{
		return true;
	}
	else
	{
		return false;
	}
		
}

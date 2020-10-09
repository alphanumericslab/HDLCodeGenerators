#include "../Header/Header.h"
#include <map>

std::map <int,int> net_hist;

void cal_net_degree(Graph &g, int c){
	
	for (int i=0; i<c; i++)
	{
		for(int j=0; j<g.get_element(i).get_num_outs(); j++)
		{
			net_hist[g.get_element(i).get_degree(j)]++;
		}

	}
}

int print_net_degree(){
	string name = "net_degree_log.csv";
	string path =  string(getcwd(NULL, 0)) + output_path;
	ofstream file(path + name);
	if(file.is_open())
	{
		for (auto p= net_hist.begin(); p!= net_hist.end(); ++p) 
		{
			file << p->first << " , " << p->second  << '\n';
		}
		
		return 1;
	}
	else
	{
		cerr << "Could not open file to print net degree distribution.\n";
		return -1;
	}
}
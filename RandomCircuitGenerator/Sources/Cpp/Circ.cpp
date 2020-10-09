// we see circuit as a graph
// as any other graphs it has an array of nodes and an adjacency ist

#include "../Header/Header.h"
#include <stack>

Graph::Graph() {
}

Graph::Graph(int vertices, int num_pi, int num_po, bool latch_in, bool latch_out, bool comb_loop, bool seq_loop) {
	this->vertices = vertices;
	this->num_pi = num_pi;
	this->num_po = num_po;
	this->latch_in = latch_in;
	this->latch_out = latch_out;
	this->comb_loop = comb_loop;
	this->seq_loop = seq_loop;
	elements.resize(vertices);
	PI_degree.resize(num_pi);
	PO_mdl.resize(num_po);
	PO_port.resize(num_po);

	ins_adj_mdl.resize(vertices);
	ins_adj_port.resize(vertices);
	inout_adj_mdl.resize(vertices);
	inout_adj_port.resize(vertices);


	for (int i = 0; i < num_pi; i++)
		PI_degree[i] = 0;

	for (int i = 0; i < num_po; i++)
	{
		PO_mdl[i] = unconnected;
		PO_port[i] = unconnected;
	}

}



int Graph::set_size(int i, int in, int inout) {//module i has #in input ports


	ins_adj_mdl[i].resize(in);
	ins_adj_port[i].resize(in);

	inout_adj_mdl[i].resize(inout);
	inout_adj_port[i].resize(inout);

	for (int j = 0; j < in; j++)
	{
		ins_adj_mdl[i][j] = unconnected;
		ins_adj_port[i][j] = unconnected;
	}

	for (int j = 0; j < inout; j++)
	{
		inout_adj_mdl[i][j] = highZ;
		inout_adj_mdl[i][j] = highZ;
	}

	return 0;//return type is actually for exception handling, add it later
}

Node Graph::get_element(int index) {
	return elements[index];
}

bool Graph::set_element(int index, Node node){
	if (index < vertices)
	{
		elements[index] = node;
		return true;
	}
	else
	{
		return false;
	}
}
int Graph::get_PI_degree(int index) {
	return PI_degree[index];
}

int Graph::get_vertices() {
	return vertices;
}

bool Graph::get_latch_in() {
	return latch_in;
}

bool Graph::get_latch_out() {
	return latch_out;
}

bool Graph::get_comb_loop() {
	return comb_loop;
}

bool Graph::get_seq_loop() {
	return seq_loop;
}

bool Graph::inc_PI_degree(int index) {

	if (PI_degree[index] < max_net_degree)
	{
		PI_degree[index]++;
		return true;
	}
	else
		return false;
}

bool Graph::dec_PI_degree(int index) {

	if (PI_degree[index] > 0)
	{
		PI_degree[index]--;
		return true;
	}
	else
		return false;
}

bool Graph::add_edge(int out_mdl, int out_port, int in_mdl, int in_port) {

	if (out_mdl == toGND || out_mdl == toVCC)
	{
		ins_adj_mdl[in_mdl][in_port] = out_mdl;
		ins_adj_port[in_mdl][in_port] = out_port;
		return true;
	}
	else if (elements[out_mdl].inc_degree(out_port))
	{
		ins_adj_mdl[in_mdl][in_port] = out_mdl;
		ins_adj_port[in_mdl][in_port] = out_port;
		return true;
	}
	else
		return false;
}


void Graph::del_edge(int out_mdl, int out_port, int in_mdl, int in_port) {

	ins_adj_mdl[in_mdl][in_port] = unconnected;
	ins_adj_port[in_mdl][in_port] = unconnected;

	if (out_mdl >= 0) //not vcc, gnd, ...
	{
		elements[out_mdl].dec_degree(out_port);

	}

}


bool Graph::add_inout_edge(int io1_mdl, int io1_port, int io2_mdl, int io2_port) {
	if (io1_port >= 0 && io2_port >= 0)
	{
		inout_adj_mdl[io1_mdl][io1_port] = io2_mdl;
		inout_adj_port[io1_mdl][io1_port] = io2_port;
		inout_adj_mdl[io2_mdl][io2_port] = io1_mdl;
		inout_adj_port[io2_mdl][io2_port] = io1_port;
		return true;
	}
	else
	{
		return false;
	}

}

void Graph::del_inout_edge(int io1_mdl, int io1_port, int io2_mdl, int io2_port) {
	inout_adj_mdl[io1_mdl][io1_port] = highZ;
	inout_adj_port[io1_mdl][io1_port] = highZ;
	inout_adj_mdl[io2_mdl][io2_port] = highZ;
	inout_adj_port[io2_mdl][io2_port] = highZ;
}

bool Graph::add_PI(int pi, int mdl, int port) {
	if (inc_PI_degree(pi))
	{
		ins_adj_mdl[mdl][port] = toPI;
		ins_adj_port[mdl][port] = pi;
		return true;
	}
	else
		return false;
}
void Graph::del_PI(int pi, int mdl, int port) {
	ins_adj_mdl[mdl][port] = unconnected;
	ins_adj_port[mdl][port] = unconnected;
	dec_PI_degree(pi);
}


bool Graph::add_PO(int po, int mdl, int port) {

	if (elements[mdl].inc_degree(port))
	{
		PO_mdl[po] = mdl;
		PO_port[po] = port;
		return true;
	}
	else
		return false;

}

void Graph::del_PO(int po, int mdl, int port) {
	PO_mdl[po] = unconnected;
	PO_port[po] = unconnected;
	elements[mdl].dec_degree(port);
}


int Graph::get_PO_mdl(int index) {
	return PO_mdl[index];
}

int Graph::get_PO_port(int index) {
	return PO_port[index];
}

int Graph::get_PO_size() {
	return PO_mdl.size();
}

bool Graph::extra_PO(int po, int mdl, int port)
{
	PO_mdl.push_back(mdl);
	PO_port.push_back(port);
	elements[mdl].inc_degree(port);//change zero to one
	return true;
}


bool Graph::DFS(int s, bool has_comb_loop, bool has_seq_loop)
{
	bool has_loop = false;

	// Initially mark all verices as not visited
	vector<bool> visited(vertices, false);
	vector<bool> finished(vertices, false);

	// Create a stack for DFS
	stack<int> stack;
	int elm;
	int uvc;
	// Push the current source node.
	if (!has_comb_loop & has_seq_loop & elements[s].get_clk())
		return false;
	stack.push(s);

	while (!has_loop && !stack.empty())
	{
		// Pop a vertex from stack and print it
		s = stack.top();
		stack.pop();

		// Stack may contain same vertex twice. So
		// we need to print the popped item only
		// if it is not visited.
		if (!visited[s])
		{
			//cout << s << " ";
			visited[s] = true;
		}

		// Get all adjacent vertices of the popped vertex s
		// If a adjacent has not been visited, then push it
		// to the stack.
		elm = 0;
		uvc = 0;
		for (int i = 0; i < elements[s].get_num_ins(); i++)
		{
			elm = ins_adj_mdl[s][i];

			if (elm != unconnected && elm != toPI && elm != toGND && elm != toVCC)
			{
				if (!has_comb_loop & has_seq_loop & elements[elm].get_clk()) //no loop in path that has a 
					continue;

				if (!visited[elm])
				{
					uvc++;
					if (uvc == 1)
						stack.push(s); //push parent before all childs
					stack.push(elm);
				}
				else if (!finished[elm])
				{
					has_loop = true;
					//cout<<elm<<" loop!\n";
					break;
				}
			}
		}

		if (uvc == 0)
			finished[s] = true;

	}

	return has_loop;
}



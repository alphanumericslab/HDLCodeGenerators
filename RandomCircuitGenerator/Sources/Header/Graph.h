#pragma once

#include <string>
#include "Node.h"


class Graph {

	int vertices; //number of vertices
	int num_pi; //number of primary inputs
	int num_po; //number of primary outputs
	bool latch_in; //latch primary inputs or not
	bool latch_out; //latch primary outputs or not
	bool comb_loop; // combinational loop is allowed or not
	bool seq_loop; // seuential loop is allowed or not
	vector<Node> elements;
	vec_int PI_degree;
	vec_int PO_mdl;
	vec_int PO_port;
	vector<vec_int> ins_adj_mdl; //adjacency list of input ports of all modules that keeps which module the port is connected to
	vector<vec_int> ins_adj_port; //adjacency list of input ports that keeps which port the port is connected to
	vector<vec_int> inout_adj_mdl;
	vector<vec_int> inout_adj_port;
	//vector<vector<bool>> tl;

public:
	Graph();
	Graph(int, int, int, bool, bool, bool, bool); //constructor

	int get_vertices();
	bool get_latch_in();
	bool get_latch_out();
	bool get_comb_loop();
	bool get_seq_loop();

	int set_size(int, int, int);

	Node get_element(int);
	bool set_element(int, Node);

	//	int get_ins_adj_mdl(int, int);
	//	int get_ins_adj_port(int, int);

	//	int get_inout_adj_mdl(int, int);
	//	int get_inout_adj_port(int, int);



	int get_PI_degree(int);
	bool inc_PI_degree(int);
	bool dec_PI_degree(int);

	int get_PO_mdl(int);
	int get_PO_port(int);

	int get_PO_size();

	bool add_edge(int, int, int, int);
	void del_edge(int, int, int, int);

	bool add_inout_edge(int, int, int, int);
	void del_inout_edge(int, int, int, int);

	bool add_PI(int, int, int);
	void del_PI(int, int, int);

	bool add_PO(int, int, int);
	void del_PO(int, int, int);

	bool extra_PO(int, int, int);
	bool DFS(int, bool, bool);
	void select(vector<Node>&);
	int setIO(int&, int&, string);
	int connect_inputs(string);

	bool check_DFS(int, int, int, int);
	bool check_degree(int, int, int, int);

	double make_layer(int, int, int, bool, bool, string, string, string, int);
	

	int dangle(int, bool, bool, string);
	int mutate_add(int, string);
	int mutate_replace(int, string);
	int mutate_swap(int, string);

	void print(char, int, int, string, string, vector<string>&);
	void print_primitive(ofstream &, int, string);
	void print_other(ifstream &, ofstream &, int, bool);
	void latching_start(string, string, vector<string>&);
	void latching_finish(string, string, vector<string>&);
};


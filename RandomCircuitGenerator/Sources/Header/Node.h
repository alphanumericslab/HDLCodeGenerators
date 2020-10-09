#pragma once
#include <vector>
using namespace std;
typedef std::vector<int> vec_int;

class Node {
	int ID;
	bool is_primitive;
	bool has_clk;
	bool has_rst;
	bool has_ce;
	int num_ins;
	int num_outs;
	int num_inouts;
	vec_int degree;

	// there may be an array that keeps track of connected and unconnected ports
public:

	Node();
	Node(int, bool, bool, bool, bool, int, int, int);

	int get_ID();
	bool get_prim();
	bool get_clk();
	bool get_rst();
	bool get_ce();
	int get_num_ins();
	int get_num_outs();
	int get_num_inouts();
	int get_degree(int);

	void set_clk(bool);
	void set_rst(bool);
	void set_ce(bool);
	void set_num_ins(int);
	void set_num_outs(int);
	void set_num_inouts(int);

	bool inc_degree(int);
	bool dec_degree(int);
};

#include "../Header/Header.h"

Node::Node() {
}

Node::Node(int id, bool prim, bool clk, bool rst, bool ce, int in, int out, int inout) {
	ID = id;
	is_primitive = prim;
	has_clk = clk;
	has_rst = rst;
	has_ce = ce;
	num_ins = in;
	num_outs = out;
	num_inouts = inout;

	degree.resize(out); //keep fanout of each output port

	for (int i = 0; i < out; i++)
		degree[i]=0;
}

int Node::get_ID(){
	return ID;
}

bool Node::get_prim(){
	return is_primitive;
}

bool Node::get_clk(){
	return has_clk;
}

bool Node::get_rst() {
	return has_rst;
}

bool Node::get_ce(){
	return has_ce;
}
int Node::get_num_ins() {
	return num_ins;
}

int Node::get_num_outs() {
	return num_outs;
}

int Node::get_degree(int index) {
	return degree[index];
}


int Node::get_num_inouts() {
	return num_inouts;
}

void Node::set_clk( bool clk){
	has_clk = clk;
}

void Node::set_rst(bool rst) {
	has_rst = rst;
}

void Node::set_ce(bool ce) {
	has_ce = ce;
}

void Node::set_num_ins(int in) {
	num_ins = in;
}

void Node::set_num_outs(int out) {
	num_outs = out;
}

void Node::set_num_inouts(int inout) {
	num_inouts = inout;
}

bool Node::inc_degree(int index) {

	if(degree[index] < max_net_degree)
	{
		degree[index]++;
		return true;
	}
	else
		return false;
}

bool Node::dec_degree(int index) {
	if (degree[index] > 0)
	{
		degree[index]--;
		return true;
	}
	else
		return false;
}
#pragma once
#include <cstdlib>
#include <iostream>
#include <fstream>
#include <cstdlib>
#include "Node.h"
#include "Graph.h"


#ifdef _WIN32
    #include <direct.h>
    #define getcwd _getcwd // stupid MSFT "deprecation" warning
	#define mkdir	_mkdir
	#define rmdir	_rmdir
#else
    #include <unistd.h>
	#include <sys/stat.h>
	#define mkdir(arg1)  mkdir(arg1,0777)
#endif


extern int max_net_degree;
extern int sum_ins;
extern int sum_outs;
extern int sum_inouts;
extern int sum_of_ins;

extern bool layer_has_clk;
extern bool layer_has_rst;
extern bool layer_has_ce;

#define maximum_net_degree 100 //may change this to any desired value

// Don't change these 
#define toPI -1 
#define toPO -2
#define toVCC -3
#define toGND -4
#define unconnected -5
#define highZ -6
#define outOfBound -7

extern string output_path;
extern string insts_path;
extern string Verilog_lib_path;

class couple {
	int module;
	int port;

public:
	couple();
	couple(int, int);
	int get_module();
	int get_port();
};

class triple{
	int mut1;
	int mut2;
	int mut3;
public:
	triple();
	triple(int, int, int);
	int get_type1();
	int get_type2();
	int get_type3();
	void inc_type1();
	void inc_type2();
	void inc_type3();
};

int circ_gen(int, int, int, int, int, int, int, bool, bool, bool, bool, bool, bool);

void make_dir (int, int, int, int);
int list_it(vector<string>&, vector <string>&, const char*, int);
int clean_it (string);

vec_int init_lib(vector<string>&,  vector<string>&, vector<Node>& );

double make_top(int, int, int, string, string, string);

void rand_init(int, int);
int rand_default(int, int, int);
int rand_c();
int rand_mod(int, int);

bool is_ANSI(string, string);
int parser(string, string);
int not_ansi_parser (string, string);
int ansi_parser(string, string);
int xlnx_parser(string, string);
void syr_parser(string, string);
void older_format(string, string);
void newer_format(string, string);

string token(string, int, int);
int tokenize(string, fstream &, int);
void gather(fstream &, fstream &, string, string);
void create_blackBox(fstream &, string);

int tcl_gen_part3(int, int, int, string, string, int);
int tcl_gen_part2(Graph &, vector<string>&, vec_int &, int, int, string, string, int);
int tcl_gen_part1(int, string , string, string, string);
void script_gen(string , string);
void script_finish(string, string, string, string);
void script_start(string);

int print_net_degree();
void cal_net_degree(Graph&, int);

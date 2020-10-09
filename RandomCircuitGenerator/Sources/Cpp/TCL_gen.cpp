#include "../Header/Header.h"

vector<vector <bool>> print_file;

int tcl_gen_part1(int num_files, string path, string name, string top_mdl, string comp_dir) {

	ofstream TCL;
	
	TCL.open(path + name + ".tcl", ios::out);

	int loc = 0;
	string tcl_path = path + name + ".tcl";
	while ((loc = tcl_path.find("\\", loc))> 0)
	{
		tcl_path.replace(loc, 1, "/");
	}

	script_gen(string(getcwd(NULL, 0)) + output_path + "TCL/" + top_mdl + ".cmd", tcl_path);
	vector<bool> pf;
	pf.resize(num_files, true);
	print_file.push_back(pf);
	TCL << "# where all output will be created" << endl;
	TCL << "set compile_directory " << comp_dir << endl;
	TCL << "# the top level of our HDL source" << endl;
	TCL << "set top_name " << top_mdl << endl;
	TCL << "#input source files" << endl;
	TCL << "set hdl_files {\n";
	TCL.close();
	return 1;
}

int tcl_gen_part2(Graph &g, vector<string>& files, vec_int &vfiles, int N, int L, string path, string name, int version) {

	ofstream TCL;
	TCL.open(path + name + ".tcl", ios::app);
	int loc = 0;
	string CWD = string(getcwd(NULL, 0));
	while ((loc = CWD.find("\\", loc))> 0)
	{
		CWD.replace(loc, 1, "/");
	}

	//script << "# files from library\n";
	for (int i = 0; i < g.get_vertices(); i++)
	{
		if (print_file[version][vfiles[g.get_element(i).get_ID()]])
		{
			if ((loc = files[vfiles[g.get_element(i).get_ID()]].find("xlnx__")) != 0) // do not add xilinx templates file
			{
				if(!g.get_element(i).get_prim()) //not language primitive
					TCL << "\t" << CWD << Verilog_lib_path << files[vfiles[g.get_element(i).get_ID()]] << ".v\n";
			}

			print_file[version][vfiles[g.get_element(i).get_ID()]] = false; //do not print it again
		}
	}

	TCL << "\t" << CWD << output_path + "Verilog/";

	if (version == 0) //original one
	{
		TCL << "original";
	}
	else
	{
		TCL << "mutant_" << version - 1;
	}

	TCL << "/N_" << N << "/Layer_" << N << "_" << L << ".v \n";

	TCL.close();
	return 1;
}

int tcl_gen_part3(int N, int L, int C, string path, string name, int version) {

	ofstream TCL;
	TCL.open(path + name + ".tcl", ios::app);
	int loc = 0;
	string CWD = string(getcwd(NULL, 0));
	while ((loc = CWD.find("\\", loc))> 0)
	{
		CWD.replace(loc, 1, "/");
	}


	for (int i = 0; i < N; i++)
	{
		TCL << "\t" << CWD << output_path + "Verilog/";

		if (version == 0) //original one
			TCL << "original";
		else
			TCL << "mutant_" << version - 1;

		TCL << "/N_" << N << "/Disjoint_" << i << ".v \n";
	}

	TCL << "\t" << CWD << output_path + "Verilog/";

	if (version == 0) //original one
		TCL << "original";
	else
		TCL << "mutant_" << version - 1;

	TCL << "/final" << "/TOP_" << N << "_" << L << "_" << C << ".v \n}\n";

	TCL << "#create compile dirctory if it has not been created before\n";
	TCL << "if {![file isdirectory $compile_directory]} {\n";
	TCL << "\tfile mkdir $compile_directory\n";
	TCL << "}\ncd $compile_directory\n";
	TCL << "# make project in compile directory if there is none\n";
	TCL << "project new project_top\n";

	TCL << "# add specified files to project\n";
	TCL << "foreach filename $hdl_files {\n";
	TCL << "\txfile add $filename\n";
	TCL << "\tputs \"Adding file $filename to the project.\"\n}\n";
	
	TCL << "project save\n";
	TCL << "project close\n";
	TCL.close();
	return 1;
}


void script_gen(string path, string tcl_name)
{
	ofstream sc; //script file
	sc.open(path, ios::app);
	sc << "xtclsh " << tcl_name << endl;
	//sc << "syr_paser.exe  main.cpp " << comp_dir << "/" << "  " << top_mdl << "  " << endl << endl;
	sc.close();
}

void script_start(string path){
	ofstream sc (path, ios::out); //script file
}

void script_finish(string path, string output_folder, string final_dest, string top_name) {
	ofstream sc; //script file
	//sc.open(path, ios::app);
	//sc << output_folder << " ." << endl;
	/*
	sc << "md " << final_dest << top_name << endl;
	sc << "move " << output_folder << "\\SYR  " << final_dest <<top_name << endl;
	sc << "move " << output_folder << "\\TCL  " << final_dest << top_name << endl;
	sc << "move " << output_folder << "\\Verilog  " << final_dest << top_name << endl;
	
	sc << "cd C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\VC\n";
	sc << "vcvarsall.bat\n";
	sc << "cd C:\\Users\\Laleh\\Desktop\n";
	sc << "cl syr_pars\n";
	*/
	
}
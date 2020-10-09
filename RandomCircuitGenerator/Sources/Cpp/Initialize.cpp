#include "../Header/Header.h"


vec_int init_lib(vector<string>& files, vector<string>& paths, vector<Node>& lib) {


	int lib_size = 0;

	vector <string> primitives;
	vector <string> ::iterator it;

	/****** Create a list of Verilog prmitives *********/

	primitives.push_back("and");
	primitives.push_back("nand");
	primitives.push_back("or");
	primitives.push_back("nor");
	primitives.push_back("xor");
	primitives.push_back("xnor");
	primitives.push_back("buf");
	primitives.push_back("not");
	primitives.push_back("bufif0");
	primitives.push_back("bufif1");
	primitives.push_back("notif0");
	primitives.push_back("notif1");
	/******************************/

	int fsize = files.size(); 
	vec_int in_which_file;
	in_which_file.resize(fsize);

	for (int i = 0; i < fsize; i++)
	{
		int count = parser(files[i], paths[i]);
		lib_size += count;
		in_which_file[i] = i;
		
		if (count>1)
		{
			for (int j = 1; j < count; j++)
			{
				files.push_back(files[i] + "_" + to_string(j));
				
				paths.push_back(paths[i] + "_" + to_string(j));

				in_which_file.push_back(i);
			}

		}
	}
	
	
	lib.resize(lib_size);
	

	for (int i = 0; i < lib_size; i++)
	{
		bool has_clk = false;
		bool has_rst = false;
		bool has_ce = false;
		int num_ins = 0;
		int num_outs = 0;
		int num_inouts = 0;

		ifstream instant;
		
		instant.open(insts_path + files[i] + "_instant.txt");

		if (instant.is_open())
		{
			string line;
			getline(instant, line);//name of module
			string ID = line;
			bool is_prim = false;
			for (int it = 0; it < primitives.size(); it++)
			{
				if (ID.compare(primitives[it]) == 0)
				{
					is_prim = true;
					break;
				}
			}
			/* GET INFO OF INPUT PORTS */
			getline(instant, line);
			int num = atoi(line.c_str());

			for (int j = 0; j < num; j++)
			{
				getline(instant, line, '\t');
				int temp = atoi(line.c_str());

				getline(instant, line); //reads name, we dont need it
				if (line == "clk")
				{
					has_clk = true;
				}
				else if (line == "rst")
				{
					has_rst = true;
				}
				else if (line == "ce")
				{
					has_ce = true;
				}
				else
				{
					num_ins += temp;
				}

			}

			/* GET INFO OF OUTPUT PORTS */
			getline(instant, line);
			num = atoi(line.c_str());


			for (int j = 0; j < num; j++)
			{
				getline(instant, line, '\t');
				int temp = atoi(line.c_str());
				num_outs += temp;
				getline(instant, line); //reads name, we dont need it

			}

			/* GET INFO OF INOUT PORTS */
			getline(instant, line);
			num = atoi(line.c_str());

			for (int j = 0; j < num; j++)
			{
				getline(instant, line, '\t');
				int temp = atoi(line.c_str());
				num_inouts += temp;
				getline(instant, line); //reads name, we dont need it

			}

			
			lib[i] = Node(i, is_prim, has_clk, has_rst, has_ce, num_ins, num_outs, num_inouts);

			instant.close();
		}
		

		else
		{
			cerr << "Cannot open instant.txt in function \"init_lib\".\n";
		}
	}

	return in_which_file;
}

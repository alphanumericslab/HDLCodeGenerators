#include "../Header/Header.h"

int xlnx_parser(string name, string fpath) {

	ifstream file(fpath + ".v");
	
	//ofstream tmpl;
		
	int count = 0;
	if (file.is_open())
	{
		fstream inputs;
		fstream outputs;
		fstream inouts;
		fstream instant;
		//fstream params;
		string file_name = name;
		string str;
		int loc1, loc2;
		while (getline(file, str))
		{
			inputs.open(insts_path + "inputs.txt", ios::out);
			outputs.open(insts_path + "outputs.txt", ios::out);
			inouts.open(insts_path + "inouts.txt", ios::out);
			
			bool go_param = false;
			bool read_params = false;
			bool read_ports = false;
			bool end = false;
			bool cont = false;
			
			int num_ins = 0;
			int num_outs = 0;
			int num_inouts = 0;
			int num_params = 0;

			string com;
			string mdl_name;

			if (count>0)
			{
				file_name = name+ "_" +  to_string(count);
			}
			
			do
			{
					
				if ((loc1 = str.find("//")) >= 0)
				{
					com = str.substr(loc1 + 2);
					str = str.substr(0, loc1);

				}
				if (!cont && (loc1 = str.find("#")) >= 0)
				{
					mdl_name = token(str, 0, loc1);
					
					instant.open(insts_path + file_name + "_instant.txt", ios::out);
					//tmpl.open(path + file_name + "_template.v", ios::out);
					//tmpl << "// INSTANTIATION TEMPLATE\n";
					instant << mdl_name << endl;
					//tmpl << mdl_name << " #(" << endl;
					instant.close();
					go_param = true;
					cont = true;
					//count++;

				}
				else if (!cont && (loc2 = str.find("_inst"))>=0 && (loc1 = str.find("(")) >= 0)
				{
					mdl_name = token(str, 0, loc2);
					loc1 = mdl_name.size()/2;
					mdl_name = mdl_name.substr(0, loc1);
					
					instant.open(insts_path + file_name + "_instant.txt", ios::out);
					//tmpl.open(path + file_name + "_template.v", ios::out);
					//tmpl << "// INSTANTIATION TEMPLATE\n";
					instant << mdl_name << endl;
					//tmpl << mdl_name << " ";
					instant.close();
					cont = true;
					//count++;
					
				}

				if (read_params)
				{
					loc1 = str.find(".");
					loc2 = str.find(",");
					if(loc2 == -1)
					{
					loc2 = str.find(")");
					}
					if (loc1>-1 && loc2 > loc1)
					{
						//port_name = str.substr(loc1, loc2 - loc1 + 1);
						num_params++;
						//params << port_name <<"\t "<<endl;
						//tmpl << port_name << endl;
						
					}
				}
				if (read_ports)
				{
					loc1 = str.find(".");
					loc2 = str.find("(");
					string length;
					string port_name;
					if (loc1>-1 && loc2 > loc1)
					{
						port_name = str.substr(loc1 + 1, loc2 - loc1 - 1);
						
						if ((loc1 = com.find("-bit")) >= 0)
						{
							length = token(com, 0, loc1);
						}
						else
						{
							length = "1";
						}
						
						if ((loc1 = com.find("input")) >= 0)
						{
							//direction = "input";
							inputs << length << "\t" << port_name << endl;
							num_ins++;
						}
						else if ((loc1 = com.find("output")) >= 0)
						{
							//direction = "output";
							outputs << length << "\t" << port_name << endl;
							num_outs++;
						}
						else if ((loc1 = com.find("inout")) >= 0)
						{
							//direction = "inout";
							inouts << length << "\t" << port_name << endl;
							num_inouts++;
						}

						
					}


				}
				if ((loc1 = str.find(".")) == -1 && read_params && (loc2 = str.find(")")) >= 0)
				{
					read_params = false;
					go_param = false;
					//tmpl << ")\n";
				}
				if ((loc1 = str.find(".")) == -1 && (loc1 = str.find(")")) >= 0 && (loc2 = str.find(";")) >= 0)
				{
					end = true;
					read_ports = false;
				}

				if (go_param && (loc1 = str.find("#"))>=0 && (loc1 = str.find("(")) >= 0)
				{
					read_params = true;
				}
				
				if (!go_param &&  (loc1 = str.find("_inst"))>=0 && (loc1 = str.find("(")) >= 0)
				{
					read_ports = true;
					read_params = false;
					
				}
			}while (!end && getline(file, str));
			
			if (end)
			{
				count++;
				inouts.close();
				inputs.close();
				outputs.close();
				
				//tmpl.close();

				instant.open(insts_path + file_name+"_instant.txt", ios::app);
				//instant << num_params << endl;
				
				//gather(instant, params, "params.txt", file_name);
				instant << num_ins << endl;
	
				gather(instant, inputs, insts_path + "inputs.txt", file_name);

				instant << num_outs << endl;

				gather(instant, outputs, insts_path + "outputs.txt", file_name);

				instant << num_inouts << endl;
		
				gather(instant, inouts, insts_path + "inouts.txt", file_name);
				instant.close();
				create_blackBox(instant, file_name);
			}

		
		}
	}
	return count;
}

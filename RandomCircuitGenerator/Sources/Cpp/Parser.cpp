#include "../Header/Header.h"


int parser (string fname, string fpath){
	int loc;
	if ((loc = fname.find("xlnx__"))>=0)
	{
		return xlnx_parser(fname, fpath);
	}
	if (is_ANSI (fname, fpath))
	{
		return ansi_parser (fname, fpath);
	}
	else
	{
		return not_ansi_parser (fname, fpath);
	}
}


int not_ansi_parser(string fname, string fpath) {

	string path = string(getcwd(NULL, 0));
		fstream instant;
		instant.open(insts_path + fname + "_instant.txt", ios::out);
		if (instant.is_open())
		{
			//	ofstream tmpl;
			//	tmpl.open(path + fname + "_template.v", ios::out);

			fstream inputs;
			inputs.open(insts_path + "inputs.txt", ios::out);
			fstream outputs;
			outputs.open(insts_path + "outputs.txt", ios::out);
			fstream inouts;
			inouts.open(insts_path + "inouts.txt", ios::out);

			ifstream file(fpath + ".v");
			if (file.is_open())
			{

				/*********** LIST OF INPUT OUTPUT ******/
				string part1 = "";
				string part2 = "";
				string str;
				int count = 0;
				int cm1, cm2, cm2_end, cm1_end;
				int loc1, loc2, loc;
				while (getline(file, str, ';'))
				{
					int cnt = false;
					int end = -1;
					int num_ins = 0;
					int num_outs = 0;
					int num_inouts = 0;
					str = part1 + " " + str;
					string file_name = fname;
					if (count > 0)
					{
						file_name = fname + "_" + to_string(count);
						instant.open(insts_path + file_name + "_instant.txt", ios::out);

						//tmpl.open(path + file_name + "_template.v", ios::out);
					}

					do
					{
						/* Ignore comments **/
						int first_size = str.size();
						part1 = "";
						part2 = "";
						do
						{
							cm1 = -1;
							cm2 = -1;
							cm2_end = -1;

							if ((cm2 = str.find("/*")) >= 0)
							{
								part1 = str.substr(0, cm2);
								cm2_end = str.find("*/", cm2);
								while (cm2_end < 0)
								{
									getline(file, str);
									cm2_end = str.find("*/");
								}

								part2 = str.substr(cm2_end + 2, str.size());
								str = part1 + " " + part2;

							}

							else if ((cm1 = str.find("//")) >= 0)
							{
								part1 = str.substr(0, cm1);
								cm1_end = str.find('\n', cm1 + 2);
								if (cm1_end >= 0)
								{
									part2 = str.substr(cm1_end + 1, str.size());
									str = part1 + " " + part2;
								}
								else//there is an ';' at the end of string
								{
									str = part1;
								}

							}

						} while (cm1 >= 0 || cm2 >= 0); //if it has found one then continue, if none is find, there is not any

														/*********************************/

						if (!cnt)
						{

							if ((loc1 = str.find("module")) >= 0)
							{
								loc1 = loc1 + 6;
								loc2 = str.find('(');
								string name = token(str, loc1, loc2);

								instant << name << endl;
								//tmpl << name<<" " ;
								count++;
								cnt = true;
								//tmpl.close();
								instant.close();
							}

						}
						else
						{
							if ((loc1 = str.find("input")) >= 0
								&& (loc1 > 0 ? (!isalnum(str[loc1 - 1]) && str[loc1 - 1] != '_') : true)
								&& (loc1 + 5 < str.size() ? !isalnum(str[loc1 + 5]) : true))
							{
								if ((loc2 = str.find("wire")) > 0)
								{
									loc = loc2 + 4;
								}
								else
								{
									loc = loc1 + 5;
								}
								num_ins += tokenize(str, inputs, loc);
							}
							else if ((loc1 = str.find("output")) >= 0
								&& (loc1 > 0 ? (!isalnum(str[loc1 - 1]) && str[loc1 - 1] != '_') : true)
								&& (loc1 + 6 < str.size() ? !isalnum(str[loc1 + 6]) : true))
							{
								if ((loc2 = str.find("reg")) > 0)
								{
									loc = loc2 + 3;
								}
								else if ((loc2 = str.find("wire")) > 0)
								{
									loc = loc2 + 4;
								}
								else
								{
									loc = loc1 + 6;
								}

								num_outs += tokenize(str, outputs, loc);

							}

							else if ((loc1 = str.find("inout")) >= 0
								&& (loc1 > 0 ? (!isalnum(str[loc1 - 1]) && str[loc1 - 1] != '_') : true)
								&& (loc1 + 5 < str.size() ? !isalnum(str[loc1 + 5]) : true))
							{
								if ((loc2 = str.find("wire")) > 0)
								{
									loc = loc2 + 4;
								}
								else
								{
									loc = loc1 + 5;
								}
								num_inouts += tokenize(str, inouts, loc);

							}
						}
						end = str.find("endmodule");
						int x;
						if (end >= 0 && (x = str.find("module", end + 9)) > 0) //reading begining of another module
						{
							//int off = -1 * (first_size - x);
							//file.seekg(off, std::ios::cur);
							part1 = str.substr(x);

						}
						else
						{
							part1 = "";
						}


					} while (end == -1 && getline(file, str, ';'));
					inputs.close();
					outputs.close();
					inouts.close();

					instant.open(insts_path + file_name + "_instant.txt", ios::app);
					instant << num_ins << endl;

					gather(instant, inputs, insts_path + "inputs.txt", file_name);

					instant << num_outs << endl;

					gather(instant, outputs, insts_path + "outputs.txt", file_name);

					instant << num_inouts << endl;

					gather(instant, inouts, insts_path + "inouts.txt", file_name);
					instant.close();

					//create_blackBox(instant, file_name);

				}

				return count;

			}
			else
			{
				cerr << "\nParser is unable to open " << fname << ".v.\n";
				return 0;
			}
		}
		else
		{
			return 0;
		}

	}

	string token(string str, int loc1, int loc2) {

		string name = "";
		for (int i = loc1; i<loc2; i++)
		{
			if (!isspace(str[i]) && str[i] != ')')
			{
				name += str[i];
			}
		}
		return name;
	}

	int tokenize(string str, fstream &file, int loc) {

		int loc1, loc2, loc3, loc4;
		string temp;
		loc1 = loc;
		string s_index1, s_index2;
		int index, index1, index2;
		int num = 0;

		if ((loc3 = str.find('['))> 0)
		{
			loc4 = str.find(':');
			s_index1 = token(str, loc3 + 1, loc4);
			index1 = atoi(s_index1.c_str());
			loc3 = loc4;
			loc4 = str.find(']');
			s_index2 = token(str, loc3 + 1, loc4);
			index2 = atoi(s_index2.c_str());
			if (index1 > index2)
			{
				index = index1 - index2 + 1;
			}
			else
			{
				index = index2 - index1 + 1;
			}
			loc1 = loc4 + 1;
		}
		else
		{
			index = 1;
		}


		while (loc1 < str.size())
		{
			loc2 = str.find(',', loc1);
			if (loc2 <0)
			{
				loc2 = str.find(";", loc1);
			}
			if (loc2 <0)
			{
				loc2 = str.size();
			}
			temp = token(str, loc1, loc2);
			if (temp.find_first_not_of(" \t\n\v\f\r") != std::string::npos)
			{
				num++;
				file << index << "\t" << temp << endl;
				
			}

			loc1 = loc2 + 1;
		}
		return num;
	}

	void gather(fstream & instant, fstream & file, string file_name, string fname) {

		string line, name, size;

		file.open(file_name, ios::in);
		while (getline(file, line, '\t'))
		{
			instant << line << "\t";
			getline(file, line);

			instant << line << endl;
		}
		file.close();

	}
	void create_blackBox(fstream & instant, string fname) {

		string line, mdl_name;
		string space;
		int num, size;
		ofstream black_box;
		ofstream tmpl;
		int newLine = 1;
		tmpl.open(insts_path + fname + "_template.v", ios::app);
		instant.open(insts_path + fname + "_instant.txt", ios::in);
		black_box.open(insts_path + fname + "_blackBox.v");

		black_box << "//\n// Black Box\n//\n";
		
		getline(instant, mdl_name); //name of module
		black_box << "module " << mdl_name << " (";
		//tmpl << mdl_name << " ";
		/*
		getline(instant, line); //number of parameters

		num = atoi(line.c_str());//number of parameters
		if (num >0)
		{
			tmpl << "#(";//list parameters
			for (int i = 0; i<num - 1; i++)
			{
				getline(instant, line, '\t');
				tmpl << line << ", ";
				getline(instant, line);
			}
			getline(instant, line, '\t');
			tmpl << line << ") ";
			getline(instant, line);
		}
		*/
		tmpl << mdl_name << "_inst (\n";
		getline(instant, line);
			num = atoi(line.c_str());
		for (int i = 0; i < 3; i++)//input, output, inout
		{
			int j;
			for (j = 0; j < num; j++)
			{
				newLine++;
				
				getline(instant, line, '\t');
				getline(instant, line);
				black_box << line;
				tmpl << "\t." << line << "(" << line << ")";
				if(j < num-1)
				{
					black_box << ", ";
					tmpl << ",\n";
				}
				if (newLine % 7 == 0)//just to mae it neater!
				{
					black_box << "\n";
				}

			}
			getline(instant, line);
			num = atoi(line.c_str());
			if(j>0 && num>0)
			{
				black_box << ", ";
				tmpl << ",\n";
			}

		}

		black_box << ");\n";
		tmpl << "\n);";
		tmpl.close();
		/* DECLARE PORTS */

		instant.clear();
		instant.seekg(0, ios::beg);

		/* INPUTS*/
		getline(instant, line);//name of module
		
		getline(instant, line);//number of inputs

		num = atoi(line.c_str());

		for (int j = 0; j < num; j++)
		{
			getline(instant, line, '\t');
			size = atoi(line.c_str()) - 1;
			getline(instant, line);
			black_box << "input ";
			if (size>0)
			{
				black_box << "[" << size << " : " << 0 << "] ";
			}

			black_box << line << ";\n";
		}


		/* OUTPUTS */

		getline(instant, line);

		num = atoi(line.c_str());

		for (int j = 0; j < num; j++)
		{
			getline(instant, line, '\t');
			size = atoi(line.c_str()) - 1;
			getline(instant, line);
			black_box << "output ";
			if (size>0)
			{
				black_box << "[" << size << " : " << 0 << "] ";
			}

			black_box << line << ";\n";
		}


		/* INOUTS*/


		getline(instant, line);

		num = atoi(line.c_str());

		for (int j = 0; j < num; j++)
		{
			getline(instant, line, '\t');
			size = atoi(line.c_str()) - 1;
			getline(instant, line);
			black_box << "inout ";
			if (size>0)
			{
				black_box << "[" << size << " : " << 0 << "] ";
			}

			black_box << line << ";\n";
		}

		//////
		black_box << "\nendmodule";
		instant.close();
		black_box.close();
	}

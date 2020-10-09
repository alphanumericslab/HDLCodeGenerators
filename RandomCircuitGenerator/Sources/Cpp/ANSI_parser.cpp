#include "../Header/Header.h"


bool is_ANSI(string fname, string fpath) {

	ifstream file(fpath + ".v");

	if (file.is_open())
	{
		bool cont = true;
		string keep_str  = "";
		//bool hasDirection = false;
		string str = "";
		int  loc1, loc2;
		int cm1, cm2, cm2_end;
		while (cont)
		{
			getline(file, str);
			str = keep_str+ " " + str;
			/********* IGNORE COMMENTS ******
			/*************** IGNORE COMMENTS ******************/
			do
			{
				cm1 = -1;
				cm2 = -1;
				cm2_end = -1;

				string part1, part2;

				if ((cm2 = str.find("/*")) >= 0)
				{
					part1 = str.substr(0, cm2);
					cm2_end = str.find("*/", cm2);
					while (cm2_end == -1)
					{
						getline(file, str);
						cm2_end = str.find("*/");
					}

					part2 = str.substr(cm2_end + 2);
					str = part1 +" "+ part2;

				}

				else if ((cm1 = str.find("//"))>=0)
				{
					str = str.substr(0, cm1);	
				}
				
			} while (cm1>=0 || cm2>=0); //if it has found one then continue, if none is find, there is not any

			/************/
			if ((loc1 = str.find("module"))>=0)
			{
				str = str.substr(loc1);
				if((loc2 = str.find("("))>0)
				{
					//str = str.substr(loc2);
					if((loc2 = str.find(","))>0)
					{
						str = str.substr(0, loc2+1);
						cont = false;
					}
					else if ((loc2 = str.find(")"))>0)
					{
						str = str.substr(0, loc2+1);
						cont = false;
					}
				}
				
				
				keep_str = str;
			}
			
		}
		if( ((loc1 = str.find ("input"))>=0 &&  (isspace(str[loc1+5]) || str[loc1+5]==','))//in case that input is part of port name
			|| ((loc1 = str.find ("output"))>=0 &&  (isspace(str[loc1+6]) || str[loc1+6]==','))
			|| ((loc1 = str.find ("inout"))>=0 &&  (isspace(str[loc1+5]) || str[loc1+5]==',')) )
		{
			//hasDirection = true;
			return true;
		}
		else
		{
			//hasDirection = false;
			return false;
		}
		
		file.close();
	}
	else
	{
		cerr << "is_ANSI: file" << fname << ".v cannot be opened ";
		return false;
	}

}

int ansi_parser(string fname, string fpath) {
	
	string file_name = fname;
	ifstream file(fpath + ".v");

	fstream instant;
	instant.open(insts_path + fname + "_instant.txt", ios::out);
	fstream inputs;
	inputs.open(insts_path + "inputs.txt", ios::out);
	fstream outputs;
	outputs.open(insts_path + "outputs.txt", ios::out);
	fstream inouts;
	inouts.open(insts_path + "inouts.txt", ios::out);

	//ofstream tmpl;
	//tmpl.open(insts_path + fname + "_template.v", ios::out);

	string str = "";
	string keep_str = "";
	string part1, part2, name;
	int loc, loc1, loc2;
	int num_ins, num_outs, num_inouts;
	char delim = '\n';
	string direction = "";
	
	bool hasDirection = false;
	bool stop, cont;
	int cm1, cm2, cm2_end;
	bool name_found;

	if (file.is_open())
	{

		num_ins = 0;
		num_outs = 0;
		num_inouts = 0;
		
		stop = false;
		hasDirection = false;
		name_found = false;
		cont = true;
		while (cont)
		{
			//ignore comments
			
			cm1 = -1;
			cm2 = -1;
			cm2_end = -1;
			hasDirection = false;
			
			if (!stop)
			{
				getline(file, str);
				str = keep_str + " " + str;
				keep_str = "";
				
			}
			else
			{
				cont = false;
			}
			
			do
			{
				part1 = "";
				part2 = "";
				if ((cm2 = str.find("/*")) >= 0)
				{
					part1 = str.substr(0, cm2); //in case there is some code before comment
												 //read till you reach /*
					cm2_end = str.find("*/");
					while (cm2_end == -1)
					{
						getline(file, str);
						cm2_end = str.find("*/");
					}
					part2 = str.substr(cm2_end + 2);
					str = part1 + part2;

				}

				if ((cm1 = str.find("//")) >= 0)
				{
					str = str.substr(0, cm1);
				}
			} while (cm1>=0 || cm2>=0);

			
			// now we have a string without any comments
			if (!name_found)
			{
				if ((loc1 = str.find("module ")) >= 0)
				{
					if (str.find_first_not_of(" \t\n\v\f\r", loc1 + 6) != std::string::npos)
					{
						if ((loc2 = str.find("(")) > 0)
						{
							name = token(str, loc1 + 6, loc2);
							str = str.substr(loc2+1);
						}
						else
						{
							name = token(str, loc1 + 6, str.size());
							str = "";
						}
						
						keep_str = ""; //name founded, no need to keep string 
						
						instant << name << endl;
						//tmpl << name << " ";
						//tmpl.close();
						instant.close();
						//delim = ',';
						name_found = true;

					}
					else
					{
						keep_str = str;
					}
				}
			}
			if (name_found)
			{
				
				if ((loc1 = str.find(',')) >= 0)
				{
					keep_str = str.substr(loc1 + 1);
					str = str.substr(0, loc1);

				}
				else if ((loc1 = str.find(")")) >= 0)
				{
					str = str.substr(0, loc1);
					keep_str = ""; //its been the last element
					stop = true;
				}
				

				/* Serach for ANSI-STYLE port declaration */
				if ((loc1 = str.find("input")) >= 0
					&& (loc1>0 ? (!isalnum(str[loc1-1]) && str[loc1-1] != '_') : true)
					&& (loc1+5 <str.size() ? !isalnum(str[loc1+5]) :true ))
				{
					direction = "input";
					hasDirection = true;
					if ((loc2 = str.find("wire"))>0)
					{
						loc = loc2 + 4;
					}
					else
					{
						loc = loc1 + 5;
					}
					num_ins += tokenize(str, inputs, loc);
					str = "";
				}
				else if ((loc1 = str.find("output")) >= 0
					&& (loc1>0 ? (!isalnum(str[loc1-1]) && str[loc1-1] != '_') : true)
					&& (loc1+6 <str.size() ? !isalnum(str[loc1+6]) :true ))
				{
					direction = "output";
					hasDirection = true;
					if ((loc2 = str.find("reg"))>0)
					{
						loc = loc2 + 3;
					}
					else if ((loc2 = str.find("wire"))>0)
					{
						loc = loc2 + 4;
					}
					else
					{
						loc = loc1 + 6;
					}

					num_outs += tokenize(str, outputs, loc);
					str = "";

				}
				else if ((loc1 = str.find("inout")) >= 0
					&& (loc1>0 ? (!isalnum(str[loc1-1]) && str[loc1-1] != '_') : true)
					&& (loc1+5 <str.size() ? !isalnum(str[loc1+5]) :true ))
				{
					direction = "inout";
					hasDirection = true;
					if ((loc2 = str.find("wire"))>0)
					{
						loc = loc2 + 4;
					}
					else
					{
						loc = loc1 + 5;
					}
					num_inouts += tokenize(str, inouts, loc);
					str = "";

				}
				else if (!hasDirection)
				{
					str = direction + " " + str;
					keep_str = str + "," + keep_str;
					direction = "";
					
				}
			}
			
			
		}//end of while( getline)
		inputs.close();
		outputs.close();
		inouts.close();

		instant.open(insts_path + fname + "_instant.txt", ios::app);
		instant << num_ins << endl;

		gather(instant, inputs, insts_path + "inputs.txt", fname);

		instant << num_outs << endl;

		gather(instant, outputs, insts_path + "outputs.txt", fname);

		instant << num_inouts << endl; 
		
		gather(instant, inouts, insts_path + "inouts.txt", fname);
		instant.close();

		create_blackBox(instant, fname);
		return 1;	
	
	}
	else
	{
		cerr << "\nParser is unable to open "<<fname<<".v.\n";
		return 0;
	}
}


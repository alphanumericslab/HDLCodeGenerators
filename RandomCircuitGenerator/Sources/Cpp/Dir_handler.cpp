#include "../Header/Header.h"

#include <dirent.h>


void make_dir(int n, int l, int c, int t){

	string path = string(getcwd(NULL, 0)) + output_path;
	clean_it (path); //clean up destination folder
	mkdir(path.c_str());

	string folder = path + "TCL";
	mkdir(folder.c_str());

	folder = path + "Logs";
	mkdir(folder.c_str());

	folder = path + "tmp";
	mkdir(folder.c_str());

	path += "Verilog/";
	mkdir(path.c_str());

	folder = path + "original";
	mkdir(folder.c_str());
	folder = path + "original/final";
	mkdir(folder.c_str());

	for (int i=0; i<t; i++)
	{
		folder = path + "mutant_" + to_string(i);
		mkdir(folder.c_str());
		folder += "/final";
		mkdir(folder.c_str());
	}
	
	for (int i=0; i<= n; i++)
	{
		folder = path + "original/N_"  + to_string(i);
		mkdir(folder.c_str());

		for (int j = 0; j < t; j++)
		{
			folder = path + "/mutant_" + to_string(j) + "/N_" + to_string(i);
			mkdir(folder.c_str());
		}
			
	}
	
}


int list_it(vector <string> & Verilog_files, vector <string>& files_paths, const char* path, int length){
	DIR *dir;
	struct dirent *ent;
	
	int l;

	if ((dir = opendir (path)) != NULL) 
	{
	  /* print all the files and directories within directory */
	  while ((ent = readdir (dir)) != NULL) 
	  {
		  string name = ent->d_name;
		  if(name!="." && name!= ".." && ent->d_type == DT_DIR)
		  {
			 l = name.size();
			 name = string(path, length)+ "/" + name;
			 //cout<<name<<endl;
			 list_it(Verilog_files, files_paths, name.c_str(), l+length+1);
		  }

		  else
		  {
			l = name.size();
			if(l>2 && name[l-1]=='v' && name[l-2]=='.')//non-empty name of file with ".v" format and 
			{
				name.resize(l-2); //get file name without format
				
				Verilog_files.push_back(name);
				files_paths.push_back(string(path, length)+"/"+name);
				//cout<<string(path, length)+'/'+name<<endl;
				
			}
		  }
	  }
	  closedir (dir);
	  return Verilog_files.size();
	} 
	else 
	{
	  /* could not open directory */
	  perror ("could not open directory");
	  return -1;
	}
}

int clean_it (string folder_address){
	const char* path = folder_address.c_str();
	int length = folder_address.size();

	DIR *dir;
	struct dirent *ent;

	if ((dir = opendir (path)) != NULL) 
	{
	  /* print all the files and directories within directory */
	  while ((ent = readdir (dir)) != NULL) 
	  {
		  string name = ent->d_name;
		  if(name!="." && name!= ".." && ent->d_type == DT_DIR)
		  {
			  name = string(path, length)+ "/" + name;
			  clean_it(name);
		  }

		  else if (name!="." && name!= "..")
		  {
			 name = string(path, length)+ "/" + name;
			if (remove(name.c_str()) != 0)
				perror("Error deleting file");
			
		  }
	  }
	  closedir (dir);
	  //remove (path);
	  return 1;
	} 
	else 
	{
	  /* could not open directory */
	 // perror ("could not open directory");
	  return -1;
	}
}

#include "../Header/Header.h"
#include <random>

std::random_device rd;
std::mt19937 gen(rd());
std::uniform_int_distribution<> dis;

std::random_device rd_m;
std::mt19937 gen_m(rd_m());
std::uniform_int_distribution<> dis_m;


void rand_init(int min, int max) {
	dis = std::uniform_int_distribution<>(min, max);
	//cout<<"entropy: "<<rd_m.entropy()<<endl;
	//cout<<"dis max:  "<<dis.max()<<endl;
	
}

int rand_c() {
	return dis(gen);
	//return gen() % 4;
}

int rand_mod(int min, int max) { //interval [min, max)
	if (max > min)
	{
		return min + (dis_m(gen_m) % (max - min));
		//return min + (gen_m() % (max - min));
	}
	else
	{
		return -1;
	}
}


int rand_default(int min, int max, int num) {
	if (max>min)
	{
		return min + rand() % (max - min);
	}
	else
	{
		return -1;
	}


}



/**************************/
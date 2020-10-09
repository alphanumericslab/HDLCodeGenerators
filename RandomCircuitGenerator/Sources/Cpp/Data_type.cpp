#include "../Header/Header.h"

couple::couple() {}

couple::couple(int m, int p) {
	module = m;
	port = p;
}

int couple::get_module() {
	return module;
}

int couple::get_port() {
	return port;
}

triple::triple(){}

triple::triple(int t1, int t2, int t3) {
	mut1 = t1;
	mut2 = t2;
	mut3 = t3;
}

int triple::get_type1() {
	return mut1;
}

int triple::get_type2() {
	return mut2;
}

int triple::get_type3() {
	return mut3;
}

void triple::inc_type1() {
	mut1++;
}

void triple::inc_type2() {
	mut2++;
}

void triple::inc_type3() {
	mut3++;
}
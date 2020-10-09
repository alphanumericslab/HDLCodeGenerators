call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvarsall.bat" x86_amd64

if not exist "../../EXE" mkdir "../../EXE"
cd ../Cpp

del "nlc.exe"
cl main.cpp circ_gen.cpp circ.cpp check_loop.cpp  dangle.cpp connect.cpp ^
	initialize.cpp latch_it.cpp make_top.cpp Mutate.cpp Node.cpp Parser.cpp ^
	ANSI_parser.cpp print.cpp Rand.cpp Select.cpp SetIO.cpp TCL_gen.cpp ^
	xilinx_parser.cpp Data_type.cpp dir_handler.cpp  stats.cpp make_layer.cpp

ren  main.exe nlc.exe	
xcopy /y nlc.exe  ..\..\EXE

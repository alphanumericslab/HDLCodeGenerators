    print("always @(posedge clk) begin", file=of)
    print("\tif(reset) begin", file=of)
    print("\t\trowSetInNo <= 0;", file=of)
    print("\tend", file=of)
    print("\telse if(enable & (inSeries=={})".format(HRR-1), end='', file=of)
    if Enable_Vector_Latch<=0:
        print(" & inReady", end='', file=of)
    print(") begin", file=of)
    if Enable_Vector_Latch>0:
        print("\t\tif(rowSetInNo==0) begin", file=of)
        print("\t\t\tif(inReady) begin", file=of)
        print("\t", end='', file=of)
        print("\t\t\trowSetInNo <= 1;", file=of)
        print("\t\t\tend", file=of)
        print("\t\tend", file=of)
        print("\t\telse ", end='', file=of)
    else:
        print("\t\t", end='', file=of)
    print("if(rowSetInNo=={})".format(RS-1), end='', file=of)
    print(" begin", file=of)
    print("\t\t\trowSetInNo <= 0;", file=of)
    print("\t\tend", file=of)
    if Enable_Vector_Latch<=0 or RS>2:
        print("\t\telse begin", file=of)
        if RS==2:
            print("\t\t\trowSetInNo <= 1;", file=of)
        elif RS==3 and Enable_Vector_Latch>0:
            print("\t\t\trowSetInNo <= 2;", file=of)            
        else:
            print("\t\t\trowSetInNo <= rowSetInNo+1;", file=of)
        print("\t\tend", file=of)
    print("\tend", file=of)
    print("end", file=of)
    print(file=of)
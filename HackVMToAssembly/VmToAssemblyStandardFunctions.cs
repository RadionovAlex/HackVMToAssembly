namespace HackVMToAssembly
{
    public class VmToAssemblyStandardFunctions
    {

        public static string GoToProgramStartDefinition => @"
@ProgramStart
0;JMP";

        public static string ProgramStartDefinition => @"
(ProgramStart)
";

        public static string ProgramVariablesInitialization => @"
@256
D=A
@SP
M=D
";


        public static string AddDefinition => @"// get the value of the last stack value and decrease stack pointer
@SP
A=M-1
D=M     // write to the D register value of the last stack value
@SP
M=M-1   // decrease stack pointer

@SP
A=M-1
M=D+M   // makes calculation with the second argument ( last stack value ), rewrtie first stack value and let 
	    // SP be in the same value
@R13
A=M
0;JMP";

        public static string SubDefinition => @"// get the value of the last stack value and decrease stack pointer
@SP
A=M-1
D=M    // write to the D register value of the last stack value
@SP
M=M-1   // decrease stack pointer

@SP
A=M-1
M=M-D   // makes calculation with the second argument ( last stack value ), rewrtie first stack value and let 
	    // SP be in the same value
@R13
A=M
0;JMP";

        public static string NegDefinition => @"// get the value of the last stack value and decrease stack pointer

@SP
A=M-1
M=-M    // makes calculation with the second argument ( last stack value ), rewrtie first stack value and let 
	    // SP be in the same value
@R13
A=M
0;JMP";

        public static string NotDefinition => @"// get the value of the last stack value and decrease stack pointer

@SP
A=M-1
M=!M    // makes calculation with the second argument ( last stack value ), rewrtie first stack value and let 
	    // SP be in the same value
@R13
A=M
0;JMP";

        public static string EqualDefiniton => @"// get the value of the last stack value and decrease stack pointer
@SP
A=M-1
D=M  // write to the D register value of the last stack value
@SP
M=M-1 // decrease stack pointer
A=M-1
D=M-D 

@EqualTrue
D;JEQ

@SP
A=M-1
M=0
@R13 // get the value from the R13 register ( return place )
A=M
0;JMP // jump to the called function

(EqualTrue)
@SP
A=M-1
M=-1
@R13 // get the value from the R13 register ( return place )
A=M
0;JMP // jump to the called function";



        public static string GreatThanDefinition => @"// get the value of the last stack value and decrease stack pointer
@SP
A=M-1
D=M  // write to the D register value of the last stack value
@SP
M=M-1 // decrease stack pointer
A=M-1
D=M-D 

@GreatThanTrue
D;JGT

@SP
A=M-1
M=0
@R13 // get the value from the R13 register ( return place )
A=M
0;JMP // jump to the called function

(GreatThanTrue)
@SP
A=M-1
M=-1
@R13 // get the value from the R13 register ( return place )
A=M
0;JMP // jump to the called function";

        public static string LessThanDefinition => @"// get the value of the last stack value and decrease stack pointer
@SP
A=M-1
D=M  // write to the D register value of the last stack value
@SP
M=M-1 // decrease stack pointer
A=M-1
D=M-D 

@LessThanTrue
D;JLT

@SP
A=M-1
M=0
@R13 // get the value from the R13 register ( return place )
A=M
0;JMP // jump to the called function

(LessThanTrue)
@SP
A=M-1
M=-1
@R13 // get the value from the R13 register ( return place )
A=M
0;JMP // jump to the called function";


        public static string AndDefinition => @"// get the value of the last stack value and decrease stack pointer
@SP
A=M-1
D=M    // write to the D register value of the last stack value
@SP
M=M-1   // decrease stack pointer

@SP
A=M-1
M=D&M  // makes calculation with the second argument ( last stack value ), rewrtie first stack value and let 
	    // SP be in the same value
@R13
A=M
0;JMP";

        public static string OrDefinition => @"// get the value of the last stack value and decrease stack pointer
@SP
A=M-1
D=M    // write to the D register value of the last stack value
@SP
M=M-1   // decrease stack pointer

@SP
A=M-1
M=D|M  // makes calculation with the second argument ( last stack value ), rewrtie first stack value and let 
	    // SP be in the same value
@R13
A=M
0;JMP";

        public static string PushDefinition => @"
@R14
D=M
@SP // A is at SP now
A=M // go to address of SP
M=D // write to the RAM[A] (where pointer points) temporary value from D register  (or from R14)
@SP
M=M+1  // increase stack pointer index

@R13
A=M
0;JMP";


        public static string PutConstantIntoD(int value) =>
            @$"@{value}
D=A
@R14
M=D";
    }
}
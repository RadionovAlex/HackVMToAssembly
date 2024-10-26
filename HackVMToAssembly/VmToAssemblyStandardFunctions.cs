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
@SP // A is at SP now
A=M // go to address of SP
M=D // write to the RAM[A] (where pointer points) temporary value from D register  (or from R14)
@SP
M=M+1  // increase stack pointer index
";

        public static string GoToR13CodeRaw => @"
@R13
A=M
0;JMP
";


        public static string PutConstantIntoR14(int value) =>
            @$"@{value}
D=A
";

        public static string PutSegmentIndexValueIntoR14(string segment, int index) =>
            @$"@{index}
D=A
@{segment}
A=D+M
D=M
";

        public static string PutPointerIndexValueIntoR14(int ponterValue, int index) =>
            $@"@{ponterValue}
D=A
@{index}
A=D+A
D=M
";

        public static string PopDefinition => @"
// go to stackPointer, get previous value, write it in the R14. Then -> decrease stackPointer
@SP
A=M-1
D=M

@R14
M=D

@SP
M=M-1

@R13
A=M
0;JMP
";

        public static string PopR14IntoConstant(int value) =>
           @$"//it should be actually called. By in case there is should be an stack cleaning - ok..";

        public static string PopR14IntoSegment(string segment, int index) =>
            $@"
@{index}
D=A
@{segment}
D=D+M
@R15
M=D

@R14
D=M

@R15
A=M
M=D
";

        public static string PopR14IntoPointerIndex(int pointer, int index) =>
            $@"
@{pointer}
D=A
@{index}
D=D+A
@R15
M=D

@R14
D=M

@R15
A=M
M=D
";

    }
}
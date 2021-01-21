# Watson.NET
A C# implentation of the WATSON configuration language  
  
Original implementation found [here](https://github.com/genkami/watson)

## Usage

`Watson encode -t TYPE [--initial-mode MODE] [FILE]`  

Converts a file `FILE` of type `TYPE` using the initial lexer mode `MODE` and outputs its Watson Representation to the standard output.

`Watson decode -t TYPE [--initial-mode MODE] [FILE...]`  

Converts several Watson files to the specified format `TYPE` using the initial lexer mode `MODE` and outputs it to the standard output.

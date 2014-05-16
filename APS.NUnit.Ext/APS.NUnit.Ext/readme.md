Operations for getting embedded resources.
	1. Single file methods
		1. Type + filename - Single
			1. Get object's type, and name of file.
			2. Get stream using concat of type namespace and name of file.
	2. Multiple file methods
		1. Assembl(y/ies) (all embedded resouces) - All
			1. GetManifestResouceNames 
			3. Iterate and create files from list.
		2. Type(s) (all embedded resources) - All
			1. Get assembly of type.
			2. GetManifestResouceNames 
			3. Iterate and create files from list.
		3. Type(s) (all namespace resources) - AllNs
			1. GetManifestResouceNames
			2. GetManifestResourceStream(type, filename)

Output for extracted embedded resources
	1. List of Successes.
		1. FileInfo
		2. Assembly
		3. Namespace
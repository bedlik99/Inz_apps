# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.20

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Disable VCS-based implicit rules.
% : %,v

# Disable VCS-based implicit rules.
% : RCS/%

# Disable VCS-based implicit rules.
% : RCS/%,v

# Disable VCS-based implicit rules.
% : SCCS/s.%

# Disable VCS-based implicit rules.
% : s.%

.SUFFIXES: .hpux_make_needs_suffix_list

# Command-line flag to silence nested $(MAKE).
$(VERBOSE)MAKESILENT = -s

#Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/local/bin/cmake

# The command to remove a file.
RM = /usr/local/bin/cmake -E rm -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /home/cerber/Documents/lab_supervision/module_service_program

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /home/cerber/Documents/lab_supervision/module_service_program

# Include any dependencies generated for this target.
include IOConfig/CMakeFiles/IOConfig.dir/depend.make
# Include any dependencies generated by the compiler for this target.
include IOConfig/CMakeFiles/IOConfig.dir/compiler_depend.make

# Include the progress variables for this target.
include IOConfig/CMakeFiles/IOConfig.dir/progress.make

# Include the compile flags for this target's objects.
include IOConfig/CMakeFiles/IOConfig.dir/flags.make

IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o: IOConfig/CMakeFiles/IOConfig.dir/flags.make
IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o: IOConfig/IOConfig.cpp
IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o: IOConfig/CMakeFiles/IOConfig.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/cerber/Documents/lab_supervision/module_service_program/CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building CXX object IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o"
	cd /home/cerber/Documents/lab_supervision/module_service_program/IOConfig && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -MD -MT IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o -MF CMakeFiles/IOConfig.dir/IOConfig.cpp.o.d -o CMakeFiles/IOConfig.dir/IOConfig.cpp.o -c /home/cerber/Documents/lab_supervision/module_service_program/IOConfig/IOConfig.cpp

IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/IOConfig.dir/IOConfig.cpp.i"
	cd /home/cerber/Documents/lab_supervision/module_service_program/IOConfig && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/cerber/Documents/lab_supervision/module_service_program/IOConfig/IOConfig.cpp > CMakeFiles/IOConfig.dir/IOConfig.cpp.i

IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/IOConfig.dir/IOConfig.cpp.s"
	cd /home/cerber/Documents/lab_supervision/module_service_program/IOConfig && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/cerber/Documents/lab_supervision/module_service_program/IOConfig/IOConfig.cpp -o CMakeFiles/IOConfig.dir/IOConfig.cpp.s

# Object files for target IOConfig
IOConfig_OBJECTS = \
"CMakeFiles/IOConfig.dir/IOConfig.cpp.o"

# External object files for target IOConfig
IOConfig_EXTERNAL_OBJECTS =

IOConfig/libIOConfig.a: IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o
IOConfig/libIOConfig.a: IOConfig/CMakeFiles/IOConfig.dir/build.make
IOConfig/libIOConfig.a: IOConfig/CMakeFiles/IOConfig.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir=/home/cerber/Documents/lab_supervision/module_service_program/CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Linking CXX static library libIOConfig.a"
	cd /home/cerber/Documents/lab_supervision/module_service_program/IOConfig && $(CMAKE_COMMAND) -P CMakeFiles/IOConfig.dir/cmake_clean_target.cmake
	cd /home/cerber/Documents/lab_supervision/module_service_program/IOConfig && $(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/IOConfig.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
IOConfig/CMakeFiles/IOConfig.dir/build: IOConfig/libIOConfig.a
.PHONY : IOConfig/CMakeFiles/IOConfig.dir/build

IOConfig/CMakeFiles/IOConfig.dir/clean:
	cd /home/cerber/Documents/lab_supervision/module_service_program/IOConfig && $(CMAKE_COMMAND) -P CMakeFiles/IOConfig.dir/cmake_clean.cmake
.PHONY : IOConfig/CMakeFiles/IOConfig.dir/clean

IOConfig/CMakeFiles/IOConfig.dir/depend:
	cd /home/cerber/Documents/lab_supervision/module_service_program && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /home/cerber/Documents/lab_supervision/module_service_program /home/cerber/Documents/lab_supervision/module_service_program/IOConfig /home/cerber/Documents/lab_supervision/module_service_program /home/cerber/Documents/lab_supervision/module_service_program/IOConfig /home/cerber/Documents/lab_supervision/module_service_program/IOConfig/CMakeFiles/IOConfig.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : IOConfig/CMakeFiles/IOConfig.dir/depend


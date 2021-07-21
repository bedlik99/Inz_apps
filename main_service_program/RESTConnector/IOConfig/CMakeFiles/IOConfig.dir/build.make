# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.16

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:


#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:


# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list


# Suppress display of executed commands.
$(VERBOSE).SILENT:


# A target that is always out of date.
cmake_force:

.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/bin/cmake

# The command to remove a file.
RM = /usr/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program

# Include any dependencies generated for this target.
include RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/depend.make

# Include the progress variables for this target.
include RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/progress.make

# Include the compile flags for this target's objects.
include RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/flags.make

RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o: RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/flags.make
RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o: RESTConnector/IOConfig/IOConfig.cpp
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir=/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/CMakeFiles --progress-num=$(CMAKE_PROGRESS_1) "Building CXX object RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o"
	cd /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig && /usr/bin/c++  $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -o CMakeFiles/IOConfig.dir/IOConfig.cpp.o -c /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig/IOConfig.cpp

RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/IOConfig.dir/IOConfig.cpp.i"
	cd /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -E /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig/IOConfig.cpp > CMakeFiles/IOConfig.dir/IOConfig.cpp.i

RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/IOConfig.dir/IOConfig.cpp.s"
	cd /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig && /usr/bin/c++ $(CXX_DEFINES) $(CXX_INCLUDES) $(CXX_FLAGS) -S /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig/IOConfig.cpp -o CMakeFiles/IOConfig.dir/IOConfig.cpp.s

# Object files for target IOConfig
IOConfig_OBJECTS = \
"CMakeFiles/IOConfig.dir/IOConfig.cpp.o"

# External object files for target IOConfig
IOConfig_EXTERNAL_OBJECTS =

RESTConnector/IOConfig/libIOConfig.a: RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/IOConfig.cpp.o
RESTConnector/IOConfig/libIOConfig.a: RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/build.make
RESTConnector/IOConfig/libIOConfig.a: RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir=/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/CMakeFiles --progress-num=$(CMAKE_PROGRESS_2) "Linking CXX static library libIOConfig.a"
	cd /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig && $(CMAKE_COMMAND) -P CMakeFiles/IOConfig.dir/cmake_clean_target.cmake
	cd /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig && $(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/IOConfig.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/build: RESTConnector/IOConfig/libIOConfig.a

.PHONY : RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/build

RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/clean:
	cd /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig && $(CMAKE_COMMAND) -P CMakeFiles/IOConfig.dir/cmake_clean.cmake
.PHONY : RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/clean

RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/depend:
	cd /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig /home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/main_service_program/RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : RESTConnector/IOConfig/CMakeFiles/IOConfig.dir/depend

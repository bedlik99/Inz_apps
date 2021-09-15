cmake_minimum_required(VERSION 3.9)
project(IdentifyOnStart)

set(CMAKE_C_COMPILER /usr/bin/gcc-5)
set(CMAKE_CXX_COMPILER /usr/bin/g++-5)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)
#set(CMAKE_MODULE_PATH "/usr/share/SFML/cmake/Modules" ${CMAKE_MODULE_PATH})
set(CMAKE_PREFIX_PATH /usr/lib/x86_64-linux-gnu/cmake/)

find_package(SFML COMPONENTS graphics window system REQUIRED)

add_subdirectory(GraphicManagement)
add_subdirectory(IOConfig)

list(APPEND GRAPHICS_MANAGEMENT_LIB GraphicManager)
list(APPEND IO_CONFIG_LIB IOConfig)

add_executable(IdentifyOnStart identify_main.cpp) 

target_link_libraries(IdentifyOnStart PUBLIC sfml-system sfml-graphics sfml-window)
target_link_libraries(IdentifyOnStart PUBLIC 
${GRAPHICS_MANAGEMENT_LIB} 
${IO_CONFIG_LIB}
 )

target_include_directories(IdentifyOnStart PUBLIC 
"${PROJECT_BINARY_DIR}"
)
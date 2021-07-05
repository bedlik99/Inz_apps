cmake_minimum_required(VERSION 3.9)
project(IdentifyOnStartup)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)

find_package(cpprestsdk REQUIRED)
find_package(SFML COMPONENTS graphics window system REQUIRED)

add_executable(IdentifyOnStartup identify_main.cpp) 

target_link_libraries(IdentifyOnStartup PRIVATE cpprestsdk::cpprest)
target_link_libraries(IdentifyOnStartup PUBLIC sfml-system sfml-graphics sfml-window)

target_include_directories(IdentifyOnStartup PUBLIC 
"${PROJECT_BINARY_DIR}"
)
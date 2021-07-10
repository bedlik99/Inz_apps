cmake_minimum_required(VERSION 3.9)
project(IdentifyOnStartup)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)

find_package(cpprestsdk REQUIRED)
find_package(SFML COMPONENTS graphics window system REQUIRED)

add_subdirectory(GraphicManagement)
add_subdirectory(RESTConnector)

list(APPEND GRAPHICS_MANAGEMENT_LIB GraphicManager)
list(APPEND REST_CONNECTOR_LIB RESTConnector)

add_executable(IdentifyOnStartup identify_main.cpp) 

target_link_libraries(IdentifyOnStartup PRIVATE cpprestsdk::cpprest)
target_link_libraries(IdentifyOnStartup PUBLIC sfml-system sfml-graphics sfml-window)
target_link_libraries(IdentifyOnStartup PUBLIC 
${GRAPHICS_MANAGEMENT_LIB} 
${REST_CONNECTOR_LIB}
 )

target_include_directories(IdentifyOnStartup PUBLIC 
"${PROJECT_BINARY_DIR}"
)
cmake_minimum_required(VERSION 3.9)
project(MainService)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)
set(CMAKE_PREFIX_PATH /usr/lib/x86_64-linux-gnu/cmake/) #Stary system linuksowy nie znajdowal lokalizacji .cmake

find_package(cpprestsdk REQUIRED)
add_subdirectory(RESTConnector)

list(APPEND REST_CONNECTOR_LIB RESTConnector)

add_executable(MainService main_service.cpp) 

target_link_libraries(MainService PRIVATE cpprestsdk::cpprest)

target_link_libraries(MainService PUBLIC 
${REST_CONNECTOR_LIB}
-lboost_system)

target_include_directories(MainService PUBLIC 
"${PROJECT_BINARY_DIR}"
)
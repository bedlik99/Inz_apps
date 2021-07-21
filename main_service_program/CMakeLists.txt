cmake_minimum_required(VERSION 3.9)
project(HttpRestClient)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)

find_package(cpprestsdk REQUIRED)

add_subdirectory(RESTConnector)

list(APPEND REST_CONNECTOR_LIB RESTConnector)

add_executable(HttpRestClient client_service.cpp) 

target_link_libraries(HttpRestClient PRIVATE cpprestsdk::cpprest)
target_link_libraries(HttpRestClient PUBLIC ${REST_CONNECTOR_LIB})

target_include_directories(HttpRestClient PUBLIC 
"${PROJECT_BINARY_DIR}"
)
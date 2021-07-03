cmake_minimum_required(VERSION 3.9)
project(HttpRestClient)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)

find_package(cpprestsdk REQUIRED)
add_subdirectory(MessageDTO)
add_subdirectory(IOConfig)

list(APPEND EXTRA_LIBS MessageDTO)
list(APPEND EXTRA_CONF_LIB IOConfig)

add_executable(HttpRestClient client_main.cpp) # wystarczy zmienić na client_service albo client_main;

target_link_libraries(HttpRestClient PRIVATE cpprestsdk::cpprest)
target_link_libraries(HttpRestClient PUBLIC ${EXTRA_LIBS} ${EXTRA_CONF_LIB})


target_include_directories(HttpRestClient PUBLIC 
"${PROJECT_BINARY_DIR}"
)
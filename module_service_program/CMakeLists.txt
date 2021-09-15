cmake_minimum_required(VERSION 3.9)
project(ModuleService)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED True)

add_subdirectory(IOConfig)
add_subdirectory(ModuleManager)

list(APPEND IO_CONFIG_LIB IOConfig)
list(APPEND MODULE_MANAGER_LIB ModuleManager)

add_executable(ModuleService module_service.cpp) 

target_link_libraries(ModuleService PUBLIC 
${IO_CONFIG_LIB}
${MODULE_MANAGER_LIB}
)

target_include_directories(ModuleService PUBLIC 
"${PROJECT_BINARY_DIR}"
)

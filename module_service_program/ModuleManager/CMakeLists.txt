add_library(ModuleManager ModuleManager.cpp)

target_include_directories(ModuleManager
          INTERFACE "${CMAKE_CURRENT_SOURCE_DIR}"
          )